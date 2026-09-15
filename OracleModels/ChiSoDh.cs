using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class ChiSoDh
{
    public string? MaKhachHang { get; set; }

    public decimal? CsDau { get; set; }

    public decimal? CsCuoi { get; set; }

    public decimal? SanLuong { get; set; }

    public DateTime? NgayDoc { get; set; }

    public DateTime? NgayDinhKy { get; set; }

    public string? KhId { get; set; }

    public string? Thang { get; set; }

    public string? SoHoaDon { get; set; }

    public string? LoaiChiSo { get; set; }

    public string? IdCsDau { get; set; }

    public string? IdCsCuoi { get; set; }
}
