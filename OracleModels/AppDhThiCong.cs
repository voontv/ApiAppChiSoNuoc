using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class AppDhThiCong
{
    public string? IdTc { get; set; }

    public string? MaKhachHang { get; set; }

    public string? LoaiCongTrinh { get; set; }

    public DateTime? NgayGiaoTc { get; set; }

    public decimal? ChiSoLap { get; set; }

    public decimal? ChiSoThao { get; set; }

    public DateTime? NgayCapNhat { get; set; }
}
