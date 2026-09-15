using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class AppDhDmGhiChu
{
    public string MaGhiChu { get; set; } = null!;

    public string? NoiDungGhiChu { get; set; }

    public decimal? Stt { get; set; }

    public string? Nhom { get; set; }

    public string? DenKiemTra { get; set; }
}
