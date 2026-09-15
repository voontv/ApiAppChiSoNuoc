using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class CcMangLuoi
{
    public string? IdMl { get; set; }

    public string? TieuDeTb { get; set; }

    public string? NoiDungTb { get; set; }

    public string? KqSuCo { get; set; }

    public DateTime? TgSuCoTu { get; set; }

    public DateTime? TgSuCoDen { get; set; }

    public string? PhamVi { get; set; }

    public decimal? Stt { get; set; }

    public string? NguoiCapNhat { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public string? KqSuCoMa { get; set; }

    public string? TieuDeTbMa { get; set; }

    public DateTime? NgayCapNhatLai { get; set; }

    public string? ThoiGianSuCo { get; set; }

    public string? MaXiNghiep { get; set; }

    public string? TenXiNghiep { get; set; }
}
