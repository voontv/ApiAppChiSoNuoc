using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class LogThongTinKhSoDienThoai
{
    public string? MaKhachHang { get; set; }

    public string? MaBienDoc { get; set; }

    public DateTime? NgayThucHien { get; set; }

    public string? GiaTriCu { get; set; }

    public string? GiaTriMoi { get; set; }

    public decimal Id { get; set; }
}
