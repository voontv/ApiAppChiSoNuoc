using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class KDmGiaPhi
{
    public string MaPhi { get; set; } = null!;

    public string? LoaiPhi { get; set; }

    public string? KieuPhi { get; set; }

    public string? TenPhi { get; set; }

    public decimal? GiaTri { get; set; }

    public string? NguoiCapNhat { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public string? NguoiSua { get; set; }

    public DateTime? NgaySua { get; set; }
}
