using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class DmTinhTrangDongHo
{
    public string? MaTinhTrangSo { get; set; }

    public string? MoTa { get; set; }

    public string? MoTaNgan { get; set; }

    public string? NDongHoHong { get; set; }

    public string? NSuaChiSoCu { get; set; }

    public string? NCongDonChiSo { get; set; }

    public string? NNhapSlTrucTiep { get; set; }

    public string? HieuLuc { get; set; }

    public decimal? SttHienThi { get; set; }

    public string? GiaiPhap { get; set; }

    public string? NNhapChiSoMoi { get; set; }

    public string? NguoiCapNhat { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public string? MoTaSub { get; set; }

    public string? TamKhongTinhHd { get; set; }
}
