using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class AppTc08MoNuoc
{
    public string IdXacNhan { get; set; } = null!;

    public string? MaKhachHang { get; set; }

    public string? LogUser { get; set; }

    public string? MaNhanVienNt { get; set; }

    public string? MaXiNghiep { get; set; }

    public DateTime? LogDateApp { get; set; }

    public string? ViTriXacNhan { get; set; }

    public string? GhiChuThem { get; set; }

    public string? MaGhiChu { get; set; }

    public string? MaSoDoc { get; set; }

    public string? GhiChuBilling { get; set; }

    public string? NguoiDaiDienTc { get; set; }

    public string? KieuCatMo { get; set; }

    public decimal? SttHs { get; set; }

    public decimal? ChiSoDh { get; set; }

    public string? ViTriXacNhanCu { get; set; }

    public DateTime? NgayNiem { get; set; }

    public string? SoKimNiem { get; set; }

    public decimal? ChiSoNiem { get; set; }

    public decimal? ChiSoCatMo { get; set; }

    public string? IdBilling { get; set; }

    public string? MaTttc { get; set; }

    public DateTime? Ngay1XnGiaoNt { get; set; }

    public DateTime? Ngay2NtNhanXn { get; set; }

    public DateTime? Ngay3NtGiaoCn { get; set; }

    public DateTime? Ngay4CnNhanNt { get; set; }

    public DateTime? Ngay5CnTcOk { get; set; }

    public DateTime? Ngay6CnGiaoNt { get; set; }

    public DateTime? Ngay7NtNhanCn { get; set; }

    public DateTime? Ngay8NtGiaoXn { get; set; }

    public DateTime? Ngay9XnNhanNt { get; set; }

    public string? MaBienDoc { get; set; }

    public string? MaNhanVienCn { get; set; }

    public DateTime? NgayBillingGiao { get; set; }

    public DateTime? Ngay10XnBilling { get; set; }

    public string? TenFileAnh01 { get; set; }

    public string? TenFileAnh02 { get; set; }

    public string? MaKimNiem { get; set; }
}
