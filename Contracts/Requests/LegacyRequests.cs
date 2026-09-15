using System.Text.Json.Serialization;

namespace ReadMeter.Api.Contracts.Requests;

public sealed record TC_011_NHOM_TRUONG_DSHS_NHAN_XNRequest(
    [property: JsonPropertyName("KIEU_HO_SO")] string? KIEU_HO_SO,
    [property: JsonPropertyName("MA_NHOM_TRUONG")] string? MA_NHOM_TRUONG,
    [property: JsonPropertyName("TU_NGAY")] string? TU_NGAY,
    [property: JsonPropertyName("DEN_NGAY")] string? DEN_NGAY,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_012_NHOM_TRUONG_NHAN_HOSORequest(
    [property: JsonPropertyName("KIEU_HO_SO")] string? KIEU_HO_SO,
    [property: JsonPropertyName("MA_NHOM_TRUONG")] string? MA_NHOM_TRUONG,
    [property: JsonPropertyName("ID_HO_SO")] string? ID_HO_SO,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_0123_NHOM_TRUONG_DSHS_GIAO_CNRequest(
    [property: JsonPropertyName("KIEU_HO_SO")] string? KIEU_HO_SO,
    [property: JsonPropertyName("MA_NHOM_TRUONG")] string? MA_NHOM_TRUONG,
    [property: JsonPropertyName("TU_NGAY")] string? TU_NGAY,
    [property: JsonPropertyName("DEN_NGAY")] string? DEN_NGAY,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_0122_NHOM_TRUONG_BO_NHAN_HSXNRequest(
    [property: JsonPropertyName("KIEU_HO_SO")] string? KIEU_HO_SO,
    [property: JsonPropertyName("ID_HO_SO")] string? ID_HO_SO,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_013_NHOM_TRUONG_GIAO_NVRequest(
    [property: JsonPropertyName("KIEU_HO_SO")] string? KIEU_HO_SO,
    [property: JsonPropertyName("MA_CONG_NHAN")] string? MA_CONG_NHAN,
    [property: JsonPropertyName("ID_HO_SO")] string? ID_HO_SO,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_014_CONG_NHAN_DSHS_NHAN_NTRequest(
    [property: JsonPropertyName("KIEU_HO_SO")] string? KIEU_HO_SO,
    [property: JsonPropertyName("MA_CONG_NHAN")] string? MA_CONG_NHAN,
    [property: JsonPropertyName("TU_NGAY")] string? TU_NGAY,
    [property: JsonPropertyName("DEN_NGAY")] string? DEN_NGAY,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_0151_CONG_NHAN_DSHS_NHAN_TCRequest(
    [property: JsonPropertyName("KIEU_HO_SO")] string? KIEU_HO_SO,
    [property: JsonPropertyName("MA_CONG_NHAN")] string? MA_CONG_NHAN,
    [property: JsonPropertyName("TU_NGAY")] string? TU_NGAY,
    [property: JsonPropertyName("DEN_NGAY")] string? DEN_NGAY,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_0152_CONG_NHAN_BO_NHAN_HSNTRequest(
    [property: JsonPropertyName("KIEU_HO_SO")] string? KIEU_HO_SO,
    [property: JsonPropertyName("ID_HO_SO")] string? ID_HO_SO,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_015_CONG_NHAN_NHAN_HOSORequest(
    [property: JsonPropertyName("KIEU_HO_SO")] string? KIEU_HO_SO,
    [property: JsonPropertyName("ID_HO_SO")] string? ID_HO_SO,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_016_CONG_NHAN_THI_CONG_XONGRequest(
    [property: JsonPropertyName("KIEU_HO_SO")] string? KIEU_HO_SO,
    [property: JsonPropertyName("MA_CONG_NHAN")] string? MA_CONG_NHAN,
    [property: JsonPropertyName("ID_HO_SO")] string? ID_HO_SO,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_0171_CONG_NHAN_LAY_DSHS_LAN2Request(
    [property: JsonPropertyName("KIEU_HO_SO")] string? KIEU_HO_SO,
    [property: JsonPropertyName("MA_CONG_NHAN")] string? MA_CONG_NHAN,
    [property: JsonPropertyName("TU_NGAY")] string? TU_NGAY,
    [property: JsonPropertyName("DEN_NGAY")] string? DEN_NGAY,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_0172_CONG_NHAN_GIAO_NTRequest(
    [property: JsonPropertyName("KIEU_HO_SO")] string? KIEU_HO_SO,
    [property: JsonPropertyName("ID_HO_SO")] string? ID_HO_SO,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_0181_NHOM_TRUONG_DSHS_NHAN_CNRequest(
    [property: JsonPropertyName("KIEU_HO_SO")] string? KIEU_HO_SO,
    [property: JsonPropertyName("MA_NHOM_TRUONG")] string? MA_NHOM_TRUONG,
    [property: JsonPropertyName("TU_NGAY")] string? TU_NGAY,
    [property: JsonPropertyName("DEN_NGAY")] string? DEN_NGAY,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_0183_NHOM_TRUONG_BO_NHAN_HSCNRequest(
    [property: JsonPropertyName("KIEU_HO_SO")] string? KIEU_HO_SO,
    [property: JsonPropertyName("ID_HO_SO")] string? ID_HO_SO,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_0182_NHOM_TRUONG_NHAN_HSRequest(
    [property: JsonPropertyName("KIEU_HO_SO")] string? KIEU_HO_SO,
    [property: JsonPropertyName("ID_HO_SO")] string? ID_HO_SO,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_0191_NHOM_TRUONG_DSHS_GIAO_XNRequest(
    [property: JsonPropertyName("KIEU_HO_SO")] string? KIEU_HO_SO,
    [property: JsonPropertyName("MA_NHOM_TRUONG")] string? MA_NHOM_TRUONG,
    [property: JsonPropertyName("TU_NGAY")] string? TU_NGAY,
    [property: JsonPropertyName("DEN_NGAY")] string? DEN_NGAY,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_0192_NHOM_TRUONG_GIAO_XNRequest(
    [property: JsonPropertyName("KIEU_HO_SO")] string? KIEU_HO_SO,
    [property: JsonPropertyName("ID_HO_SO")] string? ID_HO_SO,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_0193_NHOM_TRUONG_LAY_DSHS_LANCUOIRequest(
    [property: JsonPropertyName("KIEU_HO_SO")] string? KIEU_HO_SO,
    [property: JsonPropertyName("MA_NHOM_TRUONG")] string? MA_NHOM_TRUONG,
    [property: JsonPropertyName("TU_NGAY")] string? TU_NGAY,
    [property: JsonPropertyName("DEN_NGAY")] string? DEN_NGAY,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_99_LAY_TRANG_THAI_HSRequest(
    [property: JsonPropertyName("KIEU_HO_SO")] string? KIEU_HO_SO,
    [property: JsonPropertyName("ID_HO_SO")] string? ID_HO_SO,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_991_NIEM_CHI_NHUA_KIEM_TRARequest(
    [property: JsonPropertyName("MA_QR_CHI_NIEM")] string? MA_QR_CHI_NIEM,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_993_DANG_NHAPRequest(
    [property: JsonPropertyName("USER")] string? USER,
    [property: JsonPropertyName("PASSWORD")] string? PASSWORD,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_994_DM_NHAN_VIENRequest(
    [property: JsonPropertyName("MA_NHOM_TRUONG")] string? MA_NHOM_TRUONG,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record TC_992_NIEM_CHI_NHUA_THEMRequest(
    [property: JsonPropertyName("MA_QR_CHI_NIEM")] string? MA_QR_CHI_NIEM,
    [property: JsonPropertyName("MA_KHACH_HANG")] string? MA_KHACH_HANG,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("USR_LOG")] string? USR_LOG,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_00_KET_NOI_DB_CHECKRequest(
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_01_DANG_NHAPRequest(
    [property: JsonPropertyName("USER")] string? USER,
    [property: JsonPropertyName("PASSWORD")] string? PASSWORD,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record DANGNHAPTHEOMANLDRequest(
    [property: JsonPropertyName("ma_khach_hang")] string? MA_KHACH_HANG,
    [property: JsonPropertyName("pass")] string? PASS,
    [property: JsonPropertyName("so_dien_thoai")] string? SO_DIEN_THOAI);

public sealed record P_011_DANG_NHAP_DOI_MAT_KHAURequest(
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("PASSWORD_OLD")] string? PASSWORD_OLD,
    [property: JsonPropertyName("PASSWORD_NEW1")] string? PASSWORD_NEW1,
    [property: JsonPropertyName("PASSWORD_NEW2")] string? PASSWORD_NEW2,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_012_LAY_GT_CANH_BAORequest(
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_013_LAY_PHIEN_BAN_APPRequest(
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_013_LAY_PHIEN_BAN_APP_TCRequest(
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_01_DANG_NHAP_LUU_TOKENRequest(
    [property: JsonPropertyName("TOKEN")] string? TOKEN,
    [property: JsonPropertyName("VER_CODE")] string? VER_CODE,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_01_DANG_NHAP_LUU_TOKEN_TCRequest(
    [property: JsonPropertyName("TOKEN")] string? TOKEN,
    [property: JsonPropertyName("VER_CODE")] string? VER_CODE,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_021_LAY_DS_SO_DOCRequest(
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("THANG")] string? THANG,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_0211_LAY_DS_SO_DOC_BSRequest(
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("THANG")] string? THANG,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_0212_LAY_DS_SO_DOC_TRA_CUURequest(
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("THANG")] string? THANG,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_022_NHAN_SO_DOCRequest(
    [property: JsonPropertyName("DANH_SACH_MA_SO_DOC")] string? DANH_SACH_MA_SO_DOC,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("THANG")] string? THANG,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_03_LAY_DS_KHACH_HANGRequest(
    [property: JsonPropertyName("ID_DOC_opt")] string? ID_DOC_opt,
    [property: JsonPropertyName("STT_SO_DOC_opt")] string? STT_SO_DOC_opt,
    [property: JsonPropertyName("MA_KH_opt")] string? MA_KH_opt,
    [property: JsonPropertyName("TEN_KH_opt")] string? TEN_KH_opt,
    [property: JsonPropertyName("DIA_CHI_DH_opt")] string? DIA_CHI_DH_opt,
    [property: JsonPropertyName("PHONE_KH_opt")] string? PHONE_KH_opt,
    [property: JsonPropertyName("MA_SO_DOC")] string? MA_SO_DOC,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("THANG")] string? THANG,
    [property: JsonPropertyName("KIEU_LOC")] string? KIEU_LOC,
    [property: JsonPropertyName("SAP_XEP_THEO")] string? SAP_XEP_THEO,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_03_LAY_DS_KHACH_HANG_SUBRequest(
    [property: JsonPropertyName("ID_DOC_opt")] string? ID_DOC_opt,
    [property: JsonPropertyName("STT_SO_DOC_opt")] string? STT_SO_DOC_opt,
    [property: JsonPropertyName("MA_KH_opt")] string? MA_KH_opt,
    [property: JsonPropertyName("TEN_KH_opt")] string? TEN_KH_opt,
    [property: JsonPropertyName("DIA_CHI_DH_opt")] string? DIA_CHI_DH_opt,
    [property: JsonPropertyName("PHONE_KH_opt")] string? PHONE_KH_opt,
    [property: JsonPropertyName("MA_SO_DOC")] string? MA_SO_DOC,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("THANG")] string? THANG,
    [property: JsonPropertyName("KIEU_LOC")] string? KIEU_LOC,
    [property: JsonPropertyName("SAP_XEP_THEO")] string? SAP_XEP_THEO,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_03_LAY_DS_KHACH_HANG_SUB_BSRequest(
    [property: JsonPropertyName("ID_DOC_opt")] string? ID_DOC_opt,
    [property: JsonPropertyName("STT_SO_DOC_opt")] string? STT_SO_DOC_opt,
    [property: JsonPropertyName("MA_KH_opt")] string? MA_KH_opt,
    [property: JsonPropertyName("TEN_KH_opt")] string? TEN_KH_opt,
    [property: JsonPropertyName("DIA_CHI_DH_opt")] string? DIA_CHI_DH_opt,
    [property: JsonPropertyName("PHONE_KH_opt")] string? PHONE_KH_opt,
    [property: JsonPropertyName("MA_SO_DOC")] string? MA_SO_DOC,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("THANG")] string? THANG,
    [property: JsonPropertyName("KIEU_LOC")] string? KIEU_LOC,
    [property: JsonPropertyName("SAP_XEP_THEO")] string? SAP_XEP_THEO,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_0313_LAY_DS_KHACH_HANG_TRA_CUURequest(
    [property: JsonPropertyName("MA_SO_DOC")] string? MA_SO_DOC,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("THANG")] string? THANG,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_0314_LAY_DS_KHACH_HANG_XEP_SORequest(
    [property: JsonPropertyName("MA_SO_DOC")] string? MA_SO_DOC,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("THANG")] string? THANG,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_043_LO_TRINH_DI_DOC_MAPRequest(
    [property: JsonPropertyName("MA_SO_DOC")] string? MA_SO_DOC,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("THANG")] string? THANG,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_043_LO_TRINH_DI_DOC_MAP_SERVER_SQLRequest(
    [property: JsonPropertyName("SQL_STRING")] string? SQL_STRING,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K,
    [property: JsonPropertyName("ND")] string? ND,
    [property: JsonPropertyName("MK")] string? MK,
    [property: JsonPropertyName("IP_ACCESS")] string? IP_ACCESS);

public sealed record P_043_LO_TRINH_DI_DOC_MAP_SERVER_SQL_CCRequest(
    [property: JsonPropertyName("SQL_STRING")] string? SQL_STRING,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K,
    [property: JsonPropertyName("ND")] string? ND,
    [property: JsonPropertyName("MK")] string? MK,
    [property: JsonPropertyName("IP_ACCESS")] string? IP_ACCESS);

public sealed record P_043_LO_TRINH_DI_DOC_MAP_SERVERRequest(
    [property: JsonPropertyName("opt_MA_KHACH_HANG")] string? opt_MA_KHACH_HANG,
    [property: JsonPropertyName("opt_MA_SO_DOC")] string? opt_MA_SO_DOC,
    [property: JsonPropertyName("opt_NGAY_DOC")] string? opt_NGAY_DOC,
    [property: JsonPropertyName("opt_MA_BIEN_DOC")] string? opt_MA_BIEN_DOC,
    [property: JsonPropertyName("THANG")] string? THANG,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K,
    [property: JsonPropertyName("ND")] string? ND,
    [property: JsonPropertyName("MK")] string? MK,
    [property: JsonPropertyName("IP_ACCESS")] string? IP_ACCESS,
    [property: JsonPropertyName("hien_thi")] string? hien_thi);

public sealed record P_043_LO_TRINH_MAP_CAT_NUOCRequest(
    [property: JsonPropertyName("SQL_STRING")] string? SQL_STRING,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K,
    [property: JsonPropertyName("ND")] string? ND,
    [property: JsonPropertyName("MK")] string? MK,
    [property: JsonPropertyName("IP_ACCESS")] string? IP_ACCESS);

public sealed record P_043_LO_TRINH_MAP_MO_NUOCRequest(
    [property: JsonPropertyName("SQL_STRING")] string? SQL_STRING,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K,
    [property: JsonPropertyName("ND")] string? ND,
    [property: JsonPropertyName("MK")] string? MK,
    [property: JsonPropertyName("IP_ACCESS")] string? IP_ACCESS);

public sealed record P_043_LO_TRINH_MAP_GUI_GBRequest(
    [property: JsonPropertyName("SQL_STRING")] string? SQL_STRING,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K,
    [property: JsonPropertyName("ND")] string? ND,
    [property: JsonPropertyName("MK")] string? MK,
    [property: JsonPropertyName("IP_ACCESS")] string? IP_ACCESS);

public sealed record P_044_LUU_TEN_FILE_ANHRequest(
    [property: JsonPropertyName("ID_DONG_HO")] string? ID_DONG_HO,
    [property: JsonPropertyName("TEN_FILE_ANH")] string? TEN_FILE_ANH,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_044_LUU_PIN_4GRequest(
    [property: JsonPropertyName("DUNG_LUONG_PIN")] string? DUNG_LUONG_PIN,
    [property: JsonPropertyName("DUNG_LUONG_4G")] string? DUNG_LUONG_4G,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_031_LAY_CHI_SO_THANG_CURequest(
    [property: JsonPropertyName("ID_DOC")] string? ID_DOC,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_032_LAY_SL_BINH_QUAN_3TRequest(
    [property: JsonPropertyName("MA_KHACH_HANG")] string? MA_KHACH_HANG,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_041_NHAP_XUAT_CS_LE_ONLINERequest(
    [property: JsonPropertyName("ID_DONG_HO")] string? ID_DONG_HO,
    [property: JsonPropertyName("MA_TINH_TRANG_DH")] string? MA_TINH_TRANG_DH,
    [property: JsonPropertyName("QUA_VONG")] string? QUA_VONG,
    [property: JsonPropertyName("LOAI_CHI_SO")] string? LOAI_CHI_SO,
    [property: JsonPropertyName("NGAY_DOC_CS")] string? NGAY_DOC_CS,
    [property: JsonPropertyName("CHI_SO_MOI")] string? CHI_SO_MOI,
    [property: JsonPropertyName("SAN_LUONG_TT")] string? SAN_LUONG_TT,
    [property: JsonPropertyName("TONG_SL")] string? TONG_SL,
    [property: JsonPropertyName("CONG_CHI_SO")] string? CONG_CHI_SO,
    [property: JsonPropertyName("SAN_LUONG_DUNG_IT")] string? SAN_LUONG_DUNG_IT,
    [property: JsonPropertyName("MA_GHI_CHU")] string? MA_GHI_CHU,
    [property: JsonPropertyName("GHI_CHU")] string? GHI_CHU,
    [property: JsonPropertyName("VI_TRI_DOC")] string? VI_TRI_DOC,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_045_CANH_BAO_SAN_LUONG_LONRequest(
    [property: JsonPropertyName("ID_DONG_HO")] string? ID_DONG_HO,
    [property: JsonPropertyName("MA_KHACH_HANG")] string? MA_KHACH_HANG,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("TONG_SL")] long TONG_SL,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_045_LUU_XEP_SO_DOCRequest(
    [property: JsonPropertyName("ID_DONG_HO")] string? ID_DONG_HO,
    [property: JsonPropertyName("MA_KHACH_HANG")] string? MA_KHACH_HANG,
    [property: JsonPropertyName("STT_CU")] string? STT_CU,
    [property: JsonPropertyName("STT_MOI")] string? STT_MOI,
    [property: JsonPropertyName("MA_SO_DOC")] string? MA_SO_DOC,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("THANG")] string? THANG,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_041_NHAP_XUAT_CS_LE_ONLINE_SUBRequest(
    [property: JsonPropertyName("ID_DONG_HO")] string? ID_DONG_HO,
    [property: JsonPropertyName("MA_TINH_TRANG_DH")] string? MA_TINH_TRANG_DH,
    [property: JsonPropertyName("QUA_VONG")] string? QUA_VONG,
    [property: JsonPropertyName("LOAI_CHI_SO")] string? LOAI_CHI_SO,
    [property: JsonPropertyName("NGAY_DOC_CS")] string? NGAY_DOC_CS,
    [property: JsonPropertyName("CHI_SO_MOI")] string? CHI_SO_MOI,
    [property: JsonPropertyName("SAN_LUONG_TT")] string? SAN_LUONG_TT,
    [property: JsonPropertyName("TONG_SL")] string? TONG_SL,
    [property: JsonPropertyName("CONG_CHI_SO")] string? CONG_CHI_SO,
    [property: JsonPropertyName("SAN_LUONG_DUNG_IT")] string? SAN_LUONG_DUNG_IT,
    [property: JsonPropertyName("MA_GHI_CHU")] string? MA_GHI_CHU,
    [property: JsonPropertyName("GHI_CHU")] string? GHI_CHU,
    [property: JsonPropertyName("VI_TRI_DOC")] string? VI_TRI_DOC,
    [property: JsonPropertyName("MA_KHACH_HANG")] string? MA_KHACH_HANG,
    [property: JsonPropertyName("MA_SO_DOC")] string? MA_SO_DOC,
    [property: JsonPropertyName("THANG")] string? THANG,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_051_KIEM_TRA_BAN_GIAO_SDRequest(
    [property: JsonPropertyName("MA_SO_DOC")] string? MA_SO_DOC,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("THANG")] string? THANG,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_05_BD_BAN_GIAO_CS_XONGRequest(
    [property: JsonPropertyName("DANH_SACH_MA_SO_DOC")] string? DANH_SACH_MA_SO_DOC,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("THANG")] string? THANG,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_81_LAY_TT_KHACH_HANGRequest(
    [property: JsonPropertyName("MA_KHACH_HANG")] string? MA_KHACH_HANG,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_82_LAY_TT_HOA_DONRequest(
    [property: JsonPropertyName("MA_KHACH_HANG")] string? MA_KHACH_HANG,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_83_LAY_TT_CHI_SORequest(
    [property: JsonPropertyName("SO_HOA_DON")] string? SO_HOA_DON,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_84_LAY_TT_GIA_NUOCRequest(
    [property: JsonPropertyName("SO_HOA_DON")] string? SO_HOA_DON,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_85_LAY_TT_GUI_SMSRequest(
    [property: JsonPropertyName("MA_KHACH_HANG")] string? MA_KHACH_HANG,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_88_LAY_TT_CAT_MO_NUOCRequest(
    [property: JsonPropertyName("MA_KHACH_HANG")] string? MA_KHACH_HANG,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_86_LAY_TT_GUI_EMAILRequest(
    [property: JsonPropertyName("MA_KHACH_HANG")] string? MA_KHACH_HANG,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_871_VE_BIEU_DO_DOC_CSRequest(
    [property: JsonPropertyName("NGAY_DK_TU")] string? NGAY_DK_TU,
    [property: JsonPropertyName("NGAY_DK_DEN")] string? NGAY_DK_DEN,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("THANG")] string? THANG,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_071_CC_CANH_BAO_MANG_LUOIRequest(
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_96_CC_DM_YEU_CAURequest(
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_97_CC_DM_QUANRequest(
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_98_CC_DM_PHUONGRequest(
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("MA_QUAN")] string? MA_QUAN,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_91_DM_XI_NGHIEPRequest(
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_92_DM_BIEN_DOCRequest(
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_921_DM_SO_DOCRequest(
    [property: JsonPropertyName("opt_MA_BIEN_DOC")] string? opt_MA_BIEN_DOC,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_99_DM_GHI_CHURequest(
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_931_DM_DIEM_THU_HORequest(
    [property: JsonPropertyName("MA_DIEM_THU")] string? MA_DIEM_THU,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_93_DM_TINH_TRANG_DHRequest(
    [property: JsonPropertyName("MA_TINH_TRANG_DH")] string? MA_TINH_TRANG_DH,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_94_DM_LOAI_CHI_SORequest(
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_95_DM_QUA_VONGRequest(
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record PF_01_GetSysDateRequest(
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record PF_01_GetSysDateStringRequest(
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record PF_01_GetSysDate_ThangRequest(
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record PF_02_LAY_CHI_SO_THAO_LAPRequest(
    [property: JsonPropertyName("MA_KHACH_HANG")] string? MA_KHACH_HANG,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_89_TINH_TIENRequest(
    [property: JsonPropertyName("MA_GIA")] string? MA_GIA,
    [property: JsonPropertyName("SAN_LUONG_SD")] string? SAN_LUONG_SD,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record A_014_CHECKIN_TT_KH_CAT_NUOCRequest(
    [property: JsonPropertyName("ID_XAC_NHAN")] string? ID_XAC_NHAN,
    [property: JsonPropertyName("MA_NHAN_VIEN")] string? MA_NHAN_VIEN,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record A_014_CHECKIN_TT_KH_CAT_NUOC_V2Request(
    [property: JsonPropertyName("ID_XAC_NHAN")] string? ID_XAC_NHAN,
    [property: JsonPropertyName("MA_NHAN_VIEN")] string? MA_NHAN_VIEN,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record A_015_CHECKIN_TT_KH_MO_NUOCRequest(
    [property: JsonPropertyName("ID_XAC_NHAN")] string? ID_XAC_NHAN,
    [property: JsonPropertyName("MA_NHAN_VIEN")] string? MA_NHAN_VIEN,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record A_015_CHECKIN_TT_KH_MO_NUOC_V2Request(
    [property: JsonPropertyName("ID_XAC_NHAN")] string? ID_XAC_NHAN,
    [property: JsonPropertyName("MA_NHAN_VIEN")] string? MA_NHAN_VIEN,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record A_016_CHECKIN_TT_KH_GUI_GBRequest(
    [property: JsonPropertyName("ID_XAC_NHAN")] string? ID_XAC_NHAN,
    [property: JsonPropertyName("MA_NHAN_VIEN")] string? MA_NHAN_VIEN,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record A_016_CHECKIN_TT_KH_GUI_GB_V2Request(
    [property: JsonPropertyName("ID_XAC_NHAN")] string? ID_XAC_NHAN,
    [property: JsonPropertyName("MA_NHAN_VIEN")] string? MA_NHAN_VIEN,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record A_011_CHECKIN_DS_KH_CAT_NUOCRequest(
    [property: JsonPropertyName("MA_NHAN_VIEN")] string? MA_NHAN_VIEN,
    [property: JsonPropertyName("TU_NGAY")] string? TU_NGAY,
    [property: JsonPropertyName("DEN_NGAY")] string? DEN_NGAY,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record A_012_CHECKIN_DS_KH_MO_NUOCRequest(
    [property: JsonPropertyName("MA_NHAN_VIEN")] string? MA_NHAN_VIEN,
    [property: JsonPropertyName("TU_NGAY")] string? TU_NGAY,
    [property: JsonPropertyName("DEN_NGAY")] string? DEN_NGAY,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record A_013_CHECKIN_DS_KH_GUI_GBRequest(
    [property: JsonPropertyName("MA_NHAN_VIEN")] string? MA_NHAN_VIEN,
    [property: JsonPropertyName("TU_NGAY")] string? TU_NGAY,
    [property: JsonPropertyName("DEN_NGAY")] string? DEN_NGAY,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record A_99_CHECKIN_DM_KIEU_CATRequest(
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record A_99_CHECKIN_DM_KIM_NIEMRequest(
    [property: JsonPropertyName("opt_MA_BIEN_DOC")] string? opt_MA_BIEN_DOC,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record A_99_CHECKIN_DM_GHI_CHU_CMNRequest(
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record A_99_CHECKIN_DM_NHOM_TRUONGRequest(
    [property: JsonPropertyName("opt_MA_BIEN_DOC")] string? opt_MA_BIEN_DOC,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record A_99_CHECKIN_DM_CONG_NHANRequest(
    [property: JsonPropertyName("opt_MA_BIEN_DOC")] string? opt_MA_BIEN_DOC,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record A_02_CHECKIN_LUU_KH_CAT_NUOCRequest(
    [property: JsonPropertyName("ID_XAC_NHAN")] string? ID_XAC_NHAN,
    [property: JsonPropertyName("MA_NHAN_VIEN")] string? MA_NHAN_VIEN,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("NGUOI_THI_CONG")] string? NGUOI_THI_CONG,
    [property: JsonPropertyName("MA_KIEU_CAT_MO")] string? MA_KIEU_CAT_MO,
    [property: JsonPropertyName("NGAY_HOAN_THANH")] string? NGAY_HOAN_THANH,
    [property: JsonPropertyName("VI_TRI_XAC_NHAN")] string? VI_TRI_XAC_NHAN,
    [property: JsonPropertyName("MA_GHI_CHU")] string? MA_GHI_CHU,
    [property: JsonPropertyName("GHI_CHU")] string? GHI_CHU,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record A_02_CHECKIN_LUU_KH_CAT_NUOC_V2Request(
    [property: JsonPropertyName("ID_XAC_NHAN")] string? ID_XAC_NHAN,
    [property: JsonPropertyName("MA_NHAN_VIEN")] string? MA_NHAN_VIEN,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("NGUOI_THI_CONG")] string? NGUOI_THI_CONG,
    [property: JsonPropertyName("MA_KIEU_CAT_MO")] string? MA_KIEU_CAT_MO,
    [property: JsonPropertyName("SO_KIM_NIEM")] string? SO_KIM_NIEM,
    [property: JsonPropertyName("CHI_SO_NIEM")] string? CHI_SO_NIEM,
    [property: JsonPropertyName("NGAY_NIEM")] string? NGAY_NIEM,
    [property: JsonPropertyName("CHI_SO_CAT")] string? CHI_SO_CAT,
    [property: JsonPropertyName("NGAY_HOAN_THANH")] string? NGAY_HOAN_THANH,
    [property: JsonPropertyName("VI_TRI_XAC_NHAN")] string? VI_TRI_XAC_NHAN,
    [property: JsonPropertyName("MA_GHI_CHU")] string? MA_GHI_CHU,
    [property: JsonPropertyName("GHI_CHU")] string? GHI_CHU,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record A_03_CHECKIN_LUU_KH_MO_NUOCRequest(
    [property: JsonPropertyName("ID_XAC_NHAN")] string? ID_XAC_NHAN,
    [property: JsonPropertyName("MA_NHAN_VIEN")] string? MA_NHAN_VIEN,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("NGUOI_THI_CONG")] string? NGUOI_THI_CONG,
    [property: JsonPropertyName("MA_KIEU_CAT_MO")] string? MA_KIEU_CAT_MO,
    [property: JsonPropertyName("NGAY_HOAN_THANH")] string? NGAY_HOAN_THANH,
    [property: JsonPropertyName("VI_TRI_XAC_NHAN")] string? VI_TRI_XAC_NHAN,
    [property: JsonPropertyName("MA_GHI_CHU")] string? MA_GHI_CHU,
    [property: JsonPropertyName("GHI_CHU")] string? GHI_CHU,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record A_03_CHECKIN_LUU_KH_MO_NUOC_V2Request(
    [property: JsonPropertyName("ID_XAC_NHAN")] string? ID_XAC_NHAN,
    [property: JsonPropertyName("MA_NHAN_VIEN")] string? MA_NHAN_VIEN,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("NGUOI_THI_CONG")] string? NGUOI_THI_CONG,
    [property: JsonPropertyName("MA_KIEU_CAT_MO")] string? MA_KIEU_CAT_MO,
    [property: JsonPropertyName("NGAY_HOAN_THANH")] string? NGAY_HOAN_THANH,
    [property: JsonPropertyName("VI_TRI_XAC_NHAN")] string? VI_TRI_XAC_NHAN,
    [property: JsonPropertyName("SO_KIM_NIEM")] string? SO_KIM_NIEM,
    [property: JsonPropertyName("CHI_SO_NIEM")] string? CHI_SO_NIEM,
    [property: JsonPropertyName("NGAY_NIEM")] string? NGAY_NIEM,
    [property: JsonPropertyName("CHI_SO_MO")] string? CHI_SO_MO,
    [property: JsonPropertyName("MA_GHI_CHU")] string? MA_GHI_CHU,
    [property: JsonPropertyName("GHI_CHU")] string? GHI_CHU,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record A_04_CHECKIN_LUU_GUI_GIAY_BAORequest(
    [property: JsonPropertyName("ID_XAC_NHAN")] string? ID_XAC_NHAN,
    [property: JsonPropertyName("MA_NHAN_VIEN")] string? MA_NHAN_VIEN,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("NGAY_HOAN_THANH")] string? NGAY_HOAN_THANH,
    [property: JsonPropertyName("VI_TRI_XAC_NHAN")] string? VI_TRI_XAC_NHAN,
    [property: JsonPropertyName("GHI_CHU")] string? GHI_CHU,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record A_00_CHECKIN_LUU_TEN_FILE_ANHRequest(
    [property: JsonPropertyName("ID_XAC_NHAN")] string? ID_XAC_NHAN,
    [property: JsonPropertyName("LOAI_CV")] string? LOAI_CV,
    [property: JsonPropertyName("TEN_FILE_ANH")] string? TEN_FILE_ANH,
    [property: JsonPropertyName("MA_NHAN_VIEN")] string? MA_NHAN_VIEN,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_061_AP_GIA_DS_KHACH_HANGRequest(
    [property: JsonPropertyName("MA_SO_DOC")] string? MA_SO_DOC,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_061_AP_GIA_DS_KHACH_HANG_V3Request(
    [property: JsonPropertyName("MA_SO_DOC")] string? MA_SO_DOC,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_061_AP_LAY_PHIEN_BANRequest(
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_061_AP_GIA_LUURequest(
    [property: JsonPropertyName("ID_AP_GIA")] string? ID_AP_GIA,
    [property: JsonPropertyName("U_SH")] string? U_SH,
    [property: JsonPropertyName("U_KD")] string? U_KD,
    [property: JsonPropertyName("U_SX")] string? U_SX,
    [property: JsonPropertyName("U_HCSN")] string? U_HCSN,
    [property: JsonPropertyName("MA_GHI_CHU")] string? MA_GHI_CHU,
    [property: JsonPropertyName("GHI_CHU_THEM")] string? GHI_CHU_THEM,
    [property: JsonPropertyName("VI_TRI_DOC")] string? VI_TRI_DOC,
    [property: JsonPropertyName("USR_LOG")] string? USR_LOG,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_061_AP_GIA_LUU_V3Request(
    [property: JsonPropertyName("ID_AP_GIA")] string? ID_AP_GIA,
    [property: JsonPropertyName("U_SH")] string? U_SH,
    [property: JsonPropertyName("U_KD")] string? U_KD,
    [property: JsonPropertyName("U_SX")] string? U_SX,
    [property: JsonPropertyName("U_HCSN")] string? U_HCSN,
    [property: JsonPropertyName("MA_GHI_CHU")] string? MA_GHI_CHU,
    [property: JsonPropertyName("GHI_CHU_THEM")] string? GHI_CHU_THEM,
    [property: JsonPropertyName("VI_TRI_DOC")] string? VI_TRI_DOC,
    [property: JsonPropertyName("USR_LOG")] string? USR_LOG,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_061_AP_GIA_LUU_FILE_ANHRequest(
    [property: JsonPropertyName("ID_AP_GIA")] string? ID_AP_GIA,
    [property: JsonPropertyName("TEN_FILE_ANH")] string? TEN_FILE_ANH,
    [property: JsonPropertyName("VI_TRI_DOC")] string? VI_TRI_DOC,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_061_AP_GIA_LUU_FILE_ANH_V2Request(
    [property: JsonPropertyName("USR_APP")] string? USR_APP,
    [property: JsonPropertyName("ID_AP_GIA")] string? ID_AP_GIA,
    [property: JsonPropertyName("TEN_FILE_ANH")] string? TEN_FILE_ANH,
    [property: JsonPropertyName("VI_TRI_DOC")] string? VI_TRI_DOC,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_061_AP_GIA_LUU_FILE_ANH_V3Request(
    [property: JsonPropertyName("USR_APP")] string? USR_APP,
    [property: JsonPropertyName("ID_AP_GIA")] string? ID_AP_GIA,
    [property: JsonPropertyName("TEN_FILE_ANH")] string? TEN_FILE_ANH,
    [property: JsonPropertyName("VI_TRI_DOC")] string? VI_TRI_DOC,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_061_LO_TRINH_DI_DOC_MAP_SERVER_SQLRequest(
    [property: JsonPropertyName("SQL_STRING")] string? SQL_STRING,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K,
    [property: JsonPropertyName("ND")] string? ND,
    [property: JsonPropertyName("MK")] string? MK);

public sealed record P_062_ADMIN_TIM_KIEM_KHRequest(
    [property: JsonPropertyName("MA_KHACH_HANG")] string? MA_KHACH_HANG,
    [property: JsonPropertyName("TEN_KHACH_HANG")] string? TEN_KHACH_HANG,
    [property: JsonPropertyName("DIA_CHI_KHACH_HANG")] string? DIA_CHI_KHACH_HANG,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_061_DM_SO_DOCRequest(
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_061_DM_SO_DOC_V2Request(
    [property: JsonPropertyName("USR_APP")] string? USR_APP,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_061_DM_SO_DOC_V3Request(
    [property: JsonPropertyName("USR_APP")] string? USR_APP,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_061_DM_GIARequest(
    [property: JsonPropertyName("GIA_CO_BAN")] string? GIA_CO_BAN,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_061_DM_GHI_CHURequest(
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_07_CC_BAO_SU_CORequest(
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("loai_yeu_cau")] string? LOAI_YEU_CAU,
    [property: JsonPropertyName("opt_MA_KHACH_HANG")] string? MA_KHACH_HANG,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("ma_quan")] string? MA_QUAN,
    [property: JsonPropertyName("ma_phuong")] string? MA_PHUONG,
    [property: JsonPropertyName("ten_khach_hang")] string? TEN_KHACH_HANG,
    [property: JsonPropertyName("dia_chi_khach_hang")] string? DIA_CHI_KHACH_HANG,
    [property: JsonPropertyName("so_dien_thoai_KH")] string? SO_DIEN_THOAI_KH,
    [property: JsonPropertyName("opt_email_kh")] string? EMAIL_KH,
    [property: JsonPropertyName("noi_dung_yeu_cau")] string? NOI_DUNG_YEU_CAU,
    [property: JsonPropertyName("khach_hang_yeu_cau")] string? KHACH_HANG_YEU_CAU,
    [property: JsonPropertyName("xu_ly_24h")] string? XU_LY_24H,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);

public sealed record P_9E_SUA_THONG_TIN_KHRequest(
    [property: JsonPropertyName("MA_KHACH_HANG")] string? MA_KHACH_HANG,
    [property: JsonPropertyName("MA_XI_NGHIEP")] string? MA_XI_NGHIEP,
    [property: JsonPropertyName("MA_BIEN_DOC")] string? MA_BIEN_DOC,
    [property: JsonPropertyName("THANG")] string? THANG,
    [property: JsonPropertyName("opt_TEN_KHACH_HANG")] string? TEN_KHACH_HANG,
    [property: JsonPropertyName("opt_DIA_CHI_DONG_HO")] string? DIA_CHI_DONG_HO,
    [property: JsonPropertyName("opt_SO_DT")] string? SO_DT,
    [property: JsonPropertyName("opt_EMAIL")] string? EMAIL,
    [property: JsonPropertyName("opt_DONG_HO_TEN")] string? DONG_HO_TEN,
    [property: JsonPropertyName("opt_DONG_HO_SERIAL")] string? DONG_HO_SERIAL,
    [property: JsonPropertyName("opt_GHI_CHU")] string? GHI_CHU,
    [property: JsonPropertyName("SO_IMEI")] string? SO_IMEI,
    [property: JsonPropertyName("PASSWORD_K")] string? PASSWORD_K);
