using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using ReadMeter.Api.Data;
using ReadMeter.Api.Models;
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
        Content = JsonSerializer.Serialize(value)
    };

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

    private async Task<ContentResult> GetCustomersForReading(
        string? id, string? sequence, string? customerCode, string? customerName, string? address,
        string? phone, string? bookCode, string? meterReader, string? month, string? filter,
        bool supplementalOnly, bool assignedBookOnly, CancellationToken cancellationToken)
    {
        var where = new StringBuilder();
        var parameters = new List<OracleParameter>();
        if (!string.IsNullOrWhiteSpace(id))
        {
            where.AppendLine("WHERE cs.ID_DCS = :id");
            parameters.Add(Param("id", id));
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

            if (assignedBookOnly)
                where.AppendLine("  AND cs.NGAY_EBILL_NHAN_KHOA IS NULL AND cs.NGAY_EBILL_NAP_BILL IS NULL AND cs.NGAY_BD_NHAN_KHOA IS NOT NULL");
        }

        if (decimal.TryParse(sequence, out var rawSequenceValue))
        {
            where.AppendLine("  AND cs.STT_SO_DOC = :sequence");
            parameters.Add(Param("sequence", rawSequenceValue));
        }
        if (filter == "1")
            where.AppendLine("  AND cs.NGAY_DOC_TUNG_DH IS NOT NULL");
        if (filter == "2")
            where.AppendLine("  AND cs.NGAY_DOC_TUNG_DH IS NULL");
        if (supplementalOnly)
            where.AppendLine("  AND cs.MA_TINH_TRANG_DH = 'BS'");
        if (!string.IsNullOrWhiteSpace(customerCode))
        {
            where.AppendLine("  AND kh.MA_KHACH_HANG LIKE :customerCode");
            parameters.Add(Param("customerCode", $"%{customerCode}%"));
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
            JOIN THONG_TIN_KH kh ON kh.MA_KHACH_HANG = cs.MA_KHACH_HANG
            {where}
              AND kh.NGAY_THANH_LY_HD IS NULL
            ORDER BY NVL(cs.STT_SO_DOC_MOI, cs.STT_SO_DOC)
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
        var employee = await _context.DmNhanVien.AsNoTracking()
            .FirstOrDefaultAsync(x => x.USR == USER, cancellationToken);
        if (employee is null)
            return JsonObject(new[] { new { ROOT = "11- Tên đăng nhập không đúng." } });
        if (employee.PAS != PASSWORD)
            return JsonObject(new[] { new { ROOT = "12- Mật khẩu không đúng." } });
        return JsonObject(new[] { new { ROOT = "00- OK", MA_BIEN_DOC = employee.MA_NHAN_VIEN, MA_XI_NGHIEP = employee.MA_CHI_NHANH } });
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

        var employee = await _context.DmNhanVien.AsNoTracking()
            .FirstOrDefaultAsync(x => x.MA_NHAN_VIEN == meterReader, cancellationToken);
        if (employee is null)
            return JsonObject(new[] { new { ROOT = "11- Tên đăng nhập không đúng." } });

        return JsonObject(new[] { new { ROOT = "00- OK", MA_BIEN_DOC = employee.MA_NHAN_VIEN, MA_XI_NGHIEP = employee.MA_CHI_NHANH } });
    }

    public async Task<ContentResult> P_011_DANG_NHAP_DOI_MAT_KHAU(string? MA_BIEN_DOC, string? PASSWORD_OLD, string? PASSWORD_NEW1, string? PASSWORD_NEW2, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(PASSWORD_NEW1) && string.IsNullOrEmpty(PASSWORD_NEW2))
            return JsonObject(new[] { new { ROOT = "14- Dữ liệu không hợp lệ." } });
        if (PASSWORD_NEW1 != PASSWORD_NEW2)
            return JsonObject(new[] { new { ROOT = "15- Mật khẩu mới không khớp. Vui lòng thử lại!" } });
        var employee = await _context.DmNhanVien
            .FirstOrDefaultAsync(x => x.MA_NHAN_VIEN == MA_BIEN_DOC && x.PAS == PASSWORD_OLD, cancellationToken);
        if (employee is null)
            return JsonObject(new[] { new { ROOT = "12- Mật khẩu không đúng." } });
        employee.PAS = PASSWORD_NEW1;
        await _context.SaveChangesAsync(cancellationToken);
        return JsonObject(new[] { new { ROOT = "00- OK" } });
    }

    public async Task<ContentResult> P_012_LAY_GT_CANH_BAO(string? MA_BIEN_DOC, string? MA_XI_NGHIEP, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var value = await _context.DmNhanVien.AsNoTracking()
            .Where(x => x.MA_NHAN_VIEN == MA_BIEN_DOC)
            .Select(x => new { ROOT = "00- OK", x.CANH_BAO_PT, x.CANH_BAO_M3, x.CANH_BAO_GIAM_PT, x.CANH_BAO_GIAM_M3 })
            .FirstOrDefaultAsync(cancellationToken);
        return JsonObject(new[] { value ?? new { ROOT = "10- Tài khoản hoặc mật khẩu không đúng.", CANH_BAO_PT = (decimal?)null, CANH_BAO_M3 = (decimal?)null, CANH_BAO_GIAM_PT = (decimal?)null, CANH_BAO_GIAM_M3 = (decimal?)null } });
    }

    public async Task<ContentResult> P_013_LAY_PHIEN_BAN_APP(string? MA_BIEN_DOC, string? MA_XI_NGHIEP, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var value = await _context.AppDhUpdate.AsNoTracking()
            .Select(x => new { ROOT = "00- OK", x.BAN_CAP_NHAT, x.DUONG_DAN, x.NGAY_CAP_NHAT })
            .FirstOrDefaultAsync(cancellationToken);
        return JsonObject(new[] { value ?? new { ROOT = "10- Không có phiên bản cập nhật.", BAN_CAP_NHAT = (string?)null, DUONG_DAN = (string?)null, NGAY_CAP_NHAT = (DateTime?)null } });
    }

    public async Task<ContentResult> P_01_DANG_NHAP_LUU_TOKEN(string? TOKEN, string? VER_CODE, string? MA_BIEN_DOC, string? MA_XI_NGHIEP, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var employee = await _context.DmNhanVien.FirstOrDefaultAsync(
            x => x.MA_NHAN_VIEN == MA_BIEN_DOC && x.MA_CHI_NHANH == MA_XI_NGHIEP, cancellationToken);
        if (employee is null)
            return JsonObject(new[] { new { ROOT = "16- Dữ liệu không tìm thấy." } });
        employee.TOKEN = TOKEN;
        employee.VER_CODE = VER_CODE;
        employee.LOG_DATE = DateTime.Now;
        await _context.SaveChangesAsync(cancellationToken);
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
        var rows = await _context.AppDhChiSo
            .Where(x => x.NGAY_EBILL_NHAN_KHOA == null
                        && x.NGAY_EBILL_NAP_BILL == null
                        && x.NGAY_BD_NHAN_KHOA == null
                        && x.MA_BIEN_DOC == MA_BIEN_DOC
                        && x.THANG == THANG
                        && x.MA_SO_DOC == DANH_SACH_MA_SO_DOC)
            .ToListAsync(cancellationToken);
        var now = DateTime.Now;
        foreach (var row in rows)
            row.NGAY_BD_NHAN_KHOA = now;
        await _context.SaveChangesAsync(cancellationToken);
        return JsonObject(new[] { new { ROOT = "00- OK" } });
    }

    public Task<ContentResult> P_03_LAY_DS_KHACH_HANG(string? ID_DOC_opt, string? STT_SO_DOC_opt, string? MA_KH_opt, string? TEN_KH_opt, string? DIA_CHI_DH_opt, string? PHONE_KH_opt, string? MA_SO_DOC, string? MA_BIEN_DOC, string? THANG, string? KIEU_LOC, string? SAP_XEP_THEO, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        GetCustomersForReading(ID_DOC_opt, STT_SO_DOC_opt, MA_KH_opt, TEN_KH_opt, DIA_CHI_DH_opt, PHONE_KH_opt, MA_SO_DOC, MA_BIEN_DOC, THANG, KIEU_LOC, false, true, cancellationToken);

    public Task<ContentResult> P_03_LAY_DS_KHACH_HANG_SUB(string? ID_DOC_opt, string? STT_SO_DOC_opt, string? MA_KH_opt, string? TEN_KH_opt, string? DIA_CHI_DH_opt, string? PHONE_KH_opt, string? MA_SO_DOC, string? MA_BIEN_DOC, string? THANG, string? KIEU_LOC, string? SAP_XEP_THEO, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        GetCustomersForReading(ID_DOC_opt, STT_SO_DOC_opt, MA_KH_opt, TEN_KH_opt, DIA_CHI_DH_opt, PHONE_KH_opt, MA_SO_DOC, MA_BIEN_DOC, THANG, KIEU_LOC, false, true, cancellationToken);

    public Task<ContentResult> P_03_LAY_DS_KHACH_HANG_SUB_BS(string? ID_DOC_opt, string? STT_SO_DOC_opt, string? MA_KH_opt, string? TEN_KH_opt, string? DIA_CHI_DH_opt, string? PHONE_KH_opt, string? MA_SO_DOC, string? MA_BIEN_DOC, string? THANG, string? KIEU_LOC, string? SAP_XEP_THEO, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        GetCustomersForReading(ID_DOC_opt, STT_SO_DOC_opt, MA_KH_opt, TEN_KH_opt, DIA_CHI_DH_opt, PHONE_KH_opt, MA_SO_DOC, MA_BIEN_DOC, THANG, KIEU_LOC, true, true, cancellationToken);

    public Task<ContentResult> P_0313_LAY_DS_KHACH_HANG_TRA_CUU(string? MA_SO_DOC, string? MA_BIEN_DOC, string? MA_XI_NGHIEP, string? THANG, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        GetCustomersForReading(null, null, null, null, null, null, MA_SO_DOC, MA_BIEN_DOC, THANG, "0", false, false, cancellationToken);

    public async Task<ContentResult> P_043_LO_TRINH_DI_DOC_MAP(string? MA_SO_DOC, string? MA_BIEN_DOC, string? MA_XI_NGHIEP, string? THANG, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await (from reading in _context.AppDhChiSo.AsNoTracking()
            join customer in _context.ThongTinKh.AsNoTracking() on reading.MA_KHACH_HANG equals customer.MA_KHACH_HANG
            where reading.MA_SO_DOC == MA_SO_DOC && reading.MA_BIEN_DOC == MA_BIEN_DOC && reading.MA_CHI_NHANH == MA_XI_NGHIEP && reading.THANG == THANG
            orderby reading.STT_SO_DOC_MOI ?? reading.STT_SO_DOC
            select new { ROOT = "00- OK", ID_DONG_HO = reading.ID_DCS, reading.MA_KHACH_HANG, customer.TEN_KHACH_HANG, customer.DIA_CHI_DONG_HO, customer.PHONE_UT1, reading.VI_TRI_DOC, reading.STT_SO_DOC, reading.STT_SO_DOC_MOI }).ToListAsync(cancellationToken);
        return JsonObject(rows.Count == 0 ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy." } } : rows.Cast<object>());
    }

    public async Task<ContentResult> P_044_LUU_TEN_FILE_ANH(string? ID_DONG_HO, string? TEN_FILE_ANH, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var row = await _context.AppDhChiSo.FirstOrDefaultAsync(x => x.ID_DCS == ID_DONG_HO, cancellationToken);
        if (row is null) return JsonObject(new[] { new { ROOT = "16- Dữ liệu không tìm thấy." } });
        row.TEN_FILE_ANH = (TEN_FILE_ANH ?? string.Empty).Replace(".jpg", "", StringComparison.OrdinalIgnoreCase) + ".jpg";
        await _context.SaveChangesAsync(cancellationToken);
        return JsonObject(new[] { new { ROOT = "00- OK" } });
    }

    public async Task<ContentResult> P_032_LAY_SL_BINH_QUAN_3T(string? MA_KHACH_HANG, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var today = DateTime.Today;
        var value = await _context.ChiSoDhSub.AsNoTracking()
            .Where(x => x.MA_KHACH_HANG == MA_KHACH_HANG
                        && x.NGAY_DINH_KY >= today.AddDays(-360)
                        && x.NGAY_DINH_KY <= today
                        && x.SLTB3T > 0)
            .OrderByDescending(x => x.ID_CS)
            .Select(x => x.SLTB3T)
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

            var info = await _context.AppDhChiSo
                .AsNoTracking()
                .Where(x =>
                    x.MA_BIEN_DOC == MA_BIEN_DOC &&
                    x.ID_DCS == ID_DONG_HO)
                .Select(x => new
                {
                    x.SL_TB_3THANG,

                    LA_CO_QUAN = _context.ThongTinKh
                        .Any(k =>
                            k.MA_KHACH_HANG == MA_KHACH_HANG &&
                            k.LOAI_KHACH_HANG == "N")
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
                info.SL_TB_3THANG ?? 0);

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
        var row = await _context.AppDhChiSo.FirstOrDefaultAsync(x => x.ID_DCS == ID_DONG_HO && x.MA_KHACH_HANG == MA_KHACH_HANG && x.MA_SO_DOC == MA_SO_DOC && x.THANG == THANG, cancellationToken);
        if (row is null) return JsonObject(new[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } });
        row.STT_SO_DOC_MOI = newIndex;
        await _context.SaveChangesAsync(cancellationToken);
        return JsonObject(new[] { new { ROOT = "00- OK" } });
    }

    public Task<ContentResult> P_041_NHAP_XUAT_CS_LE_ONLINE_SUB(string? ID_DONG_HO, string? MA_TINH_TRANG_DH, string? QUA_VONG, string? LOAI_CHI_SO, string? NGAY_DOC_CS, string? CHI_SO_MOI, string? SAN_LUONG_TT, string? TONG_SL, string? CONG_CHI_SO, string? SAN_LUONG_DUNG_IT, string? MA_GHI_CHU, string? GHI_CHU, string? VI_TRI_DOC, string? MA_KHACH_HANG, string? MA_SO_DOC, string? THANG, string? MA_XI_NGHIEP, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        SaveMeterReading(ID_DONG_HO, MA_TINH_TRANG_DH, QUA_VONG, LOAI_CHI_SO, NGAY_DOC_CS, CHI_SO_MOI, SAN_LUONG_TT, TONG_SL, CONG_CHI_SO, SAN_LUONG_DUNG_IT, MA_GHI_CHU, GHI_CHU, VI_TRI_DOC, MA_KHACH_HANG, MA_SO_DOC, THANG, MA_XI_NGHIEP, MA_BIEN_DOC, cancellationToken);

    public async Task<ContentResult> P_051_KIEM_TRA_BAN_GIAO_SD(string? MA_SO_DOC, string? MA_BIEN_DOC, string? MA_XI_NGHIEP, string? THANG, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var handedOver = await _context.AppDhChiSo.AsNoTracking().AnyAsync(x => x.THANG == THANG && x.MA_CHI_NHANH == MA_XI_NGHIEP && x.MA_BIEN_DOC == MA_BIEN_DOC && x.MA_SO_DOC == MA_SO_DOC && x.NGAY_BIEN_DOC_BG != null, cancellationToken);
        return JsonObject(new[] { new { ROOT = handedOver ? "01- TRUE" : "00- FALSE" } });
    }

    public async Task<ContentResult> P_05_BD_BAN_GIAO_CS_XONG(string? DANH_SACH_MA_SO_DOC, string? MA_BIEN_DOC, string? THANG, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var readingBooks = (DANH_SACH_MA_SO_DOC ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => x.Trim('\'', '"'))
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToArray();

        if (readingBooks.Length == 0)
            return JsonObject(new[] { new { ROOT = "01- Thieu ma so doc!" } });

        var query = _context.AppDhChiSo
            .Where(x =>
                x.NGAY_EBILL_NHAN_KHOA == null &&
                x.NGAY_EBILL_NAP_BILL == null &&
                x.NGAY_BD_NHAN_KHOA != null &&
                x.MA_BIEN_DOC == MA_BIEN_DOC &&
                x.THANG == THANG &&
                x.MA_SO_DOC != null &&
                readingBooks.Contains(x.MA_SO_DOC));

        var hasUnreadMeter = await query
            .AsNoTracking()
            .AnyAsync(x => x.NGAY_DOC_TUNG_DH == null, cancellationToken);

        if (hasUnreadMeter)
            return JsonObject(new[] { new { ROOT = "18- Chưa đọc xong. Vui lòng kiểm tra lại!" } });

        await query.ExecuteUpdateAsync(
            setters => setters.SetProperty(x => x.NGAY_BIEN_DOC_BG, DateTime.Now),
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

        var customer = await _context.ThongTinKh
            .AsNoTracking()
            .Where(x => x.MA_KHACH_HANG == maKh && x.NGAY_THANH_LY_HD == null)
            .Select(x => new
            {
                x.MA_KHACH_HANG,
                x.TEN_KHACH_HANG,
                x.DIA_CHI_KHACH_HANG,
                DIA_CHI_LAP_DAT = x.DIA_CHI_DONG_HO,
                DIEN_THOAI = x.PHONE_UT1,
                EMAIL = x.EMAIL_UT1,
                x.SO_HOP_DONG,
                x.SO_HO,
                x.SO_KHAU,
                x.DINH_MUC,
                x.MA_GIA,
                x.TEN_GIA_NUOC,
                SERIAL_DH = x.SO_SERIAL_DONG_HO,
                x.TEN_DONG_HO,
                x.NGAY_LAP_DAT,
                x.BIEN_DOC,
                MUC_DICH_SU_DUNG = x.HT_KD,
                XN_CAP_NUOC = x.CHI_NHANH
            })
            .FirstOrDefaultAsync(cancellationToken);

        var customerMs = stepWatch.ElapsedMilliseconds;
        if (customer is null)
            return JsonObject(new[] { new { ROOT = "16- Du lieu khong tim thay. Vui long kiem tra lai!" } });

        stepWatch.Restart();
        var duNo = await GetTongDuNoTheoMaKhV2Async(maKh, cancellationToken);
        var debtMs = stepWatch.ElapsedMilliseconds;

        stepWatch.Restart();
        var bq3t = await _context.ChiSoDhSub
            .AsNoTracking()
            .Where(x =>
                x.MA_KHACH_HANG == maKh &&
                x.SLTB3T > 0)
            .Select(x => x.SLTB3T)
            .FirstOrDefaultAsync(cancellationToken);
        var averageMs = stepWatch.ElapsedMilliseconds;

        var ngayLapDat = customer.NGAY_LAP_DAT?.ToString("dd/MM/yyyy") ?? string.Empty;
        var result = new[]
        {
            new
            {
                ROOT = "00- OK",
                customer.MA_KHACH_HANG,
                customer.TEN_KHACH_HANG,
                customer.DIA_CHI_KHACH_HANG,
                customer.DIA_CHI_LAP_DAT,
                customer.DIEN_THOAI,
                customer.EMAIL,
                customer.SO_HOP_DONG,
                customer.SO_HO,
                customer.SO_KHAU,
                customer.DINH_MUC,
                customer.MA_GIA,
                customer.TEN_GIA_NUOC,
                customer.SERIAL_DH,
                customer.TEN_DONG_HO,
                NGAY_LAP_DAT = ngayLapDat,
                customer.BIEN_DOC,
                customer.MUC_DICH_SU_DUNG,
                customer.XN_CAP_NUOC,
                DU_NO = duNo,
                BQ3T = bq3t
            }
        };

        Console.WriteLine($"P_81 timings MA_KHACH_HANG={maKh}: KH={customerMs}ms, DU_NO={debtMs}ms, BQ3T={averageMs}ms, TOTAL={totalWatch.ElapsedMilliseconds}ms");
        return JsonObject(result);
    }

    // ======================================================================
    // GET TỔNG DƯ NỢ THEO MÃ KHÁCH HÀNG
    // Convert từ: get_Tong_Du_No_Theo_maKH_V2
    // ======================================================================
    private async Task<string> GetTongDuNoTheoMaKhV2Async(
        string maKH,
        CancellationToken cancellationToken = default)
    {
        long tienNo;
        var dieuChinhGiamDau = await _context.CongNo
            .AsNoTracking()
            .Where(x =>
                x.MA_KHACH_HANG == maKH &&
                x.TONG_TIEN < 0 &&
                x.TRANG_THAI == 1 &&
                x.MA_DON_VI == 0 &&
                x.LOAI_HOA_DON == 5)
            .OrderByDescending(x => x.NGAY_HD_PHAT_HANH)
            .Select(x => new
            {
                x.TONG_TIEN,
                x.SO_HOA_DON_THAY_THE
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (dieuChinhGiamDau is not null)
        {
            tienNo = await GetTongTienNoDieuChinhSubAsync(
                maKH,
                dieuChinhGiamDau.TONG_TIEN,
                dieuChinhGiamDau.SO_HOA_DON_THAY_THE,
                cancellationToken);
        }
        else
        {
            // VB:
            //
            // SELECT SUM(
            //      TONG_TIEN
            //      - TONG_THANH_TOAN_BILL
            //      - TONG_TIEN_GIAM_TRU
            // )
            // FROM CONG_NO
            // WHERE MA_KHACH_HANG = maKH
            // AND TRANG_THAI = 1
            // AND MA_DON_VI = 0
            // AND TONG_TIEN >
            //     (TONG_THANH_TOAN_BILL + TONG_TIEN_GIAM_TRU)
            // AND LOAI_HOA_DON <> '5'

            var tongNo = await _context.CongNo
                .AsNoTracking()
                .Where(x =>
                    x.MA_KHACH_HANG == maKH &&
                    x.TRANG_THAI == 1 &&
                    x.MA_DON_VI == 0 &&
                    x.LOAI_HOA_DON != 5)
                .Select(x => new
                {
                    Total = x.TONG_TIEN ?? 0,
                    Paid = (x.TONG_THANH_TOAN ?? 0) > (x.TONG_THANH_TOAN_BILL ?? 0)
                        ? (x.TONG_THANH_TOAN ?? 0)
                        : (x.TONG_THANH_TOAN_BILL ?? 0),
                    Discount = x.TONG_TIEN_GIAM_TRU ?? 0
                })
                .Where(x => x.Total > x.Paid + x.Discount)
                .Select(x => (decimal?)(x.Total - x.Paid - x.Discount))
                .SumAsync(cancellationToken);

            tienNo = Convert.ToInt64(tongNo ?? 0m);
        }

        if (tienNo <= 0)
            return "Hết nợ";

        return tienNo.ToString(CultureInfo.InvariantCulture);
    }


    // ======================================================================
    // KIỂM TRA KHÁCH HÀNG CÓ HÓA ĐƠN ĐIỀU CHỈNH GIẢM KHÔNG
    // Convert từ: kiem_tra_dc_giam
    // ======================================================================
    private async Task<bool> KiemTraDcGiamAsync(
        string custId,
        CancellationToken cancellationToken = default)
    {
        return await _context.CongNo
            .AsNoTracking()
            .AnyAsync(x =>
                x.MA_KHACH_HANG == custId &&
                x.TONG_TIEN < 0 &&
                x.TRANG_THAI == 1 &&
                x.MA_DON_VI == 0 &&
                x.LOAI_HOA_DON == 5,
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

        var dsSoHoaDonThayThe = await _context.CongNo
            .AsNoTracking()
            .Where(x =>
                x.MA_KHACH_HANG == custId &&
                x.TONG_TIEN < 0 &&
                x.TRANG_THAI == 1 &&
                x.MA_DON_VI == 0 &&
                x.LOAI_HOA_DON == 5 &&
                x.SO_HOA_DON_THAY_THE != null)
            .Select(x => x.SO_HOA_DON_THAY_THE!)
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

        var hoaDonCanDc = await _context.CongNo
            .AsNoTracking()
            .Where(x => x.SO_HOA_DON == shdCanDcGiam)
            .Select(x => new
            {
                x.NGAY_HD_PHAT_HANH,
                x.TONG_THANH_TOAN_BILL,
                x.TONG_TIEN
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (hoaDonCanDc == null)
            return 0L;


        var tongTienCanDcGiam =
            Convert.ToDecimal(hoaDonCanDc.TONG_TIEN);

        var tongTtCanDcGiam =
            Convert.ToDecimal(hoaDonCanDc.TONG_THANH_TOAN_BILL);


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

        if (hoaDonCanDc.NGAY_HD_PHAT_HANH == null)
            return 0L;

        var ngayPhatHanh =
            Convert.ToDateTime(hoaDonCanDc.NGAY_HD_PHAT_HANH);

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

        var query = _context.CongNo
            .AsNoTracking()
            .Where(x =>
                x.MA_KHACH_HANG == custId &&
                x.TRANG_THAI == 1 &&
                x.MA_DON_VI == 0);


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
                    x.LOAI_HOA_DON == 5 &&

                    x.TONG_TIEN >
                        (
                            x.TONG_THANH_TOAN_BILL +
                            x.TONG_TIEN_GIAM_TRU
                        ) &&

                    x.NGAY_HD_PHAT_HANH >= tuNgay &&
                    x.NGAY_HD_PHAT_HANH <= denNgay);
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
                    x.LOAI_HOA_DON != 5 &&

                    x.TONG_TIEN >
                        (
                            x.TONG_THANH_TOAN_BILL +
                            x.TONG_TIEN_GIAM_TRU
                        ) &&

                    !dsSoHoaDonThayThe.Contains(x.SO_HOA_DON) &&

                    x.NGAY_HD_PHAT_HANH >= tuNgay &&
                    x.NGAY_HD_PHAT_HANH <= denNgay);
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
                    x.TONG_TIEN !=
                        (
                            x.TONG_THANH_TOAN_BILL +
                            x.TONG_TIEN_GIAM_TRU
                        ) &&

                    x.NGAY_HD_PHAT_HANH >= tuNgay &&
                    x.NGAY_HD_PHAT_HANH <= denNgay);
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
                    x.TONG_TIEN
                    -
                    (
                        x.TONG_THANH_TOAN_BILL
                        -
                        x.TONG_TIEN_GIAM_TRU
                    )
                ))
            .SumAsync(cancellationToken);


        return Convert.ToInt64(tongNo ?? 0m);
    }
    public async Task<ContentResult> P_82_LAY_TT_HOA_DON(string? MA_KHACH_HANG, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var fromDate = DateTime.Today.AddDays(-150);
        var rows = await (
            from invoice in _context.CongNo.AsNoTracking()
            join unit in _context.LogBankDonVi.AsNoTracking() on invoice.MA_DON_VI equals unit.MA into units
            from unit in units.DefaultIfEmpty()
            where invoice.MA_KHACH_HANG == MA_KHACH_HANG && invoice.NGAY_HD_PHAT_HANH >= fromDate
            orderby invoice.SO_HOA_DON
            select new { invoice, UNIT_NAME = unit.TEN_SUB })
            .ToListAsync(cancellationToken);
        if (rows.Count == 0)
            return JsonObject(new[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } });
        return JsonObject(rows.Select(x =>
        {
            var paid = Math.Max(x.invoice.TONG_THANH_TOAN ?? 0, x.invoice.TONG_THANH_TOAN_BILL ?? 0);
            var status = x.invoice.TRANG_THAI?.ToString();
            return new
            {
                ROOT = "00- OK", x.invoice.SO_HOA_DON, NGAY_PHAT_HANH_HD = x.invoice.NGAY_HD_PHAT_HANH,
                THANG_HD = x.invoice.THANG, TONG_SAN_LUONG = x.invoice.TONG_SL, x.invoice.TONG_TIEN,
                NGAY_THANH_TOAN = x.invoice.NGAY_THANH_TOAN ?? x.invoice.NGAY_THANH_TOAN_BILL,
                TONG_THANH_TOAN = paid, x.invoice.TONG_TIEN_GIAM_TRU,
                HINH_THUC_TT = status == "2" ? "" : x.invoice.HINH_THUC_TT_BILL switch { "C" => "Tiền mặt", "B" => "Ủy nhiệm thu", "A" => "Chuyển khoản", "S" => "Khấu trừ nội bộ", _ => x.invoice.HINH_THUC_TT_BILL },
                NOI_THANH_TOAN = x.UNIT_NAME, x.invoice.SERI_HOA_DON,
                TRANG_THAI_HD = status switch { "0" => "Chưa phát hành", "1" => "Phát hành", "2" or "3" => "Hủy hóa đơn", "4" => "Trả dần", _ => "" },
                LOAI_HOA_DON = x.invoice.LOAI_HOA_DON?.ToString() switch { "1" => "Định kỳ", "2" => "Tài chính", "3" => "Truy thu", "4" => "Điều chỉnh tăng", "5" => "Điều chỉnh giảm", _ => "" },
                NGAY_HUY_HD = x.invoice.NGAY_HD_HUY,
                TINH_TRANG_NO = status == "2" ? "Hủy hóa đơn" : paid == x.invoice.TONG_TIEN ? "Hết nợ" : paid == 0 ? "Đang nợ" : "Thu một phần",
                GHI_CHU = x.invoice.DIEN_GIAI
            };
        }));
    }

    public async Task<ContentResult> P_83_LAY_TT_CHI_SO(string? SO_HOA_DON, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await _context.ChiSoDh.AsNoTracking().Where(x => x.SO_HOA_DON == SO_HOA_DON)
            .Select(x => new { x.CS_DAU, x.CS_CUOI, x.SAN_LUONG, x.LOAI_CHI_SO })
            .ToListAsync(cancellationToken);
        return JsonObject(rows.Count == 0
            ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } }
            : rows.Select(x => new
            {
                ROOT = "00- OK",
                CHI_SO_CU = x.CS_DAU,
                CHI_SO_MOI = x.CS_CUOI,
                x.SAN_LUONG,
                LOAI_CHI_SO = x.LOAI_CHI_SO == "0" ? "Định kỳ" : x.LOAI_CHI_SO == "6" ? "Chốt chỉ số" : x.LOAI_CHI_SO
            }).Cast<object>());
    }

    public async Task<ContentResult> P_84_LAY_TT_GIA_NUOC(string? SO_HOA_DON, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(SO_HOA_DON) && SO_HOA_DON.Length != 9)
            return JsonObject(new[] { new { ROOT = $"15- Độ dài dữ liệu không hợp lệ (MKH: {SO_HOA_DON})" } });

        var rows = await _context.CongNoGia.AsNoTracking()
            .Where(x => x.SO_HOA_DON == SO_HOA_DON)
            .Select(x => new
            {
                ROOT = "00- OK",
                SAN_LUONG = x.TONG_SL,
                DON_GIA = x.TIEN_GIA_CB,
                THANH_TIEN = x.THANH_TIEN,
                PHI = x.THUE_BVMT,
                THUE = x.VAT,
                TONG_TIEN = x.TONG_TIEN,
                TEN_GIA = x.TEN_GIA
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
            join type in _context.DmLoaiSmsEmail.AsNoTracking() on log.LOAI_SMS equals type.MA_LOAI
            join status in _context.DmTrangThaiSms.AsNoTracking() on log.KET_QUA equals status.MA_LOAI
            where log.MA_KHACH_HANG == MA_KHACH_HANG && log.NGAY_GUI >= fromDate
            orderby log.NGAY_GUI descending
            select new { ROOT = "00- OK", LOAI_SMS = type.TEN_LOAI, TRANG_THAI = status.TEN_LOAI, log.SO_DIEN_THOAI, log.NGAY_GUI, NOI_DUNG = log.NOI_DUNG_SMS }).ToListAsync(cancellationToken);
        return JsonObject(rows.Count == 0 ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy." } } : rows.Cast<object>());
    }

    public async Task<ContentResult> P_88_LAY_TT_CAT_MO_NUOC(string? MA_KHACH_HANG, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var customerCode = (MA_KHACH_HANG ?? string.Empty).PadLeft(9, '0');
        var catMoNuocRows = await _billing.Aereport.AsNoTracking()
            .Where(x => x.CUSTID == customerCode)
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
            from log in _context.LogEmail.AsNoTracking()
            join customer in _context.ThongTinKh.AsNoTracking()
                on log.MA_KHACH_HANG equals customer.MA_KHACH_HANG
            join emailType in _context.DmLoaiSmsEmail.AsNoTracking()
                on log.LOAI_EMAIL equals emailType.MA_LOAI
            join status in _context.DmTrangThaiEmail.AsNoTracking()
                on log.KET_QUA equals status.MA_LOAI
            where log.MA_KHACH_HANG == MA_KHACH_HANG
                  && log.NGAY_GUI >= fromDate
                  && log.NGAY_GUI <= toDate
            orderby log.NGAY_GUI descending
            select new
            {
                ROOT = "00- OK",
                LOAI_EMAIL = emailType.TEN_LOAI,
                TRANG_THAI = status.TEN_LOAI,
                EMAIL = log.EMAIL,
                NGAY_GUI = log.NGAY_GUI
            })
            .ToListAsync(cancellationToken);

        return JsonObject(rows.Count == 0
            ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } }
            : rows.Cast<object>());
    }

    public async Task<ContentResult> P_96_CC_DM_YEU_CAU(string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await _context.CcDmNoiDungYc.AsNoTracking()
            .Where(x => x.APP_DH == "1")
            .OrderBy(x => x.STT)
            .Select(x => new { ROOT = "00- OK", MA_LOAI_YEU_CAU = x.ID_NOI_DUNG, TEN_LOAI_YEU_CAU = x.NOI_DUNG_YC })
            .ToListAsync(cancellationToken);
        return JsonObject(rows);
    }

    public async Task<ContentResult> P_97_CC_DM_QUAN(string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await _context.KDmDiaChinh.AsNoTracking()
            .Where(x => x.PARENT_MA == "0001")
            .OrderBy(x => x.MA_DIA_CHINH)
            .Select(x => new { ROOT = "00- OK", MA_PHUONG = x.MA_DIA_CHINH, TEN_PHUONG = x.TEN_DIA_CHINH })
            .ToListAsync(cancellationToken);
        return JsonObject(rows);
    }

    public async Task<ContentResult> P_98_CC_DM_PHUONG(string? MA_BIEN_DOC, string? MA_QUAN, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await _context.KDmDiaChinh.AsNoTracking()
            .Where(x => x.MA_PHUONG == null && x.MA_QUAN == MA_QUAN)
            .OrderBy(x => x.MA_DIA_CHINH)
            .Select(x => new { ROOT = "00- OK", MA_PHUONG = x.MA_DIA_CHINH, TEN_PHUONG = x.TEN_DIA_CHINH })
            .ToListAsync(cancellationToken);
        return JsonObject(rows);
    }

    public async Task<ContentResult> P_91_DM_XI_NGHIEP(string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await _context.Dm01DonVi.AsNoTracking()
            .Where(x => x.KY_HIEU != null && x.KY_HIEU != "0")
            .OrderBy(x => x.MA_DON_VI)
            .Select(x => new { ROOT = "00- OK", MA = x.MA_DON_VI, TEN = x.MA_DON_VI + "- " + x.TEN_DON_VI })
            .ToListAsync(cancellationToken);
        return JsonObject(rows);
    }

    public async Task<ContentResult> P_92_DM_BIEN_DOC(string? MA_XI_NGHIEP, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await _context.DmNhanVien.AsNoTracking()
            .Where(x => x.DOC_CHI_SO == "1" && x.MA_CHI_NHANH == MA_XI_NGHIEP)
            .OrderBy(x => x.TEN_NHAN_VIEN)
            .Select(x => new { ROOT = "00- OK", MA = x.MA_NHAN_VIEN, TEN = x.MA_NHAN_VIEN + "- " + x.TEN_NHAN_VIEN })
            .ToListAsync(cancellationToken);
        return JsonObject(rows);
    }

    public async Task<ContentResult> P_921_DM_SO_DOC(string? opt_MA_BIEN_DOC, string? MA_XI_NGHIEP, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var query = _context.DmSoDoc.AsNoTracking()
            .Where(x => x.MA_CHI_NHANH == MA_XI_NGHIEP && x.HIEU_LUC == "1");
        if (!string.IsNullOrWhiteSpace(opt_MA_BIEN_DOC))
        {
            query = query.Where(x => x.MA_BIEN_DOC == opt_MA_BIEN_DOC);
        }
        var rows = await query.OrderBy(x => x.MA_SO_DOC)
            .Select(x => new { ROOT = "00- OK", MA = x.MA_SO_DOC, TEN = x.MA_SO_DOC + "- " + x.TEN_SO_DOC, x.NGAY_DOC })
            .ToListAsync(cancellationToken);
        return JsonObject(rows.Count == 0 ? new[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } } : rows.Cast<object>());
    }

    public async Task<ContentResult> P_99_DM_GHI_CHU(string? MA_XI_NGHIEP, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await _context.AppDhDmGhiChu.AsNoTracking()
            .Where(x => x.NHOM == "DCS" || x.NHOM == null)
            .OrderBy(x => x.STT)
            .Select(x => new { ROOT = "00- OK", MA = x.MA_GHI_CHU, TEN = x.NOI_DUNG_GHI_CHU })
            .ToListAsync(cancellationToken);
        return JsonObject(rows);
    }

    public async Task<ContentResult> P_931_DM_DIEM_THU_HO(string? MA_DIEM_THU, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        decimal? locationCode = decimal.TryParse(MA_DIEM_THU, out var parsedCode) ? parsedCode : null;
        var query =
            from location in _context.DmDiemThuTien.AsNoTracking()
            join unit in _context.LogBankDonVi.AsNoTracking()
                on location.MA_DON_VI_THU equals unit.MA into units
            from unit in units.DefaultIfEmpty()
            where location.HIEU_LUC == "0"
                  && location.MA_DON_VI_THU != 29
                  && (string.IsNullOrEmpty(MA_DIEM_THU) || location.MA_DON_VI_THU == locationCode)
            select new
            {
                ROOT = "00- OK",
                TEN_DON_VI_THU = unit.TEN == null ? null : unit.TEN.Replace("&", " và "),
                TEN_DIEM_GD = location.TEN_DIEM_GD == null ? null : location.TEN_DIEM_GD.Replace("&", ""),
                DIA_CHI_GD = location.DIA_CHI_GD == null ? null : location.DIA_CHI_GD.Replace("&", ""),
                THOI_GIAN_GD = "_ _:_ _",
                location.VI_TRI_DIEM_THU,
                location.DIA_CHI_GOOGLE
            };
        var rows = await query.ToListAsync(cancellationToken);
        return JsonObject(rows.Count == 0
            ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } }
            : rows.Cast<object>());
    }

    public async Task<ContentResult> P_93_DM_TINH_TRANG_DH(string? MA_TINH_TRANG_DH, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var query = _context.DmTinhTrangDongHo.AsNoTracking().Where(x => x.HIEU_LUC == "1");
        if (!string.IsNullOrWhiteSpace(MA_TINH_TRANG_DH))
        {
            query = query.Where(x => x.MA_TINH_TRANG_SO == MA_TINH_TRANG_DH);
        }
        var rows = await query.OrderBy(x => x.STT_HIEN_THI)
            .Select(x => new
            {
                ROOT = "00- OK",
                MA = x.MA_TINH_TRANG_SO,
                TEN = x.MO_TA_SUB,
                x.N_SUA_CHI_SO_CU,
                x.N_NHAP_CHI_SO_MOI,
                x.N_NHAP_SL_TRUC_TIEP,
                x.N_CONG_DON_CHI_SO
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
        var value = await _context.AppDhThangDoc.AsNoTracking()
            .OrderByDescending(x => x.ID_THANG_DOC)
            .Select(x => new { x.THANG, x.NAM })
            .FirstOrDefaultAsync(cancellationToken);
        return JsonObject(new[]
        {
            new { ROOT = value is null ? "16- Dữ liệu không tìm thấy." : "00- OK", THANG_DOC = value is null ? null : $"{value.THANG}/{value.NAM}" }
        });
    }

    public async Task<ContentResult> PF_02_LAY_CHI_SO_THAO_LAP(string? MA_KHACH_HANG, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var value = await _context.AppDhThiCong.AsNoTracking()
            .Where(x => x.MA_KHACH_HANG == MA_KHACH_HANG)
            .OrderByDescending(x => x.ID_TC)
            .Select(x => new { ROOT = "00- OK", x.CHI_SO_THAO, x.CHI_SO_LAP })
            .FirstOrDefaultAsync(cancellationToken);
        return JsonObject(new[] { value ?? new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!", CHI_SO_THAO = (decimal?)null, CHI_SO_LAP = (decimal?)null } });
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

        async Task<(decimal VatRate, long TienPhi)> TienThuePhiAsync(
            string maGiaCB,
            int slSD)
        {
            if (string.IsNullOrWhiteSpace(maGiaCB))
                return (0m, 0L);

            maGiaCB = maGiaCB.Trim();

            // --------------------------------------------------------
            // 1. Lấy loại giá SH / SX / KD / HC...
            // --------------------------------------------------------

            var loai = await _context.KDmGiaSub
                .AsNoTracking()
                .Where(x => x.MA_GIA == maGiaCB)
                .Select(x => x.LOAI)
                .FirstOrDefaultAsync(cancellationToken);

            decimal tyLePhi;

            if (loai == "SH")
            {
                // Sinh hoạt: 100%
                tyLePhi = 1m;
            }
            else
            {
                // Các loại khác: 80%
                //
                // VB cũ:
                // Nếu không tìm thấy K_DM_GIA_SUB thì mặc định 100%.
                if (loai == null)
                    tyLePhi = 1m;
                else
                    tyLePhi = 0.8m;
            }

            // VB:
            // slDaChia = slSD * tylePhi
            long slDaChia = Convert.ToInt64(slSD * tyLePhi);

            // --------------------------------------------------------
            // 2. Lấy MA_THUE_VAT + MA_PHI_BVMT của GIÁ CƠ BẢN
            // --------------------------------------------------------

            var giaCoBan = await _context.KDmGia
                .AsNoTracking()
                .Where(x =>
                    x.HIEU_LUC == "1" &&
                    x.KIEU_GIA == "0" &&
                    x.KY_HIEU_GIA == maGiaCB)
                .Select(x => new
                {
                    x.MA_THUE_VAT,
                    x.MA_PHI_BVMT
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (giaCoBan == null)
                return (0m, 0L);

            // --------------------------------------------------------
            // 3. VAT
            // --------------------------------------------------------

            decimal vatRate = 0m;

            if (!string.IsNullOrWhiteSpace(giaCoBan.MA_THUE_VAT))
            {
                var vatGiaTri = await _context.KDmGiaPhi
                    .AsNoTracking()
                    .Where(x => x.MA_PHI == giaCoBan.MA_THUE_VAT)
                    .Select(x => x.GIA_TRI)
                    .FirstOrDefaultAsync(cancellationToken);

                vatRate = Convert.ToDecimal(vatGiaTri) / 100m;
            }

            // --------------------------------------------------------
            // 4. PHÍ BVMT / NT
            // --------------------------------------------------------

            long tienPhi = 0L;

            if (!string.IsNullOrWhiteSpace(giaCoBan.MA_PHI_BVMT))
            {
                var phi = await _context.KDmGiaPhi
                    .AsNoTracking()
                    .Where(x => x.MA_PHI == giaCoBan.MA_PHI_BVMT)
                    .Select(x => new
                    {
                        x.GIA_TRI,
                        x.KIEU_PHI
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (phi != null)
                {
                    decimal giaTri = Convert.ToDecimal(phi.GIA_TRI);

                    if (phi.KIEU_PHI == "$")
                    {
                        // VB:
                        // tienPhi = slDaChia * giatri
                        tienPhi = Convert.ToInt64(
                            slDaChia * giaTri);
                    }
                    else if (phi.KIEU_PHI == "%")
                    {
                        // VB:
                        // tienPhi = slDaChia * (giatri / 100)
                        tienPhi = Convert.ToInt64(
                            slDaChia * (giaTri / 100m));
                    }
                }
            }

            return (vatRate, tienPhi);
        }

        // ============================================================
        // GIÁ PHẦN TRĂM
        //
        // VB:
        // tinhBacThangGiaPhanTram
        //
        // Return:
        // ThanhTien
        // ThueVat
        // TienPhi
        // ============================================================

        async Task<(long ThanhTien, long ThueVat, long TienPhi)>
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

                var thuePhi = await TienThuePhiAsync(
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

        async Task<(
            long ThanhTien,
            long ThueVat,
            long TienPhi,
            int SlConLai)>
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

                var thuePhi = await TienThuePhiAsync(
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
                x.HIEU_LUC == "1" &&
                x.KY_HIEU_GIA == MA_GIA)
            .Select(x => new
            {
                x.CHUOI_GIA_DM,
                x.CHUOI_GIA_GT,
                x.KIEU_TINH
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

        var chuoiGiaDm =
            price.CHUOI_GIA_DM ?? string.Empty;

        var chuoiGiaGt =
            price.CHUOI_GIA_GT ?? string.Empty;

        var kieuTinh =
            price.KIEU_TINH ?? string.Empty;

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
                                        await TienThuePhiAsync(
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
                                        await TienThuePhiAsync(
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
                            await TinhBacThangGiaPhanTramAsync(
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
                            await TienThuePhiAsync(
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
                            await TinhBacThangGiaGopAsync(
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
                            await TinhBacThangGiaPhanTramAsync(
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
                                    await TinhBacThangGiaGopAsync(
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
                                    await TinhBacThangGiaPhanTramAsync(
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
                                await TinhBacThangGiaGopAsync(
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
                                await TinhBacThangGiaPhanTramAsync(
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
                                await TinhBacThangGiaGopAsync(
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
                                await TinhBacThangGiaPhanTramAsync(
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
                                await TinhBacThangGiaGopAsync(
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
                                await TinhBacThangGiaPhanTramAsync(
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
        var row = await _context.AppDhChiSo.FirstOrDefaultAsync(x => x.MA_KHACH_HANG == r.MA_KHACH_HANG && x.MA_CHI_NHANH == r.MA_XI_NGHIEP && x.MA_BIEN_DOC == r.MA_BIEN_DOC && x.THANG == r.THANG, cancellationToken);
        if (row is null) return JsonObject(new[] { new { ROOT = "16- Dữ liệu không tìm thấy." } });
        var changed = false;
        if (!string.IsNullOrWhiteSpace(r.TEN_KHACH_HANG)) { row.SUA_TEN_KHACH_HANG = r.TEN_KHACH_HANG; changed = true; }
        if (!string.IsNullOrWhiteSpace(r.DIA_CHI_DONG_HO)) { row.SUA_DIA_CHI_DONG_HO = r.DIA_CHI_DONG_HO; changed = true; }
        if (!string.IsNullOrWhiteSpace(r.SO_DT)) { row.SUA_SO_DT = r.SO_DT; changed = true; }
        if (!string.IsNullOrWhiteSpace(r.EMAIL)) { row.SUA_EMAIL = r.EMAIL; changed = true; }
        if (!string.IsNullOrWhiteSpace(r.DONG_HO_TEN)) { row.SUA_DH_TEN = r.DONG_HO_TEN; changed = true; }
        if (!string.IsNullOrWhiteSpace(r.DONG_HO_SERIAL)) { row.SUA_DH_SERIAL = r.DONG_HO_SERIAL; changed = true; }
        if (!string.IsNullOrWhiteSpace(r.GHI_CHU)) { row.SUA_GHI_CHU = r.GHI_CHU; changed = true; }
        if (!changed) return JsonObject(new[] { new { ROOT = "14- Dữ liệu không hợp lệ." } });
        await _context.SaveChangesAsync(cancellationToken);
        return JsonObject(new[] { new { ROOT = "00- OK" } });
    }

    public async Task<ContentResult> A_011_CHECKIN_DS_KH_CAT_NUOC(string? MA_NHAN_VIEN, string? TU_NGAY, string? DEN_NGAY, string? MA_XI_NGHIEP, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await (from job in _context.AppCheckInCatNuoc.AsNoTracking()
            join customer in _context.ThongTinKh.AsNoTracking() on job.MA_KHACH_HANG equals customer.MA_KHACH_HANG
            where job.MA_NHAN_VIEN == MA_NHAN_VIEN && job.MA_XI_NGHIEP == MA_XI_NGHIEP
            orderby job.STT
            select new { ROOT = "00- OK", job.ID_XAC_NHAN, job.MA_KHACH_HANG, customer.TEN_KHACH_HANG, customer.DIA_CHI_DONG_HO, SO_DIEN_THOAI = customer.PHONE_UT1, job.NGAY_XN_BG_NHAN_VIEN, job.NGAY_HOAN_THANH, job.MA_SO_DOC, job.TEN_FILE_ANH, job.GHI_CHU_XN }).ToListAsync(cancellationToken);
        return JsonObject(rows.Count == 0 ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy." } } : rows.Cast<object>());
    }

    public async Task<ContentResult> A_012_CHECKIN_DS_KH_MO_NUOC(string? MA_NHAN_VIEN, string? TU_NGAY, string? DEN_NGAY, string? MA_XI_NGHIEP, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await (from job in _context.AppCheckInMoNuoc.AsNoTracking()
            join customer in _context.ThongTinKh.AsNoTracking() on job.MA_KHACH_HANG equals customer.MA_KHACH_HANG
            where job.MA_NHAN_VIEN == MA_NHAN_VIEN && job.MA_XI_NGHIEP == MA_XI_NGHIEP
            orderby job.ID_XAC_NHAN
            select new { ROOT = "00- OK", job.ID_XAC_NHAN, job.MA_KHACH_HANG, customer.TEN_KHACH_HANG, customer.DIA_CHI_DONG_HO, SO_DIEN_THOAI = customer.PHONE_UT1, job.NGAY_XN_BG_NHAN_VIEN, job.NGAY_HOAN_THANH, job.MA_SO_DOC, job.TEN_FILE_ANH, job.GHI_CHU_XN }).ToListAsync(cancellationToken);
        return JsonObject(rows.Count == 0 ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy." } } : rows.Cast<object>());
    }

    public async Task<ContentResult> A_013_CHECKIN_DS_KH_GUI_GB(string? MA_NHAN_VIEN, string? TU_NGAY, string? DEN_NGAY, string? MA_XI_NGHIEP, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await (from job in _context.AppCheckInGuiGiay.AsNoTracking()
            join customer in _context.ThongTinKh.AsNoTracking() on job.MA_KHACH_HANG equals customer.MA_KHACH_HANG
            where job.MA_NHAN_VIEN == MA_NHAN_VIEN && job.MA_XI_NGHIEP == MA_XI_NGHIEP
            orderby job.ID_XAC_NHAN
            select new { ROOT = "00- OK", job.ID_XAC_NHAN, job.MA_KHACH_HANG, customer.TEN_KHACH_HANG, customer.DIA_CHI_DONG_HO, SO_DIEN_THOAI = customer.PHONE_UT1, job.NGAY_XN_BG_NHAN_VIEN, job.NGAY_HOAN_THANH, job.MA_SO_DOC, job.TEN_FILE_ANH, job.GHI_CHU_XN }).ToListAsync(cancellationToken);
        return JsonObject(rows.Count == 0 ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy." } } : rows.Cast<object>());
    }

    public async Task<ContentResult> A_99_CHECKIN_DM_KIEU_CAT(string? MA_XI_NGHIEP, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var rows = await _context.LogBankTienMoNuoc.AsNoTracking()
            .OrderBy(x => x.KIEU_CAT)
            .Select(x => new { ROOT = "00- OK", MA = x.KIEU_CAT, TEN = x.KIEU_CAT + "- " + x.GHI_CHU })
            .ToListAsync(cancellationToken);
        return JsonObject(rows);
    }

    public async Task<ContentResult> A_02_CHECKIN_LUU_KH_CAT_NUOC(string? ID_XAC_NHAN, string? MA_NHAN_VIEN, string? MA_XI_NGHIEP, string? NGUOI_THI_CONG, string? MA_KIEU_CAT_MO, string? NGAY_HOAN_THANH, string? VI_TRI_XAC_NHAN, string? MA_GHI_CHU, string? GHI_CHU, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var row = await _context.AppCheckInCatNuoc.FirstOrDefaultAsync(x => x.ID_XAC_NHAN == ID_XAC_NHAN && x.MA_NHAN_VIEN == MA_NHAN_VIEN && x.MA_XI_NGHIEP == MA_XI_NGHIEP, cancellationToken);
        if (row is null) return JsonObject(new[] { new { ROOT = "16- Dữ liệu không tìm thấy." } });
        row.NGUOI_THI_CONG = NGUOI_THI_CONG; row.KIEU_CAT_MO = MA_KIEU_CAT_MO; row.VI_TRI_XAC_NHAN_CU = row.VI_TRI_XAC_NHAN; row.VI_TRI_XAC_NHAN = VI_TRI_XAC_NHAN; row.MA_GHI_CHU = MA_GHI_CHU; row.GHI_CHU_THEM = GHI_CHU; row.LOG_DATE_APP = DateTime.Now;
        row.NGAY_HOAN_THANH = DateTime.TryParse(NGAY_HOAN_THANH, out var date) ? date : DateTime.Now;
        await _context.SaveChangesAsync(cancellationToken); return JsonObject(new[] { new { ROOT = "00- OK" } });
    }

    public async Task<ContentResult> A_03_CHECKIN_LUU_KH_MO_NUOC(string? ID_XAC_NHAN, string? MA_NHAN_VIEN, string? MA_XI_NGHIEP, string? NGUOI_THI_CONG, string? MA_KIEU_CAT_MO, string? NGAY_HOAN_THANH, string? VI_TRI_XAC_NHAN, string? MA_GHI_CHU, string? GHI_CHU, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var row = await _context.AppCheckInMoNuoc.FirstOrDefaultAsync(x => x.ID_XAC_NHAN == ID_XAC_NHAN && x.MA_NHAN_VIEN == MA_NHAN_VIEN && x.MA_XI_NGHIEP == MA_XI_NGHIEP, cancellationToken);
        if (row is null) return JsonObject(new[] { new { ROOT = "16- Dữ liệu không tìm thấy." } });
        row.NGUOI_THI_CONG = NGUOI_THI_CONG; row.KIEU_CAT_MO = MA_KIEU_CAT_MO; row.VI_TRI_XAC_NHAN_CU = row.VI_TRI_XAC_NHAN; row.VI_TRI_XAC_NHAN = VI_TRI_XAC_NHAN; row.MA_GHI_CHU = MA_GHI_CHU; row.GHI_CHU_THEM = GHI_CHU; row.LOG_DATE_APP = DateTime.Now;
        row.NGAY_HOAN_THANH = DateTime.TryParse(NGAY_HOAN_THANH, out var date) ? date : DateTime.Now;
        await _context.SaveChangesAsync(cancellationToken); return JsonObject(new[] { new { ROOT = "00- OK" } });
    }

    public async Task<ContentResult> A_00_CHECKIN_LUU_TEN_FILE_ANH(string? ID_XAC_NHAN, string? LOAI_CV, string? TEN_FILE_ANH, string? MA_NHAN_VIEN, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var changed = false;
        if (LOAI_CV == "1") { var row = await _context.AppCheckInCatNuoc.FirstOrDefaultAsync(x => x.ID_XAC_NHAN == ID_XAC_NHAN && x.MA_NHAN_VIEN == MA_NHAN_VIEN, cancellationToken); if (row != null) { row.TEN_FILE_ANH = TEN_FILE_ANH; changed = true; } }
        else if (LOAI_CV == "2") { var row = await _context.AppCheckInMoNuoc.FirstOrDefaultAsync(x => x.ID_XAC_NHAN == ID_XAC_NHAN && x.MA_NHAN_VIEN == MA_NHAN_VIEN, cancellationToken); if (row != null) { row.TEN_FILE_ANH = TEN_FILE_ANH; changed = true; } }
        else if (LOAI_CV == "3") { var row = await _context.AppCheckInGuiGiay.FirstOrDefaultAsync(x => x.ID_XAC_NHAN == ID_XAC_NHAN && x.MA_NHAN_VIEN == MA_NHAN_VIEN, cancellationToken); if (row != null) { row.TEN_FILE_ANH = TEN_FILE_ANH; changed = true; } }
        if (!changed) return JsonObject(new[] { new { ROOT = "16- Dữ liệu không tìm thấy." } });
        await _context.SaveChangesAsync(cancellationToken); return JsonObject(new[] { new { ROOT = "00- OK" } });
    }

}









