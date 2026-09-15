using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class LogEmail
{
    public string? Email { get; set; }

    public DateTime? NgayGui { get; set; }

    public string? MaKhachHang { get; set; }

    public string? SoHoaDon { get; set; }

    public string? LoaiEmail { get; set; }

    public string? KetQua { get; set; }

    public string? LogEmailId { get; set; }

    public DateTime? NgayPhhd { get; set; }

    public string? Usr { get; set; }

    public string? MaMau { get; set; }

    public string? DaLuuEbilsub { get; set; }
}
