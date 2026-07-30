namespace ReadMeter.Api.Models;

public sealed class LogSmsEntity
{
    public string? MA_KHACH_HANG { get; set; }
    public string? SO_DIEN_THOAI { get; set; }
    public string? NOI_DUNG_SMS { get; set; }
    public DateTime? NGAY_GUI { get; set; }
    public string? LOAI_SMS { get; set; }
    public string? KET_QUA { get; set; }
}

public sealed class DmTrangThaiSmsEntity
{
    public string? MA_LOAI { get; set; }
    public string? TEN_LOAI { get; set; }
}

public interface ITcWorkItem
{
    string? ID_XAC_NHAN { get; set; }
    string? MA_KHACH_HANG { get; set; }
    string? MA_NHAN_VIEN_NT { get; set; }
    string? MA_XI_NGHIEP { get; set; }
    string? GHI_CHU_BILLING { get; set; }
    decimal? STT_HS { get; set; }
    string? MA_TTTC { get; set; }
    DateTime? NGAY_1_XN_GIAO_NT { get; set; }
    DateTime? NGAY_2_NT_NHAN_XN { get; set; }
    DateTime? NGAY_3_NT_GIAO_CN { get; set; }
    DateTime? NGAY_4_CN_NHAN_NT { get; set; }
    DateTime? NGAY_5_CN_TC_OK { get; set; }
    DateTime? NGAY_6_CN_GIAO_NT { get; set; }
    DateTime? NGAY_7_NT_NHAN_CN { get; set; }
    DateTime? NGAY_8_NT_GIAO_XN { get; set; }
    DateTime? NGAY_9_XN_NHAN_NT { get; set; }
    string? MA_BIEN_DOC { get; set; }
    string? MA_NHAN_VIEN_CN { get; set; }
}

public sealed class AppCheckInCatNuocEntity
{
    public string? ID_XAC_NHAN { get; set; }
    public string? MA_KHACH_HANG { get; set; }
    public string? LOG_USER { get; set; }
    public DateTime? NGAY_XN_BG_NHAN_VIEN { get; set; }
    public DateTime? NGAY_XN_NHAN_LAI_BG { get; set; }
    public string? MA_NHAN_VIEN { get; set; }
    public string? MA_XI_NGHIEP { get; set; }
    public DateTime? LOG_DATE_APP { get; set; }
    public string? VI_TRI_XAC_NHAN { get; set; }
    public string? TEN_FILE_ANH { get; set; }
    public string? GHI_CHU_THEM { get; set; }
    public string? MA_GHI_CHU { get; set; }
    public decimal? NGAY_DOC { get; set; }
    public string? MA_SO_DOC { get; set; }
    public DateTime? NGAY_HOAN_THANH { get; set; }
    public string? GHI_CHU_XN { get; set; }
    public string? THANG { get; set; }
    public string? NGUOI_THI_CONG { get; set; }
    public string? KIEU_CAT_MO { get; set; }
    public decimal? STT { get; set; }
    public decimal? CHI_SO_DH { get; set; }
    public string? VI_TRI_XAC_NHAN_CU { get; set; }
    public DateTime? NGAY_NIEM { get; set; }
    public string? SO_KIM_NIEM { get; set; }
    public decimal? CHI_SO_NIEM { get; set; }
    public decimal? CHI_SO_CAT { get; set; }
    public string? ID_BILLING { get; set; }
}

public sealed class AppCheckInGuiGiayEntity
{
    public string? ID_XAC_NHAN { get; set; }
    public string? MA_KHACH_HANG { get; set; }
    public string? LOG_USER { get; set; }
    public DateTime? NGAY_XN_BG_NHAN_VIEN { get; set; }
    public DateTime? NGAY_XN_NHAN_LAI_BG { get; set; }
    public string? MA_NHAN_VIEN { get; set; }
    public string? MA_XI_NGHIEP { get; set; }
    public DateTime? LOG_DATE_APP { get; set; }
    public string? VI_TRI_XAC_NHAN { get; set; }
    public string? TEN_FILE_ANH { get; set; }
    public string? GHI_CHU_THEM { get; set; }
    public decimal? NGAY_DOC { get; set; }
    public string? MA_SO_DOC { get; set; }
    public DateTime? NGAY_HOAN_THANH { get; set; }
    public string? GHI_CHU_XN { get; set; }
    public string? THANG { get; set; }
    public decimal? STT { get; set; }
    public decimal? CHI_SO_DH { get; set; }
    public string? VI_TRI_XAC_NHAN_CU { get; set; }
}

public sealed class AppCheckInMoNuocEntity
{
    public string? ID_XAC_NHAN { get; set; }
    public string? MA_KHACH_HANG { get; set; }
    public string? LOG_USER { get; set; }
    public DateTime? NGAY_XN_BG_NHAN_VIEN { get; set; }
    public DateTime? NGAY_XN_NHAN_LAI_BG { get; set; }
    public string? MA_NHAN_VIEN { get; set; }
    public string? MA_XI_NGHIEP { get; set; }
    public DateTime? LOG_DATE_APP { get; set; }
    public string? VI_TRI_XAC_NHAN { get; set; }
    public string? TEN_FILE_ANH { get; set; }
    public string? GHI_CHU_THEM { get; set; }
    public string? MA_GHI_CHU { get; set; }
    public decimal? NGAY_DOC { get; set; }
    public string? MA_SO_DOC { get; set; }
    public DateTime? NGAY_HOAN_THANH { get; set; }
    public string? GHI_CHU_XN { get; set; }
    public string? THANG { get; set; }
    public string? NGUOI_THI_CONG { get; set; }
    public string? KIEU_CAT_MO { get; set; }
    public decimal? STT { get; set; }
    public decimal? CHI_SO_DH { get; set; }
    public string? VI_TRI_XAC_NHAN_CU { get; set; }
    public DateTime? NGAY_NIEM { get; set; }
    public string? SO_KIM_NIEM { get; set; }
    public decimal? CHI_SO_NIEM { get; set; }
    public decimal? CHI_SO_MO { get; set; }
    public string? ID_BILLING { get; set; }
}

public sealed class AppDhChiSoEntity
{
    public string? ID_DCS { get; set; }
    public string? MA_KHACH_HANG { get; set; }
    public string? THANG { get; set; }
    public DateTime? NGAY_DOC_DK { get; set; }
    public DateTime? NGAY_DOC_CS { get; set; }
    public decimal? CHI_SO_CU { get; set; }
    public decimal? CHI_SO_MOI { get; set; }
    public decimal? SAN_LUONG_TT { get; set; }
    public decimal? TONG_SL { get; set; }
    public string? MA_TINH_TRANG_DH { get; set; }
    public string? LOG_USER { get; set; }
    public DateTime? NGAY_EBILL_BG { get; set; }
    public DateTime? NGAY_BIEN_DOC_BG { get; set; }
    public DateTime? NGAY_BD_NHAN_KHOA { get; set; }
    public DateTime? NGAY_EBILL_NHAN_KHOA { get; set; }
    public string? MA_SO_DOC { get; set; }
    public string? MA_BIEN_DOC { get; set; }
    public string? MA_CHI_NHANH { get; set; }
    public decimal? STT_SO_DOC { get; set; }
    public DateTime? NGAY_EBILL_NAP_BILL { get; set; }
    public string? QUA_VONG { get; set; }
    public string? LOAI_CHI_SO { get; set; }
    public decimal? CONG_CHI_SO { get; set; }
    public DateTime? NGAY_DOC_TUNG_DH { get; set; }
    public decimal? SAN_LUONG_DUNG_IT { get; set; }
    public DateTime? NGAY_DOC_TRUOC { get; set; }
    public string? GHI_CHU { get; set; }
    public string? VI_TRI_DOC { get; set; }
    public string? VI_TRI_DOC_CU { get; set; }
    public string? TEN_FILE_ANH { get; set; }
    public string? SUA_TEN_KHACH_HANG { get; set; }
    public string? SUA_DIA_CHI_DONG_HO { get; set; }
    public string? SUA_SO_DT { get; set; }
    public string? SUA_EMAIL { get; set; }
    public string? SUA_DH_TEN { get; set; }
    public string? SUA_DH_SERIAL { get; set; }
    public string? SUA_GHI_CHU { get; set; }
    public string? MA_GHI_CHU { get; set; }
    public decimal? SL_TB_3THANG { get; set; }
    public string? DOC_BO_SUNG { get; set; }
    public decimal? STT_SO_DOC_MOI { get; set; }
    public decimal? CHI_SO_MOI_SUB { get; set; }
    public decimal? TONG_SL_SUB { get; set; }
    public decimal? THOI_GIAN_TRE { get; set; }
}

public sealed class AppDhDmGhiChuEntity
{
    public string? MA_GHI_CHU { get; set; }
    public string? NOI_DUNG_GHI_CHU { get; set; }
    public decimal? STT { get; set; }
    public string? NHOM { get; set; }
    public string? DEN_KIEM_TRA { get; set; }
}

public sealed class AppDhLogBienDocEntity
{
    public string? ID_BD_LOG { get; set; }
    public string? MA_BIEN_DOC { get; set; }
    public DateTime? LOG_DATE { get; set; }
    public string? THUOC_HAM { get; set; }
    public string? DIEN_GIAI { get; set; }
    public string? MA_XI_NGHIEP { get; set; }
    public string? IMEI { get; set; }
    public decimal? THOI_GIAN_LOAD { get; set; }
}

public sealed class AppDhThangDocEntity
{
    public string? ID_THANG_DOC { get; set; }
    public string? THANG { get; set; }
    public string? NAM { get; set; }
}

public sealed class AppDhThiCongEntity
{
    public string? ID_TC { get; set; }
    public string? MA_KHACH_HANG { get; set; }
    public string? LOAI_CONG_TRINH { get; set; }
    public DateTime? NGAY_GIAO_TC { get; set; }
    public decimal? CHI_SO_LAP { get; set; }
    public decimal? CHI_SO_THAO { get; set; }
    public DateTime? NGAY_CAP_NHAT { get; set; }
}

public sealed class AppDhUpdateEntity
{
    public string? BAN_CAP_NHAT { get; set; }
    public string? DUONG_DAN { get; set; }
    public DateTime? NGAY_CAP_NHAT { get; set; }
    public decimal? TG_CAP_NHAT_SL_TV { get; set; }
    public string? ACT_ID_DOC { get; set; }
}

public sealed class AppDhUpdateThiCongEntity
{
    public string? BAN_CAP_NHAT { get; set; }
    public string? DUONG_DAN { get; set; }
    public DateTime? NGAY_CAP_NHAT { get; set; }
    public decimal? TG_CAP_NHAT_SL_TV { get; set; }
    public string? ACT_ID_DOC { get; set; }
}

public sealed class AppExDmGhiChuEntity
{
    public string? MA_GHI_CHU { get; set; }
    public string? TEN_GHI_CHU { get; set; }
    public decimal? STT { get; set; }
    public string? DIS { get; set; }
}

public sealed class AppExDmNhanVienEntity
{
    public string? MA_NHAN_VIEN { get; set; }
    public string? TEN_NHAN_VIEN { get; set; }
    public decimal? STT { get; set; }
    public decimal? DOAN { get; set; }
    public string? MA_XN { get; set; }
}

public sealed class AppExDssdEntity
{
    public string? MA_SO_DOC { get; set; }
    public string? VUNG { get; set; }
    public string? DOAN { get; set; }
    public string? MA_NHAN_VIEN { get; set; }
    public string? HOAN_THANH { get; set; }
    public string? DIS { get; set; }
    public DateTime? NGAY { get; set; }
    public string? MA_SO_DOC_CU { get; set; }
    public string? MA_NHAN_VIEN_CU { get; set; }
    public string? GHI_CHU { get; set; }
}

public sealed class AppExDssdSubEntity
{
    public string? MA_SO_DOC { get; set; }
    public string? VUNG { get; set; }
    public string? DOAN { get; set; }
    public string? MA_NHAN_VIEN { get; set; }
    public string? HOAN_THANH { get; set; }
    public string? DIS { get; set; }
    public DateTime? NGAY { get; set; }
    public string? MA_SO_DOC_CU { get; set; }
    public string? MA_NHAN_VIEN_CU { get; set; }
    public string? GHI_CHU { get; set; }
}

public sealed class AppExKiemTraGiaEntity
{
    public string? MA_KHACH_HANG { get; set; }
    public string? TEN_KHACH_HANG { get; set; }
    public string? DIA_CHI_DONG_HO { get; set; }
    public string? MA_SO_DOC { get; set; }
    public decimal? STT_SO_DOC { get; set; }
    public string? MA_XI_NGHIEP { get; set; }
    public string? TINH_TRANG_DH { get; set; }
    public string? T01_MA_GIA { get; set; }
    public decimal? T01_SAN_LUONG { get; set; }
    public string? T02_MA_GIA { get; set; }
    public decimal? T02_SAN_LUONG { get; set; }
    public string? T03_MA_GIA { get; set; }
    public decimal? T03_SAN_LUONG { get; set; }
    public string? SO_DIEN_THOAI { get; set; }
    public string? MA_BIEN_DOC { get; set; }
    public string? T01_THANG { get; set; }
    public string? T02_THANG { get; set; }
    public string? T03_THANG { get; set; }
    public string? T01_TEN_GIA { get; set; }
    public string? T02_TEN_GIA { get; set; }
    public string? T03_TEN_GIA { get; set; }
    public string? U_NGUOI_DUNG { get; set; }
    public DateTime? U_LOG_DATE { get; set; }
    public string? U_VI_TRI_DOC { get; set; }
    public string? U_SH { get; set; }
    public string? U_KD { get; set; }
    public string? U_SX { get; set; }
    public string? U_HCSN { get; set; }
    public string? U_MA_GHI_CHU { get; set; }
    public string? U_GHI_CHU_THEM { get; set; }
    public string? U_TEN_FILE_ANH { get; set; }
    public string? ID_AP_GIA { get; set; }
    public decimal? SO_HO { get; set; }
    public decimal? SO_KHAU { get; set; }
    public decimal? DINH_MUC { get; set; }
    public string? TEMP { get; set; }
    public string? SO_SERIAL_DONG_HO { get; set; }
    public decimal? CHI_SO_CUOI { get; set; }
    public string? MA_GIA { get; set; }
    public DateTime? NGAY_GIAO { get; set; }
}

public sealed class AppExKiemTraGiaSubEntity
{
    public string? MA_KHACH_HANG { get; set; }
    public string? TEN_KHACH_HANG { get; set; }
    public string? DIA_CHI_DONG_HO { get; set; }
    public string? MA_SO_DOC { get; set; }
    public decimal? STT_SO_DOC { get; set; }
    public string? MA_XI_NGHIEP { get; set; }
    public string? TINH_TRANG_DH { get; set; }
    public string? T01_MA_GIA { get; set; }
    public decimal? T01_SAN_LUONG { get; set; }
    public string? T02_MA_GIA { get; set; }
    public decimal? T02_SAN_LUONG { get; set; }
    public string? T03_MA_GIA { get; set; }
    public decimal? T03_SAN_LUONG { get; set; }
    public string? SO_DIEN_THOAI { get; set; }
    public string? MA_BIEN_DOC { get; set; }
    public string? T01_THANG { get; set; }
    public string? T02_THANG { get; set; }
    public string? T03_THANG { get; set; }
    public string? T01_TEN_GIA { get; set; }
    public string? T02_TEN_GIA { get; set; }
    public string? T03_TEN_GIA { get; set; }
    public string? U_NGUOI_DUNG { get; set; }
    public DateTime? U_LOG_DATE { get; set; }
    public string? U_VI_TRI_DOC { get; set; }
    public string? U_SH { get; set; }
    public string? U_KD { get; set; }
    public string? U_SX { get; set; }
    public string? U_HCSN { get; set; }
    public string? U_MA_GHI_CHU { get; set; }
    public string? U_GHI_CHU_THEM { get; set; }
    public string? U_TEN_FILE_ANH { get; set; }
    public string? ID_AP_GIA { get; set; }
    public decimal? SO_HO { get; set; }
    public decimal? SO_KHAU { get; set; }
    public decimal? DINH_MUC { get; set; }
    public string? TEMP { get; set; }
    public string? SO_SERIAL_DONG_HO { get; set; }
    public decimal? CHI_SO_CUOI { get; set; }
    public string? MA_GIA { get; set; }
    public DateTime? NGAY_GIAO { get; set; }
}

public sealed class AppExTsEntity
{
    public string? SO_DOC { get; set; }
}

public sealed class AppExUpdateEntity
{
    public string? BAN_CAP_NHAT { get; set; }
    public string? DUONG_DAN { get; set; }
    public DateTime? NGAY_CAP_NHAT { get; set; }
    public decimal? TG_CAP_NHAT_SL_TV { get; set; }
    public string? ACT_ID_DOC { get; set; }
}

public sealed class AppTc00LapMoiEntity : ITcWorkItem
{
    public string? ID_XAC_NHAN { get; set; }
    public string? MA_KHACH_HANG { get; set; }
    public string? LOG_USER { get; set; }
    public string? MA_NHAN_VIEN_NT { get; set; }
    public string? MA_XI_NGHIEP { get; set; }
    public DateTime? LOG_DATE_APP { get; set; }
    public string? VI_TRI_XAC_NHAN { get; set; }
    public string? GHI_CHU_THEM { get; set; }
    public string? MA_GHI_CHU { get; set; }
    public string? MA_SO_DOC { get; set; }
    public string? GHI_CHU_BILLING { get; set; }
    public string? NGUOI_DAI_DIEN_TC { get; set; }
    public string? KIEU_CAT_MO { get; set; }
    public decimal? STT_HS { get; set; }
    public decimal? CHI_SO_DH { get; set; }
    public string? VI_TRI_XAC_NHAN_CU { get; set; }
    public DateTime? NGAY_NIEM { get; set; }
    public string? SO_KIM_NIEM { get; set; }
    public decimal? CHI_SO_NIEM { get; set; }
    public decimal? CHI_SO_CAT_MO { get; set; }
    public string? ID_BILLING { get; set; }
    public string? MA_TTTC { get; set; }
    public DateTime? NGAY_1_XN_GIAO_NT { get; set; }
    public DateTime? NGAY_2_NT_NHAN_XN { get; set; }
    public DateTime? NGAY_3_NT_GIAO_CN { get; set; }
    public DateTime? NGAY_4_CN_NHAN_NT { get; set; }
    public DateTime? NGAY_5_CN_TC_OK { get; set; }
    public DateTime? NGAY_6_CN_GIAO_NT { get; set; }
    public DateTime? NGAY_7_NT_NHAN_CN { get; set; }
    public DateTime? NGAY_8_NT_GIAO_XN { get; set; }
    public DateTime? NGAY_9_XN_NHAN_NT { get; set; }
    public string? MA_BIEN_DOC { get; set; }
    public string? MA_NHAN_VIEN_CN { get; set; }
    public DateTime? NGAY_BILLING_GIAO { get; set; }
    public DateTime? NGAY_10_XN_BILLING { get; set; }
    public string? TEN_FILE_ANH_01 { get; set; }
    public string? TEN_FILE_ANH_02 { get; set; }
    public string? MA_KIM_NIEM { get; set; }
}

public sealed class AppTc01CaiTaoEntity : ITcWorkItem
{
    public string? ID_XAC_NHAN { get; set; }
    public string? MA_KHACH_HANG { get; set; }
    public string? LOG_USER { get; set; }
    public string? MA_NHAN_VIEN_NT { get; set; }
    public string? MA_XI_NGHIEP { get; set; }
    public DateTime? LOG_DATE_APP { get; set; }
    public string? VI_TRI_XAC_NHAN { get; set; }
    public string? GHI_CHU_THEM { get; set; }
    public string? MA_GHI_CHU { get; set; }
    public string? MA_SO_DOC { get; set; }
    public string? GHI_CHU_BILLING { get; set; }
    public string? NGUOI_DAI_DIEN_TC { get; set; }
    public string? KIEU_CAT_MO { get; set; }
    public decimal? STT_HS { get; set; }
    public decimal? CHI_SO_DH { get; set; }
    public string? VI_TRI_XAC_NHAN_CU { get; set; }
    public DateTime? NGAY_NIEM { get; set; }
    public string? SO_KIM_NIEM { get; set; }
    public decimal? CHI_SO_NIEM { get; set; }
    public decimal? CHI_SO_CAT_MO { get; set; }
    public string? ID_BILLING { get; set; }
    public string? MA_TTTC { get; set; }
    public DateTime? NGAY_1_XN_GIAO_NT { get; set; }
    public DateTime? NGAY_2_NT_NHAN_XN { get; set; }
    public DateTime? NGAY_3_NT_GIAO_CN { get; set; }
    public DateTime? NGAY_4_CN_NHAN_NT { get; set; }
    public DateTime? NGAY_5_CN_TC_OK { get; set; }
    public DateTime? NGAY_6_CN_GIAO_NT { get; set; }
    public DateTime? NGAY_7_NT_NHAN_CN { get; set; }
    public DateTime? NGAY_8_NT_GIAO_XN { get; set; }
    public DateTime? NGAY_9_XN_NHAN_NT { get; set; }
    public string? MA_BIEN_DOC { get; set; }
    public string? MA_NHAN_VIEN_CN { get; set; }
    public DateTime? NGAY_BILLING_GIAO { get; set; }
    public DateTime? NGAY_10_XN_BILLING { get; set; }
    public string? TEN_FILE_ANH_01 { get; set; }
    public string? TEN_FILE_ANH_02 { get; set; }
    public string? MA_KIM_NIEM { get; set; }
}

public sealed class AppTc02ThayDhEntity : ITcWorkItem
{
    public string? ID_XAC_NHAN { get; set; }
    public string? MA_KHACH_HANG { get; set; }
    public string? LOG_USER { get; set; }
    public string? MA_NHAN_VIEN_NT { get; set; }
    public string? MA_XI_NGHIEP { get; set; }
    public DateTime? LOG_DATE_APP { get; set; }
    public string? VI_TRI_XAC_NHAN { get; set; }
    public string? GHI_CHU_THEM { get; set; }
    public string? MA_GHI_CHU { get; set; }
    public string? MA_SO_DOC { get; set; }
    public string? GHI_CHU_BILLING { get; set; }
    public string? NGUOI_DAI_DIEN_TC { get; set; }
    public string? KIEU_CAT_MO { get; set; }
    public decimal? STT_HS { get; set; }
    public decimal? CHI_SO_DH { get; set; }
    public string? VI_TRI_XAC_NHAN_CU { get; set; }
    public DateTime? NGAY_NIEM { get; set; }
    public string? SO_KIM_NIEM { get; set; }
    public decimal? CHI_SO_NIEM { get; set; }
    public decimal? CHI_SO_CAT_MO { get; set; }
    public string? ID_BILLING { get; set; }
    public string? MA_TTTC { get; set; }
    public DateTime? NGAY_1_XN_GIAO_NT { get; set; }
    public DateTime? NGAY_2_NT_NHAN_XN { get; set; }
    public DateTime? NGAY_3_NT_GIAO_CN { get; set; }
    public DateTime? NGAY_4_CN_NHAN_NT { get; set; }
    public DateTime? NGAY_5_CN_TC_OK { get; set; }
    public DateTime? NGAY_6_CN_GIAO_NT { get; set; }
    public DateTime? NGAY_7_NT_NHAN_CN { get; set; }
    public DateTime? NGAY_8_NT_GIAO_XN { get; set; }
    public DateTime? NGAY_9_XN_NHAN_NT { get; set; }
    public string? MA_BIEN_DOC { get; set; }
    public string? MA_NHAN_VIEN_CN { get; set; }
    public DateTime? NGAY_BILLING_GIAO { get; set; }
    public DateTime? NGAY_10_XN_BILLING { get; set; }
    public string? TEN_FILE_ANH_01 { get; set; }
    public string? TEN_FILE_ANH_02 { get; set; }
    public string? MA_KIM_NIEM { get; set; }
}

public sealed class AppTc03CanDoEntity : ITcWorkItem
{
    public string? ID_XAC_NHAN { get; set; }
    public string? MA_KHACH_HANG { get; set; }
    public string? LOG_USER { get; set; }
    public string? MA_NHAN_VIEN_NT { get; set; }
    public string? MA_XI_NGHIEP { get; set; }
    public DateTime? LOG_DATE_APP { get; set; }
    public string? VI_TRI_XAC_NHAN { get; set; }
    public string? GHI_CHU_THEM { get; set; }
    public string? MA_GHI_CHU { get; set; }
    public string? MA_SO_DOC { get; set; }
    public string? GHI_CHU_BILLING { get; set; }
    public string? NGUOI_DAI_DIEN_TC { get; set; }
    public string? KIEU_CAT_MO { get; set; }
    public decimal? STT_HS { get; set; }
    public decimal? CHI_SO_DH { get; set; }
    public string? VI_TRI_XAC_NHAN_CU { get; set; }
    public DateTime? NGAY_NIEM { get; set; }
    public string? SO_KIM_NIEM { get; set; }
    public decimal? CHI_SO_NIEM { get; set; }
    public decimal? CHI_SO_CAT_MO { get; set; }
    public string? ID_BILLING { get; set; }
    public string? MA_TTTC { get; set; }
    public DateTime? NGAY_1_XN_GIAO_NT { get; set; }
    public DateTime? NGAY_2_NT_NHAN_XN { get; set; }
    public DateTime? NGAY_3_NT_GIAO_CN { get; set; }
    public DateTime? NGAY_4_CN_NHAN_NT { get; set; }
    public DateTime? NGAY_5_CN_TC_OK { get; set; }
    public DateTime? NGAY_6_CN_GIAO_NT { get; set; }
    public DateTime? NGAY_7_NT_NHAN_CN { get; set; }
    public DateTime? NGAY_8_NT_GIAO_XN { get; set; }
    public DateTime? NGAY_9_XN_NHAN_NT { get; set; }
    public string? MA_BIEN_DOC { get; set; }
    public string? MA_NHAN_VIEN_CN { get; set; }
    public DateTime? NGAY_BILLING_GIAO { get; set; }
    public DateTime? NGAY_10_XN_BILLING { get; set; }
    public string? TEN_FILE_ANH_01 { get; set; }
    public string? TEN_FILE_ANH_02 { get; set; }
    public string? MA_KIM_NIEM { get; set; }
}

public sealed class AppTc04SucSaEntity : ITcWorkItem
{
    public string? ID_XAC_NHAN { get; set; }
    public string? MA_KHACH_HANG { get; set; }
    public string? LOG_USER { get; set; }
    public string? MA_NHAN_VIEN_NT { get; set; }
    public string? MA_XI_NGHIEP { get; set; }
    public DateTime? LOG_DATE_APP { get; set; }
    public string? VI_TRI_XAC_NHAN { get; set; }
    public string? GHI_CHU_THEM { get; set; }
    public string? MA_GHI_CHU { get; set; }
    public string? MA_SO_DOC { get; set; }
    public string? GHI_CHU_BILLING { get; set; }
    public string? NGUOI_DAI_DIEN_TC { get; set; }
    public string? KIEU_CAT_MO { get; set; }
    public decimal? STT_HS { get; set; }
    public decimal? CHI_SO_DH { get; set; }
    public string? VI_TRI_XAC_NHAN_CU { get; set; }
    public DateTime? NGAY_NIEM { get; set; }
    public string? SO_KIM_NIEM { get; set; }
    public decimal? CHI_SO_NIEM { get; set; }
    public decimal? CHI_SO_CAT_MO { get; set; }
    public string? ID_BILLING { get; set; }
    public string? MA_TTTC { get; set; }
    public DateTime? NGAY_1_XN_GIAO_NT { get; set; }
    public DateTime? NGAY_2_NT_NHAN_XN { get; set; }
    public DateTime? NGAY_3_NT_GIAO_CN { get; set; }
    public DateTime? NGAY_4_CN_NHAN_NT { get; set; }
    public DateTime? NGAY_5_CN_TC_OK { get; set; }
    public DateTime? NGAY_6_CN_GIAO_NT { get; set; }
    public DateTime? NGAY_7_NT_NHAN_CN { get; set; }
    public DateTime? NGAY_8_NT_GIAO_XN { get; set; }
    public DateTime? NGAY_9_XN_NHAN_NT { get; set; }
    public string? MA_BIEN_DOC { get; set; }
    public string? MA_NHAN_VIEN_CN { get; set; }
    public DateTime? NGAY_BILLING_GIAO { get; set; }
    public DateTime? NGAY_10_XN_BILLING { get; set; }
    public string? TEN_FILE_ANH_01 { get; set; }
    public string? TEN_FILE_ANH_02 { get; set; }
    public string? MA_KIM_NIEM { get; set; }
}

public sealed class AppTc06OngBeEntity : ITcWorkItem
{
    public string? ID_XAC_NHAN { get; set; }
    public string? MA_KHACH_HANG { get; set; }
    public string? LOG_USER { get; set; }
    public string? MA_NHAN_VIEN_NT { get; set; }
    public string? MA_XI_NGHIEP { get; set; }
    public DateTime? LOG_DATE_APP { get; set; }
    public string? VI_TRI_XAC_NHAN { get; set; }
    public string? GHI_CHU_THEM { get; set; }
    public string? MA_GHI_CHU { get; set; }
    public string? MA_SO_DOC { get; set; }
    public string? GHI_CHU_BILLING { get; set; }
    public string? NGUOI_DAI_DIEN_TC { get; set; }
    public string? KIEU_CAT_MO { get; set; }
    public decimal? STT_HS { get; set; }
    public decimal? CHI_SO_DH { get; set; }
    public string? VI_TRI_XAC_NHAN_CU { get; set; }
    public DateTime? NGAY_NIEM { get; set; }
    public string? SO_KIM_NIEM { get; set; }
    public decimal? CHI_SO_NIEM { get; set; }
    public decimal? CHI_SO_CAT_MO { get; set; }
    public string? ID_BILLING { get; set; }
    public string? MA_TTTC { get; set; }
    public DateTime? NGAY_1_XN_GIAO_NT { get; set; }
    public DateTime? NGAY_2_NT_NHAN_XN { get; set; }
    public DateTime? NGAY_3_NT_GIAO_CN { get; set; }
    public DateTime? NGAY_4_CN_NHAN_NT { get; set; }
    public DateTime? NGAY_5_CN_TC_OK { get; set; }
    public DateTime? NGAY_6_CN_GIAO_NT { get; set; }
    public DateTime? NGAY_7_NT_NHAN_CN { get; set; }
    public DateTime? NGAY_8_NT_GIAO_XN { get; set; }
    public DateTime? NGAY_9_XN_NHAN_NT { get; set; }
    public string? MA_BIEN_DOC { get; set; }
    public string? MA_NHAN_VIEN_CN { get; set; }
    public DateTime? NGAY_BILLING_GIAO { get; set; }
    public DateTime? NGAY_10_XN_BILLING { get; set; }
    public string? TEN_FILE_ANH_01 { get; set; }
    public string? TEN_FILE_ANH_02 { get; set; }
    public string? MA_KIM_NIEM { get; set; }
}

public sealed class AppTc07CatNuocEntity : ITcWorkItem
{
    public string? ID_XAC_NHAN { get; set; }
    public string? MA_KHACH_HANG { get; set; }
    public string? LOG_USER { get; set; }
    public string? MA_NHAN_VIEN_NT { get; set; }
    public string? MA_XI_NGHIEP { get; set; }
    public DateTime? LOG_DATE_APP { get; set; }
    public string? VI_TRI_XAC_NHAN { get; set; }
    public string? GHI_CHU_THEM { get; set; }
    public string? MA_GHI_CHU { get; set; }
    public string? MA_SO_DOC { get; set; }
    public string? GHI_CHU_BILLING { get; set; }
    public string? NGUOI_DAI_DIEN_TC { get; set; }
    public string? KIEU_CAT_MO { get; set; }
    public decimal? STT_HS { get; set; }
    public decimal? CHI_SO_DH { get; set; }
    public string? VI_TRI_XAC_NHAN_CU { get; set; }
    public DateTime? NGAY_NIEM { get; set; }
    public string? SO_KIM_NIEM { get; set; }
    public decimal? CHI_SO_NIEM { get; set; }
    public decimal? CHI_SO_CAT_MO { get; set; }
    public string? ID_BILLING { get; set; }
    public string? MA_TTTC { get; set; }
    public DateTime? NGAY_1_XN_GIAO_NT { get; set; }
    public DateTime? NGAY_2_NT_NHAN_XN { get; set; }
    public DateTime? NGAY_3_NT_GIAO_CN { get; set; }
    public DateTime? NGAY_4_CN_NHAN_NT { get; set; }
    public DateTime? NGAY_5_CN_TC_OK { get; set; }
    public DateTime? NGAY_6_CN_GIAO_NT { get; set; }
    public DateTime? NGAY_7_NT_NHAN_CN { get; set; }
    public DateTime? NGAY_8_NT_GIAO_XN { get; set; }
    public DateTime? NGAY_9_XN_NHAN_NT { get; set; }
    public string? MA_BIEN_DOC { get; set; }
    public string? MA_NHAN_VIEN_CN { get; set; }
    public DateTime? NGAY_BILLING_GIAO { get; set; }
    public DateTime? NGAY_10_XN_BILLING { get; set; }
    public string? TEN_FILE_ANH_01 { get; set; }
    public string? TEN_FILE_ANH_02 { get; set; }
    public string? MA_KIM_NIEM { get; set; }
}

public sealed class AppTc08MoNuocEntity : ITcWorkItem
{
    public string? ID_XAC_NHAN { get; set; }
    public string? MA_KHACH_HANG { get; set; }
    public string? LOG_USER { get; set; }
    public string? MA_NHAN_VIEN_NT { get; set; }
    public string? MA_XI_NGHIEP { get; set; }
    public DateTime? LOG_DATE_APP { get; set; }
    public string? VI_TRI_XAC_NHAN { get; set; }
    public string? GHI_CHU_THEM { get; set; }
    public string? MA_GHI_CHU { get; set; }
    public string? MA_SO_DOC { get; set; }
    public string? GHI_CHU_BILLING { get; set; }
    public string? NGUOI_DAI_DIEN_TC { get; set; }
    public string? KIEU_CAT_MO { get; set; }
    public decimal? STT_HS { get; set; }
    public decimal? CHI_SO_DH { get; set; }
    public string? VI_TRI_XAC_NHAN_CU { get; set; }
    public DateTime? NGAY_NIEM { get; set; }
    public string? SO_KIM_NIEM { get; set; }
    public decimal? CHI_SO_NIEM { get; set; }
    public decimal? CHI_SO_CAT_MO { get; set; }
    public string? ID_BILLING { get; set; }
    public string? MA_TTTC { get; set; }
    public DateTime? NGAY_1_XN_GIAO_NT { get; set; }
    public DateTime? NGAY_2_NT_NHAN_XN { get; set; }
    public DateTime? NGAY_3_NT_GIAO_CN { get; set; }
    public DateTime? NGAY_4_CN_NHAN_NT { get; set; }
    public DateTime? NGAY_5_CN_TC_OK { get; set; }
    public DateTime? NGAY_6_CN_GIAO_NT { get; set; }
    public DateTime? NGAY_7_NT_NHAN_CN { get; set; }
    public DateTime? NGAY_8_NT_GIAO_XN { get; set; }
    public DateTime? NGAY_9_XN_NHAN_NT { get; set; }
    public string? MA_BIEN_DOC { get; set; }
    public string? MA_NHAN_VIEN_CN { get; set; }
    public DateTime? NGAY_BILLING_GIAO { get; set; }
    public DateTime? NGAY_10_XN_BILLING { get; set; }
    public string? TEN_FILE_ANH_01 { get; set; }
    public string? TEN_FILE_ANH_02 { get; set; }
    public string? MA_KIM_NIEM { get; set; }
}

public sealed class AppTc99NiemChiNhuaEntity
{
    public string? ID_NCN { get; set; }
    public string? MA_KHACH_HANG { get; set; }
    public DateTime? LOG_DATE { get; set; }
    public string? MA_QR_CHI_NIEM { get; set; }
    public string? LOG_USER { get; set; }
}

public sealed class Cc01TtkhYeuCauEntity
{
    public string? ID_YEU_CAU { get; set; }
    public string? CC_SO_DIEN_THOAI { get; set; }
    public string? MA_KHACH_HANG { get; set; }
    public string? TEN_KHACH_HANG { get; set; }
    public string? DIA_CHI_DONG_HO { get; set; }
    public string? CONG_VIEC_24G { get; set; }
    public decimal? KHACH_HANG_YC_LAN { get; set; }
    public decimal? NHAN_VIEN_YC_LAN { get; set; }
    public string? NGHE_GHI_AM { get; set; }
    public string? GHI_CHU { get; set; }
    public DateTime? NGAY_CAP_NHAT { get; set; }
    public DateTime? NGAY_SUA_LAI { get; set; }
    public string? LINK_GHI_AM { get; set; }
    public decimal? TT_UU_TIEN_CV { get; set; }
    public string? NGUOI_CAP_NHAT { get; set; }
    public string? KHACH_HANG_YC { get; set; }
    public string? NHAN_VIEN_YC { get; set; }
    public string? MA_NOI_DUNG_YC { get; set; }
    public string? MA_NHAN_VIEN_YC { get; set; }
    public string? XU_LY_MA_BO_PHAN { get; set; }
    public string? XU_LY_MA_TRANG_THAI { get; set; }
    public string? MA_DC { get; set; }
    public string? MA_XI_NGHIEP { get; set; }
    public string? CC_CALL_ID_IN { get; set; }
    public string? CC_CALL_ID_OUT { get; set; }
    public string? DG_NGUOI_CAP_NHAT { get; set; }
    public DateTime? DG_NGAY_CAP_NHAT { get; set; }
    public string? DG_MA_DANH_GIA { get; set; }
    public string? DG_GHI_CHU_KQ { get; set; }
    public string? TEN_KHACH_HANG_SUB { get; set; }
    public DateTime? TC_NGAY_GIAO_NT { get; set; }
    public string? TC_MA_NHOM_TRUONG { get; set; }
    public DateTime? TC_NGAY_GIAO_CN { get; set; }
    public string? TC_MA_CONG_NHAN_01 { get; set; }
    public string? TC_MA_CONG_NHAN_02 { get; set; }
    public string? MA_QUAN { get; set; }
    public string? MA_PHUONG { get; set; }
    public DateTime? TC_NGAY_HOAN_THANH { get; set; }
    public DateTime? CC_CALL_IN_TGBD { get; set; }
    public DateTime? CC_CALL_IN_TGKT { get; set; }
    public DateTime? CC_CALL_OUT_TGBD { get; set; }
    public DateTime? CC_CALL_OUT_TGKT { get; set; }
    public DateTime? CC_RING_IN_TGBD { get; set; }
    public string? TC_SO_CONG_TRINH { get; set; }
    public string? GHI_CHU_CC { get; set; }
    public string? DG_LINK_GHI_AM { get; set; }
    public string? TC_GHI_CHU { get; set; }
    public string? CC_EMAIL { get; set; }
    public string? TEN_FILE_ANH { get; set; }
    public string? VI_TRI { get; set; }
    public string? ID_DANG_KY { get; set; }
    public string? LOAI_NGUYEN_NHAN { get; set; }
    public string? MA_NGUON_TT { get; set; }
}

public sealed class CcDmNoiDungYcEntity
{
    public string? ID_NOI_DUNG { get; set; }
    public string? NOI_DUNG_YC { get; set; }
    public string? GHI_CHU { get; set; }
    public decimal? STT { get; set; }
    public string? NHOM_YC { get; set; }
    public string? MA_NHOM_YC { get; set; }
    public string? NGUOI_TAO { get; set; }
    public DateTime? NGAY_TAO { get; set; }
    public string? MA_BP { get; set; }
    public string? NGUOI_SUA { get; set; }
    public DateTime? NGAY_SUA { get; set; }
    public string? NOI_DUNG_YC_SUB { get; set; }
    public string? APP_DH { get; set; }
    public string? ACT { get; set; }
    public string? DANH_GIA_KPI { get; set; }
    public string? NOI_DUNG_YC_CU { get; set; }
}

public sealed class CcMangLuoiEntity
{
    public string? ID_ML { get; set; }
    public string? TIEU_DE_TB { get; set; }
    public string? NOI_DUNG_TB { get; set; }
    public string? KQ_SU_CO { get; set; }
    public DateTime? TG_SU_CO_TU { get; set; }
    public DateTime? TG_SU_CO_DEN { get; set; }
    public string? PHAM_VI { get; set; }
    public decimal? STT { get; set; }
    public string? NGUOI_CAP_NHAT { get; set; }
    public DateTime? NGAY_CAP_NHAT { get; set; }
    public string? KQ_SU_CO_MA { get; set; }
    public string? TIEU_DE_TB_MA { get; set; }
    public DateTime? NGAY_CAP_NHAT_LAI { get; set; }
    public string? THOI_GIAN_SU_CO { get; set; }
    public string? MA_XI_NGHIEP { get; set; }
    public string? TEN_XI_NGHIEP { get; set; }
}

public sealed class ChiSoDhEntity
{
    public string? MA_KHACH_HANG { get; set; }
    public decimal? CS_DAU { get; set; }
    public decimal? CS_CUOI { get; set; }
    public decimal? SAN_LUONG { get; set; }
    public DateTime? NGAY_DOC { get; set; }
    public DateTime? NGAY_DINH_KY { get; set; }
    public string? KH_ID { get; set; }
    public string? THANG { get; set; }
    public string? SO_HOA_DON { get; set; }
    public string? LOAI_CHI_SO { get; set; }
    public string? ID_CS_DAU { get; set; }
    public string? ID_CS_CUOI { get; set; }
}

public sealed class ChiSoDhSubEntity
{
    public string? ID_CS { get; set; }
    public decimal? SLTB3T { get; set; }
    public DateTime? NGAY_DINH_KY { get; set; }
    public string? MA_TINH_TRANG_DH { get; set; }
    public string? MA_KHACH_HANG { get; set; }
    public string? LOAI_CHI_SO { get; set; }
    public string? TINH_TRANG_CS { get; set; }
    public string? MA_DONG_HO { get; set; }
}

public sealed class CongNoEntity
{
    public string? MA_KHACH_HANG { get; set; }
    public string? SO_HOA_DON { get; set; }
    public DateTime? NGAY_NO { get; set; }
    public decimal? TONG_SL { get; set; }
    public decimal? TONG_TIEN { get; set; }
    public decimal? TONG_THANH_TOAN { get; set; }
    public DateTime? NGAY_THANH_TOAN { get; set; }
    public DateTime? NGAY_DOC { get; set; }
    public decimal? TRANG_THAI { get; set; }
    public decimal? MA_DON_VI { get; set; }
    public decimal? THUE { get; set; }
    public decimal? PHI { get; set; }
    public decimal? THANH_TIEN { get; set; }
    public string? THANG { get; set; }
    public decimal? LOAI_KHACH_HANG { get; set; }
    public decimal? STT_SO_DOC { get; set; }
    public DateTime? NGAY_THANH_TOAN_DT { get; set; }
    public decimal? TONG_SL_GIAM_TRU { get; set; }
    public decimal? TONG_TIEN_GIAM_TRU { get; set; }
    public string? BAN_GIAO { get; set; }
    public decimal? HINH_THUC_TT { get; set; }
    public decimal? LOAI_HOA_DON { get; set; }
    public DateTime? NGAY_DK_TU { get; set; }
    public DateTime? NGAY_DK_DEN { get; set; }
    public string? MA_SO_DOC { get; set; }
    public DateTime? NGAY_PHAT_HANH_DT { get; set; }
    public string? MA_CHI_NHANH { get; set; }
    public string? SERI_HOA_DON { get; set; }
    public string? DIEN_GIAI { get; set; }
    public DateTime? NGAY_HUY_PHAT_HANH_DT { get; set; }
    public DateTime? NGAY_HUY_THANH_TOAN_DT { get; set; }
    public decimal? KHOA { get; set; }
    public DateTime? NGAY_HD_PHAT_HANH { get; set; }
    public DateTime? NGAY_HD_HUY { get; set; }
    public DateTime? NGAY_CAP_SERIAL_DT { get; set; }
    public DateTime? NGAY_BAN_GIAO_TN { get; set; }
    public string? USR { get; set; }
    public decimal? TONG_THANH_TOAN_BILL { get; set; }
    public DateTime? NGAY_THANH_TOAN_BILL { get; set; }
    public string? MA_THU_NGAN { get; set; }
    public string? SO_HOA_DON_THAY_THE { get; set; }
    public string? KIEU_DIEU_CHINH { get; set; }
    public string? HINH_THUC_TT_BILL { get; set; }
    public string? SERI_MAU { get; set; }
    public string? MA_THU_NGAN_SO { get; set; }
    public string? MA_NGAN_HANG { get; set; }
    public string? SO_TAI_KHOAN { get; set; }
    public DateTime? NGAY_BAN_GIAO_QT { get; set; }
    public string? VDC_DA_TAO { get; set; }
    public string? EMAIL_PHAT_HANH { get; set; }
    public string? EMAIL_THANH_TOAN { get; set; }
    public string? USR_TT { get; set; }
    public DateTime? NGAY_THANH_TOAN_DT_TAM { get; set; }
    public string? EMAIL_PHAT_HANH_HUY { get; set; }
    public string? MA_NHO_THU { get; set; }
    public string? VDC_DA_TAO_TT { get; set; }
    public string? SMS_PHAT_HANH { get; set; }
    public string? SMS_THANH_TOAN { get; set; }
    public string? SMS_PHAT_HANH_HUY { get; set; }
    public decimal? PHI_NUOC_THAI { get; set; }
    public decimal? TONG_PP_TRUOC_THUE { get; set; }
    public decimal? TONG_PP_THUE_VAT { get; set; }
    public decimal? TONG_PP_SAN_LUONG { get; set; }
    public string? TH_TRANS_ID_BANK { get; set; }
    public string? SO_TAI_KHOAN_BILL { get; set; }
    public DateTime? TH_NGAY_TT_LEN_BILL { get; set; }
    public decimal? TH_TIEN_TT_OK_BILL_XUONG { get; set; }
    public decimal? TH_TIEN_NO_HIEN_TAI_BILL { get; set; }
    public string? TH_MA_TT_BILL_XUONG { get; set; }
    public string? TH_KET_QUA_TT_BILL_XUONG { get; set; }
    public DateTime? TH_NGAY_TT_LOG { get; set; }
    public string? TH_THU_TAM { get; set; }
    public string? MA_GIA { get; set; }
    public string? MA_TINH_TRANG_DH { get; set; }
    public string? DA_LUU_EBILSUB1 { get; set; }
    public string? DA_LUU_EBILSUB2 { get; set; }
    public string? DA_LUU_EBILSUB3 { get; set; }
    public string? DA_LUU_EBILSUB4 { get; set; }
    public decimal? TONG_TIEN_BASE { get; set; }
    public decimal? THANH_TIEN_BASE { get; set; }
    public decimal? THUE_BASE { get; set; }
    public decimal? PHI_BASE { get; set; }
    public decimal? TONG_SL_BASE { get; set; }
    public string? ZALO_PHAT_HANH { get; set; }
    public string? ZALO_THANH_TOAN { get; set; }
    public string? MA_BIEN_DOC { get; set; }
    public string? MA_TRA_CUU { get; set; }
}

public sealed class CongNoGiaEntity
{
    public string? MA_KHACH_HANG { get; set; }
    public string? SO_HOA_DON { get; set; }
    public DateTime? NGAY_NO { get; set; }
    public decimal? TONG_SL { get; set; }
    public decimal? TONG_TIEN { get; set; }
    public decimal? VAT { get; set; }
    public decimal? THUE_BVMT { get; set; }
    public string? MA_GIA_CB { get; set; }
    public double? TIEN_GIA_CB { get; set; }
    public string? TEN_GIA { get; set; }
    public double? THANH_TIEN { get; set; }
    public string? THANG { get; set; }
    public string? ID { get; set; }
    public string? HT_KD { get; set; }
    public string? MA_GIA { get; set; }
    public decimal? PTN_DON_GIA { get; set; }
    public string? MA_LOAI_PHI { get; set; }
    public decimal? PNT_SAN_LUONG { get; set; }
    public decimal? PBV_SAN_LUONG { get; set; }
    public DateTime? NGAY_DOC { get; set; }
    public decimal? PHU_PHI_TRUOC_THUE { get; set; }
    public decimal? PHU_PHI_THUE_VAT { get; set; }
    public decimal? PHU_PHI_TY_LE { get; set; }
    public decimal? TONG_SL_EX { get; set; }
    public string? GHI_CHU_DC { get; set; }
}

public sealed class Dab02DocChiSoDhEntity
{
    public string? ID_DAB { get; set; }
    public string? MA_BIEN_DOC { get; set; }
    public decimal? TONG_HS { get; set; }
    public decimal? CHUA_DOC { get; set; }
    public decimal? DA_DOC { get; set; }
    public string? MA_XI_NGHIEP { get; set; }
    public DateTime? NGAY_THUC_HIEN { get; set; }
    public DateTime? NGAY_DOC_DK { get; set; }
    public string? MA_XI_NGHIEP_CU { get; set; }
}

public sealed class Dm01DonViEntity
{
    public string? MA_DON_VI { get; set; }
    public string? TEN_DON_VI { get; set; }
    public string? DIEN_THOAI { get; set; }
    public string? TRUONG_DON_VI { get; set; }
    public string? TRUONG_CHUC_VU { get; set; }
    public string? DAI_DIEN_KY_HD { get; set; }
    public string? DAI_DIEN_CHUC_VU { get; set; }
    public string? DIA_CHI { get; set; }
    public string? TEN_VIET_TAT { get; set; }
    public string? FAX { get; set; }
    public string? KY_HIEU { get; set; }
    public string? SO_UY_QUYEN { get; set; }
    public DateTime? NGAY_UY_QUYEN { get; set; }
    public string? NGUOI_CAP_NHAT { get; set; }
    public DateTime? NGAY_CAP_NHAT { get; set; }
    public string? NGUOI_SUA { get; set; }
    public DateTime? NGAY_SUA { get; set; }
    public string? PARENT_MA { get; set; }
    public string? IP_NAS { get; set; }
    public string? KY_HIEU_HD { get; set; }
    public string? SO_TAI_KHOAN { get; set; }
    public string? CHU_TAI_KHOAN { get; set; }
    public string? TEN_DON_VI_MOI { get; set; }
    public decimal? NGAY_KHOA_DL_TINH_LUONG { get; set; }
}

public sealed class DmDiemThuTienEntity
{
    public string? MA_DIEM { get; set; }
    public string? NHOM_DON_VI_THU { get; set; }
    public string? TEN_DIEM_GD { get; set; }
    public string? DIA_CHI_GD { get; set; }
    public string? THOI_GIAN { get; set; }
    public string? CHUOI_SMS { get; set; }
    public decimal? MA_DON_VI_THU { get; set; }
    public string? PHUONG { get; set; }
    public string? QUAN { get; set; }
    public string? DIEN_THOAI { get; set; }
    public string? GIO { get; set; }
    public string? N01 { get; set; }
    public string? N02 { get; set; }
    public string? N03 { get; set; }
    public string? N04 { get; set; }
    public string? N05 { get; set; }
    public string? N06 { get; set; }
    public string? N07 { get; set; }
    public string? N08 { get; set; }
    public string? N09 { get; set; }
    public string? N10 { get; set; }
    public string? N11 { get; set; }
    public string? N12 { get; set; }
    public string? N13 { get; set; }
    public string? N14 { get; set; }
    public string? N15 { get; set; }
    public string? N16 { get; set; }
    public string? N17 { get; set; }
    public string? N18 { get; set; }
    public string? N19 { get; set; }
    public string? N20 { get; set; }
    public string? N21 { get; set; }
    public string? N22 { get; set; }
    public string? N23 { get; set; }
    public string? N24 { get; set; }
    public string? N25 { get; set; }
    public string? N26 { get; set; }
    public string? N27 { get; set; }
    public string? N28 { get; set; }
    public string? N29 { get; set; }
    public string? N30 { get; set; }
    public string? N31 { get; set; }
    public string? HIEU_LUC { get; set; }
    public string? CHUOI_SMS_TEMP { get; set; }
    public string? TOA_DO { get; set; }
    public string? BI_SUA { get; set; }
    public DateTime? LOG_DATE { get; set; }
    public string? VI_TRI_DIEM_THU { get; set; }
    public string? DIA_CHI_GOOGLE { get; set; }
}

public sealed class DmKimNiemEntity
{
    public string? MA_KIM { get; set; }
    public string? TEN_KIM { get; set; }
    public string? MA_CHI_NHANH { get; set; }
    public string? SO_KIEM { get; set; }
}

public sealed class DmLoaiSmsEmailEntity
{
    public string? MA_LOAI { get; set; }
    public string? TEN_LOAI { get; set; }
    public decimal? TT { get; set; }
    public string? SMS_CHUAN { get; set; }
}

public sealed class DmNhanVienEntity
{
    public string? MA_NHAN_VIEN { get; set; }
    public string? TEN_NHAN_VIEN { get; set; }
    public string? DIEN_THOAI { get; set; }
    public string? MA_CHI_NHANH { get; set; }
    public string? TEN_NHAN_VIEN_D { get; set; }
    public string? NHOM_TRUONG { get; set; }
    public string? THI_CONG { get; set; }
    public string? DOC_CHI_SO { get; set; }
    public string? HIEU_LUC { get; set; }
    public string? IMEI { get; set; }
    public string? USR { get; set; }
    public string? PAS { get; set; }
    public string? TOKEN { get; set; }
    public DateTime? LOG_DATE { get; set; }
    public string? IMEI_SUB { get; set; }
    public decimal? CANH_BAO_PT { get; set; }
    public decimal? CANH_BAO_M3 { get; set; }
    public decimal? DUNG_LUONG_PIN { get; set; }
    public decimal? DUNG_LUONG_4G { get; set; }
    public string? VER_CODE { get; set; }
    public decimal? CANH_BAO_GIAM_PT { get; set; }
    public decimal? CANH_BAO_GIAM_M3 { get; set; }
    public string? DIEN_THOAI_APP { get; set; }
    public string? VER_CODE_TC { get; set; }
    public string? IMEI_SUB2 { get; set; }
    public string? MA_SO_BHXH { get; set; }
    public string? LOAI_APP { get; set; }
    public string? NHOM_TRUONG_PARENT { get; set; }
    public string? QUYEN_WEBAPP { get; set; }
}

public sealed class DmSoDocEntity
{
    public string? MA_SO_DOC { get; set; }
    public string? TEN_SO_DOC { get; set; }
    public string? NGAY_DOC { get; set; }
    public string? MA_BIEN_DOC { get; set; }
    public string? MA_THU_NGAN { get; set; }
    public string? MA_CHI_NHANH { get; set; }
    public string? HIEU_LUC { get; set; }
    public string? NGAY_DOC_2KY { get; set; }
}

public sealed class DmTinhTrangDongHoEntity
{
    public string? MA_TINH_TRANG_SO { get; set; }
    public string? MO_TA { get; set; }
    public string? MO_TA_NGAN { get; set; }
    public string? N_DONG_HO_HONG { get; set; }
    public string? N_SUA_CHI_SO_CU { get; set; }
    public string? N_CONG_DON_CHI_SO { get; set; }
    public string? N_NHAP_SL_TRUC_TIEP { get; set; }
    public string? HIEU_LUC { get; set; }
    public decimal? STT_HIEN_THI { get; set; }
    public string? GIAI_PHAP { get; set; }
    public string? N_NHAP_CHI_SO_MOI { get; set; }
    public string? NGUOI_CAP_NHAT { get; set; }
    public DateTime? NGAY_CAP_NHAT { get; set; }
    public string? MO_TA_SUB { get; set; }
    public string? TAM_KHONG_TINH_HD { get; set; }
}

public sealed class DmTrangThaiEmailEntity
{
    public string? MA_LOAI { get; set; }
    public string? TEN_LOAI { get; set; }
    public decimal? TT { get; set; }
    public string? GHI_CHU { get; set; }
}

public sealed class KDmDiaChinhEntity
{
    public string? MA_DIA_CHINH { get; set; }
    public string? PARENT_MA { get; set; }
    public string? TEN_DIA_CHINH { get; set; }
    public string? MA_QUAN { get; set; }
    public string? TEN_QUAN { get; set; }
    public string? MA_PHUONG { get; set; }
    public string? TEN_PHUONG { get; set; }
    public string? MA_DON_VI { get; set; }
    public string? NGUOI_CAP_NHAT { get; set; }
    public DateTime? NGAY_CAP_NHAT { get; set; }
    public string? NGUOI_SUA { get; set; }
    public DateTime? NGAY_SUA { get; set; }
    public string? CAP_DIA_CHINH { get; set; }
    public string? KY_HIEU { get; set; }
    public string? KY_HIEU_SUB { get; set; }
    public string? PARENT_MA_SUB { get; set; }
    public string? IS_PHUONG_MOI { get; set; }
}

public sealed class KDmGiaEntity
{
    public string? MA_GIA { get; set; }
    public string? KY_HIEU_GIA { get; set; }
    public string? TEN_GIA { get; set; }
    public DateTime? NGAY_AP_DUNG { get; set; }
    public decimal? SO_NGAY_TINH_GIA { get; set; }
    public string? KIEU_TINH { get; set; }
    public string? GIA_CT_01 { get; set; }
    public string? GIA_CT_02 { get; set; }
    public string? GIA_CT_03 { get; set; }
    public string? GIA_CT_04 { get; set; }
    public string? GIA_CT_05 { get; set; }
    public string? GIA_CT_06 { get; set; }
    public decimal? GIA_GT_01 { get; set; }
    public decimal? GIA_GT_02 { get; set; }
    public decimal? GIA_GT_03 { get; set; }
    public decimal? GIA_GT_04 { get; set; }
    public decimal? GIA_GT_05 { get; set; }
    public decimal? GIA_GT_06 { get; set; }
    public string? LOAI_KHACH_HANG { get; set; }
    public string? CHUOI_GIA_GT { get; set; }
    public string? CHUOI_GIA_DM { get; set; }
    public string? HIEU_LUC { get; set; }
    public string? KIEU_GIA { get; set; }
    public decimal? GIA_CO_BAN { get; set; }
    public string? NGUOI_CAP_NHAT { get; set; }
    public DateTime? NGAY_CAP_NHAT { get; set; }
    public string? NGUOI_SUA { get; set; }
    public DateTime? NGAY_SUA { get; set; }
    public string? MA_MUC_DICH_SD { get; set; }
    public string? MA_THUE_VAT { get; set; }
    public string? MA_PHI_BVMT { get; set; }
}

public sealed class KDmGiaPhiEntity
{
    public string? MA_PHI { get; set; }
    public string? LOAI_PHI { get; set; }
    public string? KIEU_PHI { get; set; }
    public string? TEN_PHI { get; set; }
    public decimal? GIA_TRI { get; set; }
    public string? NGUOI_CAP_NHAT { get; set; }
    public DateTime? NGAY_CAP_NHAT { get; set; }
    public string? NGUOI_SUA { get; set; }
    public DateTime? NGAY_SUA { get; set; }
}

public sealed class KDmGiaSubEntity
{
    public string? MA_GIA { get; set; }
    public string? TEN_GIA { get; set; }
    public string? TEN_GIA_SUB { get; set; }
    public string? TEN_GIA_ONLY { get; set; }
    public string? LOAI { get; set; }
}

public sealed class LogBankDonViEntity
{
    public decimal? MA { get; set; }
    public string? TEN { get; set; }
    public decimal? CHA { get; set; }
    public string? DIA_CHI { get; set; }
    public string? DIEN_THOAI { get; set; }
    public string? TEN_SUB { get; set; }
    public decimal? ID_SXEP { get; set; }
    public decimal? MA_GOP { get; set; }
    public string? TEN_GOP { get; set; }
    public string? IP_DON_VI { get; set; }
    public string? MAT_KHAU { get; set; }
    public string? LOAI_DON_VI { get; set; }
    public string? NGUOI_DUNG { get; set; }
    public string? TEN_BILL { get; set; }
    public string? IP_DON_VI_SUB { get; set; }
    public string? ACT { get; set; }
    public string? IS_MAC_ADD { get; set; }
    public string? ME_VIEW { get; set; }
    public string? IS_DIEM_THU_TT { get; set; }
    public string? TEN_VIET_TAT { get; set; }
    public string? MA_NH_BILLING { get; set; }
    public string? PATH_DOI_SOAT { get; set; }
    public string? BO_LOG { get; set; }
    public decimal? SLHD_ONLINE { get; set; }
    public DateTime? SLHD_ONLINE_DATE { get; set; }
    public decimal? SLHD_ONLINE_TIEN { get; set; }
    public string? BO_LOG_FILE { get; set; }
    public string? SU_DUNG_FILE_ACCESS { get; set; }
    public string? DOI_SOAT_FTP_GOP { get; set; }
}

public sealed class LogBankFixEntity
{
    public string? DON_VI { get; set; }
    public string? TRANG_THAI { get; set; }
    public string? GHI_CHU { get; set; }
    public string? ERR { get; set; }
    public string? INFOCUST_CHECK { get; set; }
    public string? INFOCUST_CHECK_DETAIL { get; set; }
    public string? DEBT_CHECK { get; set; }
    public string? DEBT_CHECK_DETAIL { get; set; }
    public string? DEBT_PAYMENT { get; set; }
    public string? DEBT_PAYMENT_DETAIL { get; set; }
    public string? DEBT_CANCEL { get; set; }
    public string? DEBT_CANCEL_DETAIL { get; set; }
    public string? SYSTEM_CHECK { get; set; }
    public string? SYSTEM_CHECK_DETAIL { get; set; }
    public decimal? INFOCUST_CHECK_TOUT { get; set; }
    public decimal? DEBT_CHECK_TOUT { get; set; }
    public decimal? DEBT_PAYMENT_TOUT { get; set; }
    public decimal? DEBT_CANCEL_TOUT { get; set; }
    public decimal? SYSTEM_CHECK_TOUT { get; set; }
    public decimal? TOUT_ITEM_4 { get; set; }
    public string? TEMPLATE { get; set; }
    public string? DEBT_CHECK_V2 { get; set; }
    public string? DEBT_CHECK_V2_DETAIL { get; set; }
    public string? DEBT_PAYMENT_V2 { get; set; }
    public string? DEBT_PAYMENT_V2_DETAIL { get; set; }
    public decimal? DEBT_CHECK_V2_TOUT { get; set; }
    public decimal? DEBT_PAYMENT_V2_TOUT { get; set; }
}

public sealed class LogBankTienMoNuocEntity
{
    public decimal? SO_TIEN { get; set; }
    public DateTime? NGAY_TAO { get; set; }
    public string? KIEU_CAT { get; set; }
    public string? GHI_CHU { get; set; }
}

public sealed class LogEmailEntity
{
    public string? EMAIL { get; set; }
    public DateTime? NGAY_GUI { get; set; }
    public string? MA_KHACH_HANG { get; set; }
    public string? SO_HOA_DON { get; set; }
    public string? LOAI_EMAIL { get; set; }
    public string? KET_QUA { get; set; }
    public string? LOG_EMAIL_ID { get; set; }
    public DateTime? NGAY_PHHD { get; set; }
    public string? USR { get; set; }
    public string? MA_MAU { get; set; }
    public string? DA_LUU_EBILSUB { get; set; }
}

public sealed class PasswordEntity
{
    public string? USR { get; set; }
    public string? PAS { get; set; }
    public string? TEN_DON_VI { get; set; }
    public string? HO_TEN { get; set; }
    public string? NGAY_TAO { get; set; }
    public string? MENU { get; set; }
    public string? NHOM { get; set; }
    public decimal? TMR_MOVEFILE { get; set; }
    public string? TEN_CONG_TY { get; set; }
    public string? MAC_ERR { get; set; }
    public string? MAC_USR { get; set; }
    public string? TB { get; set; }
    public string? USR_BILLING { get; set; }
    public string? PAS_BILLING { get; set; }
    public string? IP_BILLING { get; set; }
    public string? MA_DON_VI { get; set; }
    public string? CHI_DOC { get; set; }
    public string? DON_VI_LOG { get; set; }
    public string? UPD { get; set; }
    public DateTime? NGAY_UPDATE { get; set; }
    public string? CB_CAT_NUOC { get; set; }
    public decimal? CB_THOI_GIAN_LAN { get; set; }
    public string? TMR_TRANGTHAI { get; set; }
    public string? TMR_ACCSESS { get; set; }
    public decimal? TMR { get; set; }
    public string? IP_ENABLE { get; set; }
    public string? IP_ACCSESS_01 { get; set; }
    public string? IP_ACCSESS_02 { get; set; }
    public string? PARENT_MA_DON_VI { get; set; }
}

public sealed class ThongTinKhEntity
{
    public string? MA_KHACH_HANG { get; set; }
    public string? SO_HOP_DONG { get; set; }
    public string? LOAI_KHACH_HANG { get; set; }
    public string? TEN_KHACH_HANG { get; set; }
    public string? DIA_CHI_KHACH_HANG { get; set; }
    public string? DIA_CHI_DONG_HO { get; set; }
    public string? DIEN_THOAI { get; set; }
    public decimal? SO_HO { get; set; }
    public decimal? SO_KHAU { get; set; }
    public decimal? DINH_MUC { get; set; }
    public string? MA_GIA { get; set; }
    public string? TEN_GIA_NUOC { get; set; }
    public string? CONG_THUC_GIA { get; set; }
    public string? CHUOI_GIA { get; set; }
    public string? MA_DONG_HO { get; set; }
    public string? TEN_DONG_HO { get; set; }
    public string? SO_SERIAL_DONG_HO { get; set; }
    public string? BIEN_DOC { get; set; }
    public string? THU_NGAN { get; set; }
    public DateTime? NGAY_LAP_DAT { get; set; }
    public string? VUNG { get; set; }
    public string? MA_SO_DOC { get; set; }
    public string? TEN_SO_DOC { get; set; }
    public string? HT_KD { get; set; }
    public string? TEN_DUONG { get; set; }
    public string? TEN_PHUONG { get; set; }
    public string? TEN_QUAN { get; set; }
    public string? MA_DC { get; set; }
    public string? CHI_NHANH { get; set; }
    public string? MA_CHI_NHANH { get; set; }
    public string? HOP_THU { get; set; }
    public string? MA_SO_THUE { get; set; }
    public string? SO_TAI_KHOAN { get; set; }
    public DateTime? NGAY_THANH_LY_HD { get; set; }
    public string? DIEN_THOAI_DD { get; set; }
    public string? MAT_KHAU_MD5 { get; set; }
    public string? MAT_KHAU_BAN_DAU { get; set; }
    public string? MA_NGAN_HANG { get; set; }
    public string? EMAIL_KH_MOI { get; set; }
    public string? MA_NHO_THU { get; set; }
    public decimal? CHI_SO_THAO_LAP { get; set; }
    public string? CO_DONG_HO { get; set; }
    public string? SMS { get; set; }
    public string? TEN_MANG { get; set; }
    public string? EMAIL_THONG_BAO { get; set; }
    public string? PHONE_UT1 { get; set; }
    public string? PHONE_UT2 { get; set; }
    public string? EMAIL_UT1 { get; set; }
    public string? EMAIL_UT2 { get; set; }
    public string? MA_NHOM_KH_MES { get; set; }
    public decimal? STT_SO_DOC { get; set; }
    public string? MA_DIEM_THU { get; set; }
    public string? TEN_DANG_NHAP { get; set; }
    public string? MUC_DICH_GIA { get; set; }
    public string? MA_KHACH_HANG_PARENT { get; set; }
    public DateTime? NGAY_HOP_DONG { get; set; }
    public string? PHONE_UT1_BK { get; set; }
    public string? EMAIL_UT1_BK { get; set; }
    public string? PHONE_UT1_BILL_BK { get; set; }
    public string? EMAIL_UT1_BILL_BK { get; set; }
    public string? TEN_MANG_CHUYEN { get; set; }
    public string? MA_KHACH_HANG_PR { get; set; }
    public string? MA_BIEN_DOC { get; set; }
    public string? MA_TINH_TRANG_DH { get; set; }
    public string? NGAY_DOC_SO { get; set; }
    public string? SO_O_CUA_SO { get; set; }
    public string? KIEU_HOP_DONG { get; set; }
    public string? DANG_KY_ZALO { get; set; }
    public DateTime? NGAY_HOP_DONG_GOC { get; set; }
    public string? CCCD_HO_CHIEU { get; set; }
    public string? MA_QHNS_KH { get; set; }
    public string? MA_CHI_NHANH_CU { get; set; }
    public string? VUNG_CU { get; set; }
    public string? MA_DC_CU { get; set; }
    public string? TEN_PHUONG_CU { get; set; }
    public string? CHI_NHANH_CU { get; set; }
}
