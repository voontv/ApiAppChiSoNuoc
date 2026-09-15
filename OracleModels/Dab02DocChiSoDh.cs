using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class Dab02DocChiSoDh
{
    public string IdDab { get; set; } = null!;

    public string? MaBienDoc { get; set; }

    public decimal? TongHs { get; set; }

    public decimal? ChuaDoc { get; set; }

    public decimal? DaDoc { get; set; }

    public string? MaXiNghiep { get; set; }

    public DateTime? NgayThucHien { get; set; }

    public DateTime? NgayDocDk { get; set; }

    public string? MaXiNghiepCu { get; set; }
}
