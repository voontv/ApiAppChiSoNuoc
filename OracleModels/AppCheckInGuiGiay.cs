using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class AppCheckInGuiGiay
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

    public decimal? NgayDoc { get; set; }

    public string? MaSoDoc { get; set; }

    public DateTime? NgayHoanThanh { get; set; }

    public string? GhiChuXn { get; set; }

    public string? Thang { get; set; }

    public decimal? Stt { get; set; }

    public decimal? ChiSoDh { get; set; }

    public string? ViTriXacNhanCu { get; set; }
}
