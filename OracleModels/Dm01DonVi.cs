using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class Dm01DonVi
{
    public string MaDonVi { get; set; } = null!;

    public string? TenDonVi { get; set; }

    public string? DienThoai { get; set; }

    public string? TruongDonVi { get; set; }

    public string? TruongChucVu { get; set; }

    public string? DaiDienKyHd { get; set; }

    public string? DaiDienChucVu { get; set; }

    public string? DiaChi { get; set; }

    public string? TenVietTat { get; set; }

    public string? Fax { get; set; }

    public string? KyHieu { get; set; }

    public string? SoUyQuyen { get; set; }

    public DateTime? NgayUyQuyen { get; set; }

    public string? NguoiCapNhat { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public string? NguoiSua { get; set; }

    public DateTime? NgaySua { get; set; }

    public string? ParentMa { get; set; }

    public string? IpNas { get; set; }

    public string? KyHieuHd { get; set; }

    public string? SoTaiKhoan { get; set; }

    public string? ChuTaiKhoan { get; set; }

    public string? TenDonViMoi { get; set; }

    public decimal? NgayKhoaDlTinhLuong { get; set; }
}
