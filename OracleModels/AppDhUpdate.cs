using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class AppDhUpdate
{
    public string? BanCapNhat { get; set; }

    public string? DuongDan { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public decimal? TgCapNhatSlTv { get; set; }

    public string? ActIdDoc { get; set; }
}
