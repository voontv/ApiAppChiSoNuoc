using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class AppExKiemTraGium
{
    public string MaKhachHang { get; set; } = null!;

    public string? TenKhachHang { get; set; }

    public string? DiaChiDongHo { get; set; }

    public string? MaSoDoc { get; set; }

    public decimal? SttSoDoc { get; set; }

    public string? MaXiNghiep { get; set; }

    public string? TinhTrangDh { get; set; }

    public string? T01MaGia { get; set; }

    public decimal? T01SanLuong { get; set; }

    public string? T02MaGia { get; set; }

    public decimal? T02SanLuong { get; set; }

    public string? T03MaGia { get; set; }

    public decimal? T03SanLuong { get; set; }

    public string? SoDienThoai { get; set; }

    public string? MaBienDoc { get; set; }

    public string? T01Thang { get; set; }

    public string? T02Thang { get; set; }

    public string? T03Thang { get; set; }

    public string? T01TenGia { get; set; }

    public string? T02TenGia { get; set; }

    public string? T03TenGia { get; set; }

    public string? UNguoiDung { get; set; }

    public DateTime? ULogDate { get; set; }

    public string? UViTriDoc { get; set; }

    public string? USh { get; set; }

    public string? UKd { get; set; }

    public string? USx { get; set; }

    public string? UHcsn { get; set; }

    public string? UMaGhiChu { get; set; }

    public string? UGhiChuThem { get; set; }

    public string? UTenFileAnh { get; set; }

    public string IdApGia { get; set; } = null!;

    public decimal? SoHo { get; set; }

    public decimal? SoKhau { get; set; }

    public decimal? DinhMuc { get; set; }

    public string? Temp { get; set; }

    public string? SoSerialDongHo { get; set; }

    public decimal? ChiSoCuoi { get; set; }

    public string? MaGia { get; set; }

    public DateTime? NgayGiao { get; set; }
}
