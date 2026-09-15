using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class LogBankTienMoNuoc
{
    public decimal? SoTien { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? KieuCat { get; set; }

    public string? GhiChu { get; set; }
}
