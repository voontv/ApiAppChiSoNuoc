using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ReadMeter.Api.OracleModels;

public partial class ReadMeterDbContext : DbContext
{
    public ReadMeterDbContext()
    {
    }

    public ReadMeterDbContext(DbContextOptions<ReadMeterDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppCheckInCatNuoc> AppCheckInCatNuocs { get; set; }

    public virtual DbSet<AppCheckInGuiGiay> AppCheckInGuiGiays { get; set; }

    public virtual DbSet<AppCheckInMoNuoc> AppCheckInMoNuocs { get; set; }

    public virtual DbSet<AppDhChiSo> AppDhChiSos { get; set; }

    public virtual DbSet<AppDhDmGhiChu> AppDhDmGhiChus { get; set; }

    public virtual DbSet<AppDhLogBienDoc> AppDhLogBienDocs { get; set; }

    public virtual DbSet<AppDhThangDoc> AppDhThangDocs { get; set; }

    public virtual DbSet<AppDhThiCong> AppDhThiCongs { get; set; }

    public virtual DbSet<AppDhUpdate> AppDhUpdates { get; set; }

    public virtual DbSet<AppDhUpdateThiCong> AppDhUpdateThiCongs { get; set; }

    public virtual DbSet<AppExDmGhiChu> AppExDmGhiChus { get; set; }

    public virtual DbSet<AppExDmNhanVien> AppExDmNhanViens { get; set; }

    public virtual DbSet<AppExDssd> AppExDssds { get; set; }

    public virtual DbSet<AppExDssdSub> AppExDssdSubs { get; set; }

    public virtual DbSet<AppExKiemTraGiaSub> AppExKiemTraGiaSubs { get; set; }

    public virtual DbSet<AppExKiemTraGium> AppExKiemTraGia { get; set; }

    public virtual DbSet<AppExT> AppExTs { get; set; }

    public virtual DbSet<AppExUpdate> AppExUpdates { get; set; }

    public virtual DbSet<AppTc00LapMoi> AppTc00LapMois { get; set; }

    public virtual DbSet<AppTc01CaiTao> AppTc01CaiTaos { get; set; }

    public virtual DbSet<AppTc02ThayDh> AppTc02ThayDhs { get; set; }

    public virtual DbSet<AppTc03CanDo> AppTc03CanDos { get; set; }

    public virtual DbSet<AppTc04SucSa> AppTc04SucSas { get; set; }

    public virtual DbSet<AppTc06OngBe> AppTc06OngBes { get; set; }

    public virtual DbSet<AppTc07CatNuoc> AppTc07CatNuocs { get; set; }

    public virtual DbSet<AppTc08MoNuoc> AppTc08MoNuocs { get; set; }

    public virtual DbSet<AppTc99NiemChiNhua> AppTc99NiemChiNhuas { get; set; }

    public virtual DbSet<Cc01TtkhYeuCau> Cc01TtkhYeuCaus { get; set; }

    public virtual DbSet<CcDmNoiDungYc> CcDmNoiDungYcs { get; set; }

    public virtual DbSet<CcMangLuoi> CcMangLuois { get; set; }

    public virtual DbSet<ChiSoDh> ChiSoDhs { get; set; }

    public virtual DbSet<ChiSoDhSub> ChiSoDhSubs { get; set; }

    public virtual DbSet<CongNo> CongNos { get; set; }

    public virtual DbSet<CongNoGium> CongNoGia { get; set; }

    public virtual DbSet<Dab02DocChiSoDh> Dab02DocChiSoDhs { get; set; }

    public virtual DbSet<Dm01DonVi> Dm01DonVis { get; set; }

    public virtual DbSet<DmDiemThuTien> DmDiemThuTiens { get; set; }

    public virtual DbSet<DmKimNiem> DmKimNiems { get; set; }

    public virtual DbSet<DmLoaiSmsEmail> DmLoaiSmsEmails { get; set; }

    public virtual DbSet<DmNhanVien> DmNhanViens { get; set; }

    public virtual DbSet<DmSoDoc> DmSoDocs { get; set; }

    public virtual DbSet<DmTinhTrangDongHo> DmTinhTrangDongHos { get; set; }

    public virtual DbSet<DmTrangThaiEmail> DmTrangThaiEmails { get; set; }

    public virtual DbSet<DmTrangThaiSm> DmTrangThaiSms { get; set; }

    public virtual DbSet<KDmDiaChinh> KDmDiaChinhs { get; set; }

    public virtual DbSet<KDmGiaPhi> KDmGiaPhis { get; set; }

    public virtual DbSet<KDmGiaSub> KDmGiaSubs { get; set; }

    public virtual DbSet<KDmGium> KDmGia { get; set; }

    public virtual DbSet<LogBankDonVi> LogBankDonVis { get; set; }

    public virtual DbSet<LogBankFix> LogBankFixes { get; set; }

    public virtual DbSet<LogBankTienMoNuoc> LogBankTienMoNuocs { get; set; }

    public virtual DbSet<LogEmail> LogEmails { get; set; }

    public virtual DbSet<LogSm> LogSms { get; set; }

    public virtual DbSet<Password> Passwords { get; set; }

    public virtual DbSet<ThongTinKh> ThongTinKhs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("DATA SOURCE=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=200.201.222.8)(PORT=1521))(CONNECT_DATA=(SID=GEN)));USER ID=gen_loc; Password=heovang;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("GEN_LOC");

        modelBuilder.Entity<AppCheckInCatNuoc>(entity =>
        {
            entity.HasKey(e => e.IdXacNhan).HasName("APP_DH_XAC_NHAC_VI_TRI_PK");

            entity.ToTable("APP_CHECK_IN_CAT_NUOC");

            entity.HasIndex(e => e.Stt, "APP_CHECK_IN_CAT_NUOC_INDEX1");

            entity.Property(e => e.IdXacNhan)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_XAC_NHAN");
            entity.Property(e => e.ChiSoCat)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_CAT");
            entity.Property(e => e.ChiSoDh)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_DH");
            entity.Property(e => e.ChiSoNiem)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_NIEM");
            entity.Property(e => e.GhiChuThem)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("GHI_CHU_THEM");
            entity.Property(e => e.GhiChuXn)
                .HasMaxLength(80)
                .HasColumnName("GHI_CHU_XN");
            entity.Property(e => e.IdBilling)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_BILLING");
            entity.Property(e => e.KieuCatMo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("KIEU_CAT_MO");
            entity.Property(e => e.LogDateApp)
                .HasColumnType("DATE")
                .HasColumnName("LOG_DATE_APP");
            entity.Property(e => e.LogUser)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("LOG_USER");
            entity.Property(e => e.MaGhiChu)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_GHI_CHU");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaNhanVien)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN");
            entity.Property(e => e.MaSoDoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SO_DOC");
            entity.Property(e => e.MaXiNghiep)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_XI_NGHIEP");
            entity.Property(e => e.NgayDoc)
                .HasColumnType("NUMBER")
                .HasColumnName("NGAY_DOC");
            entity.Property(e => e.NgayHoanThanh)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_HOAN_THANH");
            entity.Property(e => e.NgayNiem)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_NIEM");
            entity.Property(e => e.NgayXnBgNhanVien)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_XN_BG_NHAN_VIEN");
            entity.Property(e => e.NgayXnNhanLaiBg)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_XN_NHAN_LAI_BG");
            entity.Property(e => e.NguoiThiCong)
                .HasMaxLength(50)
                .HasColumnName("NGUOI_THI_CONG");
            entity.Property(e => e.SoKimNiem)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SO_KIM_NIEM");
            entity.Property(e => e.Stt)
                .HasColumnType("NUMBER")
                .HasColumnName("STT");
            entity.Property(e => e.TenFileAnh)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH");
            entity.Property(e => e.Thang)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("THANG");
            entity.Property(e => e.ViTriXacNhan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN");
            entity.Property(e => e.ViTriXacNhanCu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN_CU");
        });

        modelBuilder.Entity<AppCheckInGuiGiay>(entity =>
        {
            entity.HasKey(e => e.IdXacNhan).HasName("APP_CHECK_IN_GUI_GIAY_PK");

            entity.ToTable("APP_CHECK_IN_GUI_GIAY");

            entity.HasIndex(e => e.Stt, "APP_CHECK_IN_GUI_GIAY_INDEX1");

            entity.Property(e => e.IdXacNhan)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_XAC_NHAN");
            entity.Property(e => e.ChiSoDh)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_DH");
            entity.Property(e => e.GhiChuThem)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("GHI_CHU_THEM");
            entity.Property(e => e.GhiChuXn)
                .HasMaxLength(80)
                .HasColumnName("GHI_CHU_XN");
            entity.Property(e => e.LogDateApp)
                .HasColumnType("DATE")
                .HasColumnName("LOG_DATE_APP");
            entity.Property(e => e.LogUser)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("LOG_USER");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaNhanVien)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN");
            entity.Property(e => e.MaSoDoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SO_DOC");
            entity.Property(e => e.MaXiNghiep)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_XI_NGHIEP");
            entity.Property(e => e.NgayDoc)
                .HasColumnType("NUMBER")
                .HasColumnName("NGAY_DOC");
            entity.Property(e => e.NgayHoanThanh)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_HOAN_THANH");
            entity.Property(e => e.NgayXnBgNhanVien)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_XN_BG_NHAN_VIEN");
            entity.Property(e => e.NgayXnNhanLaiBg)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_XN_NHAN_LAI_BG");
            entity.Property(e => e.Stt)
                .HasColumnType("NUMBER")
                .HasColumnName("STT");
            entity.Property(e => e.TenFileAnh)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH");
            entity.Property(e => e.Thang)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("THANG");
            entity.Property(e => e.ViTriXacNhan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN");
            entity.Property(e => e.ViTriXacNhanCu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN_CU");
        });

        modelBuilder.Entity<AppCheckInMoNuoc>(entity =>
        {
            entity.HasKey(e => e.IdXacNhan).HasName("APP_CHECK_IN_MO_NUOC_PK");

            entity.ToTable("APP_CHECK_IN_MO_NUOC");

            entity.HasIndex(e => e.Stt, "APP_CHECK_IN_MO_NUOC_INDEX1");

            entity.Property(e => e.IdXacNhan)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_XAC_NHAN");
            entity.Property(e => e.ChiSoDh)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_DH");
            entity.Property(e => e.ChiSoMo)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_MO");
            entity.Property(e => e.ChiSoNiem)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_NIEM");
            entity.Property(e => e.GhiChuThem)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("GHI_CHU_THEM");
            entity.Property(e => e.GhiChuXn)
                .HasMaxLength(80)
                .HasColumnName("GHI_CHU_XN");
            entity.Property(e => e.IdBilling)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_BILLING");
            entity.Property(e => e.KieuCatMo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("KIEU_CAT_MO");
            entity.Property(e => e.LogDateApp)
                .HasColumnType("DATE")
                .HasColumnName("LOG_DATE_APP");
            entity.Property(e => e.LogUser)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("LOG_USER");
            entity.Property(e => e.MaGhiChu)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_GHI_CHU");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaNhanVien)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN");
            entity.Property(e => e.MaSoDoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SO_DOC");
            entity.Property(e => e.MaXiNghiep)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_XI_NGHIEP");
            entity.Property(e => e.NgayDoc)
                .HasColumnType("NUMBER")
                .HasColumnName("NGAY_DOC");
            entity.Property(e => e.NgayHoanThanh)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_HOAN_THANH");
            entity.Property(e => e.NgayNiem)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_NIEM");
            entity.Property(e => e.NgayXnBgNhanVien)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_XN_BG_NHAN_VIEN");
            entity.Property(e => e.NgayXnNhanLaiBg)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_XN_NHAN_LAI_BG");
            entity.Property(e => e.NguoiThiCong)
                .HasMaxLength(50)
                .HasColumnName("NGUOI_THI_CONG");
            entity.Property(e => e.SoKimNiem)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SO_KIM_NIEM");
            entity.Property(e => e.Stt)
                .HasColumnType("NUMBER")
                .HasColumnName("STT");
            entity.Property(e => e.TenFileAnh)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH");
            entity.Property(e => e.Thang)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("THANG");
            entity.Property(e => e.ViTriXacNhan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN");
            entity.Property(e => e.ViTriXacNhanCu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN_CU");
        });

        modelBuilder.Entity<AppDhChiSo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("APP_DH_CHI_SO");

            entity.HasIndex(e => e.IdDcs, "APP_DH_CHI_SO_INDEX1");

            entity.HasIndex(e => e.SttSoDoc, "APP_DH_CHI_SO_INDEX10");

            entity.HasIndex(e => e.NgayDocDk, "APP_DH_CHI_SO_INDEX11");

            entity.HasIndex(e => e.NgayDocTungDh, "APP_DH_CHI_SO_INDEX12");

            entity.HasIndex(e => e.MaKhachHang, "APP_DH_CHI_SO_INDEX2");

            entity.HasIndex(e => e.Thang, "APP_DH_CHI_SO_INDEX3");

            entity.HasIndex(e => e.MaSoDoc, "APP_DH_CHI_SO_INDEX4");

            entity.HasIndex(e => e.MaBienDoc, "APP_DH_CHI_SO_INDEX5");

            entity.HasIndex(e => e.NgayEbillBg, "APP_DH_CHI_SO_INDEX6");

            entity.HasIndex(e => e.NgayEbillNapBill, "APP_DH_CHI_SO_INDEX7");

            entity.HasIndex(e => e.NgayEbillNhanKhoa, "APP_DH_CHI_SO_INDEX8");

            entity.HasIndex(e => e.MaChiNhanh, "APP_DH_CHI_SO_INDEX9");

            entity.Property(e => e.ChiSoCu)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_CU");
            entity.Property(e => e.ChiSoMoi)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_MOI");
            entity.Property(e => e.ChiSoMoiSub)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_MOI_SUB");
            entity.Property(e => e.CongChiSo)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER")
                .HasColumnName("CONG_CHI_SO");
            entity.Property(e => e.DocBoSung)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("DOC_BO_SUNG");
            entity.Property(e => e.GhiChu)
                .HasMaxLength(80)
                .HasColumnName("GHI_CHU");
            entity.Property(e => e.IdDcs)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_DCS");
            entity.Property(e => e.LoaiChiSo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("0 ")
                .HasColumnName("LOAI_CHI_SO");
            entity.Property(e => e.LogUser)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("LOG_USER");
            entity.Property(e => e.MaBienDoc)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_BIEN_DOC");
            entity.Property(e => e.MaChiNhanh)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_CHI_NHANH");
            entity.Property(e => e.MaGhiChu)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_GHI_CHU");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaSoDoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SO_DOC");
            entity.Property(e => e.MaTinhTrangDh)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_TINH_TRANG_DH");
            entity.Property(e => e.NgayBdNhanKhoa)
                .HasDefaultValueSql("NULL ")
                .HasColumnType("DATE")
                .HasColumnName("NGAY_BD_NHAN_KHOA");
            entity.Property(e => e.NgayBienDocBg)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_BIEN_DOC_BG");
            entity.Property(e => e.NgayDocCs)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_DOC_CS");
            entity.Property(e => e.NgayDocDk)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_DOC_DK");
            entity.Property(e => e.NgayDocTruoc)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_DOC_TRUOC");
            entity.Property(e => e.NgayDocTungDh)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_DOC_TUNG_DH");
            entity.Property(e => e.NgayEbillBg)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_EBILL_BG");
            entity.Property(e => e.NgayEbillNapBill)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_EBILL_NAP_BILL");
            entity.Property(e => e.NgayEbillNhanKhoa)
                .HasDefaultValueSql("NULL ")
                .HasColumnType("DATE")
                .HasColumnName("NGAY_EBILL_NHAN_KHOA");
            entity.Property(e => e.QuaVong)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("NULL ")
                .HasColumnName("QUA_VONG");
            entity.Property(e => e.SanLuongDungIt)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER")
                .HasColumnName("SAN_LUONG_DUNG_IT");
            entity.Property(e => e.SanLuongTt)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER")
                .HasColumnName("SAN_LUONG_TT");
            entity.Property(e => e.SlTb3thang)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER")
                .HasColumnName("SL_TB_3THANG");
            entity.Property(e => e.SttSoDoc)
                .HasColumnType("NUMBER")
                .HasColumnName("STT_SO_DOC");
            entity.Property(e => e.SttSoDocMoi)
                .HasColumnType("NUMBER")
                .HasColumnName("STT_SO_DOC_MOI");
            entity.Property(e => e.SuaDhSerial)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("SUA_DH_SERIAL");
            entity.Property(e => e.SuaDhTen)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SUA_DH_TEN");
            entity.Property(e => e.SuaDiaChiDongHo)
                .HasMaxLength(80)
                .HasColumnName("SUA_DIA_CHI_DONG_HO");
            entity.Property(e => e.SuaEmail)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SUA_EMAIL");
            entity.Property(e => e.SuaGhiChu)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("SUA_GHI_CHU");
            entity.Property(e => e.SuaSoDt)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("SUA_SO_DT");
            entity.Property(e => e.SuaTenKhachHang)
                .HasMaxLength(80)
                .HasColumnName("SUA_TEN_KHACH_HANG");
            entity.Property(e => e.TenFileAnh)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH");
            entity.Property(e => e.Thang)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("THANG");
            entity.Property(e => e.ThoiGianTre)
                .HasColumnType("NUMBER")
                .HasColumnName("THOI_GIAN_TRE");
            entity.Property(e => e.TongSl)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER")
                .HasColumnName("TONG_SL");
            entity.Property(e => e.TongSlSub)
                .HasColumnType("NUMBER")
                .HasColumnName("TONG_SL_SUB");
            entity.Property(e => e.ViTriDoc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_DOC");
            entity.Property(e => e.ViTriDocCu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_DOC_CU");
        });

        modelBuilder.Entity<AppDhDmGhiChu>(entity =>
        {
            entity.HasKey(e => e.MaGhiChu).HasName("APP_DH_DM_GHI_CHU_PK");

            entity.ToTable("APP_DH_DM_GHI_CHU");

            entity.Property(e => e.MaGhiChu)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_GHI_CHU");
            entity.Property(e => e.DenKiemTra)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("DEN_KIEM_TRA");
            entity.Property(e => e.Nhom)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("NHOM");
            entity.Property(e => e.NoiDungGhiChu)
                .HasMaxLength(100)
                .HasColumnName("NOI_DUNG_GHI_CHU");
            entity.Property(e => e.Stt)
                .HasColumnType("NUMBER")
                .HasColumnName("STT");
        });

        modelBuilder.Entity<AppDhLogBienDoc>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("APP_DH_LOG_BIEN_DOC");

            entity.HasIndex(e => e.IdBdLog, "APP_DH_LOG_BIEN_DOC_INDEX1");

            entity.HasIndex(e => e.MaBienDoc, "APP_DH_LOG_BIEN_DOC_MBD");

            entity.HasIndex(e => e.LogDate, "APP_DH_LOG_BIEN_DOC_N");

            entity.Property(e => e.DienGiai)
                .HasMaxLength(140)
                .HasColumnName("DIEN_GIAI");
            entity.Property(e => e.IdBdLog)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_BD_LOG");
            entity.Property(e => e.Imei)
                .HasMaxLength(18)
                .IsUnicode(false)
                .HasColumnName("IMEI");
            entity.Property(e => e.LogDate)
                .HasColumnType("DATE")
                .HasColumnName("LOG_DATE");
            entity.Property(e => e.MaBienDoc)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("MA_BIEN_DOC");
            entity.Property(e => e.MaXiNghiep)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_XI_NGHIEP");
            entity.Property(e => e.ThoiGianLoad)
                .HasColumnType("NUMBER")
                .HasColumnName("THOI_GIAN_LOAD");
            entity.Property(e => e.ThuocHam)
                .HasMaxLength(33)
                .IsUnicode(false)
                .HasColumnName("THUOC_HAM");
        });

        modelBuilder.Entity<AppDhThangDoc>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("APP_DH_THANG_DOC");

            entity.Property(e => e.IdThangDoc)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_THANG_DOC");
            entity.Property(e => e.Nam)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("NAM");
            entity.Property(e => e.Thang)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("THANG");
        });

        modelBuilder.Entity<AppDhThiCong>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("APP_DH_THI_CONG");

            entity.HasIndex(e => e.IdTc, "APP_DH_THI_CONG_INDEX1");

            entity.HasIndex(e => e.MaKhachHang, "APP_DH_THI_CONG_INDEX2");

            entity.Property(e => e.ChiSoLap)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_LAP");
            entity.Property(e => e.ChiSoThao)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_THAO");
            entity.Property(e => e.IdTc)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_TC");
            entity.Property(e => e.LoaiCongTrinh)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("LOAI_CONG_TRINH");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.NgayCapNhat)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.NgayGiaoTc)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_GIAO_TC");
        });

        modelBuilder.Entity<AppDhUpdate>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("APP_DH_UPDATE");

            entity.Property(e => e.ActIdDoc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ACT_ID_DOC");
            entity.Property(e => e.BanCapNhat)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BAN_CAP_NHAT");
            entity.Property(e => e.DuongDan)
                .HasMaxLength(110)
                .IsUnicode(false)
                .HasColumnName("DUONG_DAN");
            entity.Property(e => e.NgayCapNhat)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.TgCapNhatSlTv)
                .HasColumnType("NUMBER")
                .HasColumnName("TG_CAP_NHAT_SL_TV");
        });

        modelBuilder.Entity<AppDhUpdateThiCong>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("APP_DH_UPDATE_THI_CONG");

            entity.Property(e => e.ActIdDoc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ACT_ID_DOC");
            entity.Property(e => e.BanCapNhat)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BAN_CAP_NHAT");
            entity.Property(e => e.DuongDan)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DUONG_DAN");
            entity.Property(e => e.NgayCapNhat)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.TgCapNhatSlTv)
                .HasColumnType("NUMBER")
                .HasColumnName("TG_CAP_NHAT_SL_TV");
        });

        modelBuilder.Entity<AppExDmGhiChu>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("APP_EX_DM_GHI_CHU");

            entity.Property(e => e.Dis)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("DIS");
            entity.Property(e => e.MaGhiChu)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_GHI_CHU");
            entity.Property(e => e.Stt)
                .HasColumnType("NUMBER")
                .HasColumnName("STT");
            entity.Property(e => e.TenGhiChu)
                .HasMaxLength(50)
                .HasColumnName("TEN_GHI_CHU");
        });

        modelBuilder.Entity<AppExDmNhanVien>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("APP_EX_DM_NHAN_VIEN");

            entity.Property(e => e.Doan)
                .HasColumnType("NUMBER")
                .HasColumnName("DOAN");
            entity.Property(e => e.MaNhanVien)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN");
            entity.Property(e => e.MaXn)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_XN");
            entity.Property(e => e.Stt)
                .HasColumnType("NUMBER")
                .HasColumnName("STT");
            entity.Property(e => e.TenNhanVien)
                .HasMaxLength(50)
                .HasColumnName("TEN_NHAN_VIEN");
        });

        modelBuilder.Entity<AppExDssd>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("APP_EX_DSSD");

            entity.Property(e => e.Dis)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("NULL ")
                .HasColumnName("DIS");
            entity.Property(e => e.Doan)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("DOAN");
            entity.Property(e => e.GhiChu)
                .HasMaxLength(50)
                .HasColumnName("GHI_CHU");
            entity.Property(e => e.HoanThanh)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("HOAN_THANH");
            entity.Property(e => e.MaNhanVien)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN");
            entity.Property(e => e.MaNhanVienCu)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN_CU");
            entity.Property(e => e.MaSoDoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SO_DOC");
            entity.Property(e => e.MaSoDocCu)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SO_DOC_CU");
            entity.Property(e => e.Ngay)
                .HasColumnType("DATE")
                .HasColumnName("NGAY");
            entity.Property(e => e.Vung)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("VUNG");
        });

        modelBuilder.Entity<AppExDssdSub>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("APP_EX_DSSD_SUB");

            entity.Property(e => e.Dis)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("DIS");
            entity.Property(e => e.Doan)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("DOAN");
            entity.Property(e => e.GhiChu)
                .HasMaxLength(50)
                .HasColumnName("GHI_CHU");
            entity.Property(e => e.HoanThanh)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("HOAN_THANH");
            entity.Property(e => e.MaNhanVien)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN");
            entity.Property(e => e.MaNhanVienCu)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN_CU");
            entity.Property(e => e.MaSoDoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SO_DOC");
            entity.Property(e => e.MaSoDocCu)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SO_DOC_CU");
            entity.Property(e => e.Ngay)
                .HasColumnType("DATE")
                .HasColumnName("NGAY");
            entity.Property(e => e.Vung)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("VUNG");
        });

        modelBuilder.Entity<AppExKiemTraGiaSub>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("APP_EX_KIEM_TRA_GIA_SUB");

            entity.Property(e => e.ChiSoCuoi)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_CUOI");
            entity.Property(e => e.DiaChiDongHo)
                .HasMaxLength(180)
                .IsUnicode(false)
                .HasColumnName("DIA_CHI_DONG_HO");
            entity.Property(e => e.DinhMuc)
                .HasColumnType("NUMBER")
                .HasColumnName("DINH_MUC");
            entity.Property(e => e.IdApGia)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_AP_GIA");
            entity.Property(e => e.MaBienDoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_BIEN_DOC");
            entity.Property(e => e.MaGia)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MA_GIA");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaSoDoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SO_DOC");
            entity.Property(e => e.MaXiNghiep)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_XI_NGHIEP");
            entity.Property(e => e.NgayGiao)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_GIAO");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("SO_DIEN_THOAI");
            entity.Property(e => e.SoHo)
                .HasColumnType("NUMBER")
                .HasColumnName("SO_HO");
            entity.Property(e => e.SoKhau)
                .HasColumnType("NUMBER")
                .HasColumnName("SO_KHAU");
            entity.Property(e => e.SoSerialDongHo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SO_SERIAL_DONG_HO");
            entity.Property(e => e.SttSoDoc)
                .HasColumnType("NUMBER")
                .HasColumnName("STT_SO_DOC");
            entity.Property(e => e.T01MaGia)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("T01_MA_GIA");
            entity.Property(e => e.T01SanLuong)
                .HasColumnType("NUMBER")
                .HasColumnName("T01_SAN_LUONG");
            entity.Property(e => e.T01TenGia)
                .HasMaxLength(80)
                .HasColumnName("T01_TEN_GIA");
            entity.Property(e => e.T01Thang)
                .HasMaxLength(10)
                .HasColumnName("T01_THANG");
            entity.Property(e => e.T02MaGia)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("T02_MA_GIA");
            entity.Property(e => e.T02SanLuong)
                .HasColumnType("NUMBER")
                .HasColumnName("T02_SAN_LUONG");
            entity.Property(e => e.T02TenGia)
                .HasMaxLength(80)
                .HasColumnName("T02_TEN_GIA");
            entity.Property(e => e.T02Thang)
                .HasMaxLength(10)
                .HasColumnName("T02_THANG");
            entity.Property(e => e.T03MaGia)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("T03_MA_GIA");
            entity.Property(e => e.T03SanLuong)
                .HasColumnType("NUMBER")
                .HasColumnName("T03_SAN_LUONG");
            entity.Property(e => e.T03TenGia)
                .HasMaxLength(80)
                .HasColumnName("T03_TEN_GIA");
            entity.Property(e => e.T03Thang)
                .HasMaxLength(10)
                .HasColumnName("T03_THANG");
            entity.Property(e => e.Temp)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("TEMP");
            entity.Property(e => e.TenKhachHang)
                .HasMaxLength(170)
                .IsUnicode(false)
                .HasColumnName("TEN_KHACH_HANG");
            entity.Property(e => e.TinhTrangDh)
                .HasMaxLength(3)
                .HasColumnName("TINH_TRANG_DH");
            entity.Property(e => e.UGhiChuThem)
                .HasMaxLength(80)
                .HasColumnName("U_GHI_CHU_THEM");
            entity.Property(e => e.UHcsn)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("U_HCSN");
            entity.Property(e => e.UKd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("U_KD");
            entity.Property(e => e.ULogDate)
                .HasColumnType("DATE")
                .HasColumnName("U_LOG_DATE");
            entity.Property(e => e.UMaGhiChu)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("U_MA_GHI_CHU");
            entity.Property(e => e.UNguoiDung)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("U_NGUOI_DUNG");
            entity.Property(e => e.USh)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("U_SH");
            entity.Property(e => e.USx)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("U_SX");
            entity.Property(e => e.UTenFileAnh)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("U_TEN_FILE_ANH");
            entity.Property(e => e.UViTriDoc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("U_VI_TRI_DOC");
        });

        modelBuilder.Entity<AppExKiemTraGium>(entity =>
        {
            entity.HasKey(e => e.IdApGia).HasName("APP_EX_KIEM_TRA_GIA_PK");

            entity.ToTable("APP_EX_KIEM_TRA_GIA");

            entity.HasIndex(e => e.MaKhachHang, "APP_EX_KIEM_TRA_GIA_INDEX1");

            entity.HasIndex(e => e.ULogDate, "APP_EX_KIEM_TRA_GIA_INDEX2");

            entity.HasIndex(e => e.MaSoDoc, "APP_EX_KIEM_TRA_GIA_INDEX3");

            entity.Property(e => e.IdApGia)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_AP_GIA");
            entity.Property(e => e.ChiSoCuoi)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_CUOI");
            entity.Property(e => e.DiaChiDongHo)
                .HasMaxLength(180)
                .IsUnicode(false)
                .HasColumnName("DIA_CHI_DONG_HO");
            entity.Property(e => e.DinhMuc)
                .HasColumnType("NUMBER")
                .HasColumnName("DINH_MUC");
            entity.Property(e => e.MaBienDoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_BIEN_DOC");
            entity.Property(e => e.MaGia)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MA_GIA");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaSoDoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SO_DOC");
            entity.Property(e => e.MaXiNghiep)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_XI_NGHIEP");
            entity.Property(e => e.NgayGiao)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_GIAO");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("SO_DIEN_THOAI");
            entity.Property(e => e.SoHo)
                .HasColumnType("NUMBER")
                .HasColumnName("SO_HO");
            entity.Property(e => e.SoKhau)
                .HasColumnType("NUMBER")
                .HasColumnName("SO_KHAU");
            entity.Property(e => e.SoSerialDongHo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SO_SERIAL_DONG_HO");
            entity.Property(e => e.SttSoDoc)
                .HasColumnType("NUMBER")
                .HasColumnName("STT_SO_DOC");
            entity.Property(e => e.T01MaGia)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("T01_MA_GIA");
            entity.Property(e => e.T01SanLuong)
                .HasColumnType("NUMBER")
                .HasColumnName("T01_SAN_LUONG");
            entity.Property(e => e.T01TenGia)
                .HasMaxLength(80)
                .HasColumnName("T01_TEN_GIA");
            entity.Property(e => e.T01Thang)
                .HasMaxLength(10)
                .HasColumnName("T01_THANG");
            entity.Property(e => e.T02MaGia)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("T02_MA_GIA");
            entity.Property(e => e.T02SanLuong)
                .HasColumnType("NUMBER")
                .HasColumnName("T02_SAN_LUONG");
            entity.Property(e => e.T02TenGia)
                .HasMaxLength(80)
                .HasColumnName("T02_TEN_GIA");
            entity.Property(e => e.T02Thang)
                .HasMaxLength(10)
                .HasColumnName("T02_THANG");
            entity.Property(e => e.T03MaGia)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("T03_MA_GIA");
            entity.Property(e => e.T03SanLuong)
                .HasColumnType("NUMBER")
                .HasColumnName("T03_SAN_LUONG");
            entity.Property(e => e.T03TenGia)
                .HasMaxLength(80)
                .HasColumnName("T03_TEN_GIA");
            entity.Property(e => e.T03Thang)
                .HasMaxLength(10)
                .HasColumnName("T03_THANG");
            entity.Property(e => e.Temp)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("TEMP");
            entity.Property(e => e.TenKhachHang)
                .HasMaxLength(170)
                .IsUnicode(false)
                .HasColumnName("TEN_KHACH_HANG");
            entity.Property(e => e.TinhTrangDh)
                .HasMaxLength(3)
                .HasColumnName("TINH_TRANG_DH");
            entity.Property(e => e.UGhiChuThem)
                .HasMaxLength(80)
                .HasColumnName("U_GHI_CHU_THEM");
            entity.Property(e => e.UHcsn)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("U_HCSN");
            entity.Property(e => e.UKd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("U_KD");
            entity.Property(e => e.ULogDate)
                .HasColumnType("DATE")
                .HasColumnName("U_LOG_DATE");
            entity.Property(e => e.UMaGhiChu)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("U_MA_GHI_CHU");
            entity.Property(e => e.UNguoiDung)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("U_NGUOI_DUNG");
            entity.Property(e => e.USh)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("U_SH");
            entity.Property(e => e.USx)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("U_SX");
            entity.Property(e => e.UTenFileAnh)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("U_TEN_FILE_ANH");
            entity.Property(e => e.UViTriDoc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("U_VI_TRI_DOC");
        });

        modelBuilder.Entity<AppExT>(entity =>
        {
            entity.HasKey(e => e.SoDoc).HasName("APP_EX_TS_PK");

            entity.ToTable("APP_EX_TS");

            entity.Property(e => e.SoDoc)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .HasColumnName("SO_DOC");
        });

        modelBuilder.Entity<AppExUpdate>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("APP_EX_UPDATE");

            entity.Property(e => e.ActIdDoc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ACT_ID_DOC");
            entity.Property(e => e.BanCapNhat)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BAN_CAP_NHAT");
            entity.Property(e => e.DuongDan)
                .HasMaxLength(110)
                .IsUnicode(false)
                .HasColumnName("DUONG_DAN");
            entity.Property(e => e.NgayCapNhat)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.TgCapNhatSlTv)
                .HasColumnType("NUMBER")
                .HasColumnName("TG_CAP_NHAT_SL_TV");
        });

        modelBuilder.Entity<AppTc00LapMoi>(entity =>
        {
            entity.HasKey(e => e.IdXacNhan).HasName("APP_TC_00_LAP_MOI_PK");

            entity.ToTable("APP_TC_00_LAP_MOI");

            entity.Property(e => e.IdXacNhan)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_XAC_NHAN");
            entity.Property(e => e.ChiSoCatMo)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_CAT_MO");
            entity.Property(e => e.ChiSoDh)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_DH");
            entity.Property(e => e.ChiSoNiem)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_NIEM");
            entity.Property(e => e.GhiChuBilling)
                .HasMaxLength(200)
                .HasColumnName("GHI_CHU_BILLING");
            entity.Property(e => e.GhiChuThem)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("GHI_CHU_THEM");
            entity.Property(e => e.IdBilling)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_BILLING");
            entity.Property(e => e.KieuCatMo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("KIEU_CAT_MO");
            entity.Property(e => e.LogDateApp)
                .HasColumnType("DATE")
                .HasColumnName("LOG_DATE_APP");
            entity.Property(e => e.LogUser)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("LOG_USER");
            entity.Property(e => e.MaBienDoc)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_BIEN_DOC");
            entity.Property(e => e.MaGhiChu)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_GHI_CHU");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaKimNiem)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_KIM_NIEM");
            entity.Property(e => e.MaNhanVienCn)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN_CN");
            entity.Property(e => e.MaNhanVienNt)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN_NT");
            entity.Property(e => e.MaSoDoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SO_DOC");
            entity.Property(e => e.MaTttc)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_TTTC");
            entity.Property(e => e.MaXiNghiep)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_XI_NGHIEP");
            entity.Property(e => e.Ngay10XnBilling)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_10_XN_BILLING");
            entity.Property(e => e.Ngay1XnGiaoNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_1_XN_GIAO_NT");
            entity.Property(e => e.Ngay2NtNhanXn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_2_NT_NHAN_XN");
            entity.Property(e => e.Ngay3NtGiaoCn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_3_NT_GIAO_CN");
            entity.Property(e => e.Ngay4CnNhanNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_4_CN_NHAN_NT");
            entity.Property(e => e.Ngay5CnTcOk)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_5_CN_TC_OK");
            entity.Property(e => e.Ngay6CnGiaoNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_6_CN_GIAO_NT");
            entity.Property(e => e.Ngay7NtNhanCn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_7_NT_NHAN_CN");
            entity.Property(e => e.Ngay8NtGiaoXn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_8_NT_GIAO_XN");
            entity.Property(e => e.Ngay9XnNhanNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_9_XN_NHAN_NT");
            entity.Property(e => e.NgayBillingGiao)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_BILLING_GIAO");
            entity.Property(e => e.NgayNiem)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_NIEM");
            entity.Property(e => e.NguoiDaiDienTc)
                .HasMaxLength(50)
                .HasColumnName("NGUOI_DAI_DIEN_TC");
            entity.Property(e => e.SoKimNiem)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SO_KIM_NIEM");
            entity.Property(e => e.SttHs)
                .HasColumnType("NUMBER")
                .HasColumnName("STT_HS");
            entity.Property(e => e.TenFileAnh01)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH_01");
            entity.Property(e => e.TenFileAnh02)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH_02");
            entity.Property(e => e.ViTriXacNhan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN");
            entity.Property(e => e.ViTriXacNhanCu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN_CU");
        });

        modelBuilder.Entity<AppTc01CaiTao>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("APP_TC_01_CAI_TAO");

            entity.Property(e => e.ChiSoCatMo)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_CAT_MO");
            entity.Property(e => e.ChiSoDh)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_DH");
            entity.Property(e => e.ChiSoNiem)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_NIEM");
            entity.Property(e => e.GhiChuBilling)
                .HasMaxLength(200)
                .HasColumnName("GHI_CHU_BILLING");
            entity.Property(e => e.GhiChuThem)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("GHI_CHU_THEM");
            entity.Property(e => e.IdBilling)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_BILLING");
            entity.Property(e => e.IdXacNhan)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_XAC_NHAN");
            entity.Property(e => e.KieuCatMo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("KIEU_CAT_MO");
            entity.Property(e => e.LogDateApp)
                .HasColumnType("DATE")
                .HasColumnName("LOG_DATE_APP");
            entity.Property(e => e.LogUser)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("LOG_USER");
            entity.Property(e => e.MaBienDoc)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_BIEN_DOC");
            entity.Property(e => e.MaGhiChu)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_GHI_CHU");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaKimNiem)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_KIM_NIEM");
            entity.Property(e => e.MaNhanVienCn)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN_CN");
            entity.Property(e => e.MaNhanVienNt)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN_NT");
            entity.Property(e => e.MaSoDoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SO_DOC");
            entity.Property(e => e.MaTttc)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_TTTC");
            entity.Property(e => e.MaXiNghiep)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_XI_NGHIEP");
            entity.Property(e => e.Ngay10XnBilling)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_10_XN_BILLING");
            entity.Property(e => e.Ngay1XnGiaoNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_1_XN_GIAO_NT");
            entity.Property(e => e.Ngay2NtNhanXn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_2_NT_NHAN_XN");
            entity.Property(e => e.Ngay3NtGiaoCn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_3_NT_GIAO_CN");
            entity.Property(e => e.Ngay4CnNhanNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_4_CN_NHAN_NT");
            entity.Property(e => e.Ngay5CnTcOk)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_5_CN_TC_OK");
            entity.Property(e => e.Ngay6CnGiaoNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_6_CN_GIAO_NT");
            entity.Property(e => e.Ngay7NtNhanCn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_7_NT_NHAN_CN");
            entity.Property(e => e.Ngay8NtGiaoXn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_8_NT_GIAO_XN");
            entity.Property(e => e.Ngay9XnNhanNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_9_XN_NHAN_NT");
            entity.Property(e => e.NgayBillingGiao)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_BILLING_GIAO");
            entity.Property(e => e.NgayNiem)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_NIEM");
            entity.Property(e => e.NguoiDaiDienTc)
                .HasMaxLength(50)
                .HasColumnName("NGUOI_DAI_DIEN_TC");
            entity.Property(e => e.SoKimNiem)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SO_KIM_NIEM");
            entity.Property(e => e.SttHs)
                .HasColumnType("NUMBER")
                .HasColumnName("STT_HS");
            entity.Property(e => e.TenFileAnh01)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH_01");
            entity.Property(e => e.TenFileAnh02)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH_02");
            entity.Property(e => e.ViTriXacNhan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN");
            entity.Property(e => e.ViTriXacNhanCu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN_CU");
        });

        modelBuilder.Entity<AppTc02ThayDh>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("APP_TC_02_THAY_DH");

            entity.Property(e => e.ChiSoCatMo)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_CAT_MO");
            entity.Property(e => e.ChiSoDh)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_DH");
            entity.Property(e => e.ChiSoNiem)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_NIEM");
            entity.Property(e => e.GhiChuBilling)
                .HasMaxLength(200)
                .HasColumnName("GHI_CHU_BILLING");
            entity.Property(e => e.GhiChuThem)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("GHI_CHU_THEM");
            entity.Property(e => e.IdBilling)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_BILLING");
            entity.Property(e => e.IdXacNhan)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_XAC_NHAN");
            entity.Property(e => e.KieuCatMo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("KIEU_CAT_MO");
            entity.Property(e => e.LogDateApp)
                .HasColumnType("DATE")
                .HasColumnName("LOG_DATE_APP");
            entity.Property(e => e.LogUser)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("LOG_USER");
            entity.Property(e => e.MaBienDoc)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_BIEN_DOC");
            entity.Property(e => e.MaGhiChu)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_GHI_CHU");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaKimNiem)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_KIM_NIEM");
            entity.Property(e => e.MaNhanVienCn)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN_CN");
            entity.Property(e => e.MaNhanVienNt)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN_NT");
            entity.Property(e => e.MaSoDoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SO_DOC");
            entity.Property(e => e.MaTttc)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_TTTC");
            entity.Property(e => e.MaXiNghiep)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_XI_NGHIEP");
            entity.Property(e => e.Ngay10XnBilling)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_10_XN_BILLING");
            entity.Property(e => e.Ngay1XnGiaoNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_1_XN_GIAO_NT");
            entity.Property(e => e.Ngay2NtNhanXn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_2_NT_NHAN_XN");
            entity.Property(e => e.Ngay3NtGiaoCn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_3_NT_GIAO_CN");
            entity.Property(e => e.Ngay4CnNhanNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_4_CN_NHAN_NT");
            entity.Property(e => e.Ngay5CnTcOk)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_5_CN_TC_OK");
            entity.Property(e => e.Ngay6CnGiaoNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_6_CN_GIAO_NT");
            entity.Property(e => e.Ngay7NtNhanCn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_7_NT_NHAN_CN");
            entity.Property(e => e.Ngay8NtGiaoXn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_8_NT_GIAO_XN");
            entity.Property(e => e.Ngay9XnNhanNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_9_XN_NHAN_NT");
            entity.Property(e => e.NgayBillingGiao)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_BILLING_GIAO");
            entity.Property(e => e.NgayNiem)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_NIEM");
            entity.Property(e => e.NguoiDaiDienTc)
                .HasMaxLength(50)
                .HasColumnName("NGUOI_DAI_DIEN_TC");
            entity.Property(e => e.SoKimNiem)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SO_KIM_NIEM");
            entity.Property(e => e.SttHs)
                .HasColumnType("NUMBER")
                .HasColumnName("STT_HS");
            entity.Property(e => e.TenFileAnh01)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH_01");
            entity.Property(e => e.TenFileAnh02)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH_02");
            entity.Property(e => e.ViTriXacNhan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN");
            entity.Property(e => e.ViTriXacNhanCu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN_CU");
        });

        modelBuilder.Entity<AppTc03CanDo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("APP_TC_03_CAN_DO");

            entity.Property(e => e.ChiSoCatMo)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_CAT_MO");
            entity.Property(e => e.ChiSoDh)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_DH");
            entity.Property(e => e.ChiSoNiem)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_NIEM");
            entity.Property(e => e.GhiChuBilling)
                .HasMaxLength(200)
                .HasColumnName("GHI_CHU_BILLING");
            entity.Property(e => e.GhiChuThem)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("GHI_CHU_THEM");
            entity.Property(e => e.IdBilling)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_BILLING");
            entity.Property(e => e.IdXacNhan)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_XAC_NHAN");
            entity.Property(e => e.KieuCatMo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("KIEU_CAT_MO");
            entity.Property(e => e.LogDateApp)
                .HasColumnType("DATE")
                .HasColumnName("LOG_DATE_APP");
            entity.Property(e => e.LogUser)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("LOG_USER");
            entity.Property(e => e.MaBienDoc)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_BIEN_DOC");
            entity.Property(e => e.MaGhiChu)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_GHI_CHU");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaKimNiem)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_KIM_NIEM");
            entity.Property(e => e.MaNhanVienCn)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN_CN");
            entity.Property(e => e.MaNhanVienNt)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN_NT");
            entity.Property(e => e.MaSoDoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SO_DOC");
            entity.Property(e => e.MaTttc)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_TTTC");
            entity.Property(e => e.MaXiNghiep)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_XI_NGHIEP");
            entity.Property(e => e.Ngay10XnBilling)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_10_XN_BILLING");
            entity.Property(e => e.Ngay1XnGiaoNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_1_XN_GIAO_NT");
            entity.Property(e => e.Ngay2NtNhanXn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_2_NT_NHAN_XN");
            entity.Property(e => e.Ngay3NtGiaoCn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_3_NT_GIAO_CN");
            entity.Property(e => e.Ngay4CnNhanNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_4_CN_NHAN_NT");
            entity.Property(e => e.Ngay5CnTcOk)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_5_CN_TC_OK");
            entity.Property(e => e.Ngay6CnGiaoNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_6_CN_GIAO_NT");
            entity.Property(e => e.Ngay7NtNhanCn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_7_NT_NHAN_CN");
            entity.Property(e => e.Ngay8NtGiaoXn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_8_NT_GIAO_XN");
            entity.Property(e => e.Ngay9XnNhanNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_9_XN_NHAN_NT");
            entity.Property(e => e.NgayBillingGiao)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_BILLING_GIAO");
            entity.Property(e => e.NgayNiem)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_NIEM");
            entity.Property(e => e.NguoiDaiDienTc)
                .HasMaxLength(50)
                .HasColumnName("NGUOI_DAI_DIEN_TC");
            entity.Property(e => e.SoKimNiem)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SO_KIM_NIEM");
            entity.Property(e => e.SttHs)
                .HasColumnType("NUMBER")
                .HasColumnName("STT_HS");
            entity.Property(e => e.TenFileAnh01)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH_01");
            entity.Property(e => e.TenFileAnh02)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH_02");
            entity.Property(e => e.ViTriXacNhan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN");
            entity.Property(e => e.ViTriXacNhanCu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN_CU");
        });

        modelBuilder.Entity<AppTc04SucSa>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("APP_TC_04_SUC_SA");

            entity.Property(e => e.ChiSoCatMo)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_CAT_MO");
            entity.Property(e => e.ChiSoDh)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_DH");
            entity.Property(e => e.ChiSoNiem)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_NIEM");
            entity.Property(e => e.GhiChuBilling)
                .HasMaxLength(200)
                .HasColumnName("GHI_CHU_BILLING");
            entity.Property(e => e.GhiChuThem)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("GHI_CHU_THEM");
            entity.Property(e => e.IdBilling)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_BILLING");
            entity.Property(e => e.IdXacNhan)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_XAC_NHAN");
            entity.Property(e => e.KieuCatMo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("KIEU_CAT_MO");
            entity.Property(e => e.LogDateApp)
                .HasColumnType("DATE")
                .HasColumnName("LOG_DATE_APP");
            entity.Property(e => e.LogUser)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("LOG_USER");
            entity.Property(e => e.MaBienDoc)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_BIEN_DOC");
            entity.Property(e => e.MaGhiChu)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_GHI_CHU");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaKimNiem)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_KIM_NIEM");
            entity.Property(e => e.MaNhanVienCn)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN_CN");
            entity.Property(e => e.MaNhanVienNt)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN_NT");
            entity.Property(e => e.MaSoDoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SO_DOC");
            entity.Property(e => e.MaTttc)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_TTTC");
            entity.Property(e => e.MaXiNghiep)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_XI_NGHIEP");
            entity.Property(e => e.Ngay10XnBilling)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_10_XN_BILLING");
            entity.Property(e => e.Ngay1XnGiaoNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_1_XN_GIAO_NT");
            entity.Property(e => e.Ngay2NtNhanXn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_2_NT_NHAN_XN");
            entity.Property(e => e.Ngay3NtGiaoCn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_3_NT_GIAO_CN");
            entity.Property(e => e.Ngay4CnNhanNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_4_CN_NHAN_NT");
            entity.Property(e => e.Ngay5CnTcOk)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_5_CN_TC_OK");
            entity.Property(e => e.Ngay6CnGiaoNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_6_CN_GIAO_NT");
            entity.Property(e => e.Ngay7NtNhanCn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_7_NT_NHAN_CN");
            entity.Property(e => e.Ngay8NtGiaoXn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_8_NT_GIAO_XN");
            entity.Property(e => e.Ngay9XnNhanNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_9_XN_NHAN_NT");
            entity.Property(e => e.NgayBillingGiao)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_BILLING_GIAO");
            entity.Property(e => e.NgayNiem)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_NIEM");
            entity.Property(e => e.NguoiDaiDienTc)
                .HasMaxLength(50)
                .HasColumnName("NGUOI_DAI_DIEN_TC");
            entity.Property(e => e.SoKimNiem)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SO_KIM_NIEM");
            entity.Property(e => e.SttHs)
                .HasColumnType("NUMBER")
                .HasColumnName("STT_HS");
            entity.Property(e => e.TenFileAnh01)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH_01");
            entity.Property(e => e.TenFileAnh02)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH_02");
            entity.Property(e => e.ViTriXacNhan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN");
            entity.Property(e => e.ViTriXacNhanCu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN_CU");
        });

        modelBuilder.Entity<AppTc06OngBe>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("APP_TC_06_ONG_BE");

            entity.Property(e => e.ChiSoCatMo)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_CAT_MO");
            entity.Property(e => e.ChiSoDh)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_DH");
            entity.Property(e => e.ChiSoNiem)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_NIEM");
            entity.Property(e => e.GhiChuBilling)
                .HasMaxLength(200)
                .HasColumnName("GHI_CHU_BILLING");
            entity.Property(e => e.GhiChuThem)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("GHI_CHU_THEM");
            entity.Property(e => e.IdBilling)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_BILLING");
            entity.Property(e => e.IdXacNhan)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_XAC_NHAN");
            entity.Property(e => e.KieuCatMo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("KIEU_CAT_MO");
            entity.Property(e => e.LogDateApp)
                .HasColumnType("DATE")
                .HasColumnName("LOG_DATE_APP");
            entity.Property(e => e.LogUser)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("LOG_USER");
            entity.Property(e => e.MaBienDoc)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_BIEN_DOC");
            entity.Property(e => e.MaGhiChu)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_GHI_CHU");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaKimNiem)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_KIM_NIEM");
            entity.Property(e => e.MaNhanVienCn)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN_CN");
            entity.Property(e => e.MaNhanVienNt)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN_NT");
            entity.Property(e => e.MaSoDoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SO_DOC");
            entity.Property(e => e.MaTttc)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_TTTC");
            entity.Property(e => e.MaXiNghiep)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_XI_NGHIEP");
            entity.Property(e => e.Ngay10XnBilling)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_10_XN_BILLING");
            entity.Property(e => e.Ngay1XnGiaoNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_1_XN_GIAO_NT");
            entity.Property(e => e.Ngay2NtNhanXn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_2_NT_NHAN_XN");
            entity.Property(e => e.Ngay3NtGiaoCn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_3_NT_GIAO_CN");
            entity.Property(e => e.Ngay4CnNhanNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_4_CN_NHAN_NT");
            entity.Property(e => e.Ngay5CnTcOk)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_5_CN_TC_OK");
            entity.Property(e => e.Ngay6CnGiaoNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_6_CN_GIAO_NT");
            entity.Property(e => e.Ngay7NtNhanCn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_7_NT_NHAN_CN");
            entity.Property(e => e.Ngay8NtGiaoXn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_8_NT_GIAO_XN");
            entity.Property(e => e.Ngay9XnNhanNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_9_XN_NHAN_NT");
            entity.Property(e => e.NgayBillingGiao)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_BILLING_GIAO");
            entity.Property(e => e.NgayNiem)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_NIEM");
            entity.Property(e => e.NguoiDaiDienTc)
                .HasMaxLength(50)
                .HasColumnName("NGUOI_DAI_DIEN_TC");
            entity.Property(e => e.SoKimNiem)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SO_KIM_NIEM");
            entity.Property(e => e.SttHs)
                .HasColumnType("NUMBER")
                .HasColumnName("STT_HS");
            entity.Property(e => e.TenFileAnh01)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH_01");
            entity.Property(e => e.TenFileAnh02)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH_02");
            entity.Property(e => e.ViTriXacNhan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN");
            entity.Property(e => e.ViTriXacNhanCu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN_CU");
        });

        modelBuilder.Entity<AppTc07CatNuoc>(entity =>
        {
            entity.HasKey(e => e.IdXacNhan).HasName("APP_TC_CAT_NUOC_PK");

            entity.ToTable("APP_TC_07_CAT_NUOC");

            entity.HasIndex(e => e.IdBilling, "APP_TC_CAT_NUOC_INDEX1");

            entity.Property(e => e.IdXacNhan)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_XAC_NHAN");
            entity.Property(e => e.ChiSoCatMo)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_CAT_MO");
            entity.Property(e => e.ChiSoDh)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_DH");
            entity.Property(e => e.ChiSoNiem)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_NIEM");
            entity.Property(e => e.GhiChuBilling)
                .HasMaxLength(200)
                .HasColumnName("GHI_CHU_BILLING");
            entity.Property(e => e.GhiChuThem)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("GHI_CHU_THEM");
            entity.Property(e => e.IdBilling)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_BILLING");
            entity.Property(e => e.KieuCatMo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("KIEU_CAT_MO");
            entity.Property(e => e.LogDateApp)
                .HasColumnType("DATE")
                .HasColumnName("LOG_DATE_APP");
            entity.Property(e => e.LogUser)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("LOG_USER");
            entity.Property(e => e.MaBienDoc)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_BIEN_DOC");
            entity.Property(e => e.MaGhiChu)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_GHI_CHU");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaKimNiem)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_KIM_NIEM");
            entity.Property(e => e.MaNhanVienCn)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN_CN");
            entity.Property(e => e.MaNhanVienNt)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN_NT");
            entity.Property(e => e.MaSoDoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SO_DOC");
            entity.Property(e => e.MaTttc)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_TTTC");
            entity.Property(e => e.MaXiNghiep)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_XI_NGHIEP");
            entity.Property(e => e.Ngay10XnBilling)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_10_XN_BILLING");
            entity.Property(e => e.Ngay1XnGiaoNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_1_XN_GIAO_NT");
            entity.Property(e => e.Ngay2NtNhanXn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_2_NT_NHAN_XN");
            entity.Property(e => e.Ngay3NtGiaoCn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_3_NT_GIAO_CN");
            entity.Property(e => e.Ngay4CnNhanNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_4_CN_NHAN_NT");
            entity.Property(e => e.Ngay5CnTcOk)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_5_CN_TC_OK");
            entity.Property(e => e.Ngay6CnGiaoNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_6_CN_GIAO_NT");
            entity.Property(e => e.Ngay7NtNhanCn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_7_NT_NHAN_CN");
            entity.Property(e => e.Ngay8NtGiaoXn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_8_NT_GIAO_XN");
            entity.Property(e => e.Ngay9XnNhanNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_9_XN_NHAN_NT");
            entity.Property(e => e.NgayBillingGiao)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_BILLING_GIAO");
            entity.Property(e => e.NgayNiem)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_NIEM");
            entity.Property(e => e.NguoiDaiDienTc)
                .HasMaxLength(50)
                .HasColumnName("NGUOI_DAI_DIEN_TC");
            entity.Property(e => e.SoKimNiem)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SO_KIM_NIEM");
            entity.Property(e => e.SttHs)
                .HasColumnType("NUMBER")
                .HasColumnName("STT_HS");
            entity.Property(e => e.TenFileAnh01)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH_01");
            entity.Property(e => e.TenFileAnh02)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH_02");
            entity.Property(e => e.ViTriXacNhan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN");
            entity.Property(e => e.ViTriXacNhanCu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN_CU");
        });

        modelBuilder.Entity<AppTc08MoNuoc>(entity =>
        {
            entity.HasKey(e => e.IdXacNhan).HasName("APP_TC_08_MO_NUOC_PK");

            entity.ToTable("APP_TC_08_MO_NUOC");

            entity.Property(e => e.IdXacNhan)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_XAC_NHAN");
            entity.Property(e => e.ChiSoCatMo)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_CAT_MO");
            entity.Property(e => e.ChiSoDh)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_DH");
            entity.Property(e => e.ChiSoNiem)
                .HasColumnType("NUMBER")
                .HasColumnName("CHI_SO_NIEM");
            entity.Property(e => e.GhiChuBilling)
                .HasMaxLength(200)
                .HasColumnName("GHI_CHU_BILLING");
            entity.Property(e => e.GhiChuThem)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("GHI_CHU_THEM");
            entity.Property(e => e.IdBilling)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_BILLING");
            entity.Property(e => e.KieuCatMo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("KIEU_CAT_MO");
            entity.Property(e => e.LogDateApp)
                .HasColumnType("DATE")
                .HasColumnName("LOG_DATE_APP");
            entity.Property(e => e.LogUser)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("LOG_USER");
            entity.Property(e => e.MaBienDoc)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_BIEN_DOC");
            entity.Property(e => e.MaGhiChu)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_GHI_CHU");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaKimNiem)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_KIM_NIEM");
            entity.Property(e => e.MaNhanVienCn)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN_CN");
            entity.Property(e => e.MaNhanVienNt)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN_NT");
            entity.Property(e => e.MaSoDoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SO_DOC");
            entity.Property(e => e.MaTttc)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_TTTC");
            entity.Property(e => e.MaXiNghiep)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_XI_NGHIEP");
            entity.Property(e => e.Ngay10XnBilling)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_10_XN_BILLING");
            entity.Property(e => e.Ngay1XnGiaoNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_1_XN_GIAO_NT");
            entity.Property(e => e.Ngay2NtNhanXn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_2_NT_NHAN_XN");
            entity.Property(e => e.Ngay3NtGiaoCn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_3_NT_GIAO_CN");
            entity.Property(e => e.Ngay4CnNhanNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_4_CN_NHAN_NT");
            entity.Property(e => e.Ngay5CnTcOk)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_5_CN_TC_OK");
            entity.Property(e => e.Ngay6CnGiaoNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_6_CN_GIAO_NT");
            entity.Property(e => e.Ngay7NtNhanCn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_7_NT_NHAN_CN");
            entity.Property(e => e.Ngay8NtGiaoXn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_8_NT_GIAO_XN");
            entity.Property(e => e.Ngay9XnNhanNt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_9_XN_NHAN_NT");
            entity.Property(e => e.NgayBillingGiao)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_BILLING_GIAO");
            entity.Property(e => e.NgayNiem)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_NIEM");
            entity.Property(e => e.NguoiDaiDienTc)
                .HasMaxLength(50)
                .HasColumnName("NGUOI_DAI_DIEN_TC");
            entity.Property(e => e.SoKimNiem)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SO_KIM_NIEM");
            entity.Property(e => e.SttHs)
                .HasColumnType("NUMBER")
                .HasColumnName("STT_HS");
            entity.Property(e => e.TenFileAnh01)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH_01");
            entity.Property(e => e.TenFileAnh02)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH_02");
            entity.Property(e => e.ViTriXacNhan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN");
            entity.Property(e => e.ViTriXacNhanCu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_XAC_NHAN_CU");
        });

        modelBuilder.Entity<AppTc99NiemChiNhua>(entity =>
        {
            entity.HasKey(e => e.IdNcn).HasName("APP_TC_99_NIEM_CHI_NHUA_PK");

            entity.ToTable("APP_TC_99_NIEM_CHI_NHUA");

            entity.Property(e => e.IdNcn)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_NCN");
            entity.Property(e => e.LogDate)
                .HasColumnType("DATE")
                .HasColumnName("LOG_DATE");
            entity.Property(e => e.LogUser)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("LOG_USER");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaQrChiNiem)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MA_QR_CHI_NIEM");
        });

        modelBuilder.Entity<Cc01TtkhYeuCau>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CC_01_TTKH_YEU_CAU");

            entity.HasIndex(e => e.IdYeuCau, "CC_TTKH_YEU_CAU_INDEX1");

            entity.HasIndex(e => e.CcSoDienThoai, "CC_TTKH_YEU_CAU_INDEX2");

            entity.HasIndex(e => e.MaKhachHang, "CC_TTKH_YEU_CAU_INDEX3");

            entity.HasIndex(e => e.NgayCapNhat, "CC_TTKH_YEU_CAU_INDEX4");

            entity.HasIndex(e => e.XuLyMaTrangThai, "CC_TTKH_YEU_CAU_INDEX5");

            entity.Property(e => e.CcCallIdIn)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("CC_CALL_ID_IN");
            entity.Property(e => e.CcCallIdOut)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("CC_CALL_ID_OUT");
            entity.Property(e => e.CcCallInTgbd)
                .HasColumnType("DATE")
                .HasColumnName("CC_CALL_IN_TGBD");
            entity.Property(e => e.CcCallInTgkt)
                .HasColumnType("DATE")
                .HasColumnName("CC_CALL_IN_TGKT");
            entity.Property(e => e.CcCallOutTgbd)
                .HasColumnType("DATE")
                .HasColumnName("CC_CALL_OUT_TGBD");
            entity.Property(e => e.CcCallOutTgkt)
                .HasColumnType("DATE")
                .HasColumnName("CC_CALL_OUT_TGKT");
            entity.Property(e => e.CcEmail)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("CC_EMAIL");
            entity.Property(e => e.CcRingInTgbd)
                .HasColumnType("DATE")
                .HasColumnName("CC_RING_IN_TGBD");
            entity.Property(e => e.CcSoDienThoai)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CC_SO_DIEN_THOAI");
            entity.Property(e => e.CongViec24g)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("CONG_VIEC_24G");
            entity.Property(e => e.DgGhiChuKq)
                .HasMaxLength(200)
                .HasColumnName("DG_GHI_CHU_KQ");
            entity.Property(e => e.DgLinkGhiAm)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("DG_LINK_GHI_AM");
            entity.Property(e => e.DgMaDanhGia)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("DG_MA_DANH_GIA");
            entity.Property(e => e.DgNgayCapNhat)
                .HasColumnType("DATE")
                .HasColumnName("DG_NGAY_CAP_NHAT");
            entity.Property(e => e.DgNguoiCapNhat)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("DG_NGUOI_CAP_NHAT");
            entity.Property(e => e.DiaChiDongHo)
                .HasMaxLength(150)
                .HasColumnName("DIA_CHI_DONG_HO");
            entity.Property(e => e.GhiChu)
                .HasMaxLength(200)
                .HasColumnName("GHI_CHU");
            entity.Property(e => e.GhiChuCc)
                .HasMaxLength(200)
                .HasColumnName("GHI_CHU_CC");
            entity.Property(e => e.IdDangKy)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_DANG_KY");
            entity.Property(e => e.IdYeuCau)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_YEU_CAU");
            entity.Property(e => e.KhachHangYc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("KHACH_HANG_YC");
            entity.Property(e => e.KhachHangYcLan)
                .HasColumnType("NUMBER")
                .HasColumnName("KHACH_HANG_YC_LAN");
            entity.Property(e => e.LinkGhiAm)
                .HasMaxLength(220)
                .IsUnicode(false)
                .HasColumnName("LINK_GHI_AM");
            entity.Property(e => e.LoaiNguyenNhan)
                .HasMaxLength(20)
                .HasColumnName("LOAI_NGUYEN_NHAN");
            entity.Property(e => e.MaDc)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_DC");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaNguonTt)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_NGUON_TT");
            entity.Property(e => e.MaNhanVienYc)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN_YC");
            entity.Property(e => e.MaNoiDungYc)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_NOI_DUNG_YC");
            entity.Property(e => e.MaPhuong)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_PHUONG");
            entity.Property(e => e.MaQuan)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_QUAN");
            entity.Property(e => e.MaXiNghiep)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_XI_NGHIEP");
            entity.Property(e => e.NgayCapNhat)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.NgaySuaLai)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_SUA_LAI");
            entity.Property(e => e.NgheGhiAm)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("NGHE_GHI_AM");
            entity.Property(e => e.NguoiCapNhat)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("NGUOI_CAP_NHAT");
            entity.Property(e => e.NhanVienYc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("NHAN_VIEN_YC");
            entity.Property(e => e.NhanVienYcLan)
                .HasColumnType("NUMBER")
                .HasColumnName("NHAN_VIEN_YC_LAN");
            entity.Property(e => e.TcGhiChu)
                .HasMaxLength(200)
                .HasColumnName("TC_GHI_CHU");
            entity.Property(e => e.TcMaCongNhan01)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("TC_MA_CONG_NHAN_01");
            entity.Property(e => e.TcMaCongNhan02)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("TC_MA_CONG_NHAN_02");
            entity.Property(e => e.TcMaNhomTruong)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("TC_MA_NHOM_TRUONG");
            entity.Property(e => e.TcNgayGiaoCn)
                .HasColumnType("DATE")
                .HasColumnName("TC_NGAY_GIAO_CN");
            entity.Property(e => e.TcNgayGiaoNt)
                .HasColumnType("DATE")
                .HasColumnName("TC_NGAY_GIAO_NT");
            entity.Property(e => e.TcNgayHoanThanh)
                .HasColumnType("DATE")
                .HasColumnName("TC_NGAY_HOAN_THANH");
            entity.Property(e => e.TcSoCongTrinh)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("TC_SO_CONG_TRINH");
            entity.Property(e => e.TenFileAnh)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_FILE_ANH");
            entity.Property(e => e.TenKhachHang)
                .HasMaxLength(180)
                .HasColumnName("TEN_KHACH_HANG");
            entity.Property(e => e.TenKhachHangSub)
                .HasMaxLength(200)
                .HasColumnName("TEN_KHACH_HANG_SUB");
            entity.Property(e => e.TtUuTienCv)
                .HasColumnType("NUMBER")
                .HasColumnName("TT_UU_TIEN_CV");
            entity.Property(e => e.ViTri)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI");
            entity.Property(e => e.XuLyMaBoPhan)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("XU_LY_MA_BO_PHAN");
            entity.Property(e => e.XuLyMaTrangThai)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("XU_LY_MA_TRANG_THAI");
        });

        modelBuilder.Entity<CcDmNoiDungYc>(entity =>
        {
            entity.HasKey(e => e.IdNoiDung).HasName("CC_DM_NOI_DUNG_YC_PK");

            entity.ToTable("CC_DM_NOI_DUNG_YC");

            entity.Property(e => e.IdNoiDung)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("ID_NOI_DUNG");
            entity.Property(e => e.Act)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("1 ")
                .HasColumnName("ACT");
            entity.Property(e => e.AppDh)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("APP_DH");
            entity.Property(e => e.DanhGiaKpi)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("0 ")
                .HasColumnName("DANH_GIA_KPI");
            entity.Property(e => e.GhiChu)
                .HasMaxLength(50)
                .HasColumnName("GHI_CHU");
            entity.Property(e => e.MaBp)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_BP");
            entity.Property(e => e.MaNhomYc)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_NHOM_YC");
            entity.Property(e => e.NgaySua)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_SUA");
            entity.Property(e => e.NgayTao)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_TAO");
            entity.Property(e => e.NguoiSua)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NGUOI_SUA");
            entity.Property(e => e.NguoiTao)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NGUOI_TAO");
            entity.Property(e => e.NhomYc)
                .HasMaxLength(20)
                .HasColumnName("NHOM_YC");
            entity.Property(e => e.NoiDungYc)
                .HasMaxLength(120)
                .HasColumnName("NOI_DUNG_YC");
            entity.Property(e => e.NoiDungYcCu)
                .HasMaxLength(120)
                .HasColumnName("NOI_DUNG_YC_CU");
            entity.Property(e => e.NoiDungYcSub)
                .HasMaxLength(120)
                .HasColumnName("NOI_DUNG_YC_SUB");
            entity.Property(e => e.Stt)
                .HasColumnType("NUMBER")
                .HasColumnName("STT");
        });

        modelBuilder.Entity<CcMangLuoi>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CC_MANG_LUOI");

            entity.HasIndex(e => e.IdMl, "CC_MANG_LUOI_INDEX1");

            entity.Property(e => e.IdMl)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_ML");
            entity.Property(e => e.KqSuCo)
                .HasMaxLength(50)
                .HasColumnName("KQ_SU_CO");
            entity.Property(e => e.KqSuCoMa)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("KQ_SU_CO_MA");
            entity.Property(e => e.MaXiNghiep)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_XI_NGHIEP");
            entity.Property(e => e.NgayCapNhat)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.NgayCapNhatLai)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_CAP_NHAT_LAI");
            entity.Property(e => e.NguoiCapNhat)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("NGUOI_CAP_NHAT");
            entity.Property(e => e.NoiDungTb)
                .HasMaxLength(200)
                .HasColumnName("NOI_DUNG_TB");
            entity.Property(e => e.PhamVi)
                .HasMaxLength(120)
                .HasColumnName("PHAM_VI");
            entity.Property(e => e.Stt)
                .HasColumnType("NUMBER")
                .HasColumnName("STT");
            entity.Property(e => e.TenXiNghiep)
                .HasMaxLength(50)
                .HasColumnName("TEN_XI_NGHIEP");
            entity.Property(e => e.TgSuCoDen)
                .HasColumnType("DATE")
                .HasColumnName("TG_SU_CO_DEN");
            entity.Property(e => e.TgSuCoTu)
                .HasColumnType("DATE")
                .HasColumnName("TG_SU_CO_TU");
            entity.Property(e => e.ThoiGianSuCo)
                .HasMaxLength(80)
                .HasColumnName("THOI_GIAN_SU_CO");
            entity.Property(e => e.TieuDeTb)
                .HasMaxLength(50)
                .HasColumnName("TIEU_DE_TB");
            entity.Property(e => e.TieuDeTbMa)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("TIEU_DE_TB_MA");
        });

        modelBuilder.Entity<ChiSoDh>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CHI_SO_DH");

            entity.HasIndex(e => e.Thang, "CHI_SO_DH_INDEX1");

            entity.HasIndex(e => e.NgayDinhKy, "CHI_SO_DH_NGAYDK");

            entity.HasIndex(e => e.KhId, "TT_INDEX");

            entity.HasIndex(e => e.IdCsCuoi, "TT_INDEX_ID_CS_CUOI");

            entity.HasIndex(e => e.IdCsDau, "TT_INDEX_ID_CS_DAU");

            entity.HasIndex(e => e.MaKhachHang, "TT_INDEX_SUB");

            entity.HasIndex(e => e.SoHoaDon, "TT_INDEX_SUB3");

            entity.Property(e => e.CsCuoi)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CS_CUOI");
            entity.Property(e => e.CsDau)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CS_DAU");
            entity.Property(e => e.IdCsCuoi)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_CS_CUOI");
            entity.Property(e => e.IdCsDau)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_CS_DAU");
            entity.Property(e => e.KhId)
                .HasMaxLength(10)
                .HasColumnName("KH_ID");
            entity.Property(e => e.LoaiChiSo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("LOAI_CHI_SO");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(10)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.NgayDinhKy)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_DINH_KY");
            entity.Property(e => e.NgayDoc)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_DOC");
            entity.Property(e => e.SanLuong)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SAN_LUONG");
            entity.Property(e => e.SoHoaDon)
                .HasMaxLength(15)
                .HasDefaultValueSql("0\n   ")
                .HasColumnName("SO_HOA_DON");
            entity.Property(e => e.Thang)
                .HasMaxLength(20)
                .HasColumnName("THANG");
        });

        modelBuilder.Entity<ChiSoDhSub>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CHI_SO_DH_SUB");

            entity.HasIndex(e => e.IdCs, "TTS_INDEX_ID_CS");

            entity.HasIndex(e => e.MaKhachHang, "TTS_INDEX_MAKH");

            entity.Property(e => e.IdCs)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_CS");
            entity.Property(e => e.LoaiChiSo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("LOAI_CHI_SO");
            entity.Property(e => e.MaDongHo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_DONG_HO");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaTinhTrangDh)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_TINH_TRANG_DH");
            entity.Property(e => e.NgayDinhKy)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_DINH_KY");
            entity.Property(e => e.Sltb3t)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SLTB3T");
            entity.Property(e => e.TinhTrangCs)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("TINH_TRANG_CS");
        });

        modelBuilder.Entity<CongNo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CONG_NO");

            entity.HasIndex(e => e.NgayCapSerialDt, "CN_CAP_SERIAL");

            entity.HasIndex(e => e.EmailPhatHanh, "CN_EMAIL_PHAT_HANH");

            entity.HasIndex(e => e.EmailThanhToan, "CN_EMAIL_THANH_TOAN");

            entity.HasIndex(e => e.HinhThucTt, "CN_INDEX_HTTT");

            entity.HasIndex(e => e.KieuDieuChinh, "CN_KIEU_DIEU_CHINH");

            entity.HasIndex(e => e.LoaiKhachHang, "CN_LOAI_KHACH_HANG");

            entity.HasIndex(e => e.MaBienDoc, "CN_MA_BIEN_DOC");

            entity.HasIndex(e => e.MaChiNhanh, "CN_MA_CHI_NHANH");

            entity.HasIndex(e => e.MaDonVi, "CN_MA_DON_VI");

            entity.HasIndex(e => e.MaGia, "CN_MA_GIA");

            entity.HasIndex(e => e.MaKhachHang, "CN_MA_KHACH_HANG");

            entity.HasIndex(e => e.MaSoDoc, "CN_MA_SO_DOC");

            entity.HasIndex(e => e.MaThuNgan, "CN_MA_THU_NGAN");

            entity.HasIndex(e => e.NgayBanGiaoQt, "CN_NGAY_BAN_GIAO_QT");

            entity.HasIndex(e => e.NgayBanGiaoTn, "CN_NGAY_BAN_GIAO_TN");

            entity.HasIndex(e => e.NgayDkDen, "CN_NGAY_DK_DEN");

            entity.HasIndex(e => e.NgayDoc, "CN_NGAY_DOC");

            entity.HasIndex(e => e.NgayHdHuy, "CN_NGAY_HD_HUY");

            entity.HasIndex(e => e.NgayHdPhatHanh, "CN_NGAY_HD_PHAT_HANH");

            entity.HasIndex(e => e.NgayHuyThanhToanDt, "CN_NGAY_HUY_HD");

            entity.HasIndex(e => e.NgayHuyPhatHanhDt, "CN_NGAY_HUY_TB");

            entity.HasIndex(e => e.NgayPhatHanhDt, "CN_NGAY_PH_TB");

            entity.HasIndex(e => e.NgayThanhToan, "CN_NGAY_THANH_TOAN");

            entity.HasIndex(e => e.NgayThanhToanBill, "CN_NGAY_THANH_TOAN_BILL");

            entity.HasIndex(e => e.NgayThanhToanDt, "CN_NGAY_THANH_TOAN_DT");

            entity.HasIndex(e => e.NgayThanhToanDtTam, "CN_NGAY_THANH_TOAN_DT_TAM");

            entity.HasIndex(e => e.SeriHoaDon, "CN_SERI");

            entity.HasIndex(e => e.SoHoaDon, "CN_SO_HOA_DON");

            entity.HasIndex(e => e.SoHoaDonThayThe, "CN_SO_HOA_DON_THAY_THE");

            entity.HasIndex(e => e.Thang, "CN_THANG");

            entity.HasIndex(e => e.ThMaTtBillXuong, "CN_TH_MA_TT_BILL_XUONG");

            entity.HasIndex(e => e.ThNgayTtLenBill, "CN_TH_NGAY_TT_LEN_BILL");

            entity.HasIndex(e => e.ThThuTam, "CN_TH_THU_TAM");

            entity.HasIndex(e => e.TongThanhToan, "CN_TONG_THANH_TOAN");

            entity.HasIndex(e => e.TongThanhToanBill, "CN_TONG_THANH_TOAN_BILL");

            entity.HasIndex(e => e.TrangThai, "CN_TRANG_THAI");

            entity.HasIndex(e => e.ThTransIdBank, "CN_TRANS_ID_BANK");

            entity.Property(e => e.BanGiao)
                .HasMaxLength(2)
                .HasDefaultValueSql("0 ")
                .HasColumnName("BAN_GIAO");
            entity.Property(e => e.DaLuuEbilsub1)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("DA_LUU_EBILSUB1");
            entity.Property(e => e.DaLuuEbilsub2)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("DA_LUU_EBILSUB2");
            entity.Property(e => e.DaLuuEbilsub3)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("DA_LUU_EBILSUB3");
            entity.Property(e => e.DaLuuEbilsub4)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("DA_LUU_EBILSUB4");
            entity.Property(e => e.DienGiai)
                .HasMaxLength(110)
                .HasColumnName("DIEN_GIAI");
            entity.Property(e => e.EmailPhatHanh)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("EMAIL_PHAT_HANH");
            entity.Property(e => e.EmailPhatHanhHuy)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("EMAIL_PHAT_HANH_HUY");
            entity.Property(e => e.EmailThanhToan)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("EMAIL_THANH_TOAN");
            entity.Property(e => e.HinhThucTt)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("HINH_THUC_TT");
            entity.Property(e => e.HinhThucTtBill)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("HINH_THUC_TT_BILL");
            entity.Property(e => e.Khoa)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER")
                .HasColumnName("KHOA");
            entity.Property(e => e.KieuDieuChinh)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("NULL ")
                .HasColumnName("KIEU_DIEU_CHINH");
            entity.Property(e => e.LoaiHoaDon)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER")
                .HasColumnName("LOAI_HOA_DON");
            entity.Property(e => e.LoaiKhachHang)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("LOAI_KHACH_HANG");
            entity.Property(e => e.MaBienDoc)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_BIEN_DOC");
            entity.Property(e => e.MaChiNhanh)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_CHI_NHANH");
            entity.Property(e => e.MaDonVi)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("MA_DON_VI");
            entity.Property(e => e.MaGia)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("MA_GIA");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(10)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaNganHang)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NGAN_HANG");
            entity.Property(e => e.MaNhoThu)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("MA_NHO_THU");
            entity.Property(e => e.MaSoDoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SO_DOC");
            entity.Property(e => e.MaThuNgan)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_THU_NGAN");
            entity.Property(e => e.MaThuNganSo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_THU_NGAN_SO");
            entity.Property(e => e.MaTinhTrangDh)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_TINH_TRANG_DH");
            entity.Property(e => e.MaTraCuu)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("MA_TRA_CUU");
            entity.Property(e => e.NgayBanGiaoQt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_BAN_GIAO_QT");
            entity.Property(e => e.NgayBanGiaoTn)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_BAN_GIAO_TN");
            entity.Property(e => e.NgayCapSerialDt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_CAP_SERIAL_DT");
            entity.Property(e => e.NgayDkDen)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_DK_DEN");
            entity.Property(e => e.NgayDkTu)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_DK_TU");
            entity.Property(e => e.NgayDoc)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_DOC");
            entity.Property(e => e.NgayHdHuy)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_HD_HUY");
            entity.Property(e => e.NgayHdPhatHanh)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_HD_PHAT_HANH");
            entity.Property(e => e.NgayHuyPhatHanhDt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_HUY_PHAT_HANH_DT");
            entity.Property(e => e.NgayHuyThanhToanDt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_HUY_THANH_TOAN_DT");
            entity.Property(e => e.NgayNo)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_NO");
            entity.Property(e => e.NgayPhatHanhDt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_PHAT_HANH_DT");
            entity.Property(e => e.NgayThanhToan)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_THANH_TOAN");
            entity.Property(e => e.NgayThanhToanBill)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_THANH_TOAN_BILL");
            entity.Property(e => e.NgayThanhToanDt)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_THANH_TOAN_DT");
            entity.Property(e => e.NgayThanhToanDtTam)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_THANH_TOAN_DT_TAM");
            entity.Property(e => e.Phi)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("PHI");
            entity.Property(e => e.PhiBase)
                .HasColumnType("NUMBER")
                .HasColumnName("PHI_BASE");
            entity.Property(e => e.PhiNuocThai)
                .HasColumnType("NUMBER")
                .HasColumnName("PHI_NUOC_THAI");
            entity.Property(e => e.SeriHoaDon)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("SERI_HOA_DON");
            entity.Property(e => e.SeriMau)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("SERI_MAU");
            entity.Property(e => e.SmsPhatHanh)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("SMS_PHAT_HANH");
            entity.Property(e => e.SmsPhatHanhHuy)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("SMS_PHAT_HANH_HUY");
            entity.Property(e => e.SmsThanhToan)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("SMS_THANH_TOAN");
            entity.Property(e => e.SoHoaDon)
                .HasMaxLength(9)
                .HasColumnName("SO_HOA_DON");
            entity.Property(e => e.SoHoaDonThayThe)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("SO_HOA_DON_THAY_THE");
            entity.Property(e => e.SoTaiKhoan)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SO_TAI_KHOAN");
            entity.Property(e => e.SoTaiKhoanBill)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("SO_TAI_KHOAN_BILL");
            entity.Property(e => e.SttSoDoc)
                .HasColumnType("NUMBER")
                .HasColumnName("STT_SO_DOC");
            entity.Property(e => e.ThKetQuaTtBillXuong)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("TH_KET_QUA_TT_BILL_XUONG");
            entity.Property(e => e.ThMaTtBillXuong)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("TH_MA_TT_BILL_XUONG");
            entity.Property(e => e.ThNgayTtLenBill)
                .HasColumnType("DATE")
                .HasColumnName("TH_NGAY_TT_LEN_BILL");
            entity.Property(e => e.ThNgayTtLog)
                .HasColumnType("DATE")
                .HasColumnName("TH_NGAY_TT_LOG");
            entity.Property(e => e.ThThuTam)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("TH_THU_TAM");
            entity.Property(e => e.ThTienNoHienTaiBill)
                .HasColumnType("NUMBER")
                .HasColumnName("TH_TIEN_NO_HIEN_TAI_BILL");
            entity.Property(e => e.ThTienTtOkBillXuong)
                .HasColumnType("NUMBER")
                .HasColumnName("TH_TIEN_TT_OK_BILL_XUONG");
            entity.Property(e => e.ThTransIdBank)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("TH_TRANS_ID_BANK");
            entity.Property(e => e.Thang)
                .HasMaxLength(10)
                .HasColumnName("THANG");
            entity.Property(e => e.ThanhTien)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMBER")
                .HasColumnName("THANH_TIEN");
            entity.Property(e => e.ThanhTienBase)
                .HasColumnType("NUMBER")
                .HasColumnName("THANH_TIEN_BASE");
            entity.Property(e => e.Thue)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("THUE");
            entity.Property(e => e.ThueBase)
                .HasColumnType("NUMBER")
                .HasColumnName("THUE_BASE");
            entity.Property(e => e.TongPpSanLuong)
                .HasColumnType("NUMBER")
                .HasColumnName("TONG_PP_SAN_LUONG");
            entity.Property(e => e.TongPpThueVat)
                .HasColumnType("NUMBER")
                .HasColumnName("TONG_PP_THUE_VAT");
            entity.Property(e => e.TongPpTruocThue)
                .HasColumnType("NUMBER")
                .HasColumnName("TONG_PP_TRUOC_THUE");
            entity.Property(e => e.TongSl)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TONG_SL");
            entity.Property(e => e.TongSlBase)
                .HasColumnType("NUMBER")
                .HasColumnName("TONG_SL_BASE");
            entity.Property(e => e.TongSlGiamTru)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TONG_SL_GIAM_TRU");
            entity.Property(e => e.TongThanhToan)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TONG_THANH_TOAN");
            entity.Property(e => e.TongThanhToanBill)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER")
                .HasColumnName("TONG_THANH_TOAN_BILL");
            entity.Property(e => e.TongTien)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TONG_TIEN");
            entity.Property(e => e.TongTienBase)
                .HasColumnType("NUMBER")
                .HasColumnName("TONG_TIEN_BASE");
            entity.Property(e => e.TongTienGiamTru)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TONG_TIEN_GIAM_TRU");
            entity.Property(e => e.TrangThai)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TRANG_THAI");
            entity.Property(e => e.Usr)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("USR");
            entity.Property(e => e.UsrTt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("USR_TT");
            entity.Property(e => e.VdcDaTao)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("NULL ")
                .HasColumnName("VDC_DA_TAO");
            entity.Property(e => e.VdcDaTaoTt)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("VDC_DA_TAO_TT");
            entity.Property(e => e.ZaloPhatHanh)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ZALO_PHAT_HANH");
            entity.Property(e => e.ZaloThanhToan)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ZALO_THANH_TOAN");
        });

        modelBuilder.Entity<CongNoGium>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CONG_NO_GIA");

            entity.HasIndex(e => e.Id, "CNG_INDEX");

            entity.HasIndex(e => e.NgayDoc, "CNG_INDEX4");

            entity.HasIndex(e => e.MaKhachHang, "CNG_INDEX_SUB");

            entity.HasIndex(e => e.SoHoaDon, "CNG_INDEX_SUB3");

            entity.HasIndex(e => e.MaGia, "CNG_MA_GIA");

            entity.HasIndex(e => e.NgayNo, "CNG_NGAY_NO");

            entity.Property(e => e.GhiChuDc)
                .HasMaxLength(110)
                .HasColumnName("GHI_CHU_DC");
            entity.Property(e => e.HtKd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("HT_KD");
            entity.Property(e => e.Id)
                .HasMaxLength(15)
                .HasColumnName("ID");
            entity.Property(e => e.MaGia)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_GIA");
            entity.Property(e => e.MaGiaCb)
                .HasMaxLength(10)
                .HasColumnName("MA_GIA_CB");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(10)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaLoaiPhi)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("MA_LOAI_PHI");
            entity.Property(e => e.NgayDoc)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_DOC");
            entity.Property(e => e.NgayNo)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_NO");
            entity.Property(e => e.PbvSanLuong)
                .HasColumnType("NUMBER(9,2)")
                .HasColumnName("PBV_SAN_LUONG");
            entity.Property(e => e.PhuPhiThueVat)
                .HasColumnType("NUMBER")
                .HasColumnName("PHU_PHI_THUE_VAT");
            entity.Property(e => e.PhuPhiTruocThue)
                .HasColumnType("NUMBER")
                .HasColumnName("PHU_PHI_TRUOC_THUE");
            entity.Property(e => e.PhuPhiTyLe)
                .HasColumnType("NUMBER")
                .HasColumnName("PHU_PHI_TY_LE");
            entity.Property(e => e.PntSanLuong)
                .HasColumnType("NUMBER(9,2)")
                .HasColumnName("PNT_SAN_LUONG");
            entity.Property(e => e.PtnDonGia)
                .HasColumnType("NUMBER")
                .HasColumnName("PTN_DON_GIA");
            entity.Property(e => e.SoHoaDon)
                .HasMaxLength(15)
                .HasColumnName("SO_HOA_DON");
            entity.Property(e => e.TenGia)
                .HasMaxLength(255)
                .HasColumnName("TEN_GIA");
            entity.Property(e => e.Thang)
                .HasMaxLength(20)
                .HasColumnName("THANG");
            entity.Property(e => e.ThanhTien)
                .HasDefaultValueSql("0")
                .HasColumnType("FLOAT")
                .HasColumnName("THANH_TIEN");
            entity.Property(e => e.ThueBvmt)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMBER")
                .HasColumnName("THUE_BVMT");
            entity.Property(e => e.TienGiaCb)
                .HasDefaultValueSql("0")
                .HasColumnType("FLOAT")
                .HasColumnName("TIEN_GIA_CB");
            entity.Property(e => e.TongSl)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TONG_SL");
            entity.Property(e => e.TongSlEx)
                .HasColumnType("NUMBER")
                .HasColumnName("TONG_SL_EX");
            entity.Property(e => e.TongTien)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMBER")
                .HasColumnName("TONG_TIEN");
            entity.Property(e => e.Vat)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMBER")
                .HasColumnName("VAT");
        });

        modelBuilder.Entity<Dab02DocChiSoDh>(entity =>
        {
            entity.HasKey(e => e.IdDab).HasName("DAB_02_DOC_CHI_SO_DH_PK");

            entity.ToTable("DAB_02_DOC_CHI_SO_DH");

            entity.HasIndex(e => e.NgayDocDk, "DAB_022_GHI_DH_NGAY_DOC_INDEX2");

            entity.HasIndex(e => e.MaBienDoc, "DAB_022_GHI_DH_NGAY_DOC_INDEX3");

            entity.Property(e => e.IdDab)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("ID_DAB");
            entity.Property(e => e.ChuaDoc)
                .HasColumnType("NUMBER")
                .HasColumnName("CHUA_DOC");
            entity.Property(e => e.DaDoc)
                .HasColumnType("NUMBER")
                .HasColumnName("DA_DOC");
            entity.Property(e => e.MaBienDoc)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_BIEN_DOC");
            entity.Property(e => e.MaXiNghiep)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_XI_NGHIEP");
            entity.Property(e => e.MaXiNghiepCu)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_XI_NGHIEP_CU");
            entity.Property(e => e.NgayDocDk)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_DOC_DK");
            entity.Property(e => e.NgayThucHien)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_THUC_HIEN");
            entity.Property(e => e.TongHs)
                .HasColumnType("NUMBER")
                .HasColumnName("TONG_HS");
        });

        modelBuilder.Entity<Dm01DonVi>(entity =>
        {
            entity.HasKey(e => e.MaDonVi).HasName("DM_01_DON_VI_PK");

            entity.ToTable("DM_01_DON_VI");

            entity.Property(e => e.MaDonVi)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_DON_VI");
            entity.Property(e => e.ChuTaiKhoan)
                .HasMaxLength(50)
                .HasColumnName("CHU_TAI_KHOAN");
            entity.Property(e => e.DaiDienChucVu)
                .HasMaxLength(70)
                .HasColumnName("DAI_DIEN_CHUC_VU");
            entity.Property(e => e.DaiDienKyHd)
                .HasMaxLength(50)
                .HasColumnName("DAI_DIEN_KY_HD");
            entity.Property(e => e.DiaChi)
                .HasMaxLength(100)
                .HasColumnName("DIA_CHI");
            entity.Property(e => e.DienThoai)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("DIEN_THOAI");
            entity.Property(e => e.Fax)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("FAX");
            entity.Property(e => e.IpNas)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("IP_NAS");
            entity.Property(e => e.KyHieu)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("KY_HIEU");
            entity.Property(e => e.KyHieuHd)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("KY_HIEU_HD");
            entity.Property(e => e.NgayCapNhat)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.NgayKhoaDlTinhLuong)
                .HasColumnType("NUMBER")
                .HasColumnName("NGAY_KHOA_DL_TINH_LUONG");
            entity.Property(e => e.NgaySua)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_SUA");
            entity.Property(e => e.NgayUyQuyen)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_UY_QUYEN");
            entity.Property(e => e.NguoiCapNhat)
                .HasMaxLength(10)
                .HasColumnName("NGUOI_CAP_NHAT");
            entity.Property(e => e.NguoiSua)
                .HasMaxLength(10)
                .HasColumnName("NGUOI_SUA");
            entity.Property(e => e.ParentMa)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("PARENT_MA");
            entity.Property(e => e.SoTaiKhoan)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("SO_TAI_KHOAN");
            entity.Property(e => e.SoUyQuyen)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("SO_UY_QUYEN");
            entity.Property(e => e.TenDonVi)
                .HasMaxLength(100)
                .HasColumnName("TEN_DON_VI");
            entity.Property(e => e.TenDonViMoi)
                .HasMaxLength(100)
                .HasColumnName("TEN_DON_VI_MOI");
            entity.Property(e => e.TenVietTat)
                .HasMaxLength(30)
                .HasColumnName("TEN_VIET_TAT");
            entity.Property(e => e.TruongChucVu)
                .HasMaxLength(70)
                .HasColumnName("TRUONG_CHUC_VU");
            entity.Property(e => e.TruongDonVi)
                .HasMaxLength(50)
                .HasColumnName("TRUONG_DON_VI");
        });

        modelBuilder.Entity<DmDiemThuTien>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DM_DIEM_THU_TIEN");

            entity.Property(e => e.BiSua)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("BI_SUA");
            entity.Property(e => e.ChuoiSms)
                .HasMaxLength(200)
                .HasColumnName("CHUOI_SMS");
            entity.Property(e => e.ChuoiSmsTemp)
                .HasMaxLength(200)
                .HasColumnName("CHUOI_SMS_TEMP");
            entity.Property(e => e.DiaChiGd)
                .HasMaxLength(200)
                .HasColumnName("DIA_CHI_GD");
            entity.Property(e => e.DiaChiGoogle)
                .HasMaxLength(150)
                .HasColumnName("DIA_CHI_GOOGLE");
            entity.Property(e => e.DienThoai)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("DIEN_THOAI");
            entity.Property(e => e.Gio)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("GIO");
            entity.Property(e => e.HieuLuc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("HIEU_LUC");
            entity.Property(e => e.LogDate)
                .HasColumnType("DATE")
                .HasColumnName("LOG_DATE");
            entity.Property(e => e.MaDiem)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_DIEM");
            entity.Property(e => e.MaDonViThu)
                .HasColumnType("NUMBER")
                .HasColumnName("MA_DON_VI_THU");
            entity.Property(e => e.N01)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("0 ");
            entity.Property(e => e.N02)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N03)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N04)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N05)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N06)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N07)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N08)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N09)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N10)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N11)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N12)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N13)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N14)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N15)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N16)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N17)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N18)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N19)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N20)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N21)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N22)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N23)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N24)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N25)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N26)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N27)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N28)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N29)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N30)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.N31)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.NhomDonViThu)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("NHOM_DON_VI_THU");
            entity.Property(e => e.Phuong)
                .HasMaxLength(80)
                .HasColumnName("PHUONG");
            entity.Property(e => e.Quan)
                .HasMaxLength(80)
                .HasColumnName("QUAN");
            entity.Property(e => e.TenDiemGd)
                .HasMaxLength(200)
                .HasColumnName("TEN_DIEM_GD");
            entity.Property(e => e.ThoiGian)
                .HasMaxLength(50)
                .HasColumnName("THOI_GIAN");
            entity.Property(e => e.ToaDo)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("TOA_DO");
            entity.Property(e => e.ViTriDiemThu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VI_TRI_DIEM_THU");
        });

        modelBuilder.Entity<DmKimNiem>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DM_KIM_NIEM");

            entity.Property(e => e.MaChiNhanh)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_CHI_NHANH");
            entity.Property(e => e.MaKim)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_KIM");
            entity.Property(e => e.SoKiem)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SO_KIEM");
            entity.Property(e => e.TenKim)
                .HasMaxLength(100)
                .HasColumnName("TEN_KIM");
        });

        modelBuilder.Entity<DmLoaiSmsEmail>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("DM_LOAI_SMS_EMAIL_PK");

            entity.ToTable("DM_LOAI_SMS_EMAIL");

            entity.Property(e => e.MaLoai)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_LOAI");
            entity.Property(e => e.SmsChuan)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("SMS_CHUAN");
            entity.Property(e => e.TenLoai)
                .HasMaxLength(50)
                .HasColumnName("TEN_LOAI");
            entity.Property(e => e.Tt)
                .HasColumnType("NUMBER")
                .HasColumnName("TT");
        });

        modelBuilder.Entity<DmNhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNhanVien).HasName("DM_NHAN_VIEN_PK");

            entity.ToTable("DM_NHAN_VIEN");

            entity.Property(e => e.MaNhanVien)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("MA_NHAN_VIEN");
            entity.Property(e => e.CanhBaoGiamM3)
                .HasColumnType("NUMBER")
                .HasColumnName("CANH_BAO_GIAM_M3");
            entity.Property(e => e.CanhBaoGiamPt)
                .HasColumnType("NUMBER")
                .HasColumnName("CANH_BAO_GIAM_PT");
            entity.Property(e => e.CanhBaoM3)
                .HasColumnType("NUMBER")
                .HasColumnName("CANH_BAO_M3");
            entity.Property(e => e.CanhBaoPt)
                .HasColumnType("NUMBER")
                .HasColumnName("CANH_BAO_PT");
            entity.Property(e => e.DienThoai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DIEN_THOAI");
            entity.Property(e => e.DienThoaiApp)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("DIEN_THOAI_APP");
            entity.Property(e => e.DocChiSo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("DOC_CHI_SO");
            entity.Property(e => e.DungLuong4g)
                .HasColumnType("NUMBER")
                .HasColumnName("DUNG_LUONG_4G");
            entity.Property(e => e.DungLuongPin)
                .HasColumnType("NUMBER")
                .HasColumnName("DUNG_LUONG_PIN");
            entity.Property(e => e.HieuLuc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("HIEU_LUC");
            entity.Property(e => e.Imei)
                .HasMaxLength(18)
                .IsUnicode(false)
                .HasColumnName("IMEI");
            entity.Property(e => e.ImeiSub)
                .HasMaxLength(18)
                .IsUnicode(false)
                .HasColumnName("IMEI_SUB");
            entity.Property(e => e.ImeiSub2)
                .HasMaxLength(18)
                .IsUnicode(false)
                .HasColumnName("IMEI_SUB2");
            entity.Property(e => e.LoaiApp)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("LOAI_APP");
            entity.Property(e => e.LogDate)
                .HasColumnType("DATE")
                .HasColumnName("LOG_DATE");
            entity.Property(e => e.MaChiNhanh)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_CHI_NHANH");
            entity.Property(e => e.MaSoBhxh)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("MA_SO_BHXH");
            entity.Property(e => e.NhomTruong)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("NHOM_TRUONG");
            entity.Property(e => e.NhomTruongParent)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("NHOM_TRUONG_PARENT");
            entity.Property(e => e.Pas)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PAS");
            entity.Property(e => e.QuyenWebapp)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("QUYEN_WEBAPP");
            entity.Property(e => e.TenNhanVien)
                .HasMaxLength(100)
                .HasColumnName("TEN_NHAN_VIEN");
            entity.Property(e => e.TenNhanVienD)
                .HasMaxLength(100)
                .HasColumnName("TEN_NHAN_VIEN_D");
            entity.Property(e => e.ThiCong)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("THI_CONG");
            entity.Property(e => e.Token)
                .HasMaxLength(165)
                .IsUnicode(false)
                .HasColumnName("TOKEN");
            entity.Property(e => e.Usr)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USR");
            entity.Property(e => e.VerCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("VER_CODE");
            entity.Property(e => e.VerCodeTc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("VER_CODE_TC");
        });

        modelBuilder.Entity<DmSoDoc>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DM_SO_DOC");

            entity.Property(e => e.HieuLuc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("HIEU_LUC");
            entity.Property(e => e.MaBienDoc)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_BIEN_DOC");
            entity.Property(e => e.MaChiNhanh)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_CHI_NHANH");
            entity.Property(e => e.MaSoDoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SO_DOC");
            entity.Property(e => e.MaThuNgan)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_THU_NGAN");
            entity.Property(e => e.NgayDoc)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("NGAY_DOC");
            entity.Property(e => e.NgayDoc2ky)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("NGAY_DOC_2KY");
            entity.Property(e => e.TenSoDoc)
                .HasMaxLength(100)
                .HasColumnName("TEN_SO_DOC");
        });

        modelBuilder.Entity<DmTinhTrangDongHo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DM_TINH_TRANG_DONG_HO");

            entity.HasIndex(e => e.MaTinhTrangSo, "DM_TINH_TRANG_DONG_HO_INDEX1");

            entity.Property(e => e.GiaiPhap)
                .HasMaxLength(60)
                .HasColumnName("GIAI_PHAP");
            entity.Property(e => e.HieuLuc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("HIEU_LUC");
            entity.Property(e => e.MaTinhTrangSo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MA_TINH_TRANG_SO");
            entity.Property(e => e.MoTa)
                .HasMaxLength(100)
                .HasColumnName("MO_TA");
            entity.Property(e => e.MoTaNgan)
                .HasMaxLength(30)
                .HasColumnName("MO_TA_NGAN");
            entity.Property(e => e.MoTaSub)
                .HasMaxLength(80)
                .HasColumnName("MO_TA_SUB");
            entity.Property(e => e.NCongDonChiSo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("N_CONG_DON_CHI_SO");
            entity.Property(e => e.NDongHoHong)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("N_DONG_HO_HONG");
            entity.Property(e => e.NNhapChiSoMoi)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("N_NHAP_CHI_SO_MOI");
            entity.Property(e => e.NNhapSlTrucTiep)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("N_NHAP_SL_TRUC_TIEP");
            entity.Property(e => e.NSuaChiSoCu)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("N_SUA_CHI_SO_CU");
            entity.Property(e => e.NgayCapNhat)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.NguoiCapNhat)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NGUOI_CAP_NHAT");
            entity.Property(e => e.SttHienThi)
                .HasColumnType("NUMBER")
                .HasColumnName("STT_HIEN_THI");
            entity.Property(e => e.TamKhongTinhHd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("0 ")
                .HasColumnName("TAM_KHONG_TINH_HD");
        });

        modelBuilder.Entity<DmTrangThaiEmail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DM_TRANG_THAI_EMAIL");

            entity.Property(e => e.GhiChu)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("GHI_CHU");
            entity.Property(e => e.MaLoai)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_LOAI");
            entity.Property(e => e.TenLoai)
                .HasMaxLength(50)
                .HasColumnName("TEN_LOAI");
            entity.Property(e => e.Tt)
                .HasColumnType("NUMBER")
                .HasColumnName("TT");
        });

        modelBuilder.Entity<DmTrangThaiSm>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("DM_TRANG_THAI_SMS_PK");

            entity.ToTable("DM_TRANG_THAI_SMS");

            entity.Property(e => e.MaLoai)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_LOAI");
            entity.Property(e => e.GhiChu)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("GHI_CHU");
            entity.Property(e => e.TenLoai)
                .HasMaxLength(50)
                .HasColumnName("TEN_LOAI");
            entity.Property(e => e.Tt)
                .HasColumnType("NUMBER")
                .HasColumnName("TT");
        });

        modelBuilder.Entity<KDmDiaChinh>(entity =>
        {
            entity.HasKey(e => e.MaDiaChinh).HasName("K_DM_DIA_CHINH_PK");

            entity.ToTable("K_DM_DIA_CHINH");

            entity.HasIndex(e => e.CapDiaChinh, "K_DM_DIA_CHINH_CAP_DC");

            entity.Property(e => e.MaDiaChinh)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_DIA_CHINH");
            entity.Property(e => e.CapDiaChinh)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("CAP_DIA_CHINH");
            entity.Property(e => e.IsPhuongMoi)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IS_PHUONG_MOI");
            entity.Property(e => e.KyHieu)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("KY_HIEU");
            entity.Property(e => e.KyHieuSub)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("KY_HIEU_SUB");
            entity.Property(e => e.MaDonVi)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_DON_VI");
            entity.Property(e => e.MaPhuong)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_PHUONG");
            entity.Property(e => e.MaQuan)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_QUAN");
            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("SYSDATE ")
                .HasColumnType("DATE")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.NgaySua)
                .HasDefaultValueSql("SYSDATE ")
                .HasColumnType("DATE")
                .HasColumnName("NGAY_SUA");
            entity.Property(e => e.NguoiCapNhat)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("NGUOI_CAP_NHAT");
            entity.Property(e => e.NguoiSua)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("NGUOI_SUA");
            entity.Property(e => e.ParentMa)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("PARENT_MA");
            entity.Property(e => e.ParentMaSub)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("PARENT_MA_SUB");
            entity.Property(e => e.TenDiaChinh)
                .HasMaxLength(50)
                .HasColumnName("TEN_DIA_CHINH");
            entity.Property(e => e.TenPhuong)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TEN_PHUONG");
            entity.Property(e => e.TenQuan)
                .HasMaxLength(50)
                .HasColumnName("TEN_QUAN");
        });

        modelBuilder.Entity<KDmGiaPhi>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("K_DM_GIA_PHI");

            entity.Property(e => e.GiaTri)
                .HasColumnType("NUMBER")
                .HasColumnName("GIA_TRI");
            entity.Property(e => e.KieuPhi)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("KIEU_PHI");
            entity.Property(e => e.LoaiPhi)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("LOAI_PHI");
            entity.Property(e => e.MaPhi)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("MA_PHI");
            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("SYSDATE ")
                .HasColumnType("DATE")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.NgaySua)
                .HasDefaultValueSql("SYSDATE ")
                .HasColumnType("DATE")
                .HasColumnName("NGAY_SUA");
            entity.Property(e => e.NguoiCapNhat)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("USER ")
                .HasColumnName("NGUOI_CAP_NHAT");
            entity.Property(e => e.NguoiSua)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("USER ")
                .HasColumnName("NGUOI_SUA");
            entity.Property(e => e.TenPhi)
                .HasMaxLength(100)
                .HasColumnName("TEN_PHI");
        });

        modelBuilder.Entity<KDmGiaSub>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("K_DM_GIA_SUB");

            entity.HasIndex(e => e.MaGia, "K_DM_GIA_SUB_INDEX1");

            entity.Property(e => e.Loai)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("LOAI");
            entity.Property(e => e.MaGia)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MA_GIA");
            entity.Property(e => e.TenGia)
                .HasMaxLength(200)
                .HasColumnName("TEN_GIA");
            entity.Property(e => e.TenGiaOnly)
                .HasMaxLength(30)
                .HasColumnName("TEN_GIA_ONLY");
            entity.Property(e => e.TenGiaSub)
                .HasMaxLength(200)
                .HasColumnName("TEN_GIA_SUB");
        });

        modelBuilder.Entity<KDmGium>(entity =>
        {
            entity.HasKey(e => e.MaGia).HasName("DM_GIA_PK");

            entity.ToTable("K_DM_GIA");

            entity.HasIndex(e => e.KyHieuGia, "K_DM_GIA_INDEX1");

            entity.Property(e => e.MaGia)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_GIA");
            entity.Property(e => e.ChuoiGiaDm)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("CHUOI_GIA_DM");
            entity.Property(e => e.ChuoiGiaGt)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("CHUOI_GIA_GT");
            entity.Property(e => e.GiaCoBan)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMBER(9,2)")
                .HasColumnName("GIA_CO_BAN");
            entity.Property(e => e.GiaCt01)
                .HasMaxLength(100)
                .HasColumnName("GIA_CT_01");
            entity.Property(e => e.GiaCt02)
                .HasMaxLength(100)
                .HasColumnName("GIA_CT_02");
            entity.Property(e => e.GiaCt03)
                .HasMaxLength(100)
                .HasColumnName("GIA_CT_03");
            entity.Property(e => e.GiaCt04)
                .HasMaxLength(100)
                .HasColumnName("GIA_CT_04");
            entity.Property(e => e.GiaCt05)
                .HasMaxLength(100)
                .HasColumnName("GIA_CT_05");
            entity.Property(e => e.GiaCt06)
                .HasMaxLength(100)
                .HasColumnName("GIA_CT_06");
            entity.Property(e => e.GiaGt01)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMBER(9,2)")
                .HasColumnName("GIA_GT_01");
            entity.Property(e => e.GiaGt02)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMBER(9,2)")
                .HasColumnName("GIA_GT_02");
            entity.Property(e => e.GiaGt03)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMBER(9,2)")
                .HasColumnName("GIA_GT_03");
            entity.Property(e => e.GiaGt04)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMBER(9,2)")
                .HasColumnName("GIA_GT_04");
            entity.Property(e => e.GiaGt05)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMBER(9,2)")
                .HasColumnName("GIA_GT_05");
            entity.Property(e => e.GiaGt06)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMBER(9,2)")
                .HasColumnName("GIA_GT_06");
            entity.Property(e => e.HieuLuc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("1")
                .HasColumnName("HIEU_LUC");
            entity.Property(e => e.KieuGia)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("KIEU_GIA");
            entity.Property(e => e.KieuTinh)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("KIEU_TINH");
            entity.Property(e => e.KyHieuGia)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("KY_HIEU_GIA");
            entity.Property(e => e.LoaiKhachHang)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("NULL")
                .HasColumnName("LOAI_KHACH_HANG");
            entity.Property(e => e.MaMucDichSd)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_MUC_DICH_SD");
            entity.Property(e => e.MaPhiBvmt)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("MA_PHI_BVMT");
            entity.Property(e => e.MaThueVat)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("MA_THUE_VAT");
            entity.Property(e => e.NgayApDung)
                .HasDefaultValueSql("SYSDATE")
                .HasColumnType("DATE")
                .HasColumnName("NGAY_AP_DUNG");
            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("SYSDATE")
                .HasColumnType("DATE")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.NgaySua)
                .HasDefaultValueSql("SYSDATE")
                .HasColumnType("DATE")
                .HasColumnName("NGAY_SUA");
            entity.Property(e => e.NguoiCapNhat)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("NGUOI_CAP_NHAT");
            entity.Property(e => e.NguoiSua)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("NGUOI_SUA");
            entity.Property(e => e.SoNgayTinhGia)
                .HasDefaultValueSql("30")
                .HasColumnType("NUMBER")
                .HasColumnName("SO_NGAY_TINH_GIA");
            entity.Property(e => e.TenGia)
                .HasMaxLength(100)
                .HasColumnName("TEN_GIA");
        });

        modelBuilder.Entity<LogBankDonVi>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LOG_BANK_DON_VI");

            entity.Property(e => e.Act)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ACT");
            entity.Property(e => e.BoLog)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("BO_LOG");
            entity.Property(e => e.BoLogFile)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("BO_LOG_FILE");
            entity.Property(e => e.Cha)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CHA");
            entity.Property(e => e.DiaChi)
                .HasMaxLength(200)
                .HasColumnName("DIA_CHI");
            entity.Property(e => e.DienThoai)
                .HasMaxLength(100)
                .HasColumnName("DIEN_THOAI");
            entity.Property(e => e.DoiSoatFtpGop)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("DOI_SOAT_FTP_GOP");
            entity.Property(e => e.IdSxep)
                .HasColumnType("NUMBER")
                .HasColumnName("ID_SXEP");
            entity.Property(e => e.IpDonVi)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("IP_DON_VI");
            entity.Property(e => e.IpDonViSub)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IP_DON_VI_SUB");
            entity.Property(e => e.IsDiemThuTt)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IS_DIEM_THU_TT");
            entity.Property(e => e.IsMacAdd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IS_MAC_ADD");
            entity.Property(e => e.LoaiDonVi)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("LOAI_DON_VI");
            entity.Property(e => e.Ma)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("MA");
            entity.Property(e => e.MaGop)
                .HasColumnType("NUMBER")
                .HasColumnName("MA_GOP");
            entity.Property(e => e.MaNhBilling)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("MA_NH_BILLING");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("MAT_KHAU");
            entity.Property(e => e.MeView)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("ME_VIEW");
            entity.Property(e => e.NguoiDung)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NGUOI_DUNG");
            entity.Property(e => e.PathDoiSoat)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("PATH_DOI_SOAT");
            entity.Property(e => e.SlhdOnline)
                .HasColumnType("NUMBER")
                .HasColumnName("SLHD_ONLINE");
            entity.Property(e => e.SlhdOnlineDate)
                .HasColumnType("DATE")
                .HasColumnName("SLHD_ONLINE_DATE");
            entity.Property(e => e.SlhdOnlineTien)
                .HasColumnType("NUMBER")
                .HasColumnName("SLHD_ONLINE_TIEN");
            entity.Property(e => e.SuDungFileAccess)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("SU_DUNG_FILE_ACCESS");
            entity.Property(e => e.Ten)
                .HasMaxLength(100)
                .HasColumnName("TEN");
            entity.Property(e => e.TenBill)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TEN_BILL");
            entity.Property(e => e.TenGop)
                .HasMaxLength(100)
                .HasColumnName("TEN_GOP");
            entity.Property(e => e.TenSub)
                .HasMaxLength(255)
                .HasColumnName("TEN_SUB");
            entity.Property(e => e.TenVietTat)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TEN_VIET_TAT");
        });

        modelBuilder.Entity<LogBankFix>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LOG_BANK_FIX");

            entity.Property(e => e.DebtCancel)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("DEBT_CANCEL");
            entity.Property(e => e.DebtCancelDetail)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DEBT_CANCEL_DETAIL");
            entity.Property(e => e.DebtCancelTout)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER")
                .HasColumnName("DEBT_CANCEL_TOUT");
            entity.Property(e => e.DebtCheck)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("DEBT_CHECK");
            entity.Property(e => e.DebtCheckDetail)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DEBT_CHECK_DETAIL");
            entity.Property(e => e.DebtCheckTout)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER")
                .HasColumnName("DEBT_CHECK_TOUT");
            entity.Property(e => e.DebtCheckV2)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("DEBT_CHECK_V2");
            entity.Property(e => e.DebtCheckV2Detail)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DEBT_CHECK_V2_DETAIL");
            entity.Property(e => e.DebtCheckV2Tout)
                .HasColumnType("NUMBER")
                .HasColumnName("DEBT_CHECK_V2_TOUT");
            entity.Property(e => e.DebtPayment)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("DEBT_PAYMENT");
            entity.Property(e => e.DebtPaymentDetail)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DEBT_PAYMENT_DETAIL");
            entity.Property(e => e.DebtPaymentTout)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER")
                .HasColumnName("DEBT_PAYMENT_TOUT");
            entity.Property(e => e.DebtPaymentV2)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("DEBT_PAYMENT_V2");
            entity.Property(e => e.DebtPaymentV2Detail)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DEBT_PAYMENT_V2_DETAIL");
            entity.Property(e => e.DebtPaymentV2Tout)
                .HasColumnType("NUMBER")
                .HasColumnName("DEBT_PAYMENT_V2_TOUT");
            entity.Property(e => e.DonVi)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("DON_VI");
            entity.Property(e => e.Err)
                .HasMaxLength(200)
                .HasColumnName("ERR");
            entity.Property(e => e.GhiChu)
                .HasMaxLength(150)
                .HasColumnName("GHI_CHU");
            entity.Property(e => e.InfocustCheck)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("NULL ")
                .HasColumnName("INFOCUST_CHECK");
            entity.Property(e => e.InfocustCheckDetail)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("INFOCUST_CHECK_DETAIL");
            entity.Property(e => e.InfocustCheckTout)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER")
                .HasColumnName("INFOCUST_CHECK_TOUT");
            entity.Property(e => e.SystemCheck)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("SYSTEM_CHECK");
            entity.Property(e => e.SystemCheckDetail)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("SYSTEM_CHECK_DETAIL");
            entity.Property(e => e.SystemCheckTout)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER")
                .HasColumnName("SYSTEM_CHECK_TOUT");
            entity.Property(e => e.Template)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TEMPLATE");
            entity.Property(e => e.ToutItem4)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER")
                .HasColumnName("TOUT_ITEM_4");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("TRANG_THAI");
        });

        modelBuilder.Entity<LogBankTienMoNuoc>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LOG_BANK_TIEN_MO_NUOC");

            entity.Property(e => e.GhiChu)
                .HasMaxLength(100)
                .HasColumnName("GHI_CHU");
            entity.Property(e => e.KieuCat)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("KIEU_CAT");
            entity.Property(e => e.NgayTao)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_TAO");
            entity.Property(e => e.SoTien)
                .HasColumnType("NUMBER")
                .HasColumnName("SO_TIEN");
        });

        modelBuilder.Entity<LogEmail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LOG_EMAIL");

            entity.HasIndex(e => e.LogEmailId, "EMAIL_LOG_EMAIL_ID");

            entity.HasIndex(e => e.NgayGui, "EMAIL_NGAY_GUI");

            entity.HasIndex(e => e.SoHoaDon, "EMAIL_SO_HOA_DON");

            entity.Property(e => e.DaLuuEbilsub)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("DA_LUU_EBILSUB");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.KetQua)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("KET_QUA");
            entity.Property(e => e.LoaiEmail)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("LOAI_EMAIL");
            entity.Property(e => e.LogEmailId)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("LOG_EMAIL_ID");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaMau)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_MAU");
            entity.Property(e => e.NgayGui)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_GUI");
            entity.Property(e => e.NgayPhhd)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_PHHD");
            entity.Property(e => e.SoHoaDon)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("SO_HOA_DON");
            entity.Property(e => e.Usr)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("USR");
        });

        modelBuilder.Entity<LogSm>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LOG_SMS");

            entity.HasIndex(e => e.DaLuuEbilsub, "LOG_SMS_LUU_EBILSUB");

            entity.HasIndex(e => e.KetQua, "SMS_KET_QUA");

            entity.HasIndex(e => e.LoaiSms, "SMS_LOAI_SMS");

            entity.HasIndex(e => e.LogSmsId, "SMS_LOG_SMS_ID");

            entity.HasIndex(e => e.MaKhachHang, "SMS_MA_KHACH_HANG");

            entity.HasIndex(e => e.NgayGui, "SMS_NGAY_GUI");

            entity.HasIndex(e => e.NhaMang, "SMS_NHA_MANG");

            entity.HasIndex(e => e.SoHoaDon, "SMS_SO_HOA_DON");

            entity.Property(e => e.DaLuuEbilsub)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("DA_LUU_EBILSUB");
            entity.Property(e => e.KetQua)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("KET_QUA");
            entity.Property(e => e.LoaiSms)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("LOAI_SMS");
            entity.Property(e => e.LogSmsId)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("LOG_SMS_ID");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaMau)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_MAU");
            entity.Property(e => e.NgayGui)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_GUI");
            entity.Property(e => e.NgayPhhd)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_PHHD");
            entity.Property(e => e.NhaMang)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("NHA_MANG");
            entity.Property(e => e.NoiDungSms)
                .HasMaxLength(500)
                .HasColumnName("NOI_DUNG_SMS");
            entity.Property(e => e.ShowOk)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("SHOW_OK");
            entity.Property(e => e.SmsSeTinh)
                .HasColumnType("NUMBER")
                .HasColumnName("SMS_SE_TINH");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("SO_DIEN_THOAI");
            entity.Property(e => e.SoHoaDon)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("SO_HOA_DON");
            entity.Property(e => e.Zalo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ZALO");
        });

        modelBuilder.Entity<Password>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PASSWORD");

            entity.Property(e => e.CbCatNuoc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("CB_CAT_NUOC");
            entity.Property(e => e.CbThoiGianLan)
                .HasColumnType("NUMBER")
                .HasColumnName("CB_THOI_GIAN_LAN");
            entity.Property(e => e.ChiDoc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("CHI_DOC");
            entity.Property(e => e.DonViLog)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("DON_VI_LOG");
            entity.Property(e => e.HoTen)
                .HasMaxLength(200)
                .HasColumnName("HO_TEN");
            entity.Property(e => e.IpAccsess01)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("IP_ACCSESS_01");
            entity.Property(e => e.IpAccsess02)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("IP_ACCSESS_02");
            entity.Property(e => e.IpBilling)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("IP_BILLING");
            entity.Property(e => e.IpEnable)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IP_ENABLE");
            entity.Property(e => e.MaDonVi)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_DON_VI");
            entity.Property(e => e.MacErr)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("MAC_ERR");
            entity.Property(e => e.MacUsr)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("MAC_USR");
            entity.Property(e => e.Menu)
                .HasMaxLength(1600)
                .HasColumnName("MENU");
            entity.Property(e => e.NgayTao)
                .HasMaxLength(20)
                .HasColumnName("NGAY_TAO");
            entity.Property(e => e.NgayUpdate)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_UPDATE");
            entity.Property(e => e.Nhom)
                .HasMaxLength(20)
                .HasColumnName("NHOM");
            entity.Property(e => e.ParentMaDonVi)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("PARENT_MA_DON_VI");
            entity.Property(e => e.Pas)
                .HasMaxLength(20)
                .HasColumnName("PAS");
            entity.Property(e => e.PasBilling)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PAS_BILLING");
            entity.Property(e => e.Tb)
                .HasMaxLength(200)
                .HasColumnName("TB");
            entity.Property(e => e.TenCongTy)
                .HasMaxLength(200)
                .HasColumnName("TEN_CONG_TY");
            entity.Property(e => e.TenDonVi)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("TEN_DON_VI");
            entity.Property(e => e.Tmr)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER")
                .HasColumnName("TMR");
            entity.Property(e => e.TmrAccsess)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("TMR_ACCSESS");
            entity.Property(e => e.TmrMovefile)
                .HasColumnType("NUMBER")
                .HasColumnName("TMR_MOVEFILE");
            entity.Property(e => e.TmrTrangthai)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("0 ")
                .HasColumnName("TMR_TRANGTHAI");
            entity.Property(e => e.Upd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("UPD");
            entity.Property(e => e.Usr)
                .HasMaxLength(20)
                .HasColumnName("USR");
            entity.Property(e => e.UsrBilling)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USR_BILLING");
        });

        modelBuilder.Entity<ThongTinKh>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("THONG_TIN_KH");

            entity.HasIndex(e => e.EmailKhMoi, "CN_EMAIL_KH_MOI");

            entity.HasIndex(e => e.MaKhachHang, "KH_INDEX");

            entity.HasIndex(e => e.MaKhachHangPr, "MAKH_PARENT");

            entity.HasIndex(e => e.MaSoDoc, "MA_SO_DOC");

            entity.HasIndex(e => e.NgayDocSo, "NGAY_DOC");

            entity.HasIndex(e => e.MaBienDoc, "THONG_TIN_KH_MABD");

            entity.HasIndex(e => e.MaChiNhanh, "THONG_TIN_KH_MA_CN");

            entity.HasIndex(e => e.NgayThanhLyHd, "THONG_TIN_KH_NGAYTL");

            entity.HasIndex(e => e.PhoneUt1, "THONG_TIN_KH_SDT1");

            entity.HasIndex(e => e.MaNhomKhMes, "TTKH_MA_MES");

            entity.Property(e => e.BienDoc)
                .HasMaxLength(50)
                .HasColumnName("BIEN_DOC");
            entity.Property(e => e.CccdHoChieu)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CCCD_HO_CHIEU");
            entity.Property(e => e.ChiNhanh)
                .HasMaxLength(40)
                .HasColumnName("CHI_NHANH");
            entity.Property(e => e.ChiNhanhCu)
                .HasMaxLength(40)
                .HasColumnName("CHI_NHANH_CU");
            entity.Property(e => e.ChiSoThaoLap)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CHI_SO_THAO_LAP");
            entity.Property(e => e.ChuoiGia)
                .HasMaxLength(100)
                .HasColumnName("CHUOI_GIA");
            entity.Property(e => e.CoDongHo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CO_DONG_HO");
            entity.Property(e => e.CongThucGia)
                .HasMaxLength(100)
                .HasColumnName("CONG_THUC_GIA");
            entity.Property(e => e.DangKyZalo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("DANG_KY_ZALO");
            entity.Property(e => e.DiaChiDongHo)
                .HasMaxLength(250)
                .HasColumnName("DIA_CHI_DONG_HO");
            entity.Property(e => e.DiaChiKhachHang)
                .HasMaxLength(250)
                .HasColumnName("DIA_CHI_KHACH_HANG");
            entity.Property(e => e.DienThoai)
                .HasMaxLength(20)
                .HasColumnName("DIEN_THOAI");
            entity.Property(e => e.DienThoaiDd)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("DIEN_THOAI_DD");
            entity.Property(e => e.DinhMuc)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("DINH_MUC");
            entity.Property(e => e.EmailKhMoi)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("EMAIL_KH_MOI");
            entity.Property(e => e.EmailThongBao)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("EMAIL_THONG_BAO");
            entity.Property(e => e.EmailUt1)
                .HasMaxLength(50)
                .HasColumnName("EMAIL_UT1");
            entity.Property(e => e.EmailUt1BillBk)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL_UT1_BILL_BK");
            entity.Property(e => e.EmailUt1Bk)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL_UT1_BK");
            entity.Property(e => e.EmailUt2)
                .HasMaxLength(50)
                .HasColumnName("EMAIL_UT2");
            entity.Property(e => e.HopThu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("HOP_THU");
            entity.Property(e => e.HtKd)
                .HasMaxLength(100)
                .HasColumnName("HT_KD");
            entity.Property(e => e.KieuHopDong)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("KIEU_HOP_DONG");
            entity.Property(e => e.LoaiKhachHang)
                .HasMaxLength(2)
                .HasColumnName("LOAI_KHACH_HANG");
            entity.Property(e => e.MaBienDoc)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_BIEN_DOC");
            entity.Property(e => e.MaChiNhanh)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_CHI_NHANH");
            entity.Property(e => e.MaChiNhanhCu)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_CHI_NHANH_CU");
            entity.Property(e => e.MaDc)
                .HasMaxLength(10)
                .HasColumnName("MA_DC");
            entity.Property(e => e.MaDcCu)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MA_DC_CU");
            entity.Property(e => e.MaDiemThu)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_DIEM_THU");
            entity.Property(e => e.MaDongHo)
                .HasMaxLength(50)
                .HasColumnName("MA_DONG_HO");
            entity.Property(e => e.MaGia)
                .HasMaxLength(10)
                .HasColumnName("MA_GIA");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(9)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaKhachHangParent)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG_PARENT");
            entity.Property(e => e.MaKhachHangPr)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG_PR");
            entity.Property(e => e.MaNganHang)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_NGAN_HANG");
            entity.Property(e => e.MaNhoThu)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("MA_NHO_THU");
            entity.Property(e => e.MaNhomKhMes)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("MA_NHOM_KH_MES");
            entity.Property(e => e.MaQhnsKh)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MA_QHNS_KH");
            entity.Property(e => e.MaSoDoc)
                .HasMaxLength(10)
                .HasColumnName("MA_SO_DOC");
            entity.Property(e => e.MaSoThue)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MA_SO_THUE");
            entity.Property(e => e.MaTinhTrangDh)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MA_TINH_TRANG_DH");
            entity.Property(e => e.MatKhauBanDau)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MAT_KHAU_BAN_DAU");
            entity.Property(e => e.MatKhauMd5)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("MAT_KHAU_MD5");
            entity.Property(e => e.MucDichGia)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("MUC_DICH_GIA");
            entity.Property(e => e.NgayDocSo)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("NGAY_DOC_SO");
            entity.Property(e => e.NgayHopDong)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_HOP_DONG");
            entity.Property(e => e.NgayHopDongGoc)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_HOP_DONG_GOC");
            entity.Property(e => e.NgayLapDat)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_LAP_DAT");
            entity.Property(e => e.NgayThanhLyHd)
                .HasColumnType("DATE")
                .HasColumnName("NGAY_THANH_LY_HD");
            entity.Property(e => e.PhoneUt1)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("PHONE_UT1");
            entity.Property(e => e.PhoneUt1BillBk)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("PHONE_UT1_BILL_BK");
            entity.Property(e => e.PhoneUt1Bk)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("PHONE_UT1_BK");
            entity.Property(e => e.PhoneUt2)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("PHONE_UT2");
            entity.Property(e => e.Sms)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("SMS");
            entity.Property(e => e.SoHo)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SO_HO");
            entity.Property(e => e.SoHopDong)
                .HasMaxLength(15)
                .HasColumnName("SO_HOP_DONG");
            entity.Property(e => e.SoKhau)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SO_KHAU");
            entity.Property(e => e.SoOCuaSo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("SO_O_CUA_SO");
            entity.Property(e => e.SoSerialDongHo)
                .HasMaxLength(20)
                .HasColumnName("SO_SERIAL_DONG_HO");
            entity.Property(e => e.SoTaiKhoan)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SO_TAI_KHOAN");
            entity.Property(e => e.SttSoDoc)
                .HasColumnType("NUMBER")
                .HasColumnName("STT_SO_DOC");
            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TEN_DANG_NHAP");
            entity.Property(e => e.TenDongHo)
                .HasMaxLength(50)
                .HasColumnName("TEN_DONG_HO");
            entity.Property(e => e.TenDuong)
                .HasMaxLength(200)
                .HasColumnName("TEN_DUONG");
            entity.Property(e => e.TenGiaNuoc)
                .HasMaxLength(100)
                .HasColumnName("TEN_GIA_NUOC");
            entity.Property(e => e.TenKhachHang)
                .HasMaxLength(200)
                .HasColumnName("TEN_KHACH_HANG");
            entity.Property(e => e.TenMang)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("TEN_MANG");
            entity.Property(e => e.TenMangChuyen)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("TEN_MANG_CHUYEN");
            entity.Property(e => e.TenPhuong)
                .HasMaxLength(200)
                .HasColumnName("TEN_PHUONG");
            entity.Property(e => e.TenPhuongCu)
                .HasMaxLength(50)
                .HasColumnName("TEN_PHUONG_CU");
            entity.Property(e => e.TenQuan)
                .HasMaxLength(100)
                .HasColumnName("TEN_QUAN");
            entity.Property(e => e.TenSoDoc)
                .HasMaxLength(100)
                .HasColumnName("TEN_SO_DOC");
            entity.Property(e => e.ThuNgan)
                .HasMaxLength(50)
                .HasColumnName("THU_NGAN");
            entity.Property(e => e.Vung)
                .HasMaxLength(100)
                .HasColumnName("VUNG");
            entity.Property(e => e.VungCu)
                .HasMaxLength(100)
                .HasColumnName("VUNG_CU");
        });
        modelBuilder.HasSequence("APP_DH_HE_SO_SEQ");
        modelBuilder.HasSequence("CC_LOG_ERR_SEQ");
        modelBuilder.HasSequence("DEPT_SEQ");
        modelBuilder.HasSequence("SEQ_BILLING_MAPPING");
        modelBuilder.HasSequence("SEQ_BILLING_TRANSACTION");
        modelBuilder.HasSequence("TAXOUT_SEQ");
        modelBuilder.HasSequence("WEB_01_DANG_KY_DON_SEQ");
        modelBuilder.HasSequence("WEB_10_VAN_DONG_MO_SEQ");
        modelBuilder.HasSequence("WEB_10_VAN_DONG_MO_SEQ1");
        modelBuilder.HasSequence("WEB_111_DHDT_DIEM_DO_LICH_SU_");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
