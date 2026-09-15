using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class DmLoaiSmsEmail
{
    public string MaLoai { get; set; } = null!;

    public string? TenLoai { get; set; }

    public decimal? Tt { get; set; }

    public string? SmsChuan { get; set; }
}
