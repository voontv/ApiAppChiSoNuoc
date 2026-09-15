using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class AppCheckInMoNuoc
{
    public string IdXacNhan { get; set; } = null!;

    public string? MaKhachHang { get; set; }

    public string? LogUser { get; set; }

    public DateTime? NgayXnBgNhanVien { get; set; }

    public DateTime? NgayXnNhanLaiBg { get; set; }

    public string? MaNhanVien { get; set; }

    public string? MaXiNghiep { get; set; }

    public DateTime? LogDateApp { get; set; }

    public string? ViTriXacNhan { get; set; }

    public string? TenFileAnh { get; set; }

    public string? GhiChuThem { get; set; }

    public string? MaGhiChu { get; set; }

    public decimal? NgayDoc { get; set; }

    public string? MaSoDoc { get; set; }

    public DateTime? NgayHoanThanh { get; set; }

    public string? GhiChuXn { get; set; }

    public string? Thang { get; set; }

    public string? NguoiThiCong { get; set; }

    public string? KieuCatMo { get; set; }

    public decimal? Stt { get; set; }

    public decimal? ChiSoDh { get; set; }

    public string? ViTriXacNhanCu { get; set; }

    public DateTime? NgayNiem { get; set; }

    public string? SoKimNiem { get; set; }

    public decimal? ChiSoNiem { get; set; }

    public decimal? ChiSoMo { get; set; }

    public string? IdBilling { get; set; }
}
