using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class AppDhLogBienDoc
{
    public string? IdBdLog { get; set; }

    public string? MaBienDoc { get; set; }

    public DateTime? LogDate { get; set; }

    public string? ThuocHam { get; set; }

    public string? DienGiai { get; set; }

    public string? MaXiNghiep { get; set; }

    public string? Imei { get; set; }

    public decimal? ThoiGianLoad { get; set; }
}
