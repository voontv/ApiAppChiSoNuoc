using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class LogSm
{
    public string? SoHoaDon { get; set; }

    public string? SoDienThoai { get; set; }

    public DateTime? NgayGui { get; set; }

    public string? MaKhachHang { get; set; }

    public DateTime? NgayPhhd { get; set; }

    public string? NhaMang { get; set; }

    public string? KetQua { get; set; }

    public string? LoaiSms { get; set; }

    public string? NoiDungSms { get; set; }

    public string LogSmsId { get; set; } = null!;

    public decimal? SmsSeTinh { get; set; }

    public string? ShowOk { get; set; }

    public string? MaMau { get; set; }

    public string? DaLuuEbilsub { get; set; }

    public string? Zalo { get; set; }
}
