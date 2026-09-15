using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class DmNhanVien
{
    public string MaNhanVien { get; set; } = null!;

    public string? TenNhanVien { get; set; }

    public string? DienThoai { get; set; }

    public string? MaChiNhanh { get; set; }

    public string? TenNhanVienD { get; set; }

    public string? NhomTruong { get; set; }

    public string? ThiCong { get; set; }

    public string? DocChiSo { get; set; }

    public string? HieuLuc { get; set; }

    public string? Imei { get; set; }

    public string? Usr { get; set; }

    public string? Pas { get; set; }

    public string? Token { get; set; }

    public DateTime? LogDate { get; set; }

    public string? ImeiSub { get; set; }

    public decimal? CanhBaoPt { get; set; }

    public decimal? CanhBaoM3 { get; set; }

    public decimal? DungLuongPin { get; set; }

    public decimal? DungLuong4g { get; set; }

    public string? VerCode { get; set; }

    public decimal? CanhBaoGiamPt { get; set; }

    public decimal? CanhBaoGiamM3 { get; set; }

    public string? DienThoaiApp { get; set; }

    public string? VerCodeTc { get; set; }

    public string? ImeiSub2 { get; set; }

    public string? MaSoBhxh { get; set; }

    public string? LoaiApp { get; set; }

    public string? NhomTruongParent { get; set; }

    public string? QuyenWebapp { get; set; }
}
