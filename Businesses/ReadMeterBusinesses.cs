using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadMeter.Api.Data;
using ReadMeter.Api.Models;
using ReadMeter.Api.Contracts.Requests;
using System.Globalization;
using System.Text.Json;

namespace ReadMeter.Api.Businesses;

public sealed class ReadMeterBusinesses : IReadMeterBusinesses
{
    private readonly ReadMeterDbContext _context;
    private readonly BillingDbContext _billing;

    public ReadMeterBusinesses(ReadMeterDbContext context, BillingDbContext billing)
    {
        _context = context;
        _billing = billing;
    }

    private static ContentResult JsonObject(object value) => new()
    {
        StatusCode = StatusCodes.Status200OK,
        ContentType = "application/json; charset=utf-8",
        Content = JsonSerializer.Serialize(value)
    };

    private async Task<ContentResult> GetMeterBooks(
        string? meterReader, string? month, bool supplementalOnly, bool pendingOnly,
        CancellationToken cancellationToken)
    {
        var query = _context.AppDhChiSo.AsNoTracking()
            .Where(x => x.MA_BIEN_DOC == meterReader && x.THANG == month);
        if (supplementalOnly)
            query = query.Where(x => x.MA_TINH_TRANG_DH == "BS");
        if (pendingOnly)
            query = query.Where(x => x.NGAY_EBILL_NHAN_KHOA == null && x.NGAY_EBILL_NAP_BILL == null);

        var grouped = await (
            from reading in query
            join book in _context.DmSoDoc.AsNoTracking()
                on reading.MA_SO_DOC equals book.MA_SO_DOC
            group new { reading, book } by new { reading.MA_SO_DOC, book.TEN_SO_DOC, book.NGAY_DOC } into values
            orderby values.Key.MA_SO_DOC
            select new
            {
                values.Key.MA_SO_DOC,
                values.Key.TEN_SO_DOC,
                values.Key.NGAY_DOC,
                TRANG_THAI_SO = values.Any(x => x.reading.NGAY_BD_NHAN_KHOA != null) ? "Y" : "N",
                TONG_DH = values.Count(),
                TONG_DH_DA_DOC = values.Count(x => x.reading.NGAY_DOC_TUNG_DH != null),
                TRANG_THAI_BG_SO = values.Any(x => x.reading.NGAY_BIEN_DOC_BG != null && x.reading.NGAY_EBILL_NHAN_KHOA == null) ? 2
                    : values.Any(x => x.reading.NGAY_BD_NHAN_KHOA != null && x.reading.NGAY_BIEN_DOC_BG == null) ? 1 : 0
            })
            .ToListAsync(cancellationToken);

        if (grouped.Count == 0)
            return JsonObject(new[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } });
        return JsonObject(grouped.Select((x, index) => new
        {
            ROOT = "00- OK",
            STT = index + 1,
            x.MA_SO_DOC,
            x.TEN_SO_DOC,
            x.NGAY_DOC,
            x.TRANG_THAI_SO,
            x.TONG_DH,
            x.TONG_DH_DA_DOC,
            TONG_DH_CHUA_DOC = x.TONG_DH - x.TONG_DH_DA_DOC,
            x.TRANG_THAI_BG_SO
        }));
    }

    private async Task<ContentResult> GetCustomersForReading(
        string? id, string? sequence, string? customerCode, string? customerName, string? address,
        string? phone, string? bookCode, string? meterReader, string? month, string? filter,
        bool supplementalOnly, CancellationToken cancellationToken)
    {
        var query =
            from reading in _context.AppDhChiSo.AsNoTracking()
            join customer in _context.ThongTinKh.AsNoTracking()
                on reading.MA_KHACH_HANG equals customer.MA_KHACH_HANG
            where reading.MA_SO_DOC == bookCode && reading.MA_BIEN_DOC == meterReader && reading.THANG == month
            select new { reading, customer };
        if (!string.IsNullOrWhiteSpace(id)) query = query.Where(x => x.reading.ID_DCS == id);
        if (decimal.TryParse(sequence, out var sequenceValue)) query = query.Where(x => x.reading.STT_SO_DOC == sequenceValue);
        if (!string.IsNullOrWhiteSpace(customerCode)) query = query.Where(x => x.customer.MA_KHACH_HANG.Contains(customerCode));
        if (!string.IsNullOrWhiteSpace(customerName)) query = query.Where(x => x.customer.TEN_KHACH_HANG!.Contains(customerName));
        if (!string.IsNullOrWhiteSpace(address)) query = query.Where(x => x.customer.DIA_CHI_DONG_HO!.Contains(address));
        if (!string.IsNullOrWhiteSpace(phone)) query = query.Where(x => x.customer.PHONE_UT1!.Contains(phone));
        if (filter == "1") query = query.Where(x => x.reading.NGAY_DOC_TUNG_DH != null);
        if (filter == "2") query = query.Where(x => x.reading.NGAY_DOC_TUNG_DH == null);
        if (supplementalOnly) query = query.Where(x => x.reading.MA_TINH_TRANG_DH == "BS");

        var rows = await query.OrderBy(x => x.reading.STT_SO_DOC_MOI ?? x.reading.STT_SO_DOC)
            .Select(x => new
            {
                ROOT = "00- OK", ID_DONG_HO = x.reading.ID_DCS, x.customer.MA_KHACH_HANG,
                x.customer.TEN_KHACH_HANG, x.customer.DIA_CHI_DONG_HO, x.customer.TEN_DONG_HO,
                CO_DH = x.customer.MA_DONG_HO, x.customer.HT_KD, x.customer.SO_SERIAL_DONG_HO,
                PHONE_UT1 = x.customer.PHONE_UT1, EMAIL_UT1 = x.customer.EMAIL_UT1,
                x.reading.CHI_SO_CU, x.reading.CHI_SO_MOI, x.reading.SAN_LUONG_TT, x.reading.TONG_SL,
                x.reading.MA_TINH_TRANG_DH, x.reading.STT_SO_DOC, x.reading.STT_SO_DOC_MOI,
                x.reading.NGAY_DOC_DK, x.reading.NGAY_DOC_CS, x.reading.NGAY_DOC_TUNG_DH,
                x.reading.QUA_VONG, x.reading.LOAI_CHI_SO, x.reading.CONG_CHI_SO,
                x.reading.SAN_LUONG_DUNG_IT, x.reading.MA_GHI_CHU, x.reading.GHI_CHU,
                x.reading.VI_TRI_DOC, x.reading.TEN_FILE_ANH, x.reading.SL_TB_3THANG
            })
            .ToListAsync(cancellationToken);
        return JsonObject(rows.Count == 0 ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } } : rows.Cast<object>());
    }

    private async Task<ContentResult> SaveMeterReading(
        string? id, string? status, string? rollover, string? readingType, string? readingDate,
        string? currentReading, string? actualUsage, string? totalUsage, string? accumulatedReading,
        string? lowUsage, string? noteCode, string? note, string? location,
        string? customerCode, string? bookCode, string? month, string? branch, string? meterReader,
        CancellationToken cancellationToken)
    {
        var query = _context.AppDhChiSo.Where(x => x.ID_DCS == id && x.MA_BIEN_DOC == meterReader);
        if (!string.IsNullOrWhiteSpace(customerCode)) query = query.Where(x => x.MA_KHACH_HANG == customerCode);
        if (!string.IsNullOrWhiteSpace(bookCode)) query = query.Where(x => x.MA_SO_DOC == bookCode);
        if (!string.IsNullOrWhiteSpace(month)) query = query.Where(x => x.THANG == month);
        if (!string.IsNullOrWhiteSpace(branch)) query = query.Where(x => x.MA_CHI_NHANH == branch);
        var row = await query.FirstOrDefaultAsync(cancellationToken);
        if (row is null) return JsonObject(new[] { new { ROOT = "16- Dữ liệu không tìm thấy." } });

        static decimal Number(string? value) => decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var number) ? number : 0;
        row.MA_TINH_TRANG_DH = status;
        row.QUA_VONG = rollover;
        row.LOAI_CHI_SO = readingType;
        row.NGAY_DOC_CS = DateTime.TryParseExact(readingDate, new[] { "dd/MM/yyyy", "dd/MM/yyyy HH:mm:ss", "yyyy-MM-dd" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date : DateTime.Now;
        row.CHI_SO_MOI = Number(currentReading);
        row.SAN_LUONG_TT = Number(actualUsage);
        row.TONG_SL = Number(totalUsage);
        row.CONG_CHI_SO = Number(accumulatedReading);
        row.SAN_LUONG_DUNG_IT = Number(lowUsage);
        row.MA_GHI_CHU = noteCode;
        row.GHI_CHU = note;
        row.VI_TRI_DOC_CU = row.VI_TRI_DOC;
        row.VI_TRI_DOC = location;
        row.NGAY_DOC_TUNG_DH = DateTime.Now;
        row.LOG_USER = meterReader;
        await _context.SaveChangesAsync(cancellationToken);
        return JsonObject(new[] { new { ROOT = "00- OK" } });
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
        if (employee.IMEI != SO_IMEI && employee.IMEI_SUB != SO_IMEI && employee.IMEI_SUB2 != SO_IMEI)
            return JsonObject(new[] { new { ROOT = "13- Thiết bị không hợp lệ." } });
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
        GetCustomersForReading(ID_DOC_opt, STT_SO_DOC_opt, MA_KH_opt, TEN_KH_opt, DIA_CHI_DH_opt, PHONE_KH_opt, MA_SO_DOC, MA_BIEN_DOC, THANG, KIEU_LOC, false, cancellationToken);

    public Task<ContentResult> P_03_LAY_DS_KHACH_HANG_SUB(string? ID_DOC_opt, string? STT_SO_DOC_opt, string? MA_KH_opt, string? TEN_KH_opt, string? DIA_CHI_DH_opt, string? PHONE_KH_opt, string? MA_SO_DOC, string? MA_BIEN_DOC, string? THANG, string? KIEU_LOC, string? SAP_XEP_THEO, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        GetCustomersForReading(ID_DOC_opt, STT_SO_DOC_opt, MA_KH_opt, TEN_KH_opt, DIA_CHI_DH_opt, PHONE_KH_opt, MA_SO_DOC, MA_BIEN_DOC, THANG, KIEU_LOC, false, cancellationToken);

    public Task<ContentResult> P_03_LAY_DS_KHACH_HANG_SUB_BS(string? ID_DOC_opt, string? STT_SO_DOC_opt, string? MA_KH_opt, string? TEN_KH_opt, string? DIA_CHI_DH_opt, string? PHONE_KH_opt, string? MA_SO_DOC, string? MA_BIEN_DOC, string? THANG, string? KIEU_LOC, string? SAP_XEP_THEO, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        GetCustomersForReading(ID_DOC_opt, STT_SO_DOC_opt, MA_KH_opt, TEN_KH_opt, DIA_CHI_DH_opt, PHONE_KH_opt, MA_SO_DOC, MA_BIEN_DOC, THANG, KIEU_LOC, true, cancellationToken);

    public Task<ContentResult> P_0313_LAY_DS_KHACH_HANG_TRA_CUU(string? MA_SO_DOC, string? MA_BIEN_DOC, string? MA_XI_NGHIEP, string? THANG, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken) =>
        GetCustomersForReading(null, null, null, null, null, null, MA_SO_DOC, MA_BIEN_DOC, THANG, "0", false, cancellationToken);

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

    public async Task<ContentResult> P_045_CANH_BAO_SAN_LUONG_LON(string? ID_DONG_HO, string? MA_KHACH_HANG, string? MA_BIEN_DOC, long TONG_SL, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var employee = await _context.DmNhanVien.AsNoTracking().FirstOrDefaultAsync(x => x.MA_NHAN_VIEN == MA_BIEN_DOC, cancellationToken);
        var reading = await _context.AppDhChiSo.AsNoTracking().FirstOrDefaultAsync(x => x.ID_DCS == ID_DONG_HO && x.MA_KHACH_HANG == MA_KHACH_HANG, cancellationToken);
        if (reading is null) return JsonObject(new[] { new { ROOT = "16- Dữ liệu không tìm thấy.", CANH_BAO = false } });
        var threshold = employee?.CANH_BAO_M3 ?? 0;
        return JsonObject(new[] { new { ROOT = "00- OK", CANH_BAO = threshold > 0 && TONG_SL >= threshold, SAN_LUONG = TONG_SL, NGUONG = threshold } });
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
        var rows = await _context.AppDhChiSo.Where(x => x.NGAY_EBILL_NHAN_KHOA == null && x.NGAY_EBILL_NAP_BILL == null && x.NGAY_BD_NHAN_KHOA != null && x.MA_BIEN_DOC == MA_BIEN_DOC && x.THANG == THANG && x.MA_SO_DOC == DANH_SACH_MA_SO_DOC).ToListAsync(cancellationToken);
        if (rows.Any(x => x.NGAY_DOC_TUNG_DH == null))
            return JsonObject(new[] { new { ROOT = "18- Chưa đọc xong. Vui lòng kiểm tra lại!" } });
        var now = DateTime.Now;
        foreach (var row in rows) row.NGAY_BIEN_DOC_BG = now;
        await _context.SaveChangesAsync(cancellationToken);
        return JsonObject(new[] { new { ROOT = "00- OK" } });
    }

    public async Task<ContentResult> P_81_LAY_TT_KHACH_HANG(string? MA_KHACH_HANG, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(MA_KHACH_HANG) && MA_KHACH_HANG.Length != 9)
            return JsonObject(new[] { new { ROOT = $"15- Độ dài dữ liệu không hợp lệ (MKH: {MA_KHACH_HANG})" } });
        var debt = await _context.CongNo.AsNoTracking()
            .Where(x => x.MA_KHACH_HANG == MA_KHACH_HANG && (x.TRANG_THAI == 1 || x.TRANG_THAI == 4))
            .SumAsync(x => (decimal?)(x.TONG_TIEN - (x.TONG_THANH_TOAN_BILL ?? 0)), cancellationToken) ?? 0;
        var average = await _context.ChiSoDhSub.AsNoTracking()
            .Where(x => x.MA_KHACH_HANG == MA_KHACH_HANG && x.SLTB3T > 0)
            .OrderByDescending(x => x.ID_CS)
            .Select(x => x.SLTB3T)
            .FirstOrDefaultAsync(cancellationToken);
        var row = await _context.ThongTinKh.AsNoTracking()
            .Where(x => x.MA_KHACH_HANG == MA_KHACH_HANG && x.NGAY_THANH_LY_HD == null)
            .Select(x => new
            {
                ROOT = "00- OK", x.MA_KHACH_HANG, x.TEN_KHACH_HANG, x.DIA_CHI_KHACH_HANG,
                DIA_CHI_LAP_DAT = x.DIA_CHI_DONG_HO, DIEN_THOAI = x.PHONE_UT1, EMAIL = x.EMAIL_UT1,
                x.SO_HOP_DONG, x.SO_HO, x.SO_KHAU, x.DINH_MUC, x.MA_GIA, x.TEN_GIA_NUOC,
                SERIAL_DH = x.SO_SERIAL_DONG_HO, x.TEN_DONG_HO, x.NGAY_LAP_DAT, x.BIEN_DOC,
                MUC_DICH_SU_DUNG = x.HT_KD, XN_CAP_NUOC = x.CHI_NHANH
            })
            .FirstOrDefaultAsync(cancellationToken);
        if (row is null)
            return JsonObject(new[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } });
        return JsonObject(new object[] { new { row.ROOT, row.MA_KHACH_HANG, row.TEN_KHACH_HANG, row.DIA_CHI_KHACH_HANG,
            row.DIA_CHI_LAP_DAT, row.DIEN_THOAI, row.EMAIL, row.SO_HOP_DONG, row.SO_HO, row.SO_KHAU, row.DINH_MUC,
            row.MA_GIA, row.TEN_GIA_NUOC, row.SERIAL_DH, row.TEN_DONG_HO, row.NGAY_LAP_DAT, row.BIEN_DOC,
            row.MUC_DICH_SU_DUNG, row.XN_CAP_NUOC, DU_NO = debt, BQ3T = average ?? 0 } });
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
            .Select(x => new { ROOT = "00- OK", CHI_SO_CU = x.CS_DAU, CHI_SO_MOI = x.CS_CUOI, x.SAN_LUONG,
                LOAI_CHI_SO = x.LOAI_CHI_SO == "0" ? "Định kỳ" : x.LOAI_CHI_SO == "6" ? "Chốt chỉ số" : x.LOAI_CHI_SO })
            .ToListAsync(cancellationToken);
        return JsonObject(rows.Count == 0 ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } } : rows.Cast<object>());
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
        var rows = await _billing.Aereport.AsNoTracking()
            .Where(x => x.CUSTID == customerCode)
            .OrderByDescending(x => x.ERPTID)
            .Select(x => new
            {
                ROOT = "00- OK",
                LOAI_HO_SO = x.ERPTTYPE == "0" ? "Thông báo ngừng cấp nước - Nợ tiền nước"
                    : x.ERPTTYPE == "1" ? "Thông báo ngừng cấp nước - Chi nhánh"
                    : x.ERPTTYPE == "2" ? "Ngừng cấp nước (Cắt nước)"
                    : x.ERPTTYPE == "3" ? "Cấp nước lại (Mở nước)"
                    : x.ERPTTYPE == "4" ? "Giấy mời ký lại hợp đồng tiêu thụ nước sạch"
                    : x.ERPTTYPE == "5" ? "Điều chỉnh giá tiêu thụ nước sạch" : x.ERPTTYPE,
                TRANG_THAI = x.ERPTSTS == "1" ? "Đã thực hiện" : x.ERPTSTS == "0" ? "Chưa thực hiện" : x.ERPTSTS,
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
        return JsonObject(rows.Count == 0 ? new object[] { new { ROOT = "16- Dữ liệu không tìm thấy. Vui lòng kiểm tra lại!" } } : rows.Cast<object>());
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

    public async Task<ContentResult> P_89_TINH_TIEN(string? MA_GIA, string? SAN_LUONG_SD, string? MA_BIEN_DOC, string? SO_IMEI, string? PASSWORD_K, CancellationToken cancellationToken)
    {
        var usage = decimal.TryParse(SAN_LUONG_SD, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed) ? parsed : 0;
        var price = await _context.KDmGia.AsNoTracking().Where(x => x.MA_GIA == MA_GIA || x.KY_HIEU_GIA == MA_GIA).OrderByDescending(x => x.NGAY_AP_DUNG).FirstOrDefaultAsync(cancellationToken);
        if (price is null) return JsonObject(new[] { new { ROOT = "16- Không tìm thấy giá.", SAN_LUONG = usage, TONG_TIEN = (decimal?)null } });
        var unitPrice = price.GIA_CO_BAN ?? price.GIA_GT_01 ?? 0;
        return JsonObject(new[] { new { ROOT = "00- OK", SAN_LUONG = usage, DON_GIA = unitPrice, THANH_TIEN = usage * unitPrice, THUE = 0m, PHI = 0m, TONG_TIEN = usage * unitPrice } });
    }

    public async Task<ContentResult> P_07_CC_BAO_SU_CO(P_07_CC_BAO_SU_CORequest r, CancellationToken cancellationToken)
    {
        _context.Cc01TtkhYeuCau.Add(new Cc01TtkhYeuCauEntity
        {
            ID_YEU_CAU = Guid.NewGuid().ToString("N")[..9],
            MA_NHAN_VIEN_YC = r.MA_BIEN_DOC, MA_NOI_DUNG_YC = r.LOAI_YEU_CAU,
            MA_KHACH_HANG = r.MA_KHACH_HANG, MA_XI_NGHIEP = r.MA_XI_NGHIEP,
            MA_QUAN = r.MA_QUAN, MA_PHUONG = r.MA_PHUONG, TEN_KHACH_HANG = r.TEN_KHACH_HANG,
            DIA_CHI_DONG_HO = r.DIA_CHI_KHACH_HANG, CC_SO_DIEN_THOAI = r.SO_DIEN_THOAI_KH,
            CC_EMAIL = r.EMAIL_KH, GHI_CHU = r.NOI_DUNG_YEU_CAU, KHACH_HANG_YC = r.KHACH_HANG_YEU_CAU,
            CONG_VIEC_24G = r.XU_LY_24H, NGAY_CAP_NHAT = DateTime.Now, NGUOI_CAP_NHAT = r.MA_BIEN_DOC
        });
        await _context.SaveChangesAsync(cancellationToken);
        return JsonObject(new[] { new { ROOT = "00- OK" } });
    }

    public async Task<ContentResult> P_9E_SUA_THONG_TIN_KH(P_9E_SUA_THONG_TIN_KHRequest r, CancellationToken cancellationToken)
    {
        var row = await _context.AppDhChiSo.FirstOrDefaultAsync(x => x.MA_KHACH_HANG == r.MA_KHACH_HANG && x.MA_CHI_NHANH == r.MA_XI_NGHIEP && x.MA_BIEN_DOC == r.MA_BIEN_DOC && x.THANG == r.THANG, cancellationToken);
        if (row is null) return JsonObject(new[] { new { ROOT = "16- Dữ liệu không tìm thấy." } });
        row.SUA_TEN_KHACH_HANG = r.TEN_KHACH_HANG; row.SUA_DIA_CHI_DONG_HO = r.DIA_CHI_DONG_HO;
        row.SUA_SO_DT = r.SO_DT; row.SUA_EMAIL = r.EMAIL; row.SUA_DH_TEN = r.DONG_HO_TEN;
        row.SUA_DH_SERIAL = r.DONG_HO_SERIAL; row.SUA_GHI_CHU = r.GHI_CHU;
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
