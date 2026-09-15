using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class AppExDmNhanVien
{
    public string MaNhanVien { get; set; } = null!;

    public string? TenNhanVien { get; set; }

    public decimal? Stt { get; set; }

    public decimal? Doan { get; set; }

    public string? MaXn { get; set; }
}
