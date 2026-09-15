using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class AppTc99NiemChiNhua
{
    public string IdNcn { get; set; } = null!;

    public string? MaKhachHang { get; set; }

    public DateTime? LogDate { get; set; }

    public string? MaQrChiNiem { get; set; }

    public string? LogUser { get; set; }
}
