using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class KDmDiaChinh
{
    public string MaDiaChinh { get; set; } = null!;

    public string? ParentMa { get; set; }

    public string? TenDiaChinh { get; set; }

    public string? MaQuan { get; set; }

    public string? TenQuan { get; set; }

    public string? MaPhuong { get; set; }

    public string? TenPhuong { get; set; }

    public string? MaDonVi { get; set; }

    public string? NguoiCapNhat { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public string? NguoiSua { get; set; }

    public DateTime? NgaySua { get; set; }

    public string? CapDiaChinh { get; set; }

    public string? KyHieu { get; set; }

    public string? KyHieuSub { get; set; }

    public string? ParentMaSub { get; set; }

    public string? IsPhuongMoi { get; set; }
}
