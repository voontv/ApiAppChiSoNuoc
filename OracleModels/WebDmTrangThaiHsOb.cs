using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class WebDmTrangThaiHsOb
{
    public string MaTrangThaiHs { get; set; } = null!;

    public string? TenTrangThaiHs { get; set; }

    public string? TenSub { get; set; }

    public decimal? Stt { get; set; }

    public string? HienThi { get; set; }

    public string? TenSub2 { get; set; }

    public string? Loai { get; set; }
}
