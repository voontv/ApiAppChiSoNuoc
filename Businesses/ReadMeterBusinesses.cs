using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using ReadMeter.Api.Data;
using ReadMeter.Api.Models;
using ReadMeter.Api.OracleModels;
using ReadMeter.Api.Contracts.Requests;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace ReadMeter.Api.Businesses;

public sealed class ReadMeterBusinesses : IReadMeterBusinesses
{
    private const string GetMeterReaderByCustomerUrl = "https://internal-api.dawaco.com.vn/api/Ns00HoSoMain/getmabendoc";
    private readonly ReadMeterDbContext _context;
    private readonly BillingDbContext _billing;
    private readonly IHttpClientFactory _httpClientFactory;
    private static readonly JsonSerializerOptions LegacyJsonOptions = new()
    {
        PropertyNamingPolicy = new LegacyUpperSnakeCaseNamingPolicy()
    };

    public ReadMeterBusinesses(ReadMeterDbContext context, BillingDbContext billing, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _billing = billing;
        _httpClientFactory = httpClientFactory;
    }

    private static ContentResult JsonObject(object value) => new()
    {
        StatusCode = StatusCodes.Status200OK,
        ContentType = "application/json; charset=utf-8",
        Content = JsonSerializer.Serialize(value, LegacyJsonOptions)
    };

    private sealed class LegacyUpperSnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            if (name.All(char.IsUpper))
                return name;

            var result = new StringBuilder(name.Length + 8);
            for (var index = 0; index < name.Length; index++)
            {
                var current = name[index];
                if (index > 0 && char.IsUpper(current) && char.IsLower(name[index - 1]))
                    result.Append('_');
                result.Append(char.ToUpperInvariant(current));
            }

            return result.ToString();
        }
    }

    private static string? ExtractMeterReaderCode(string response)
    {
        var value = response.Trim();
        if (string.IsNullOrWhiteSpace(value))
            return null;

        try
        {
            using var document = JsonDocument.Parse(value);
            var root = document.RootElement;
            if (root.ValueKind == JsonValueKind.String)
                return root.GetString()?.Trim();

            if (root.ValueKind == JsonValueKind.Object)
            {
                foreach (var name in new[] { "MA_BIEN_DOC", "ma_bien_doc", "maBienDoc", "mabendoc", "data", "result" })
                {
                    if (root.TryGetProperty(name, out var property))
                        return property.ValueKind == JsonValueKind.String
                            ? property.GetString()?.Trim()
                            : property.ToString().Trim();
                }
            }
        }
        catch (JsonException)
        {
        }

        return value.Trim('"').Trim();
    }

    private static object DbValue(object? value) => value ?? DBNull.Value;

    private static OracleParameter Param(string name, object? value)
    {
        var parameter = new OracleParameter(name, DbValue(value));
        if (value is string)
            parameter.OracleDbType = OracleDbType.Varchar2;
        return parameter;
    }

    private static OracleParameter NParam(string name, string? value)
    {
        return new OracleParameter(name, DbValue(value))
        {
            OracleDbType = OracleDbType.NVarchar2
        };
    }

    private async Task<List<Dictionary<string, object?>>> QueryRowsAsync(
        string sql, IEnumerable<OracleParameter> parameters, CancellationToken cancellationToken)
    {
        var connection = _context.Database.GetDbConnection();
        var shouldClose = connection.State != ConnectionState.Open;
        if (shouldClose)
            await connection.OpenAsync(cancellationToken);

        try
        {
            await using var command = connection.CreateCommand();
            command.CommandText = sql;
            command.CommandTimeout = 15;
            if (command is OracleCommand oracleCommand)
                oracleCommand.BindByName = true;
            foreach (var parameter in parameters)
                command.Parameters.Add(parameter);

            var rows = new List<Dictionary<string, object?>>();
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                var row = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
                for (var i = 0; i < reader.FieldCount; i++)
                    row[reader.GetName(i)] = await reader.IsDBNullAsync(i, cancellationToken) ? null : reader.GetValue(i);
                rows.Add(row);
            }
            return rows;
        }
        finally
        {
            if (shouldClose)
                await connection.CloseAsync();
        }
    }

    private async Task<int> ExecuteNonQueryAsync(
        string sql, IEnumerable<OracleParameter> parameters, CancellationToken cancellationToken)
    {
        var connection = _context.Database.GetDbConnection();
        var shouldClose = connection.State != ConnectionState.Open;
        if (shouldClose)
            await connection.OpenAsync(cancellationToken);

        try
        {
            await using var command = connection.CreateCommand();
            command.CommandText = sql;
            command.CommandTimeout = 15;
            if (command is OracleCommand oracleCommand)
                oracleCommand.BindByName = true;
            foreach (var parameter in parameters)
                command.Parameters.Add(parameter);

            return await command.ExecuteNonQueryAsync(cancellationToken);
        }
        finally
        {
            if (shouldClose)
                await connection.CloseAsync();
        }
    }

    private async Task<string> NextNumericIdAsync(string tableName, string columnName, int length, CancellationToken cancellationToken)
    {
        var rows = await QueryRowsAsync($"""
            SELECT LPAD(NVL(MAX(TO_NUMBER({columnName})), 0) + 1, :length, '0') NEXT_ID
            FROM {tableName}
            WHERE REGEXP_LIKE({columnName}, '^[0-9]+$')
            """, new[] { Param("length", length) }, cancellationToken);

        return rows[0]["NEXT_ID"]?.ToString() ?? "1".PadLeft(length, '0');
    }

    private async Task<ContentResult> GetMeterBooks(
        string? meterReader, string? month, bool supplementalOnly, bool pendingOnly,
        CancellationToken cancellationToken)
    {
        var where = new StringBuilder("""
            WHERE cs.MA_BIEN_DOC = :meterReader
              AND cs.THANG = :month
            """);
        var parameters = new List<OracleParameter>
        {
            Param("meterReader", meterReader),
            Param("month", month)
        };

        if (supplementalOnly)
            where.AppendLine("  AND cs.MA_TINH_TRANG_DH = 'BS'");
        if (pendingOnly)
            where.AppendLine("  AND cs.NGAY_EBILL_NHAN_KHOA IS NULL AND cs.NGAY_EBILL_NAP_BILL IS NULL");

        var rows = await QueryRowsAsync($"""
            SELECT
                '00- OK' ROOT,
                ROW_NUMBER() OVER (ORDER BY cs.MA_SO_DOC) STT,
                cs.MA_SO_DOC,
                sd.TEN_SO_DOC,
                sd.NGAY_DOC,
                CASE WHEN SUM(CASE WHEN cs.NGAY_BD_NHAN_KHOA IS NOT NULL THEN 1 ELSE 0 END) > 0 THEN 'Y' ELSE 'N' END TRANG_THAI_SO,
                COUNT(*) TONG_DH,
                SUM(CASE WHEN cs.NGAY_DOC_TUNG_DH IS NOT NULL THEN 1 ELSE 0 END) TONG_DH_DA_DOC,
                COUNT(*) - SUM(CASE WHEN cs.NGAY_DOC_TUNG_DH IS NOT NULL THEN 1 ELSE 0 END) TONG_DH_CHUA_DOC,
                CASE
                    WHEN SUM(CASE WHEN cs.NGAY_BIEN_DOC_BG IS NOT NULL AND cs.NGAY_EBILL_NHAN_KHOA IS NULL THEN 1 ELSE 0 END) > 0 THEN 2
                    WHEN SUM(CASE WHEN cs.NGAY_BD_NHAN_KHOA IS NOT NULL AND cs.NGAY_BIEN_DOC_BG IS NULL THEN 1 ELSE 0 END) > 0 THEN 1
                    ELSE 0
                END TRANG_THAI_BG_SO
            FROM APP_DH_CHI_SO cs
            JOIN DM_SO_DOC sd ON sd.MA_SO_DOC = cs.MA_SO_DOC
            {where}
            GROUP BY cs.MA_SO_DOC, sd.TEN_SO_DOC, sd.NGAY_DOC
            ORDER BY cs.MA_SO_DOC
            """, parameters, cancellationToken);

        return JsonObject(rows.Count == 0
            ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } }
            : rows.Cast<object>());

    }

    private enum CustomerReadingMode
    {
        Normal,
        Sub,
        SubBs,
        TraCuu
    }

    private static string GetCustomerReadingOrderBy(string? sortBy)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return "cs.STT_SO_DOC";

        return sortBy.Trim().ToUpperInvariant() switch
        {
            "STT_SO_DOC" => "cs.STT_SO_DOC",
            "STT_SO_DOC_MOI" => "cs.STT_SO_DOC_MOI",
            "MA_KHACH_HANG" => "cs.MA_KHACH_HANG",
            "TEN_KHACH_HANG" => "kh.TEN_KHACH_HANG",
            "DIA_CHI_DONG_HO" => "kh.DIA_CHI_DONG_HO",
            "NGAY_DOC_DK" => "cs.NGAY_DOC_DK",
            "NGAY_DOC_CS" => "cs.NGAY_DOC_CS",
            _ => "cs.STT_SO_DOC"
        };
    }

    private async Task<ContentResult> GetCustomersForReading(
        string? id, string? sequence, string? customerCode, string? customerName, string? address,
        string? phone, string? bookCode, string? meterReader, string? month, string? filter,
        string? sortBy, CustomerReadingMode mode, CancellationToken cancellationToken)
    {
        var where = new StringBuilder();
        var parameters = new List<OracleParameter>();

        var isNormalOrSub = mode is CustomerReadingMode.Normal or CustomerReadingMode.Sub;
        var isSubBs = mode == CustomerReadingMode.SubBs;
        var isTraCuu = mode == CustomerReadingMode.TraCuu;

        // Giữ đúng VB cũ cho P_03 / P_03_SUB / P_03_SUB_BS:
        // nếu có ID thì lọc ID_DCS + MA_SO_DOC, không ép MA_BIEN_DOC/THANG.
        if (!string.IsNullOrWhiteSpace(id))
        {
            where.AppendLine("WHERE cs.ID_DCS = :id");
            where.AppendLine("  AND cs.MA_SO_DOC = :bookCode");
            parameters.Add(Param("id", id));
            parameters.Add(Param("bookCode", bookCode));
        }
        else
        {
            where.AppendLine("""
                WHERE cs.MA_SO_DOC = :bookCode
                  AND cs.MA_BIEN_DOC = :meterReader
                  AND cs.THANG = :month
                """);
            parameters.Add(Param("bookCode", bookCode));
            parameters.Add(Param("meterReader", meterReader));
            parameters.Add(Param("month", month));

            // P_03 và P_03_SUB có 3 điều kiện này trong VB cũ.
            // P_03_SUB_BS đã comment 3 điều kiện này.
            // P_0313 giữ nguyên kiểu tra cứu hiện tại: không áp 3 điều kiện khóa.
            if (isNormalOrSub)
            {
                where.AppendLine("  AND cs.NGAY_EBILL_NHAN_KHOA IS NULL");
                where.AppendLine("  AND cs.NGAY_EBILL_NAP_BILL IS NULL");
                where.AppendLine("  AND cs.NGAY_BD_NHAN_KHOA IS NOT NULL");
            }
        }

        if (decimal.TryParse(sequence, out var rawSequenceValue))
        {
            where.AppendLine("  AND cs.STT_SO_DOC = :sequence");
            parameters.Add(Param("sequence", rawSequenceValue));
        }

        // VB cũ: P_03 và P_03_SUB có KIEU_LOC.
        // P_03_SUB_BS đã comment toàn bộ KIEU_LOC.
        if (isNormalOrSub)
        {
            if (filter == "1")
                where.AppendLine("  AND cs.NGAY_DOC_TUNG_DH IS NOT NULL");
            else if (filter == "2")
                where.AppendLine("  AND cs.NGAY_DOC_TUNG_DH IS NULL");
        }

        // P_03_SUB_BS luôn chỉ lấy mã tình trạng BS.
        if (isSubBs)
            where.AppendLine("  AND cs.MA_TINH_TRANG_DH = 'BS'");

        // VB cũ dùng so sánh chính xác MA_KHACH_HANG, không dùng LIKE.
        if (!string.IsNullOrWhiteSpace(customerCode))
        {
            where.AppendLine("  AND cs.MA_KHACH_HANG = :customerCode");
            parameters.Add(Param("customerCode", customerCode));
        }

        if (!string.IsNullOrWhiteSpace(customerName))
        {
            where.AppendLine("  AND UPPER(kh.TEN_KHACH_HANG) LIKE UPPER(:customerName)");
            parameters.Add(NParam("customerName", $"%{customerName}%"));
        }

        if (!string.IsNullOrWhiteSpace(address))
        {
            where.AppendLine("  AND UPPER(kh.DIA_CHI_DONG_HO) LIKE UPPER(:address)");
            parameters.Add(NParam("address", $"%{address}%"));
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            where.AppendLine("  AND kh.PHONE_UT1 LIKE :phone");
            parameters.Add(Param("phone", $"%{phone}%"));
        }

        // Ba hàm P_03 cũ dùng RIGHT OUTER JOIN từ THONG_TIN_KH sang APP_DH_CHI_SO,
        // tương đương APP_DH_CHI_SO LEFT JOIN THONG_TIN_KH.
        // Riêng P_0313 chưa có source VB trong phần đối chiếu nên giữ JOIN hiện tại.
        var joinClause = isTraCuu
            ? "JOIN THONG_TIN_KH kh ON kh.MA_KHACH_HANG = cs.MA_KHACH_HANG"
            : "LEFT JOIN THONG_TIN_KH kh ON kh.MA_KHACH_HANG = cs.MA_KHACH_HANG";

        // Ba hàm P_03 cũ dùng SAP_XEP_THEO, mặc định STT_SO_DOC.
        // P_0313 giữ nguyên thứ tự hiện tại vì chưa có hàm VB gốc để đối chiếu.
        var orderBy = isTraCuu
            ? "NVL(cs.STT_SO_DOC_MOI, cs.STT_SO_DOC)"
            : GetCustomerReadingOrderBy(sortBy);

        // PHẦN SELECT/RESPONSE GIỮ NGUYÊN như file hiện tại để không thay đổi dữ liệu trả client.
        var rows = await QueryRowsAsync($"""
            SELECT
                '00- OK' ROOT,
                cs.ID_DCS ID_DONG_HO,
                kh.MA_KHACH_HANG,
                kh.TEN_KHACH_HANG,
                kh.DIA_CHI_DONG_HO,
                kh.TEN_DONG_HO,
                kh.MA_DONG_HO CO_DH,
                kh.HT_KD,
                kh.SO_SERIAL_DONG_HO,
                kh.PHONE_UT1,
                kh.EMAIL_UT1,
                kh.MA_GIA,
                kh.SO_O_CUA_SO,
                cs.CHI_SO_CU,
                cs.CHI_SO_MOI,
                cs.SAN_LUONG_TT,
                cs.TONG_SL,
                cs.MA_TINH_TRANG_DH,
                cs.STT_SO_DOC,
                cs.STT_SO_DOC_MOI,
                cs.NGAY_DOC_DK,
                cs.NGAY_DOC_CS,
                cs.NGAY_DOC_TUNG_DH,
                CASE WHEN cs.NGAY_DOC_TUNG_DH IS NOT NULL THEN 'Đã ghi' ELSE 'Chưa ghi' END TINH_TRANG_CS,
                cs.QUA_VONG,
                cs.LOAI_CHI_SO,
                cs.CONG_CHI_SO,
                cs.SAN_LUONG_DUNG_IT,
                cs.MA_GHI_CHU,
                cs.GHI_CHU,
                cs.VI_TRI_DOC,
                cs.VI_TRI_DOC_CU,
                cs.TEN_FILE_ANH,
                cs.SL_TB_3THANG
            FROM APP_DH_CHI_SO cs
            {joinClause}
            {where}
              AND kh.NGAY_THANH_LY_HD IS NULL
            ORDER BY {orderBy}
            """, parameters, cancellationToken);

        return JsonObject(rows.Count == 0
            ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } }
            : rows.Cast<object>());

    }

    private async Task<ContentResult> SaveMeterReading(
        string? id, string? status, string? rollover, string? readingType, string? readingDate,
        string? currentReading, string? actualUsage, string? totalUsage, string? accumulatedReading,
        string? lowUsage, string? noteCode, string? note, string? location,
        string? customerCode, string? bookCode, string? month, string? branch, string? meterReader,
        CancellationToken cancellationToken)
    {
        static decimal FastNumber(string? value) => decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var number) ? number : 0;
        var parsedReadingDate = DateTime.TryParseExact(
            readingDate,
            new[] { "dd/MM/yyyy", "dd/MM/yyyy HH:mm:ss", "yyyy-MM-dd" },
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var date)
            ? date
            : DateTime.Now;

        var sql = new StringBuilder("""
            UPDATE APP_DH_CHI_SO
            SET MA_TINH_TRANG_DH = :status,
                QUA_VONG = :rollover,
                LOAI_CHI_SO = :readingType,
                NGAY_DOC_CS = :readingDate,
                CHI_SO_MOI = :currentReading,
                SAN_LUONG_TT = :actualUsage,
                TONG_SL = :totalUsage,
                CONG_CHI_SO = :accumulatedReading,
                SAN_LUONG_DUNG_IT = :lowUsage,
                MA_GHI_CHU = :noteCode,
                GHI_CHU = :note,
                VI_TRI_DOC_CU = VI_TRI_DOC,
                VI_TRI_DOC = :location,
                NGAY_DOC_TUNG_DH = SYSDATE,
                LOG_USER = :meterReader
            WHERE ID_DCS = :id
              AND MA_BIEN_DOC = :meterReader
            """);

        var parameters = new List<OracleParameter>
        {
            Param("status", status),
            Param("rollover", rollover),
            Param("readingType", readingType),
            Param("readingDate", parsedReadingDate),
            Param("currentReading", FastNumber(currentReading)),
            Param("actualUsage", FastNumber(actualUsage)),
            Param("totalUsage", FastNumber(totalUsage)),
            Param("accumulatedReading", FastNumber(accumulatedReading)),
            Param("lowUsage", FastNumber(lowUsage)),
            Param("noteCode", noteCode),
            NParam("note", note),
            Param("location", location),
            Param("meterReader", meterReader),
            Param("id", id)
        };

        static void AddOptionalFilter(StringBuilder sql, List<OracleParameter> parameters, string column, string name, string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            sql.AppendLine($"  AND {column} = :{name}");
            parameters.Add(Param(name, value));
        }

        AddOptionalFilter(sql, parameters, "MA_KHACH_HANG", "customerCode", customerCode);
        AddOptionalFilter(sql, parameters, "MA_SO_DOC", "bookCode", bookCode);
        AddOptionalFilter(sql, parameters, "THANG", "month", month);
        AddOptionalFilter(sql, parameters, "MA_CHI_NHANH", "branch", branch);

        var affectedRows = await ExecuteNonQueryAsync(sql.ToString(), parameters, cancellationToken);
        return JsonObject(new[] { new { ROOT = affectedRows == 0 ? "16- Dữ liệu không tìm thấy." : "00- OK" } });

    }

    public Task<ContentResult> P_00_KET_NOI_DB_CHECK(string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        Task.FromResult(JsonObject(new[] { new { ROOT = "00- OK" } }));

    public async Task<ContentResult> P_01_DANG_NHAP(string? USER, string? PASSWORD, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var employee = await _context.DmNhanViens.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Usr == USER, cancellationToken);
        if (employee is null)
            return JsonObject(new[] { new { ROOT = "11- Tên đăng nhập không đúng." } });
        if (employee.Pas != PASSWORD)
            return JsonObject(new[] { new { ROOT = "12- Mật khẩu không đúng." } });
        return JsonObject(new[] { new { ROOT = "00- OK", MA_BIEN_DOC = employee.MaNhanVien, MA_XI_NGHIEP = employee.MaChiNhanh } });
    }

    public async Task<ContentResult> DANGNHAPTHEOMANLD(string? MA_KHACH_HANG, string? PASS, string? SO_DIEN_THOAI, CancellationToken cancellationToken)
    {
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(15));

        var client = _httpClientFactory.CreateClient();
        using var response = await client.PostAsJsonAsync(GetMeterReaderByCustomerUrl, new
        {
            ma_khach_hang = MA_KHACH_HANG,
            pass = PASS,
            so_dien_thoai = SO_DIEN_THOAI
        }, timeout.Token);

        if (!response.IsSuccessStatusCode)
            return JsonObject(new[] { new { ROOT = "16- Dữ liệu không tìm thấy." } });

        var responseText = await response.Content.ReadAsStringAsync(timeout.Token);
        var meterReader = ExtractMeterReaderCode(responseText);
        if (string.IsNullOrWhiteSpace(meterReader))
            return JsonObject(new[] { new { ROOT = "16- Dữ liệu không tìm thấy." } });

        var employee = await _context.DmNhanViens.AsNoTracking()
            .FirstOrDefaultAsync(x => x.MaNhanVien == meterReader, cancellationToken);
        if (employee is null)
            return JsonObject(new[] { new { ROOT = "11- Tên đăng nhập không đúng." } });

        return JsonObject(new[] { new { ROOT = "00- OK", MA_BIEN_DOC = employee.MaNhanVien, MA_XI_NGHIEP = employee.MaChiNhanh } });
    }

    public async Task<ContentResult> P_011_DANG_NHAP_DOI_MAT_KHAU(string? MA_BIEN_DOC, string? PASSWORD_OLD, string? PASSWORD_NEW1, string? PASSWORD_NEW2, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(PASSWORD_NEW1) && string.IsNullOrEmpty(PASSWORD_NEW2))
            return JsonObject(new[] { new { ROOT = "14- Dữ liệu không hợp lệ." } });
        if (PASSWORD_NEW1 != PASSWORD_NEW2)
            return JsonObject(new[] { new { ROOT = "15- Mật khẩu mới không khớp. Vui lòng thử lại!" } });
        var affectedRows = await _context.DmNhanViens
            .Where(x => x.MaNhanVien == MA_BIEN_DOC && x.Pas == PASSWORD_OLD)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.Pas, PASSWORD_NEW1), cancellationToken);
        if (affectedRows == 0)
            return JsonObject(new[] { new { ROOT = "12- Mật khẩu không đúng." } });
        return JsonObject(new[] { new { ROOT = "00- OK" } });
    }

    public async Task<ContentResult> P_012_LAY_GT_CANH_BAO(string? MA_BIEN_DOC, string? MA_XI_NGHIEP, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var value = await _context.DmNhanViens.AsNoTracking()
            .Where(x => x.MaNhanVien == MA_BIEN_DOC)
            .Select(x => new { ROOT = "00- OK", x.CanhBaoPt, x.CanhBaoM3, x.CanhBaoGiamPt, x.CanhBaoGiamM3 })
            .FirstOrDefaultAsync(cancellationToken);
        return JsonObject(new[] { value ?? new { ROOT = "10- Tài khoản hoặc mật khẩu không đúng.", CanhBaoPt = (decimal?)null, CanhBaoM3 = (decimal?)null, CanhBaoGiamPt = (decimal?)null, CanhBaoGiamM3 = (decimal?)null } });
    }

    public async Task<ContentResult> P_013_LAY_PHIEN_BAN_APP(string? MA_BIEN_DOC, string? MA_XI_NGHIEP, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var value = await _context.AppDhUpdates.AsNoTracking()
            .Select(x => new { ROOT = "00- OK", x.BanCapNhat, x.DuongDan, x.NgayCapNhat })
            .FirstOrDefaultAsync(cancellationToken);
        return JsonObject(new[] { value ?? new { ROOT = "10- Không có phiên bản cập nhật.", BanCapNhat = (string?)null, DuongDan = (string?)null, NgayCapNhat = (DateTime?)null } });
    }

    public async Task<ContentResult> P_01_DANG_NHAP_LUU_TOKEN(string? TOKEN, string? VER_CODE, string? MA_BIEN_DOC, string? MA_XI_NGHIEP, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var affectedRows = await _context.DmNhanViens
            .Where(x => x.MaNhanVien == MA_BIEN_DOC && x.MaChiNhanh == MA_XI_NGHIEP)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.Token, TOKEN)
                .SetProperty(x => x.VerCode, VER_CODE)
                .SetProperty(x => x.LogDate, DateTime.Now), cancellationToken);
        if (affectedRows == 0)
            return JsonObject(new[] { new { ROOT = "16- Dữ liệu không tìm thấy." } });
        return JsonObject(new[] { new { ROOT = "00- OK" } });
    }

    public Task<ContentResult> P_021_LAY_DS_SO_DOC(string? MA_BIEN_DOC, string? MA_XI_NGHIEP, string? THANG, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        GetMeterBooks(MA_BIEN_DOC, THANG, false, true, cancellationToken);

    public Task<ContentResult> P_0211_LAY_DS_SO_DOC_BS(string? MA_BIEN_DOC, string? MA_XI_NGHIEP, string? THANG, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        GetMeterBooks(MA_BIEN_DOC, THANG, true, false, cancellationToken);

    public Task<ContentResult> P_0212_LAY_DS_SO_DOC_TRA_CUU(string? MA_BIEN_DOC, string? MA_XI_NGHIEP, string? THANG, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        GetMeterBooks(MA_BIEN_DOC, THANG, false, false, cancellationToken);

    public async Task<ContentResult> P_022_NHAN_SO_DOC(string? DANH_SACH_MA_SO_DOC, string? MA_BIEN_DOC, string? THANG, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        await _context.AppDhChiSos
            .Where(x => x.NgayEbillNhanKhoa == null
                        && x.NgayEbillNapBill == null
                        && x.NgayBdNhanKhoa == null
                        && x.MaBienDoc == MA_BIEN_DOC
                        && x.Thang == THANG
                        && x.MaSoDoc == DANH_SACH_MA_SO_DOC)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.NgayBdNhanKhoa, DateTime.Now), cancellationToken);
        return JsonObject(new[] { new { ROOT = "00- OK" } });
    }

    public Task<ContentResult> P_03_LAY_DS_KHACH_HANG(string? ID_DOC_opt, string? STT_SO_DOC_opt, string? MA_KH_opt, string? TEN_KH_opt, string? DIA_CHI_DH_opt, string? PHONE_KH_opt, string? MA_SO_DOC, string? MA_BIEN_DOC, string? THANG, string? KIEU_LOC, string? SAP_XEP_THEO, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        GetCustomersForReading(ID_DOC_opt, STT_SO_DOC_opt, MA_KH_opt, TEN_KH_opt, DIA_CHI_DH_opt, PHONE_KH_opt, MA_SO_DOC, MA_BIEN_DOC, THANG, KIEU_LOC, SAP_XEP_THEO, CustomerReadingMode.Normal, cancellationToken);

    public Task<ContentResult> P_03_LAY_DS_KHACH_HANG_SUB(string? ID_DOC_opt, string? STT_SO_DOC_opt, string? MA_KH_opt, string? TEN_KH_opt, string? DIA_CHI_DH_opt, string? PHONE_KH_opt, string? MA_SO_DOC, string? MA_BIEN_DOC, string? THANG, string? KIEU_LOC, string? SAP_XEP_THEO, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        GetCustomersForReading(ID_DOC_opt, STT_SO_DOC_opt, MA_KH_opt, TEN_KH_opt, DIA_CHI_DH_opt, PHONE_KH_opt, MA_SO_DOC, MA_BIEN_DOC, THANG, KIEU_LOC, SAP_XEP_THEO, CustomerReadingMode.Sub, cancellationToken);

    public Task<ContentResult> P_03_LAY_DS_KHACH_HANG_SUB_BS(string? ID_DOC_opt, string? STT_SO_DOC_opt, string? MA_KH_opt, string? TEN_KH_opt, string? DIA_CHI_DH_opt, string? PHONE_KH_opt, string? MA_SO_DOC, string? MA_BIEN_DOC, string? THANG, string? KIEU_LOC, string? SAP_XEP_THEO, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        GetCustomersForReading(ID_DOC_opt, STT_SO_DOC_opt, MA_KH_opt, TEN_KH_opt, DIA_CHI_DH_opt, PHONE_KH_opt, MA_SO_DOC, MA_BIEN_DOC, THANG, KIEU_LOC, SAP_XEP_THEO, CustomerReadingMode.SubBs, cancellationToken);

    public Task<ContentResult> P_0313_LAY_DS_KHACH_HANG_TRA_CUU(string? MA_SO_DOC, string? MA_BIEN_DOC, string? MA_XI_NGHIEP, string? THANG, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        GetCustomersForReading(null, null, null, null, null, null, MA_SO_DOC, MA_BIEN_DOC, THANG, "0", null, CustomerReadingMode.TraCuu, cancellationToken);

    public async Task<ContentResult> P_043_LO_TRINH_DI_DOC_MAP(
    string? MA_SO_DOC,
    string? MA_BIEN_DOC,
    string? MA_XI_NGHIEP,
    string? THANG,
    string? SO_IMEI,
    string? PASSWORD_K,
    CancellationToken cancellationToken)
    {
        // MA_XI_NGHIEP, SO_IMEI, PASSWORD_K:
        // vẫn giữ parameter để tương thích API cũ,
        // nhưng REST mới không dùng để kiểm tra.

        var data = await (
            from reading in _context.AppDhChiSos.AsNoTracking()

            join customerTemp in _context.ThongTinKhs.AsNoTracking()
                on reading.MaKhachHang equals customerTemp.MaKhachHang
                into customers

            from customer in customers.DefaultIfEmpty()

            where
                reading.MaBienDoc == MA_BIEN_DOC &&
                reading.MaSoDoc == MA_SO_DOC &&
                reading.Thang == THANG &&

                // VB:
                // THONG_TIN_KH.NGAY_THANH_LY_HD IS NULL
                customer.NgayThanhLyHd == null

            // VB:
            // ORDER BY NGAY_GHI_THUC_TE
            // = APP_DH_CHI_SO.NGAY_DOC_TUNG_DH
            orderby reading.NgayDocTungDh

            select new
            {
                reading.IdDcs,

                // Lấy đúng từ THONG_TIN_KH như VB
                MA_KHACH_HANG = customer.MaKhachHang,

                TEN_KHACH_HANG = customer.TenKhachHang,

                DIA_CHI_DONG_HO = customer.DiaChiDongHo,

                NGAY_GHI_THUC_TE = reading.NgayDocTungDh,

                reading.TongSl,
                reading.ChiSoMoi,
                reading.ViTriDoc,
                reading.ViTriDocCu
            })
            .ToListAsync(cancellationToken);


        if (data.Count == 0)
        {
            return JsonObject(new object[]
            {
            new
            {
                ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!"
            }
            });
        }


        var rows = data
            .Select(x => new
            {
                ROOT = "00- OK",

                ID_DOC = x.IdDcs,

                MA_KHACH_HANG = x.MA_KHACH_HANG,

                TEN_KHACH_HANG = x.TEN_KHACH_HANG,

                DIA_CHI_DONG_HO = x.DIA_CHI_DONG_HO,

                TINH_TRANG_CS =
                    x.NGAY_GHI_THUC_TE.HasValue
                        ? "Đã ghi"
                        : "Chưa ghi",

                TONG_SL =
                    x.TongSl == null
                        ? 0
                        : Convert.ToInt32(x.TongSl),

                CHI_SO_MOI = x.ChiSoMoi,

                VI_TRI_DOC = x.ViTriDoc,

                VI_TRI_DOC_CU = x.ViTriDocCu,

                NGAY_DOC_THUC_TE =
                    x.NGAY_GHI_THUC_TE.HasValue
                        ? x.NGAY_GHI_THUC_TE.Value.ToString(
                            "dd/MM/yyyy HH:mm:ss",
                            CultureInfo.InvariantCulture)
                        : string.Empty
            })
            .Cast<object>()
            .ToArray();


        return JsonObject(rows);
    }

    public async Task<ContentResult> P_044_LUU_TEN_FILE_ANH(string? ID_DONG_HO, string? TEN_FILE_ANH, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var fileName = (TEN_FILE_ANH ?? string.Empty).Replace(".jpg", "", StringComparison.OrdinalIgnoreCase) + ".jpg";
        var affectedRows = await _context.AppDhChiSos
            .Where(x => x.IdDcs == ID_DONG_HO)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.TenFileAnh, fileName), cancellationToken);
        if (affectedRows == 0) return JsonObject(new[] { new { ROOT = "16- Dữ liệu không tìm thấy." } });
        return JsonObject(new[] { new { ROOT = "00- OK" } });
    }

    public async Task<ContentResult> P_032_LAY_SL_BINH_QUAN_3T(string? MA_KHACH_HANG, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var today = DateTime.Today;
        var value = await _context.ChiSoDhSubs.AsNoTracking()
            .Where(x => x.MaKhachHang == MA_KHACH_HANG
                        && x.NgayDinhKy >= today.AddDays(-360)
                        && x.NgayDinhKy <= today
                        && x.Sltb3t > 0)
            .OrderByDescending(x => x.IdCs)
            .Select(x => x.Sltb3t)
            .FirstOrDefaultAsync(cancellationToken);
        return JsonObject(new[]
        {
            value is null
                ? new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!", SL_BQ3T = (decimal?)null }
                : new { ROOT = "00- OK", SL_BQ3T = value }
        });
    }

    public Task<ContentResult> P_041_NHAP_XUAT_CS_LE_ONLINE(string? ID_DONG_HO, string? MA_TINH_TRANG_DH, string? QUA_VONG, string? LOAI_CHI_SO, string? NGAY_DOC_CS, string? CHI_SO_MOI, string? SAN_LUONG_TT, string? TONG_SL, string? CONG_CHI_SO, string? SAN_LUONG_DUNG_IT, string? MA_GHI_CHU, string? GHI_CHU, string? VI_TRI_DOC, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        SaveMeterReading(ID_DONG_HO, MA_TINH_TRANG_DH, QUA_VONG, LOAI_CHI_SO, NGAY_DOC_CS, CHI_SO_MOI, SAN_LUONG_TT, TONG_SL, CONG_CHI_SO, SAN_LUONG_DUNG_IT, MA_GHI_CHU, GHI_CHU, VI_TRI_DOC, null, null, null, null, MA_BIEN_DOC, cancellationToken);

    public async Task<ContentResult> P_045_CANH_BAO_SAN_LUONG_LON(
    string? ID_DONG_HO,
    string? MA_KHACH_HANG,
    string? MA_BIEN_DOC,
    long TONG_SL,
    string? SO_IMEI,
    string? PASSWORD_K,
    CancellationToken cancellationToken)
    {
        try
        {
            // ============================================================
            // NGƯỠNG - GIỮ NGUYÊN VB GỐC
            // ============================================================

            const decimal TG_tyLeTang = 30m;
            const long TG_m3Tang = 50L;

            const decimal TG_tyLeGiam = 30m;
            const long TG_m3Giam = 50L;

            const decimal CQ_tyLeTang = 30m;
            const long CQ_m3Tang = 100L;

            const decimal CQ_tyLeGiam = 30m;
            const long CQ_m3Giam = 100L;


            // ============================================================
            // CHỈ 1 LẦN ĐI DATABASE
            //
            // VB:
            // 1. SL_TB_3THANG:
            //    MA_BIEN_DOC + ID_DCS
            //
            // 2. Cơ quan:
            //    MA_KHACH_HANG + LOAI_KHACH_HANG = "N"
            //
            // Any() sẽ dịch thành EXISTS.
            // ============================================================

            var info = await _context.AppDhChiSos
                .AsNoTracking()
                .Where(x =>
                    x.MaBienDoc == MA_BIEN_DOC &&
                    x.IdDcs == ID_DONG_HO)
                .Select(x => new
                {
                    x.SlTb3thang,

                    LA_CO_QUAN = _context.ThongTinKhs
                        .Any(k =>
                            k.MaKhachHang == MA_KHACH_HANG &&
                            k.LoaiKhachHang == "N")
                })
                .FirstOrDefaultAsync(cancellationToken);


            // ============================================================
            // VB:
            //
            // Val(GetValueField(...))
            // Nếu không có => 0
            //
            // If slTB3T <= 0 Then Return ""
            // ============================================================

            if (info == null)
                return TextResult("");

            long slTB3T = Convert.ToInt64(
                info.SlTb3thang ?? 0);

            if (slTB3T <= 0)
                return TextResult("");


            // ============================================================
            // ĐÚNG VB:
            //
            // loaiKH = "N" => cơ quan
            // ============================================================

            bool laCoQuan = info.LA_CO_QUAN;


            decimal nguongTangPT =
                laCoQuan
                    ? CQ_tyLeTang
                    : TG_tyLeTang;

            long nguongTangM3 =
                laCoQuan
                    ? CQ_m3Tang
                    : TG_m3Tang;

            decimal nguongGiamPT =
                laCoQuan
                    ? CQ_tyLeGiam
                    : TG_tyLeGiam;

            long nguongGiamM3 =
                laCoQuan
                    ? CQ_m3Giam
                    : TG_m3Giam;


            string m = "";


            // ============================================================
            // KIỂM TRA TĂNG - GIỐNG VB
            // ============================================================

            if (TONG_SL > slTB3T)
            {
                long tangM3 =
                    TONG_SL - slTB3T;

                decimal tangPT =
                    Math.Round(
                        tangM3 * 100m / slTB3T,
                        2);

                if (tangPT >= nguongTangPT &&
                    tangM3 >= nguongTangM3)
                {
                    m =
                        "Cảnh báo sản lượng TĂNG hơn " +
                        nguongTangPT +
                        "% và hơn " +
                        nguongTangM3 +
                        "m3 (so với BQ3T)." +
                        Environment.NewLine +
                        "Tỷ lệ thực tế tăng " +
                        tangPT.ToString("N2") +
                        "%, tương ứng tăng " +
                        tangM3.ToString("N0") +
                        "m3.";
                }
            }


            // ============================================================
            // KIỂM TRA GIẢM - GIỐNG VB
            // ============================================================

            if (TONG_SL < slTB3T)
            {
                long giamM3 =
                    slTB3T - TONG_SL;

                decimal giamPT =
                    Math.Round(
                        giamM3 * 100m / slTB3T,
                        2);

                if (giamPT >= nguongGiamPT &&
                    giamM3 >= nguongGiamM3)
                {
                    m =
                        "Cảnh báo sản lượng GIẢM hơn " +
                        nguongGiamPT +
                        "% và hơn " +
                        nguongGiamM3 +
                        "m3 (so với BQ3T)." +
                        Environment.NewLine +
                        "Tỷ lệ thực tế giảm " +
                        giamPT.ToString("N2") +
                        "%, tương ứng giảm " +
                        giamM3.ToString("N0") +
                        "m3.";
                }
            }


            // VB:
            // Return m
            return TextResult(m);
        }
        catch (Exception ex)
        {
            return TextResult(
                $"99- Lỗi không xác định ({ex.Message})");
        }


        ContentResult TextResult(string value)
        {
            return new ContentResult
            {
                Content = value,
                ContentType = "text/plain; charset=utf-8",
                StatusCode = 200
            };
        }
    }

    public async Task<ContentResult> P_045_LUU_XEP_SO_DOC(string? ID_DONG_HO, string? MA_KHACH_HANG, string? STT_CU, string? STT_MOI, string? MA_SO_DOC, string? MA_BIEN_DOC, string? THANG, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        if (!decimal.TryParse(STT_MOI, out var newIndex) || newIndex == 0 || STT_MOI == STT_CU)
            return JsonObject(new[] { new { ROOT = "14- Dữ liệu không hợp lệ." } });
        var affectedRows = await _context.AppDhChiSos
            .Where(x => x.IdDcs == ID_DONG_HO && x.MaKhachHang == MA_KHACH_HANG && x.MaSoDoc == MA_SO_DOC && x.Thang == THANG)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.SttSoDocMoi, newIndex), cancellationToken);
        if (affectedRows == 0) return JsonObject(new[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } });
        return JsonObject(new[] { new { ROOT = "00- OK" } });
    }

    public Task<ContentResult> P_041_NHAP_XUAT_CS_LE_ONLINE_SUB(string? ID_DONG_HO, string? MA_TINH_TRANG_DH, string? QUA_VONG, string? LOAI_CHI_SO, string? NGAY_DOC_CS, string? CHI_SO_MOI, string? SAN_LUONG_TT, string? TONG_SL, string? CONG_CHI_SO, string? SAN_LUONG_DUNG_IT, string? MA_GHI_CHU, string? GHI_CHU, string? VI_TRI_DOC, string? MA_KHACH_HANG, string? MA_SO_DOC, string? THANG, string? MA_XI_NGHIEP, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        SaveMeterReading(ID_DONG_HO, MA_TINH_TRANG_DH, QUA_VONG, LOAI_CHI_SO, NGAY_DOC_CS, CHI_SO_MOI, SAN_LUONG_TT, TONG_SL, CONG_CHI_SO, SAN_LUONG_DUNG_IT, MA_GHI_CHU, GHI_CHU, VI_TRI_DOC, MA_KHACH_HANG, MA_SO_DOC, THANG, MA_XI_NGHIEP, MA_BIEN_DOC, cancellationToken);

    public async Task<ContentResult> P_051_KIEM_TRA_BAN_GIAO_SD(string? MA_SO_DOC, string? MA_BIEN_DOC, string? MA_XI_NGHIEP, string? THANG, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var handedOver = await _context.AppDhChiSos.AsNoTracking().AnyAsync(x => x.Thang == THANG && x.MaChiNhanh == MA_XI_NGHIEP && x.MaBienDoc == MA_BIEN_DOC && x.MaSoDoc == MA_SO_DOC && x.NgayBienDocBg != null, cancellationToken);
        return JsonObject(new[] { new { ROOT = handedOver ? "01- TRUE" : "00- FALSE" } });
    }

    public async Task<ContentResult> P_05_BD_BAN_GIAO_CS_XONG(string? DANH_SACH_MA_SO_DOC, string? MA_BIEN_DOC, string? THANG, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var query = _context.AppDhChiSos
            .Where(x =>
                x.NgayEbillNhanKhoa == null &&
                x.NgayEbillNapBill == null &&
                x.NgayBdNhanKhoa != null &&
                x.MaBienDoc == MA_BIEN_DOC &&
                x.Thang == THANG &&
                x.MaSoDoc == DANH_SACH_MA_SO_DOC);

        var hasUnreadMeter = await query
            .AsNoTracking()
            .AnyAsync(x => x.NgayDocTungDh == null, cancellationToken);

        if (hasUnreadMeter)
            return JsonObject(new[] { new { ROOT = "18- Chưa đọc xong. Vui lòng kiểm tra lại!" } });

        await query.ExecuteUpdateAsync(
            setters => setters.SetProperty(x => x.NgayBienDocBg, DateTime.Now),
            cancellationToken);

        return JsonObject(new[] { new { ROOT = "00- OK" } });
    }

    public async Task<ContentResult> P_81_LAY_TT_KHACH_HANG(string? MA_KHACH_HANG, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var maKh = MA_KHACH_HANG ?? string.Empty;
        if (!string.IsNullOrEmpty(maKh) && maKh.Length != 9)
            return JsonObject(new[] { new { ROOT = $"15- Do dai du lieu khong hop le (MKH: {maKh})" } });

        var totalWatch = Stopwatch.StartNew();
        var stepWatch = Stopwatch.StartNew();

        var customer = await _context.ThongTinKhs
            .AsNoTracking()
            .Where(x => x.MaKhachHang == maKh && x.NgayThanhLyHd == null)
            .Select(x => new
            {
                x.MaKhachHang,
                x.TenKhachHang,
                x.DiaChiKhachHang,
                DIA_CHI_LAP_DAT = x.DiaChiDongHo,
                DIEN_THOAI = x.PhoneUt1,
                EMAIL = x.EmailUt1,
                x.SoHopDong,
                x.SoHo,
                x.SoKhau,
                x.DinhMuc,
                x.MaGia,
                x.TenGiaNuoc,
                SERIAL_DH = x.SoSerialDongHo,
                x.TenDongHo,
                x.NgayLapDat,
                x.BienDoc,
                MUC_DICH_SU_DUNG = x.HtKd,
                XN_CAP_NUOC = x.ChiNhanh
            })
            .FirstOrDefaultAsync(cancellationToken);

        var customerMs = stepWatch.ElapsedMilliseconds;
        if (customer is null)
            return JsonObject(new[] { new { ROOT = "16- Du lieu khong tim thay. Vui long kiem tra lai!" } });

        stepWatch.Restart();
        var duNo = await GetTongDuNoTheoMaKhV2Async(maKh, cancellationToken);
        var debtMs = stepWatch.ElapsedMilliseconds;

        stepWatch.Restart();
        var bq3t = await _context.ChiSoDhSubs
            .AsNoTracking()
            .Where(x =>
                x.MaKhachHang == maKh &&
                x.Sltb3t > 0)
            .OrderByDescending(x => x.IdCs)
            .Select(x => x.Sltb3t)
            .FirstOrDefaultAsync(cancellationToken);
        var averageMs = stepWatch.ElapsedMilliseconds;

        var ngayLapDat = customer.NgayLapDat?.ToString("dd/MM/yyyy") ?? string.Empty;
        var result = new[]
        {
            new
            {
                ROOT = "00- OK",
                customer.MaKhachHang,
                customer.TenKhachHang,
                customer.DiaChiKhachHang,
                customer.DIA_CHI_LAP_DAT,
                customer.DIEN_THOAI,
                customer.EMAIL,
                customer.SoHopDong,
                customer.SoHo,
                customer.SoKhau,
                customer.DinhMuc,
                customer.MaGia,
                customer.TenGiaNuoc,
                customer.SERIAL_DH,
                customer.TenDongHo,
                NGAY_LAP_DAT = ngayLapDat,
                customer.BienDoc,
                customer.MUC_DICH_SU_DUNG,
                customer.XN_CAP_NUOC,
                DU_NO = duNo,
                BQ3T = bq3t
            }
        };

        Console.WriteLine($"P_81 timings MA_KHACH_HANG={maKh}: KH={customerMs}ms, DU_NO={debtMs}ms, BQ3T={averageMs}ms, TOTAL={totalWatch.ElapsedMilliseconds}ms");
        return JsonObject(result);
    }




    // ============================================================================
    // TÍNH TỔNG TIỀN NỢ TRONG TRƯỜNG HỢP CÓ HÓA ĐƠN ĐIỀU CHỈNH GIẢM
    //
    // VB gốc:
    // Private Function get_tong_tien_no_dieu_chinh_sub(
    //     ByVal CUST_ID As String) As Long
    //
    // LƯU Ý QUAN TRỌNG:
    //
    // VB gốc dùng:
    //
    // TONG_TIEN - (TONG_THANH_TOAN_BILL - TONG_TIEN_GIAM_TRU)
    //
    // tương đương:
    //
    // TONG_TIEN - TONG_THANH_TOAN_BILL + TONG_TIEN_GIAM_TRU
    //
    // Giữ nguyên logic cũ.
    // ============================================================================

    private async Task<long> GetTongTienNoDieuChinhSubAsync(
        string maKH,
        CancellationToken cancellationToken = default)
    {
        // ========================================================================
        // 1. LẤY TẤT CẢ HÓA ĐƠN ĐIỀU CHỈNH GIẢM
        //
        // VB:
        //
        // SELECT TONG_TIEN, SO_HOA_DON_THAY_THE
        // FROM CONG_NO
        // WHERE MA_KHACH_HANG = CUST_ID
        // AND TONG_TIEN < 0
        // AND TRANG_THAI = 1
        // AND MA_DON_VI = 0
        // AND LOAI_HOA_DON = 5
        // ORDER BY NGAY_HD_PHAT_HANH DESC
        // ========================================================================

        var dsDieuChinhGiam = await _context.CongNos
            .AsNoTracking()
            .Where(x =>
                x.MaKhachHang == maKH &&
                x.TongTien < 0 &&
                x.TrangThai == 1 &&
                x.MaDonVi == 0 &&
                x.LoaiHoaDon == 5)
            .OrderByDescending(x => x.NgayHdPhatHanh)
            .Select(x => new
            {
                x.TongTien,
                x.SoHoaDonThayThe
            })
            .ToListAsync(cancellationToken);


        // Không có điều chỉnh giảm
        if (dsDieuChinhGiam.Count == 0)
            return 0;


        // ========================================================================
        // 2. VB lấy dòng đầu tiên sau ORDER BY NGAY_HD_PHAT_HANH DESC
        //
        // shd_can_dc_giam = SO_HOA_DON_THAY_THE
        // tong_tien_dc_GIAM_GOC = Abs(TONG_TIEN)
        // ========================================================================

        var dieuChinhGiamDau = dsDieuChinhGiam[0];

        var shdCanDcGiam =
            dieuChinhGiamDau.SoHoaDonThayThe ?? string.Empty;

        var tongTienDcGiamGoc =
            Math.Abs(dieuChinhGiamDau.TongTien ?? 0m);


        if (string.IsNullOrWhiteSpace(shdCanDcGiam))
            return 0;


        // ========================================================================
        // 3. TẠO DANH SÁCH TẤT CẢ SO_HOA_DON_THAY_THE
        //
        // VB tạo:
        //
        // 'HD1', 'HD2', 'HD3'
        //
        // để dùng:
        //
        // SO_HOA_DON NOT IN (...)
        // ========================================================================

        var dsSoHoaDonThayThe = dsDieuChinhGiam
            .Where(x =>
                !string.IsNullOrWhiteSpace(x.SoHoaDonThayThe))
            .Select(x =>
                x.SoHoaDonThayThe!)
            .ToList();


        // ========================================================================
        // 4. LẤY HÓA ĐƠN GỐC CẦN ĐIỀU CHỈNH
        //
        // VB:
        //
        // SELECT
        //     NGAY_HD_PHAT_HANH,
        //     TONG_THANH_TOAN_BILL,
        //     TONG_TIEN
        //
        // FROM CONG_NO
        //
        // WHERE SO_HOA_DON = shd_can_dc_giam
        // ========================================================================

        var hoaDonGoc = await _context.CongNos
            .AsNoTracking()
            .Where(x =>
                x.SoHoaDon == shdCanDcGiam)
            .Select(x => new
            {
                x.NgayHdPhatHanh,
                x.TongTien,
                x.TongThanhToanBill
            })
            .FirstOrDefaultAsync(cancellationToken);


        if (hoaDonGoc == null)
            return 0;


        // VB:
        //
        // tong_tien_can_dc_giam
        // tong_tt_can_dc_giam

        var tongTienCanDcGiam =
            hoaDonGoc.TongTien ?? 0m;

        var tongTtCanDcGiam =
            hoaDonGoc.TongThanhToanBill ?? 0m;


        // ========================================================================
        // 5. NGÀY HÓA ĐƠN GỐC LÙI 300 NGÀY
        //
        // VB:
        //
        // ngayPHHD_can_dc_giam =
        // DateAdd("d", -300, NGAY_HD_PHAT_HANH)
        //
        // Sau đó:
        //
        // BETWEEN ngày_lùi_300 AND Now.Date
        // ========================================================================

        if (!hoaDonGoc.NgayHdPhatHanh.HasValue)
            return 0;


        var tuNgay = hoaDonGoc.NgayHdPhatHanh
            .Value
            .Date
            .AddDays(-300);


        var denNgay = DateTime.Today;


        // ========================================================================
        // 6. SQL_MAIN GỐC
        //
        // SELECT
        //
        // (
        //     TONG_TIEN -
        //     (
        //         TONG_THANH_TOAN_BILL -
        //         TONG_TIEN_GIAM_TRU
        //     )
        // ) AS tien
        //
        // FROM CONG_NO
        //
        // WHERE MA_KHACH_HANG = CUST_ID
        // AND TRANG_THAI = 1
        // AND MA_DON_VI = 0
        // ========================================================================

        var query = _context.CongNos
            .AsNoTracking()
            .Where(x =>
                x.MaKhachHang == maKH &&
                x.TrangThai == 1 &&
                x.MaDonVi == 0);


        // ========================================================================
        // CASE 1:
        //
        // HÓA ĐƠN GỐC ĐÃ THANH TOÁN HẾT
        //
        // VB:
        //
        // If tong_tien_can_dc_giam = tong_tt_can_dc_giam Then
        // ========================================================================

        if (tongTienCanDcGiam == tongTtCanDcGiam)
        {
            // VB:
            //
            // If tong_tt_can_dc_giam >= tong_tien_dc_GIAM_GOC Then
            //
            // Nếu không thỏa thì Sql vẫn rỗng
            // => get_tong_tien_no_dieu_chinh_sub trả 0

            if (tongTtCanDcGiam < tongTienDcGiamGoc)
                return 0;


            query = query.Where(x =>

                // AND LOAI_HOA_DON <> 5
                x.LoaiHoaDon != 5 &&


                // Giữ semantics Oracle:
                // nếu một trong các trường NULL
                // thì biểu thức so sánh không thỏa

                x.TongTien.HasValue &&
                x.TongThanhToanBill.HasValue &&
                x.TongTienGiamTru.HasValue &&


                // AND TONG_TIEN >
                // (
                //     TONG_THANH_TOAN_BILL +
                //     TONG_TIEN_GIAM_TRU
                // )

                x.TongTien.Value >
                    x.TongThanhToanBill.Value +
                    x.TongTienGiamTru.Value &&


                // AND NGAY_HD_PHAT_HANH BETWEEN ...

                x.NgayHdPhatHanh.HasValue &&
                x.NgayHdPhatHanh.Value >= tuNgay &&
                x.NgayHdPhatHanh.Value <= denNgay
            );
        }


        // ========================================================================
        // CASE 2:
        //
        // HÓA ĐƠN GỐC CHƯA THANH TOÁN
        // HOẶC CHỈ THANH TOÁN MỘT PHẦN
        //
        // VB:
        //
        // ElseIf tong_tien_can_dc_giam > tong_tt_can_dc_giam Then
        // ========================================================================

        else if (tongTienCanDcGiam > tongTtCanDcGiam)
        {
            // ====================================================================
            // CASE 2.1
            //
            // TONG_TIEN_GỐC
            // =
            // TIỀN ĐIỀU CHỈNH GIẢM
            // +
            // TIỀN ĐÃ THANH TOÁN
            //
            // VB:
            //
            // If tong_tien_can_dc_giam =
            //    (tong_tien_dc_GIAM_GOC + tong_tt_can_dc_giam)
            // ====================================================================

            if (tongTienCanDcGiam ==
                tongTienDcGiamGoc + tongTtCanDcGiam)
            {
                query = query.Where(x =>

                    // AND LOAI_HOA_DON <> 5

                    x.LoaiHoaDon != 5 &&


                    x.TongTien.HasValue &&
                    x.TongThanhToanBill.HasValue &&
                    x.TongTienGiamTru.HasValue &&


                    // AND TONG_TIEN >
                    // (
                    //     TONG_THANH_TOAN_BILL +
                    //     TONG_TIEN_GIAM_TRU
                    // )

                    x.TongTien.Value >
                        x.TongThanhToanBill.Value +
                        x.TongTienGiamTru.Value &&


                    // VB:
                    //
                    // AND SO_HOA_DON NOT IN (...)
                    //
                    // Dùng danh sách toàn bộ
                    // SO_HOA_DON_THAY_THE

                    !dsSoHoaDonThayThe.Contains(
                        x.SoHoaDon ?? string.Empty) &&


                    // khoảng ngày

                    x.NgayHdPhatHanh.HasValue &&
                    x.NgayHdPhatHanh.Value >= tuNgay &&
                    x.NgayHdPhatHanh.Value <= denNgay
                );
            }


            // ====================================================================
            // CASE 2.2
            //
            // Chưa hết nợ sau điều chỉnh
            //
            // VB:
            //
            // AND TONG_TIEN <>
            // (
            //     TONG_THANH_TOAN_BILL +
            //     TONG_TIEN_GIAM_TRU
            // )
            //
            // QUAN TRỌNG:
            //
            // Nhánh VB này KHÔNG có:
            //
            // LOAI_HOA_DON <> 5
            //
            // nên C# cũng không thêm.
            // ====================================================================

            else
            {
                query = query.Where(x =>

                    x.TongTien.HasValue &&
                    x.TongThanhToanBill.HasValue &&
                    x.TongTienGiamTru.HasValue &&


                    x.TongTien.Value !=
                        x.TongThanhToanBill.Value +
                        x.TongTienGiamTru.Value &&


                    x.NgayHdPhatHanh.HasValue &&
                    x.NgayHdPhatHanh.Value >= tuNgay &&
                    x.NgayHdPhatHanh.Value <= denNgay
                );
            }
        }


        // ========================================================================
        // Nếu:
        //
        // tongTienCanDcGiam < tongTtCanDcGiam
        //
        // VB không thêm điều kiện vào Sql.
        //
        // Sql = ""
        //
        // => hàm Sub không query
        // => trả 0
        // ========================================================================

        else
        {
            return 0;
        }


        // ========================================================================
        // 7. TÍNH TỔNG NỢ
        //
        // GIỮ ĐÚNG CÔNG THỨC VB:
        //
        // TONG_TIEN -
        // (
        //     TONG_THANH_TOAN_BILL -
        //     TONG_TIEN_GIAM_TRU
        // )
        //
        // KHÔNG đổi thành:
        //
        // TONG_TIEN
        // - TONG_THANH_TOAN_BILL
        // - TONG_TIEN_GIAM_TRU
        //
        // vì sẽ khác hệ thống cũ.
        // ========================================================================

        var tongNo = await query
            .Where(x =>
                x.TongTien.HasValue &&
                x.TongThanhToanBill.HasValue &&
                x.TongTienGiamTru.HasValue)
            .Select(x =>
                (decimal?)(
                    x.TongTien.Value -
                    (
                        x.TongThanhToanBill.Value -
                        x.TongTienGiamTru.Value
                    )
                ))
            .SumAsync(cancellationToken);


        return Convert.ToInt64(
            tongNo ?? 0m);
    }


    // ============================================================================
    // TÍNH TỔNG TIỀN NỢ
    //
    // VB gốc:
    //
    // Private Function get_tong_tien_no(
    //     ByVal CUST_ID As String) As Long
    // ============================================================================

    private async Task<long> GetTongTienNoAsync(
        string maKH,
        CancellationToken cancellationToken = default)
    {
        // ========================================================================
        // KIỂM TRA CÓ ĐIỀU CHỈNH GIẢM KHÔNG
        // ========================================================================

        var coDieuChinhGiam = await KiemTraDcGiamAsync(
            maKH,
            cancellationToken);


        // ========================================================================
        // CÓ ĐIỀU CHỈNH GIẢM
        // ========================================================================

        if (coDieuChinhGiam)
        {
            return await GetTongTienNoDieuChinhSubAsync(
                maKH,
                cancellationToken);
        }


        // ========================================================================
        // KHÔNG CÓ ĐIỀU CHỈNH GIẢM
        //
        // VB:
        //
        // SELECT
        //
        // SUM(
        //     TONG_TIEN
        //     - TONG_THANH_TOAN_BILL
        //     - TONG_TIEN_GIAM_TRU
        // ) AS tienno
        //
        // FROM CONG_NO
        //
        // WHERE MA_KHACH_HANG = CUST_ID
        //
        // AND TRANG_THAI = 1
        //
        // AND MA_DON_VI = 0
        //
        // AND TONG_TIEN >
        //     (
        //         TONG_THANH_TOAN_BILL +
        //         TONG_TIEN_GIAM_TRU
        //     )
        //
        // AND LOAI_HOA_DON <> 5
        // ========================================================================

        var tongNo = await _context.CongNos
            .AsNoTracking()
            .Where(x =>

                x.MaKhachHang == maKH &&

                x.TrangThai == 1 &&

                x.MaDonVi == 0 &&

                x.LoaiHoaDon != 5 &&


                // Giữ semantics SQL Oracle
                // thay vì tự COALESCE NULL thành 0.

                x.TongTien.HasValue &&
                x.TongThanhToanBill.HasValue &&
                x.TongTienGiamTru.HasValue &&


                x.TongTien.Value >
                    x.TongThanhToanBill.Value +
                    x.TongTienGiamTru.Value
            )
            .Select(x =>
                (decimal?)(
                    x.TongTien.Value
                    - x.TongThanhToanBill.Value
                    - x.TongTienGiamTru.Value
                ))
            .SumAsync(cancellationToken);


        return Convert.ToInt64(
            tongNo ?? 0m);
    }


    // ============================================================================
    // GET TỔNG DƯ NỢ THEO MÃ KHÁCH HÀNG V2
    //
    // VB gốc:
    //
    // Public Function get_Tong_Du_No_Theo_maKH_V2(
    //     ByVal maKH As String) As String
    //
    // Nếu tiền nợ = 0
    // => "Hết nợ"
    //
    // LƯU Ý:
    //
    // VB dùng:
    //
    // If tien_no = 0 Then
    //
    // KHÔNG phải:
    //
    // tien_no <= 0
    // ============================================================================

    private async Task<string> GetTongDuNoTheoMaKhV2Async(
        string maKH,
        CancellationToken cancellationToken = default)
    {
        var tienNo = await GetTongTienNoAsync(
            maKH,
            cancellationToken);


        // VB:
        //
        // If tien_no = 0 Then
        //     tien_no = "Hết nợ"
        // End If

        if (tienNo == 0)
            return "Hết nợ";


        return tienNo.ToString(
            CultureInfo.InvariantCulture);
    }


    // ======================================================================
    // KIỂM TRA KHÁCH HÀNG CÓ HÓA ĐƠN ĐIỀU CHỈNH GIẢM KHÔNG
    // Convert từ: kiem_tra_dc_giam
    // ======================================================================
    private async Task<bool> KiemTraDcGiamAsync(
        string custId,
        CancellationToken cancellationToken = default)
    {
        return await _context.CongNos
            .AsNoTracking()
            .AnyAsync(x =>
                x.MaKhachHang == custId &&
                x.TongTien < 0 &&
                x.TrangThai == 1 &&
                x.MaDonVi == 0 &&
                x.LoaiHoaDon == 5,
                cancellationToken);
    }


    // ======================================================================
    // TÍNH TỔNG TIỀN NỢ KHI CÓ ĐIỀU CHỈNH GIẢM
    // Convert từ:
    // get_tong_tien_no_dieu_chinh_sub
    // +
    // get_sql_dieu_chinh_giam
    //
    // Không cần get_sql_dieu_chinh_giam nữa.
    // Toàn bộ điều kiện được chuyển thành LINQ.
    // ======================================================================
    private async Task<long> GetTongTienNoDieuChinhSubAsync(
        string custId,
        decimal? tongTienDieuChinhGiam,
        string? soHoaDonThayTheDau,
        CancellationToken cancellationToken = default)
    {
        var shdCanDcGiam = soHoaDonThayTheDau?.Trim() ?? string.Empty;
        var tongTienDcGiamGoc = Math.Abs(tongTienDieuChinhGiam ?? 0m);

        if (string.IsNullOrWhiteSpace(shdCanDcGiam))
            return 0L;

        var dsSoHoaDonThayThe = await _context.CongNos
            .AsNoTracking()
            .Where(x =>
                x.MaKhachHang == custId &&
                x.TongTien < 0 &&
                x.TrangThai == 1 &&
                x.MaDonVi == 0 &&
                x.LoaiHoaDon == 5 &&
                x.SoHoaDonThayThe != null)
            .Select(x => x.SoHoaDonThayThe!)
            .Distinct()
            .ToListAsync(cancellationToken);
// ------------------------------------------------------------------
        // 4. LẤY HÓA ĐƠN GỐC CẦN ĐIỀU CHỈNH
        //
        // VB:
        //
        // SELECT
        //      NGAY_HD_PHAT_HANH,
        //      TONG_THANH_TOAN_BILL,
        //      TONG_TIEN
        // FROM CONG_NO
        // WHERE SO_HOA_DON = shd_can_dc_giam
        // ------------------------------------------------------------------

        var hoaDonCanDc = await _context.CongNos
            .AsNoTracking()
            .Where(x => x.SoHoaDon == shdCanDcGiam)
            .Select(x => new
            {
                x.NgayHdPhatHanh,
                x.TongThanhToanBill,
                x.TongTien
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (hoaDonCanDc == null)
            return 0L;


        var tongTienCanDcGiam =
            Convert.ToDecimal(hoaDonCanDc.TongTien);

        var tongTtCanDcGiam =
            Convert.ToDecimal(hoaDonCanDc.TongThanhToanBill);


        // ------------------------------------------------------------------
        // 5. KHOẢNG NGÀY
        //
        // VB:
        //
        // ngày HĐ phát hành - 300 ngày
        // đến Now.Date
        //
        // Chú ý:
        // Now.Date là 00:00:00 của ngày hiện tại.
        // Giữ nguyên để giống VB.
        // ------------------------------------------------------------------

        if (hoaDonCanDc.NgayHdPhatHanh == null)
            return 0L;

        var ngayPhatHanh =
            Convert.ToDateTime(hoaDonCanDc.NgayHdPhatHanh);

        var tuNgay =
            ngayPhatHanh.Date.AddDays(-300);

        var denNgay =
            DateTime.Today;


        // ------------------------------------------------------------------
        // 6. QUERY CHÍNH
        //
        // VB:
        //
        // SELECT
        //   (
        //      TONG_TIEN
        //      - (TONG_THANH_TOAN_BILL - TONG_TIEN_GIAM_TRU)
        //   ) AS tien
        //
        // FROM CONG_NO
        //
        // WHERE
        //      MA_KHACH_HANG = custId
        //      AND TRANG_THAI = 1
        //      AND MA_DON_VI = 0
        //
        // ------------------------------------------------------------------

        var query = _context.CongNos
            .AsNoTracking()
            .Where(x =>
                x.MaKhachHang == custId &&
                x.TrangThai == 1 &&
                x.MaDonVi == 0);


        // ==================================================================
        // TRƯỜNG HỢP 1
        //
        // HÓA ĐƠN CẦN ĐIỀU CHỈNH ĐÃ THANH TOÁN ĐỦ
        //
        // tong_tien_can_dc_giam = tong_tt_can_dc_giam
        // ==================================================================

        if (tongTienCanDcGiam == tongTtCanDcGiam)
        {
            // VB:
            //
            // If tong_tt_can_dc_giam >= tong_tien_dc_GIAM_GOC Then

            if (tongTtCanDcGiam >= tongTienDcGiamGoc)
            {
                query = query.Where(x =>
                    x.LoaiHoaDon == 5 &&

                    x.TongTien >
                        (
                            x.TongThanhToanBill +
                            x.TongTienGiamTru
                        ) &&

                    x.NgayHdPhatHanh >= tuNgay &&
                    x.NgayHdPhatHanh <= denNgay);
            }
            else
            {
                // VB không tạo thêm SQL trong trường hợp này.
                // get_sql_dieu_chinh_giam trả chuỗi rỗng
                // => get_tong_tien_no_dieu_chinh_sub trả 0.
                return 0L;
            }
        }

        // ==================================================================
        // TRƯỜNG HỢP 2
        //
        // HÓA ĐƠN CHƯA THANH TOÁN
        // HOẶC CHỈ THANH TOÁN MỘT PHẦN
        //
        // tong_tien_can_dc_giam > tong_tt_can_dc_giam
        // ==================================================================

        else if (tongTienCanDcGiam > tongTtCanDcGiam)
        {
            // --------------------------------------------------------------
            // Tiền điều chỉnh giảm + tiền đã thanh toán
            // đúng bằng tổng tiền hóa đơn.
            //
            // => hóa đơn cần điều chỉnh đã được chấm hết nợ.
            // --------------------------------------------------------------

            if (tongTienCanDcGiam ==
                (tongTienDcGiamGoc + tongTtCanDcGiam))
            {
                query = query.Where(x =>
                    x.LoaiHoaDon != 5 &&

                    x.TongTien >
                        (
                            x.TongThanhToanBill +
                            x.TongTienGiamTru
                        ) &&

                    !dsSoHoaDonThayThe.Contains(x.SoHoaDon) &&

                    x.NgayHdPhatHanh >= tuNgay &&
                    x.NgayHdPhatHanh <= denNgay);
            }

            // --------------------------------------------------------------
            // Chưa chấm hết hóa đơn điều chỉnh.
            //
            // VB:
            //
            // TONG_TIEN <>
            // (TONG_THANH_TOAN_BILL + TONG_TIEN_GIAM_TRU)
            // --------------------------------------------------------------

            else
            {
                query = query.Where(x =>
                    x.TongTien !=
                        (
                            x.TongThanhToanBill +
                            x.TongTienGiamTru
                        ) &&

                    x.NgayHdPhatHanh >= tuNgay &&
                    x.NgayHdPhatHanh <= denNgay);
            }
        }

        // ==================================================================
        // VB không có nhánh xử lý nếu TONG_TIEN < TONG_THANH_TOAN_BILL.
        // get_sql_dieu_chinh_giam sẽ trả chuỗi rỗng.
        // ==================================================================

        else
        {
            return 0L;
        }


        // ------------------------------------------------------------------
        // 7. TÍNH TỔNG NỢ
        //
        // CỰC KỲ QUAN TRỌNG:
        //
        // Giữ NGUYÊN công thức VB:
        //
        // TONG_TIEN - (TONG_THANH_TOAN_BILL - TONG_TIEN_GIAM_TRU)
        //
        // Tức là:
        //
        // TONG_TIEN
        // - TONG_THANH_TOAN_BILL
        // + TONG_TIEN_GIAM_TRU
        //
        // Mặc dù nhìn nghiệp vụ khá lạ nhưng không tự sửa,
        // để kết quả Entity khớp hệ thống cũ.
        // ------------------------------------------------------------------

        var tongNo = await query
            .Select(x =>
                (decimal?)(
                    x.TongTien
                    -
                    (
                        x.TongThanhToanBill
                        -
                        x.TongTienGiamTru
                    )
                ))
            .SumAsync(cancellationToken);


        return Convert.ToInt64(tongNo ?? 0m);
    }
    public async Task<ContentResult> P_82_LAY_TT_HOA_DON(string? MA_KHACH_HANG, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var fromDate = DateTime.Today.AddDays(-150);
        var rows = await (
            from invoice in _context.CongNos.AsNoTracking()
            join unit in _context.LogBankDonVis.AsNoTracking() on invoice.MaDonVi equals unit.Ma into units
            from unit in units.DefaultIfEmpty()
            where invoice.MaKhachHang == MA_KHACH_HANG && invoice.NgayHdPhatHanh >= fromDate
            orderby invoice.SoHoaDon
            select new { invoice, UNIT_NAME = unit.TenSub })
            .ToListAsync(cancellationToken);
        if (rows.Count == 0)
            return JsonObject(new[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } });
        return JsonObject(rows.Select(x =>
        {
            var paid = Math.Max(x.invoice.TongThanhToan ?? 0, x.invoice.TongThanhToanBill ?? 0);
            var status = x.invoice.TrangThai?.ToString();
            return new
            {
                ROOT = "00- OK", x.invoice.SoHoaDon, NGAY_PHAT_HANH_HD = x.invoice.NgayHdPhatHanh,
                THANG_HD = x.invoice.Thang, TONG_SAN_LUONG = x.invoice.TongSl, x.invoice.TongTien,
                NGAY_THANH_TOAN = x.invoice.NgayThanhToan ?? x.invoice.NgayThanhToanBill,
                TONG_THANH_TOAN = paid, x.invoice.TongTienGiamTru,
                HINH_THUC_TT = status == "2" ? "" : x.invoice.HinhThucTtBill switch { "C" => "Tiền mặt", "B" => "Ủy nhiệm thu", "A" => "Chuyển khoản", "S" => "Khấu trừ nội bộ", _ => x.invoice.HinhThucTtBill },
                NOI_THANH_TOAN = x.UNIT_NAME, x.invoice.SeriHoaDon,
                TRANG_THAI_HD = status switch { "0" => "Chưa phát hành", "1" => "Phát hành", "2" or "3" => "Hủy hóa đơn", "4" => "Trả dần", _ => "" },
                LOAI_HOA_DON = x.invoice.LoaiHoaDon?.ToString() switch { "1" => "Định kỳ", "2" => "Tài chính", "3" => "Truy thu", "4" => "Điều chỉnh tăng", "5" => "Điều chỉnh giảm", _ => "" },
                NGAY_HUY_HD = x.invoice.NgayHdHuy,
                TINH_TRANG_NO = status == "2" ? "Hủy hóa đơn" : paid == x.invoice.TongTien ? "Hết nợ" : paid == 0 ? "Đang nợ" : "Thu một phần",
                GHI_CHU = x.invoice.DienGiai
            };
        }));
    }

    public async Task<ContentResult> P_83_LAY_TT_CHI_SO(string? SO_HOA_DON, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await _context.ChiSoDhs.AsNoTracking().Where(x => x.SoHoaDon == SO_HOA_DON)
            .Select(x => new { x.CsDau, x.CsCuoi, x.SanLuong, x.LoaiChiSo })
            .ToListAsync(cancellationToken);
        return JsonObject(rows.Count == 0
            ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } }
            : rows.Select(x => new
            {
                ROOT = "00- OK",
                CHI_SO_CU = x.CsDau,
                CHI_SO_MOI = x.CsCuoi,
                x.SanLuong,
                LOAI_CHI_SO = x.LoaiChiSo == "0" ? "Định kỳ" : x.LoaiChiSo == "6" ? "Chốt chỉ số" : x.LoaiChiSo
            }).Cast<object>());
    }

    public async Task<ContentResult> P_84_LAY_TT_GIA_NUOC(string? SO_HOA_DON, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(SO_HOA_DON) && SO_HOA_DON.Length != 9)
            return JsonObject(new[] { new { ROOT = $"15- Độ dài dữ liệu không hợp lệ (MKH: {SO_HOA_DON})" } });

        var rows = await _context.CongNoGia.AsNoTracking()
            .Where(x => x.SoHoaDon == SO_HOA_DON)
            .Select(x => new
            {
                ROOT = "00- OK",
                SAN_LUONG = x.TongSl,
                DON_GIA = x.TienGiaCb,
                THANH_TIEN = x.ThanhTien,
                PHI = x.ThueBvmt,
                THUE = x.Vat,
                TONG_TIEN = x.TongTien,
                TEN_GIA = x.TenGia
            })
            .ToListAsync(cancellationToken);

        return JsonObject(rows.Count == 0
            ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } }
            : rows.Cast<object>());
    }

    public async Task<ContentResult> P_85_LAY_TT_GUI_SMS(string? MA_KHACH_HANG, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var fromDate = DateTime.Today.AddDays(-120);
        var rows = await (from log in _context.LogSms.AsNoTracking()
            join type in _context.DmLoaiSmsEmails.AsNoTracking() on log.LoaiSms equals type.MaLoai
            join status in _context.DmTrangThaiSms.AsNoTracking() on log.KetQua equals status.MaLoai
            where log.MaKhachHang == MA_KHACH_HANG && log.NgayGui >= fromDate
            orderby log.NgayGui descending
            select new { ROOT = "00- OK", LOAI_SMS = type.TenLoai, TRANG_THAI = status.TenLoai, log.SoDienThoai, log.NgayGui, NOI_DUNG = log.NoiDungSms }).ToListAsync(cancellationToken);
        return JsonObject(rows.Count == 0 ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy." } } : rows.Cast<object>());
    }

    public async Task<ContentResult> P_88_LAY_TT_CAT_MO_NUOC(string? MA_KHACH_HANG, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var customerCode = (MA_KHACH_HANG ?? string.Empty).PadLeft(9, '0');
        var catMoNuocRows = await _billing.Aereport
        .AsNoTracking()
        .Where(x => x.CUSTID == customerCode)
        .OrderByDescending(x => x.DATECREATE)
        .Select(x => new
        {
            x.ERPTTYPE,
            x.ERPTSTS,
            NGAY_THUC_HIEN = x.DATECREATE,
            NGUOI_THUC_HIEN = x.EXECUTER,
            LY_DO = x.RPTREASON,
            CHI_SO_NIEM = x.READSEAL,
            CHI_SO_NUOC = x.READCURR,
            GHI_CHU_1 = x.ERPTCONTENT1,
            GHI_CHU_2 = x.ERPTCONTENT2,
            TONG_TIEN_NO = x.TOTAL
        })
        .ToListAsync(cancellationToken);

        object[] response = catMoNuocRows.Count == 0
            ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } }
            : catMoNuocRows.Select(x => new
            {
                ROOT = "00- OK",
                LOAI_HO_SO = x.ERPTTYPE == "0" ? "Thông báo ngừng cấp nước - Nợ tiền nước"
                    : x.ERPTTYPE == "1" ? "Thông báo ngừng cấp nước - Chi nhánh"
                    : x.ERPTTYPE == "2" ? "Ngừng cấp nước (Cắt nước)"
                    : x.ERPTTYPE == "3" ? "Cấp nước lại (Mở nước)"
                    : x.ERPTTYPE == "4" ? "Giấy mời ký lại hợp đồng tiêu thụ nước sạch"
                    : x.ERPTTYPE == "5" ? "Điều chỉnh giá tiêu thụ nước sạch" : x.ERPTTYPE,
                TRANG_THAI = x.ERPTSTS == "1" ? "Đã thực hiện" : x.ERPTSTS == "0" ? "Chưa thực hiện" : x.ERPTSTS,
                x.NGAY_THUC_HIEN,
                x.NGUOI_THUC_HIEN,
                x.LY_DO,
                x.CHI_SO_NIEM,
                x.CHI_SO_NUOC,
                x.GHI_CHU_1,
                x.GHI_CHU_2,
                x.TONG_TIEN_NO
            }).Cast<object>().ToArray();

        return JsonObject(response);

    }


    public async Task<ContentResult> P_86_LAY_TT_GUI_EMAIL(string? MA_KHACH_HANG, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(MA_KHACH_HANG) && MA_KHACH_HANG.Length != 9)
            return JsonObject(new[] { new { ROOT = $"15- Độ dài dữ liệu không hợp lệ (MKH: {MA_KHACH_HANG})" } });

        var today = DateTime.Today;
        var fromDate = today.AddDays(-120);
        var toDate = new DateTime(today.Year, today.Month, 1);
        var rows = await (
            from log in _context.LogEmails.AsNoTracking()
            join customer in _context.ThongTinKhs.AsNoTracking()
                on log.MaKhachHang equals customer.MaKhachHang
            join emailType in _context.DmLoaiSmsEmails.AsNoTracking()
                on log.LoaiEmail equals emailType.MaLoai
            join status in _context.DmTrangThaiEmails.AsNoTracking()
                on log.KetQua equals status.MaLoai
            where log.MaKhachHang == MA_KHACH_HANG
                  && log.NgayGui >= fromDate
                  && log.NgayGui <= toDate
            orderby log.NgayGui descending
            select new
            {
                ROOT = "00- OK",
                LOAI_EMAIL = emailType.TenLoai,
                TRANG_THAI = status.TenLoai,
                EMAIL = log.Email,
                NGAY_GUI = log.NgayGui
            })
            .ToListAsync(cancellationToken);

        return JsonObject(rows.Count == 0
            ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } }
            : rows.Cast<object>());
    }

    public async Task<ContentResult> P_96_CC_DM_YEU_CAU(string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await _context.CcDmNoiDungYcs.AsNoTracking()
            .Where(x => x.AppDh == "1")
            .OrderBy(x => x.Stt)
            .Select(x => new { ROOT = "00- OK", MA_LOAI_YEU_CAU = x.IdNoiDung, TEN_LOAI_YEU_CAU = x.NoiDungYc })
            .ToListAsync(cancellationToken);
        return JsonObject(rows);
    }

    public async Task<ContentResult> P_97_CC_DM_QUAN(string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await _context.KDmDiaChinhs.AsNoTracking()
            .Where(x => x.ParentMa == "0001")
            .OrderBy(x => x.MaDiaChinh)
            .Select(x => new { ROOT = "00- OK", MA_PHUONG = x.MaDiaChinh, TEN_PHUONG = x.TenDiaChinh })
            .ToListAsync(cancellationToken);
        return JsonObject(rows);
    }

    public async Task<ContentResult> P_98_CC_DM_PHUONG(string? MA_BIEN_DOC, string? MA_QUAN, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await _context.KDmDiaChinhs.AsNoTracking()
            .Where(x => x.MaPhuong == null && x.MaQuan == MA_QUAN)
            .OrderBy(x => x.MaDiaChinh)
            .Select(x => new { ROOT = "00- OK", MA_PHUONG = x.MaDiaChinh, TEN_PHUONG = x.TenDiaChinh })
            .ToListAsync(cancellationToken);
        return JsonObject(rows);
    }

    public async Task<ContentResult> P_91_DM_XI_NGHIEP(string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await _context.Dm01DonVis.AsNoTracking()
            .Where(x => x.KyHieu != null && x.KyHieu != "0")
            .OrderBy(x => x.MaDonVi)
            .Select(x => new { ROOT = "00- OK", MA = x.MaDonVi, TEN = x.MaDonVi + "- " + x.TenDonVi })
            .ToListAsync(cancellationToken);
        return JsonObject(rows);
    }

    public async Task<ContentResult> P_92_DM_BIEN_DOC(string? MA_XI_NGHIEP, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await _context.DmNhanViens.AsNoTracking()
            .Where(x => x.DocChiSo == "1" && x.MaChiNhanh == MA_XI_NGHIEP)
            .OrderBy(x => x.TenNhanVien)
            .Select(x => new { ROOT = "00- OK", MA = x.MaNhanVien, TEN = x.MaNhanVien + "- " + x.TenNhanVien })
            .ToListAsync(cancellationToken);
        return JsonObject(rows);
    }

    public async Task<ContentResult> P_921_DM_SO_DOC(string? opt_MA_BIEN_DOC, string? MA_XI_NGHIEP, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var query = _context.DmSoDocs.AsNoTracking()
            .Where(x => x.MaChiNhanh == MA_XI_NGHIEP && x.HieuLuc == "1");
        if (!string.IsNullOrWhiteSpace(opt_MA_BIEN_DOC))
        {
            query = query.Where(x => x.MaBienDoc == opt_MA_BIEN_DOC);
        }
        var rows = await query.OrderBy(x => x.MaSoDoc)
            .Select(x => new { ROOT = "00- OK", MA = x.MaSoDoc, TEN = x.MaSoDoc + "- " + x.TenSoDoc, x.NgayDoc })
            .ToListAsync(cancellationToken);
        return JsonObject(rows.Count == 0 ? new[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } } : rows.Cast<object>());
    }

    public async Task<ContentResult> P_99_DM_GHI_CHU(string? MA_XI_NGHIEP, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await _context.AppDhDmGhiChus.AsNoTracking()
            .Where(x => x.Nhom == "DCS" || x.Nhom == null)
            .OrderBy(x => x.Stt)
            .Select(x => new { ROOT = "00- OK", MA = x.MaGhiChu, TEN = x.NoiDungGhiChu })
            .ToListAsync(cancellationToken);
        return JsonObject(rows);
    }

    public async Task<ContentResult> P_931_DM_DIEM_THU_HO(string? MA_DIEM_THU, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        decimal? locationCode = decimal.TryParse(MA_DIEM_THU, out var parsedCode) ? parsedCode : null;
        var query =
            from location in _context.DmDiemThuTiens.AsNoTracking()
            join unit in _context.LogBankDonVis.AsNoTracking()
                on location.MaDonViThu equals unit.Ma into units
            from unit in units.DefaultIfEmpty()
            where location.HieuLuc == "0"
                  && location.MaDonViThu != 29
                  && (string.IsNullOrEmpty(MA_DIEM_THU) || location.MaDonViThu == locationCode)
            select new
            {
                ROOT = "00- OK",
                TEN_DON_VI_THU = unit.Ten == null ? null : unit.Ten.Replace("&", " và "),
                TEN_DIEM_GD = location.TenDiemGd == null ? null : location.TenDiemGd.Replace("&", ""),
                DIA_CHI_GD = location.DiaChiGd == null ? null : location.DiaChiGd.Replace("&", ""),
                THOI_GIAN_GD = "_ _:_ _",
                location.ViTriDiemThu,
                location.DiaChiGoogle
            };
        var rows = await query.ToListAsync(cancellationToken);
        return JsonObject(rows.Count == 0
            ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } }
            : rows.Cast<object>());
    }

    public async Task<ContentResult> P_93_DM_TINH_TRANG_DH(string? MA_TINH_TRANG_DH, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var query = _context.DmTinhTrangDongHos.AsNoTracking().Where(x => x.HieuLuc == "1");
        if (!string.IsNullOrWhiteSpace(MA_TINH_TRANG_DH))
        {
            query = query.Where(x => x.MaTinhTrangSo == MA_TINH_TRANG_DH);
        }
        var rows = await query.OrderBy(x => x.SttHienThi)
            .Select(x => new
            {
                ROOT = "00- OK",
                MA = x.MaTinhTrangSo,
                TEN = x.MoTaSub,
                N_SUA_CHI_SO_CU = x.NSuaChiSoCu,
                N_NHAP_CHI_SO_MOI = x.NNhapChiSoMoi,
                N_NHAP_SL_TRUC_TIEP = x.NNhapSlTrucTiep,
                N_CONG_DON_CHI_SO = x.NCongDonChiSo
            })
            .ToListAsync(cancellationToken);
        return JsonObject(rows);
    }

    public Task<ContentResult> P_94_DM_LOAI_CHI_SO(string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        Task.FromResult(JsonObject(new[]
        {
            new { ROOT = "00- OK", MA = "0", TEN = "0- Định kỳ" },
            new { ROOT = "00- OK", MA = "6", TEN = "6- Chốt chỉ số" }
        }));

    public Task<ContentResult> P_95_DM_QUA_VONG(string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        Task.FromResult(JsonObject(new[]
        {
            new { ROOT = "00- OK", MA = "N- Không", TEN = "Không" },
            new { ROOT = "00- OK", MA = "Y- Qua vòng", TEN = "Qua vòng" }
        }));

    public Task<ContentResult> PF_01_GetSysDateString(string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        Task.FromResult(JsonObject(new[] { new { ROOT = "00- OK", NGAY_HE_THONG = DateTime.Now.ToString("dd/MM/yyyy") } }));

    public async Task<ContentResult> PF_01_GetSysDate_Thang(string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var value = await _context.AppDhThangDocs.AsNoTracking()
            .OrderByDescending(x => x.IdThangDoc)
            .Select(x => new { x.Thang, x.Nam })
            .FirstOrDefaultAsync(cancellationToken);
        return JsonObject(new[]
        {
            new { ROOT = value is null ? "16- Dữ liệu không tìm thấy." : "00- OK", THANG_DOC = value is null ? null : $"{value.Thang}/{value.Nam}" }
        });
    }

    public async Task<ContentResult> PF_02_LAY_CHI_SO_THAO_LAP(string? MA_KHACH_HANG, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var value = await _context.AppDhThiCongs.AsNoTracking()
            .Where(x => x.MaKhachHang == MA_KHACH_HANG)
            .OrderByDescending(x => x.IdTc)
            .Select(x => new { ROOT = "00- OK", x.ChiSoThao, x.ChiSoLap })
            .FirstOrDefaultAsync(cancellationToken);
        return JsonObject(new[] { value ?? new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!", ChiSoThao = (decimal?)null, ChiSoLap = (decimal?)null } });
    }

    public async Task<ContentResult> P_89_TINH_TIEN(string MA_GIA, string SAN_LUONG_SD, string MA_BIEN_DOC, string SO_IMEI, string PASSWORD_K, CancellationToken cancellationToken)
    {
        // ============================================================
        // HÀM HỖ TRỢ
        // ============================================================

        static int ParseInt(string? value)
        {
            return int.TryParse(
                value,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out var number)
                ? number
                : 0;
        }

        static decimal ParseMoney(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0m;

            var text = value.Trim().Replace(',', '.');

            return decimal.TryParse(
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out var number)
                ? number
                : 0m;
        }

        // ============================================================
        // TÍNH THUẾ + PHÍ
        //
        // VB cũ:
        // - SH      => tính phí 100% sản lượng
        // - Khác SH => tính phí 80% sản lượng
        //
        // VAT:
        //     trả về tỷ lệ, ví dụ 8% => 0.08
        //
        // PHÍ:
        //     KIEU_PHI = "$" => SL * GIA_TRI
        //     KIEU_PHI = "%" => SL * GIA_TRI / 100
        // ============================================================

        var priceFeeConfigs = new Dictionary<string, (string? Loai, decimal VatRate, decimal PhiGiaTri, string? KieuPhi, bool HasPhi)>(StringComparer.Ordinal);

        (decimal VatRate, long TienPhi) TienThuePhi(string maGiaCB, int slSD)
        {
            if (string.IsNullOrWhiteSpace(maGiaCB) || !priceFeeConfigs.TryGetValue(maGiaCB.Trim(), out var config))
                return (0m, 0L);

            var tyLePhi = config.Loai is null or "SH" ? 1m : 0.8m;
            var slDaChia = Convert.ToInt64(slSD * tyLePhi);
            var tienPhi = config.HasPhi
                ? config.KieuPhi == "$" ? Convert.ToInt64(slDaChia * config.PhiGiaTri)
                : config.KieuPhi == "%" ? Convert.ToInt64(slDaChia * (config.PhiGiaTri / 100m)) : 0L
                : 0L;
            return (config.VatRate, tienPhi);
        }

        // ============================================================
        // // GIÁ PHẦN TRĂM
        //
        // VB:
        // tinhBacThangGiaPhanTram
        //
        // Return:
        // ThanhTien
        // ThueVat
        // TienPhi
        // ============================================================

        (long ThanhTien, long ThueVat, long TienPhi)
            TinhBacThangGiaPhanTramAsync(
                string chuoiGia,
                string chuoiCongThuc,
                int slDinhMuc)
        {
            long tienNuoc = 0L;
            long tienVat = 0L;
            long tienPhi = 0L;

            if (string.IsNullOrWhiteSpace(chuoiGia))
                return (0L, 0L, 0L);

            if (string.IsNullOrWhiteSpace(chuoiCongThuc))
                return (0L, 0L, 0L);

            // VB Mid(chuoiGia, 2)
            var chuoiGiaTemp =
                chuoiGia.Length > 0
                    ? chuoiGia[1..]
                    : string.Empty;

            var chuoiGiaCTTemp =
                chuoiCongThuc.Length > 0
                    ? chuoiCongThuc[1..]
                    : string.Empty;

            var danhSachGia = chuoiGiaTemp.Split(
                ';',
                StringSplitOptions.RemoveEmptyEntries |
                StringSplitOptions.TrimEntries);

            var danhSachCT = chuoiGiaCTTemp.Split(
                ';',
                StringSplitOptions.RemoveEmptyEntries |
                StringSplitOptions.TrimEntries);

            var count = Math.Min(
                danhSachGia.Length,
                danhSachCT.Length);

            for (int j = 0; j < count; j++)
            {
                var capSLvaGia = danhSachGia[j].Split(
                    '-',
                    2,
                    StringSplitOptions.TrimEntries);

                var capSLvaGiaCT = danhSachCT[j].Split(
                    '-',
                    2,
                    StringSplitOptions.TrimEntries);

                if (capSLvaGia.Length != 2 ||
                    capSLvaGiaCT.Length != 2)
                    continue;

                var ptGoc = ParseInt(capSLvaGia[0]);

                var giaGoc = ParseMoney(capSLvaGia[1]);

                // VB:
                // sl = slDinhMuc * (ptGoc / 100)
                decimal slDecimal =
                    slDinhMuc * (ptGoc / 100m);

                var sl = Convert.ToInt32(slDecimal);

                var tienTam = Convert.ToInt64(
                    slDecimal * giaGoc);

                var maGiaCB =
                    capSLvaGiaCT[1].Trim();

                var thuePhi = TienThuePhi(
                    maGiaCB,
                    sl);

                var tienVATTam = Convert.ToInt64(
                    tienTam * thuePhi.VatRate);

                tienNuoc += tienTam;
                tienVat += tienVATTam;
                tienPhi += thuePhi.TienPhi;
            }

            return (
                tienNuoc,
                tienVat,
                tienPhi);
        }

        // ============================================================
        // GIÁ GỘP / BẬC THANG $
        //
        // VB:
        // tinhBacThangGiaGop
        // ============================================================

        (
            long ThanhTien,
            long ThueVat,
            long TienPhi,
            int SlConLai)
            TinhBacThangGiaGopAsync(
                string chuoiGia,
                string chuoiCongThuc,
                int slDinhMuc,
                int slConLai)
        {
            long tienNuoc = 0L;
            long tienVat = 0L;
            long tienPhi = 0L;

            int slChoPhepDung;
            int slChenhLech;

            if (slDinhMuc < slConLai)
            {
                slChoPhepDung = slDinhMuc;
                slChenhLech = slConLai - slDinhMuc;
            }
            else
            {
                slChenhLech = 0;
                slChoPhepDung = slConLai;
            }

            int slConLaiTam = slChoPhepDung;

            var chuoiGiaTemp =
                chuoiGia.Length > 0
                    ? chuoiGia[1..]
                    : string.Empty;

            var chuoiGiaCTTemp =
                chuoiCongThuc.Length > 0
                    ? chuoiCongThuc[1..]
                    : string.Empty;

            var danhSachGia = chuoiGiaTemp.Split(
                ';',
                StringSplitOptions.RemoveEmptyEntries |
                StringSplitOptions.TrimEntries);

            var danhSachCT = chuoiGiaCTTemp.Split(
                ';',
                StringSplitOptions.RemoveEmptyEntries |
                StringSplitOptions.TrimEntries);

            var count = Math.Min(
                danhSachGia.Length,
                danhSachCT.Length);

            for (int j = 0; j < count; j++)
            {
                if (slConLaiTam == 0)
                    break;

                var capSLvaGia = danhSachGia[j].Split(
                    '-',
                    2,
                    StringSplitOptions.TrimEntries);

                var capSLvaGiaCT = danhSachCT[j].Split(
                    '-',
                    2,
                    StringSplitOptions.TrimEntries);

                if (capSLvaGia.Length != 2 ||
                    capSLvaGiaCT.Length != 2)
                    continue;

                var slGoc = ParseInt(
                    capSLvaGia[0]);

                var giaGoc = ParseMoney(
                    capSLvaGia[1]);

                var maGiaCB =
                    capSLvaGiaCT[1].Trim();

                int slTinh;

                if (slConLaiTam > slGoc)
                {
                    slTinh = slGoc;
                    slConLaiTam -= slGoc;
                }
                else
                {
                    slTinh = slConLaiTam;
                    slConLaiTam = 0;
                }

                var tienTam = Convert.ToInt64(
                    slTinh * giaGoc);

                var thuePhi = TienThuePhi(
                    maGiaCB,
                    slTinh);

                var tienVATTam = Convert.ToInt64(
                    tienTam * thuePhi.VatRate);

                tienNuoc += tienTam;
                tienVat += tienVATTam;
                tienPhi += thuePhi.TienPhi;
            }

            slConLai =
                slConLaiTam + slChenhLech;

            return (
                tienNuoc,
                tienVat,
                tienPhi,
                slConLai);
        }

        // ============================================================
        // BẮT ĐẦU P_89_TINH_TIEN
        // ============================================================

        var tongSL = ParseInt(SAN_LUONG_SD);

        if (tongSL < 0)
            tongSL = 0;

        // ============================================================
        // Lấy:
        //
        // CHUOI_GIA_DM = chuỗi đã thay mã giá thành giá tiền
        // CHUOI_GIA_GT = chuỗi công thức còn mã M1, M2, A4KDDV...
        // KIEU_TINH    = $ / %
        // ============================================================

        var price = await _context.KDmGia
            .AsNoTracking()
            .Where(x =>
                x.HieuLuc == "1" &&
                x.KyHieuGia == MA_GIA)
            .Select(x => new
            {
                x.ChuoiGiaDm,
                x.ChuoiGiaGt,
                x.KieuTinh
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (price == null)
        {
            return JsonObject(new[]
            {
            new
            {
                ROOT = "16- Khong tim thay gia.",
                THANH_TIEN = (long?)null,
                THUE_VAT = (long?)null,
                TIEN_PHI = (long?)null,
                TONG_TIEN = (long?)null
            }
        });
        }

        var priceFeeRows = await (
    from basePrice in _context.KDmGia.AsNoTracking()
    where basePrice.HieuLuc == "1" && basePrice.KieuGia == "0"

    join sub in _context.KDmGiaSubs.AsNoTracking()
        on basePrice.KyHieuGia equals sub.MaGia into subPrices
    from sub in subPrices.DefaultIfEmpty()

    join vat in _context.KDmGiaPhis.AsNoTracking()
        on basePrice.MaThueVat equals vat.MaPhi into vatFees
    from vat in vatFees.DefaultIfEmpty()

    join fee in _context.KDmGiaPhis.AsNoTracking()
        on basePrice.MaPhiBvmt equals fee.MaPhi into environmentFees
    from fee in environmentFees.DefaultIfEmpty()

    select new
    {
        basePrice.KyHieuGia,

        // Không so sánh entity với null
        Loai = sub.Loai,

        // LEFT JOIN không có dữ liệu => property SQL trả NULL
        VatRate = (vat.GiaTri ?? 0m) / 100m,

        PhiGiaTri = fee.GiaTri ?? 0m,

        KieuPhi = fee.KieuPhi,

        // Dùng property để xác định có dòng phí hay không
        HasPhi = fee.MaPhi != null
    }
).ToListAsync(cancellationToken);

        priceFeeConfigs = priceFeeRows
            .Where(x => !string.IsNullOrWhiteSpace(x.KyHieuGia))
            .GroupBy(x => x.KyHieuGia!, StringComparer.Ordinal)
            .ToDictionary(
                group => group.Key,
                group => { var item = group.First(); return (item.Loai, item.VatRate, item.PhiGiaTri, item.KieuPhi, item.HasPhi); },
                StringComparer.Ordinal);
        var chuoiGiaDm =
            price.ChuoiGiaDm ?? string.Empty;

        var chuoiGiaGt =
            price.ChuoiGiaGt ?? string.Empty;

        var kieuTinh =
            price.KieuTinh ?? string.Empty;

        var thanhPhanGia = chuoiGiaDm.Split(
            '!',
            StringSplitOptions.RemoveEmptyEntries |
            StringSplitOptions.TrimEntries);

        var thanhCongThuc = chuoiGiaGt.Split(
            '!',
            StringSplitOptions.RemoveEmptyEntries |
            StringSplitOptions.TrimEntries);

        if (thanhPhanGia.Length == 0)
        {
            return JsonObject(new[]
            {
            new
            {
                ROOT = "16- Chuoi gia khong hop le.",
                THANH_TIEN = (long?)null,
                THUE_VAT = (long?)null,
                TIEN_PHI = (long?)null,
                TONG_TIEN = (long?)null
            }
        });
        }

        long thanhTien = 0L;
        long tienVAT = 0L;
        long tienPhi = 0L;

        int slConLai = tongSL;

        var soThanhPhan = Math.Min(
            thanhPhanGia.Length,
            thanhCongThuc.Length);

        for (int i = 0; i < soThanhPhan; i++)
        {
            if (slConLai <= 0)
                break;

            var chuoiGiaSub =
                thanhPhanGia[i].Trim();

            var chuoiGiaCTSub =
                thanhCongThuc[i].Trim();

            if (string.IsNullOrWhiteSpace(chuoiGiaSub))
                continue;

            var kieuTinhSub =
                chuoiGiaSub[0];

            var capGiaTriVaGia =
                chuoiGiaSub.Split(
                    '(',
                    2,
                    StringSplitOptions.TrimEntries);

            var capGiaTriVaGiaCT =
                chuoiGiaCTSub.Split(
                    '(',
                    2,
                    StringSplitOptions.TrimEntries);

            // ========================================================
            // CASE 1: KHÔNG CÓ DẤU (
            // ========================================================

            if (capGiaTriVaGia.Length == 1)
            {
                // ----------------------------------------------------
                // Bắt đầu bằng số
                // ----------------------------------------------------

                if (char.IsDigit(kieuTinhSub))
                {
                    // =================================================
                    // Kiểu tính $
                    // =================================================

                    if (kieuTinh == "$")
                    {
                        if (chuoiGiaSub.Contains('-'))
                        {
                            var capSLvaGia =
                                chuoiGiaSub.Split(
                                    '-',
                                    2,
                                    StringSplitOptions.TrimEntries);

                            var capSLvaGiaCT =
                                chuoiGiaCTSub.Split(
                                    '-',
                                    2,
                                    StringSplitOptions.TrimEntries);

                            if (capSLvaGia.Length == 2 &&
                                capSLvaGiaCT.Length == 2)
                            {
                                var slDMGoc =
                                    ParseInt(capSLvaGia[0]);

                                var giaCBGoc =
                                    ParseMoney(capSLvaGia[1]);

                                if (slDMGoc > slConLai)
                                {
                                    var slTinh =
                                        slConLai;

                                    var tienTam =
                                        Convert.ToInt64(
                                            slTinh * giaCBGoc);

                                    thanhTien += tienTam;

                                    var tp =
                                        TienThuePhi(
                                            capSLvaGiaCT[1],
                                            slTinh);

                                    tienVAT +=
                                        Convert.ToInt64(
                                            tienTam * tp.VatRate);

                                    tienPhi +=
                                        tp.TienPhi;

                                    slConLai = 0;
                                }
                                else
                                {
                                    slConLai -= slDMGoc;

                                    var tienTam =
                                        Convert.ToInt64(
                                            slDMGoc * giaCBGoc);

                                    thanhTien += tienTam;

                                    // QUAN TRỌNG:
                                    // Giữ đúng VB cũ:
                                    //
                                    // tienThuePhi(
                                    //     capSLvaGiaCT(1),
                                    //     slConLai)
                                    //
                                    // VB truyền slConLai sau khi đã trừ.
                                    var tp =
                                        TienThuePhi(
                                            capSLvaGiaCT[1],
                                            slConLai);

                                    tienVAT +=
                                        Convert.ToInt64(
                                            tienTam * tp.VatRate);

                                    tienPhi +=
                                        tp.TienPhi;
                                }
                            }
                        }
                        else
                        {
                            // Giá đơn, ví dụ:
                            // 3809.52

                            var giaCBGoc =
                                ParseMoney(chuoiGiaSub);

                            thanhTien +=
                                Convert.ToInt64(
                                    slConLai * giaCBGoc);
                        }
                    }

                    // =================================================
                    // Kiểu tính %
                    // =================================================

                    else if (kieuTinh == "%")
                    {
                        var giaPT =
                            "%" + chuoiGiaSub;

                        var ctPT =
                            "%" + chuoiGiaCTSub;

                        var result =
                            TinhBacThangGiaPhanTramAsync(
                                giaPT,
                                ctPT,
                                tongSL);

                        thanhTien += result.ThanhTien;
                        tienVAT += result.ThueVat;
                        tienPhi += result.TienPhi;
                    }

                    // =================================================
                    // Giá cơ bản
                    // =================================================

                    else
                    {
                        var giaCBGoc =
                            ParseMoney(chuoiGiaSub);

                        var tienTam =
                            Convert.ToInt64(
                                giaCBGoc * tongSL);

                        thanhTien += tienTam;

                        var tp =
                            TienThuePhi(
                                chuoiGiaCTSub,
                                slConLai);

                        // Giữ logic VB:
                        // tienVAT += thanhtien * tyleVAT
                        tienVAT +=
                            Convert.ToInt64(
                                thanhTien * tp.VatRate);

                        tienPhi +=
                            tp.TienPhi;
                    }
                }

                // ----------------------------------------------------
                // Bắt đầu bằng $ hoặc %
                // ----------------------------------------------------

                else
                {
                    var sanLuongDM =
                        tongSL;

                    if (kieuTinhSub == '$')
                    {
                        var result =
                            TinhBacThangGiaGopAsync(
                                chuoiGiaSub,
                                chuoiGiaCTSub,
                                sanLuongDM,
                                slConLai);

                        thanhTien += result.ThanhTien;
                        tienVAT += result.ThueVat;
                        tienPhi += result.TienPhi;

                        slConLai =
                            result.SlConLai;
                    }
                    else if (kieuTinhSub == '%')
                    {
                        var result =
                            TinhBacThangGiaPhanTramAsync(
                                chuoiGiaSub,
                                chuoiGiaCTSub,
                                sanLuongDM);

                        thanhTien += result.ThanhTien;
                        tienVAT += result.ThueVat;
                        tienPhi += result.TienPhi;
                    }
                }
            }

            // ========================================================
            // CASE 2: CÓ GIÁ LỒNG (...)
            // ========================================================

            else
            {
                var giaTriGoc =
                    capGiaTriVaGia[0];

                var kieuTinhDau =
                    string.IsNullOrEmpty(giaTriGoc)
                        ? '\0'
                        : giaTriGoc[0];

                var chuoiGia =
                    capGiaTriVaGia[1]
                        .Replace(")", "")
                        .Trim();

                var chuoiGiaCT =
                    capGiaTriVaGiaCT.Length > 1
                        ? capGiaTriVaGiaCT[1]
                            .Replace(")", "")
                            .Trim()
                        : string.Empty;

                // ====================================================
                // BẮT ĐẦU BẰNG SỐ
                // ====================================================

                if (char.IsDigit(kieuTinhDau))
                {
                    // ------------------------------------------------
                    // KIEU_TINH = $
                    // ------------------------------------------------

                    if (kieuTinh == "$")
                    {
                        if (chuoiGiaSub.Contains('-'))
                        {
                            giaTriGoc =
                                giaTriGoc
                                    .Replace("$", "")
                                    .Replace("-", "")
                                    .Trim();

                            var sanLuongDM =
                                ParseInt(giaTriGoc);

                            var kieuCon =
                                chuoiGia.Length > 0
                                    ? chuoiGia[0]
                                    : '\0';

                            if (kieuCon == '$')
                            {
                                var result =
                                    TinhBacThangGiaGopAsync(
                                        chuoiGia,
                                        chuoiGiaCT,
                                        sanLuongDM,
                                        slConLai);

                                thanhTien += result.ThanhTien;
                                tienVAT += result.ThueVat;
                                tienPhi += result.TienPhi;

                                slConLai =
                                    result.SlConLai;
                            }
                            else if (kieuCon == '%')
                            {
                                var result =
                                    TinhBacThangGiaPhanTramAsync(
                                        chuoiGia,
                                        chuoiGiaCT,
                                        slConLai);

                                thanhTien += result.ThanhTien;
                                tienVAT += result.ThueVat;
                                tienPhi += result.TienPhi;
                            }
                        }
                        else
                        {
                            var giaCBGoc =
                                ParseMoney(giaTriGoc);

                            thanhTien +=
                                Convert.ToInt64(
                                    slConLai * giaCBGoc);
                        }
                    }

                    // ------------------------------------------------
                    // KIEU_TINH = %
                    // ------------------------------------------------

                    else
                    {
                        giaTriGoc =
                            giaTriGoc
                                .Replace("%", "")
                                .Replace("-", "")
                                .Trim();

                        var sanLuongDM =
                            ParseInt(giaTriGoc);

                        var kieuCon =
                            chuoiGia.Length > 0
                                ? chuoiGia[0]
                                : '\0';

                        if (kieuCon == '$')
                        {
                            sanLuongDM =
                                (sanLuongDM * slConLai) / 100;

                            var result =
                                TinhBacThangGiaGopAsync(
                                    chuoiGia,
                                    chuoiGiaCT,
                                    sanLuongDM,
                                    slConLai);

                            thanhTien += result.ThanhTien;
                            tienVAT += result.ThueVat;
                            tienPhi += result.TienPhi;

                            slConLai =
                                result.SlConLai;
                        }
                        else if (kieuCon == '%')
                        {
                            // Giữ đúng VB:
                            // gọi bằng chuoiGiaSub / chuoiGiaCTSub
                            var result =
                                TinhBacThangGiaPhanTramAsync(
                                    chuoiGiaSub,
                                    chuoiGiaCTSub,
                                    sanLuongDM);

                            thanhTien += result.ThanhTien;
                            tienVAT += result.ThueVat;
                            tienPhi += result.TienPhi;
                        }
                    }
                }

                // ====================================================
                // BẮT ĐẦU BẰNG $ HOẶC %
                // ====================================================

                else
                {
                    // ------------------------------------------------
                    // $xx-(...)
                    // ------------------------------------------------

                    if (kieuTinhDau == '$')
                    {
                        giaTriGoc =
                            giaTriGoc
                                .Replace("$", "")
                                .Replace("-", "")
                                .Trim();

                        var sanLuongDM =
                            ParseInt(giaTriGoc);

                        var kieuCon =
                            chuoiGia.Length > 0
                                ? chuoiGia[0]
                                : '\0';

                        if (kieuCon == '$')
                        {
                            var result =
                                TinhBacThangGiaGopAsync(
                                    chuoiGia,
                                    chuoiGiaCT,
                                    sanLuongDM,
                                    slConLai);

                            thanhTien += result.ThanhTien;
                            tienVAT += result.ThueVat;
                            tienPhi += result.TienPhi;

                            slConLai =
                                result.SlConLai;
                        }
                        else if (kieuCon == '%')
                        {
                            // Giữ đúng VB:
                            // gọi chuoiGiaSub thay vì chuoiGia
                            var result =
                                TinhBacThangGiaPhanTramAsync(
                                    chuoiGiaSub,
                                    chuoiGiaCTSub,
                                    sanLuongDM);

                            thanhTien += result.ThanhTien;
                            tienVAT += result.ThueVat;
                            tienPhi += result.TienPhi;
                        }
                    }

                    // ------------------------------------------------
                    // %xx-(...)
                    // ------------------------------------------------

                    else
                    {
                        giaTriGoc =
                            giaTriGoc
                                .Replace("%", "")
                                .Replace("-", "")
                                .Trim();

                        var sanLuongDM =
                            ParseInt(giaTriGoc);

                        var kieuCon =
                            chuoiGia.Length > 0
                                ? chuoiGia[0]
                                : '\0';

                        if (kieuCon == '$')
                        {
                            sanLuongDM =
                                (sanLuongDM * slConLai) / 100;

                            var result =
                                TinhBacThangGiaGopAsync(
                                    chuoiGia,
                                    chuoiGiaCT,
                                    sanLuongDM,
                                    slConLai);

                            thanhTien += result.ThanhTien;
                            tienVAT += result.ThueVat;
                            tienPhi += result.TienPhi;

                            slConLai =
                                result.SlConLai;
                        }
                        else if (kieuCon == '%')
                        {
                            // Giữ đúng VB cũ
                            var result =
                                TinhBacThangGiaPhanTramAsync(
                                    chuoiGiaSub,
                                    chuoiGiaCTSub,
                                    sanLuongDM);

                            thanhTien += result.ThanhTien;
                            tienVAT += result.ThueVat;
                            tienPhi += result.TienPhi;
                        }
                    }
                }
            }
        }

        // ============================================================
        // TỔNG TIỀN
        // ============================================================

        var tongTien =
            thanhTien +
            tienVAT +
            tienPhi;

        // ============================================================
        // RESPONSE
        // ============================================================

        return JsonObject(new[]
        {
        new
        {
            ROOT = "00- OK",
            THANH_TIEN = thanhTien,
            THUE_VAT = tienVAT,
            TIEN_PHI = tienPhi,
            TONG_TIEN = tongTien
        }
    });
    }

    public async Task<ContentResult> P_07_CC_BAO_SU_CO(P_07_CC_BAO_SU_CORequest r, CancellationToken cancellationToken)
    {
        var requestId = await NextNumericIdAsync("CC_01_TTKH_YEU_CAU", "ID_YEU_CAU", 9, cancellationToken);
        var customerRequested = r.KHACH_HANG_YEU_CAU == "Y" ? "Y" : "N";
        var employeeRequested = r.KHACH_HANG_YEU_CAU == "Y" ? "N" : "Y";

        var affectedRows = await ExecuteNonQueryAsync("""
            INSERT INTO CC_01_TTKH_YEU_CAU (
                ID_YEU_CAU,
                MA_KHACH_HANG,
                MA_NOI_DUNG_YC,
                MA_XI_NGHIEP,
                MA_QUAN,
                MA_PHUONG,
                TEN_KHACH_HANG,
                TEN_KHACH_HANG_SUB,
                DIA_CHI_DONG_HO,
                CC_SO_DIEN_THOAI,
                CC_EMAIL,
                GHI_CHU,
                XU_LY_MA_BO_PHAN,
                XU_LY_MA_TRANG_THAI,
                CONG_VIEC_24G,
                KHACH_HANG_YC,
                NHAN_VIEN_YC,
                KHACH_HANG_YC_LAN,
                NHAN_VIEN_YC_LAN,
                TT_UU_TIEN_CV,
                NGUOI_CAP_NHAT,
                NGAY_CAP_NHAT
            ) VALUES (
                :id,
                :customerCode,
                :requestType,
                :branch,
                :district,
                :ward,
                :customerName,
                :customerNameSub,
                :address,
                :phone,
                :email,
                :note,
                '02',
                '01',
                :work24h,
                :customerRequested,
                :employeeRequested,
                0,
                1,
                4,
                'app_biendoc',
                SYSDATE
            )
            """, new[]
        {
            Param("id", requestId),
            Param("customerCode", r.MA_KHACH_HANG),
            Param("requestType", r.LOAI_YEU_CAU),
            Param("branch", r.MA_XI_NGHIEP),
            Param("district", r.MA_QUAN),
            Param("ward", r.MA_PHUONG),
            NParam("customerName", r.TEN_KHACH_HANG),
            NParam("customerNameSub", $"{r.TEN_KHACH_HANG} [{r.DIA_CHI_KHACH_HANG}]"),
            NParam("address", r.DIA_CHI_KHACH_HANG),
            Param("phone", r.SO_DIEN_THOAI_KH),
            Param("email", string.IsNullOrWhiteSpace(r.EMAIL_KH) ? null : r.EMAIL_KH),
            NParam("note", r.NOI_DUNG_YEU_CAU),
            Param("work24h", r.XU_LY_24H),
            Param("customerRequested", customerRequested),
            Param("employeeRequested", employeeRequested)
        }, cancellationToken);

        return JsonObject(new[] { new { ROOT = affectedRows == 0 ? "16- Dữ liệu không tìm thấy." : "00- OK" } });
    }

    public async Task<ContentResult> P_9E_SUA_THONG_TIN_KH(P_9E_SUA_THONG_TIN_KHRequest r, CancellationToken cancellationToken)
    {
        var changed = !string.IsNullOrWhiteSpace(r.TEN_KHACH_HANG)
            || !string.IsNullOrWhiteSpace(r.DIA_CHI_DONG_HO)
            || !string.IsNullOrWhiteSpace(r.SO_DT)
            || !string.IsNullOrWhiteSpace(r.EMAIL)
            || !string.IsNullOrWhiteSpace(r.DONG_HO_TEN)
            || !string.IsNullOrWhiteSpace(r.DONG_HO_SERIAL)
            || !string.IsNullOrWhiteSpace(r.GHI_CHU);
        if (!changed) return JsonObject(new[] { new { ROOT = "14- Dữ liệu không hợp lệ." } });
        var affectedRows = await _context.AppDhChiSos
            .Where(x => x.MaKhachHang == r.MA_KHACH_HANG && x.MaChiNhanh == r.MA_XI_NGHIEP && x.MaBienDoc == r.MA_BIEN_DOC && x.Thang == r.THANG)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.SuaTenKhachHang, x => string.IsNullOrWhiteSpace(r.TEN_KHACH_HANG) ? x.SuaTenKhachHang : r.TEN_KHACH_HANG)
                .SetProperty(x => x.SuaDiaChiDongHo, x => string.IsNullOrWhiteSpace(r.DIA_CHI_DONG_HO) ? x.SuaDiaChiDongHo : r.DIA_CHI_DONG_HO)
                .SetProperty(x => x.SuaSoDt, x => string.IsNullOrWhiteSpace(r.SO_DT) ? x.SuaSoDt : r.SO_DT)
                .SetProperty(x => x.SuaEmail, x => string.IsNullOrWhiteSpace(r.EMAIL) ? x.SuaEmail : r.EMAIL)
                .SetProperty(x => x.SuaDhTen, x => string.IsNullOrWhiteSpace(r.DONG_HO_TEN) ? x.SuaDhTen : r.DONG_HO_TEN)
                .SetProperty(x => x.SuaDhSerial, x => string.IsNullOrWhiteSpace(r.DONG_HO_SERIAL) ? x.SuaDhSerial : r.DONG_HO_SERIAL)
                .SetProperty(x => x.SuaGhiChu, x => string.IsNullOrWhiteSpace(r.GHI_CHU) ? x.SuaGhiChu : r.GHI_CHU), cancellationToken);
        if (affectedRows == 0) return JsonObject(new[] { new { ROOT = "16- Dữ liệu không tìm thấy." } });
        return JsonObject(new[] { new { ROOT = "00- OK" } });
    }

    public async Task<ContentResult> A_011_CHECKIN_DS_KH_CAT_NUOC(string? MA_NHAN_VIEN, string? TU_NGAY, string? DEN_NGAY, string? MA_XI_NGHIEP, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await (from job in _context.AppCheckInCatNuocs.AsNoTracking()
            join customer in _context.ThongTinKhs.AsNoTracking() on job.MaKhachHang equals customer.MaKhachHang
            where job.MaNhanVien == MA_NHAN_VIEN && job.MaXiNghiep == MA_XI_NGHIEP
            orderby job.Stt
            select new { ROOT = "00- OK", job.IdXacNhan, job.MaKhachHang, customer.TenKhachHang, customer.DiaChiDongHo, SO_DIEN_THOAI = customer.PhoneUt1, job.NgayXnBgNhanVien, job.NgayHoanThanh, job.MaSoDoc, job.TenFileAnh, job.GhiChuXn }).ToListAsync(cancellationToken);
        return JsonObject(rows.Count == 0 ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy." } } : rows.Cast<object>());
    }

    public async Task<ContentResult> A_012_CHECKIN_DS_KH_MO_NUOC(string? MA_NHAN_VIEN, string? TU_NGAY, string? DEN_NGAY, string? MA_XI_NGHIEP, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await (from job in _context.AppCheckInMoNuocs.AsNoTracking()
            join customer in _context.ThongTinKhs.AsNoTracking() on job.MaKhachHang equals customer.MaKhachHang
            where job.MaNhanVien == MA_NHAN_VIEN && job.MaXiNghiep == MA_XI_NGHIEP
            orderby job.IdXacNhan
            select new { ROOT = "00- OK", job.IdXacNhan, job.MaKhachHang, customer.TenKhachHang, customer.DiaChiDongHo, SO_DIEN_THOAI = customer.PhoneUt1, job.NgayXnBgNhanVien, job.NgayHoanThanh, job.MaSoDoc, job.TenFileAnh, job.GhiChuXn }).ToListAsync(cancellationToken);
        return JsonObject(rows.Count == 0 ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy." } } : rows.Cast<object>());
    }

    public async Task<ContentResult> A_013_CHECKIN_DS_KH_GUI_GB(string? MA_NHAN_VIEN, string? TU_NGAY, string? DEN_NGAY, string? MA_XI_NGHIEP, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await (from job in _context.AppCheckInGuiGiays.AsNoTracking()
            join customer in _context.ThongTinKhs.AsNoTracking() on job.MaKhachHang equals customer.MaKhachHang
            where job.MaNhanVien == MA_NHAN_VIEN && job.MaXiNghiep == MA_XI_NGHIEP
            orderby job.IdXacNhan
            select new { ROOT = "00- OK", job.IdXacNhan, job.MaKhachHang, customer.TenKhachHang, customer.DiaChiDongHo, SO_DIEN_THOAI = customer.PhoneUt1, job.NgayXnBgNhanVien, job.NgayHoanThanh, job.MaSoDoc, job.TenFileAnh, job.GhiChuXn }).ToListAsync(cancellationToken);
        return JsonObject(rows.Count == 0 ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy." } } : rows.Cast<object>());
    }

    public async Task<ContentResult> A_99_CHECKIN_DM_KIEU_CAT(string? MA_XI_NGHIEP, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await _context.LogBankTienMoNuocs.AsNoTracking()
            .OrderBy(x => x.KieuCat)
            .Select(x => new { ROOT = "00- OK", MA = x.KieuCat, TEN = x.KieuCat + "- " + x.GhiChu })
            .ToListAsync(cancellationToken);
        return JsonObject(rows);
    }

    public async Task<ContentResult> A_02_CHECKIN_LUU_KH_CAT_NUOC(string? ID_XAC_NHAN, string? MA_NHAN_VIEN, string? MA_XI_NGHIEP, string? NGUOI_THI_CONG, string? MA_KIEU_CAT_MO, string? NGAY_HOAN_THANH, string? VI_TRI_XAC_NHAN, string? MA_GHI_CHU, string? GHI_CHU, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var completedAt = DateTime.TryParse(NGAY_HOAN_THANH, out var date) ? date : DateTime.Now;
        var affectedRows = await _context.AppCheckInCatNuocs
            .Where(x => x.IdXacNhan == ID_XAC_NHAN && x.MaNhanVien == MA_NHAN_VIEN && x.MaXiNghiep == MA_XI_NGHIEP)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.NguoiThiCong, NGUOI_THI_CONG)
                .SetProperty(x => x.KieuCatMo, MA_KIEU_CAT_MO)
                .SetProperty(x => x.ViTriXacNhanCu, x => x.ViTriXacNhan)
                .SetProperty(x => x.ViTriXacNhan, VI_TRI_XAC_NHAN)
                .SetProperty(x => x.MaGhiChu, MA_GHI_CHU)
                .SetProperty(x => x.GhiChuThem, GHI_CHU)
                .SetProperty(x => x.LogDateApp, DateTime.Now)
                .SetProperty(x => x.NgayHoanThanh, completedAt), cancellationToken);
        return JsonObject(new[] { new { ROOT = affectedRows == 0 ? "16- Dữ liệu không tìm thấy." : "00- OK" } });
    }

    public async Task<ContentResult> A_03_CHECKIN_LUU_KH_MO_NUOC(string? ID_XAC_NHAN, string? MA_NHAN_VIEN, string? MA_XI_NGHIEP, string? NGUOI_THI_CONG, string? MA_KIEU_CAT_MO, string? NGAY_HOAN_THANH, string? VI_TRI_XAC_NHAN, string? MA_GHI_CHU, string? GHI_CHU, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var completedAt = DateTime.TryParse(NGAY_HOAN_THANH, out var date) ? date : DateTime.Now;
        var affectedRows = await _context.AppCheckInMoNuocs
            .Where(x => x.IdXacNhan == ID_XAC_NHAN && x.MaNhanVien == MA_NHAN_VIEN && x.MaXiNghiep == MA_XI_NGHIEP)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.NguoiThiCong, NGUOI_THI_CONG)
                .SetProperty(x => x.KieuCatMo, MA_KIEU_CAT_MO)
                .SetProperty(x => x.ViTriXacNhanCu, x => x.ViTriXacNhan)
                .SetProperty(x => x.ViTriXacNhan, VI_TRI_XAC_NHAN)
                .SetProperty(x => x.MaGhiChu, MA_GHI_CHU)
                .SetProperty(x => x.GhiChuThem, GHI_CHU)
                .SetProperty(x => x.LogDateApp, DateTime.Now)
                .SetProperty(x => x.NgayHoanThanh, completedAt), cancellationToken);
        return JsonObject(new[] { new { ROOT = affectedRows == 0 ? "16- Dữ liệu không tìm thấy." : "00- OK" } });
    }

    public async Task<ContentResult> A_00_CHECKIN_LUU_TEN_FILE_ANH(string? ID_XAC_NHAN, string? LOAI_CV, string? TEN_FILE_ANH, string? MA_NHAN_VIEN, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var affectedRows = LOAI_CV switch
        {
            "1" => await _context.AppCheckInCatNuocs.Where(x => x.IdXacNhan == ID_XAC_NHAN && x.MaNhanVien == MA_NHAN_VIEN).ExecuteUpdateAsync(setters => setters.SetProperty(x => x.TenFileAnh, TEN_FILE_ANH), cancellationToken),
            "2" => await _context.AppCheckInMoNuocs.Where(x => x.IdXacNhan == ID_XAC_NHAN && x.MaNhanVien == MA_NHAN_VIEN).ExecuteUpdateAsync(setters => setters.SetProperty(x => x.TenFileAnh, TEN_FILE_ANH), cancellationToken),
            "3" => await _context.AppCheckInGuiGiays.Where(x => x.IdXacNhan == ID_XAC_NHAN && x.MaNhanVien == MA_NHAN_VIEN).ExecuteUpdateAsync(setters => setters.SetProperty(x => x.TenFileAnh, TEN_FILE_ANH), cancellationToken),
            _ => 0
        };
        return JsonObject(new[] { new { ROOT = affectedRows == 0 ? "16- Dữ liệu không tìm thấy." : "00- OK" } });
    }

}









