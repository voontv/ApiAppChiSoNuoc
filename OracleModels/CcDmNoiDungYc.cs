using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class CcDmNoiDungYc
{
    public string IdNoiDung { get; set; } = null!;

    public string? NoiDungYc { get; set; }

    public string? GhiChu { get; set; }

    public decimal? Stt { get; set; }

    public string? NhomYc { get; set; }

    public string? MaNhomYc { get; set; }

    public string? NguoiTao { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? MaBp { get; set; }

    public string? NguoiSua { get; set; }

    public DateTime? NgaySua { get; set; }

    public string? NoiDungYcSub { get; set; }

    public string? AppDh { get; set; }

    public string? Act { get; set; }

    public string? DanhGiaKpi { get; set; }

    public string? NoiDungYcCu { get; set; }
}
