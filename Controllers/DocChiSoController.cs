using Microsoft.AspNetCore.Mvc;
using ReadMeter.Api.Businesses;
using ReadMeter.Api.Contracts.Requests;

namespace ReadMeter.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class DocChiSoController : ControllerBase
{
    private readonly IReadMeterBusinesses _businesses;

    public DocChiSoController(IReadMeterBusinesses businesses) => _businesses = businesses;

    [HttpPost("P_00_KET_NOI_DB_CHECK")]
    public Task<ContentResult> P_00_KET_NOI_DB_CHECK([FromBody] P_00_KET_NOI_DB_CHECKRequest request, CancellationToken cancellationToken) =>
        _businesses.P_00_KET_NOI_DB_CHECK(request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_01_DANG_NHAP")]
    public Task<ContentResult> P_01_DANG_NHAP([FromBody] P_01_DANG_NHAPRequest request, CancellationToken cancellationToken) =>
        _businesses.P_01_DANG_NHAP(request.USER, request.PASSWORD, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("DANGNHAPTHEOMANLD")]
    public Task<ContentResult> DANGNHAPTHEOMANLD([FromBody] DANGNHAPTHEOMANLDRequest request, CancellationToken cancellationToken) =>
        _businesses.DANGNHAPTHEOMANLD(request.MA_KHACH_HANG, request.PASS, request.SO_DIEN_THOAI, cancellationToken);

    [HttpPost("P_011_DANG_NHAP_DOI_MAT_KHAU")]
    public Task<ContentResult> P_011_DANG_NHAP_DOI_MAT_KHAU([FromBody] P_011_DANG_NHAP_DOI_MAT_KHAURequest request, CancellationToken cancellationToken) =>
        _businesses.P_011_DANG_NHAP_DOI_MAT_KHAU(request.MA_BIEN_DOC, request.PASSWORD_OLD, request.PASSWORD_NEW1, request.PASSWORD_NEW2, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_012_LAY_GT_CANH_BAO")]
    public Task<ContentResult> P_012_LAY_GT_CANH_BAO([FromBody] P_012_LAY_GT_CANH_BAORequest request, CancellationToken cancellationToken) =>
        _businesses.P_012_LAY_GT_CANH_BAO(request.MA_BIEN_DOC, request.MA_XI_NGHIEP, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_013_LAY_PHIEN_BAN_APP")]
    public Task<ContentResult> P_013_LAY_PHIEN_BAN_APP([FromBody] P_013_LAY_PHIEN_BAN_APPRequest request, CancellationToken cancellationToken) =>
        _businesses.P_013_LAY_PHIEN_BAN_APP(request.MA_BIEN_DOC, request.MA_XI_NGHIEP, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_01_DANG_NHAP_LUU_TOKEN")]
    public Task<ContentResult> P_01_DANG_NHAP_LUU_TOKEN([FromBody] P_01_DANG_NHAP_LUU_TOKENRequest request, CancellationToken cancellationToken) =>
        _businesses.P_01_DANG_NHAP_LUU_TOKEN(request.TOKEN, request.VER_CODE, request.MA_BIEN_DOC, request.MA_XI_NGHIEP, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_021_LAY_DS_SO_DOC")]
    public Task<ContentResult> P_021_LAY_DS_SO_DOC([FromBody] P_021_LAY_DS_SO_DOCRequest request, CancellationToken cancellationToken) =>
        _businesses.P_021_LAY_DS_SO_DOC(request.MA_BIEN_DOC, request.MA_XI_NGHIEP, request.THANG, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_0211_LAY_DS_SO_DOC_BS")]
    public Task<ContentResult> P_0211_LAY_DS_SO_DOC_BS([FromBody] P_0211_LAY_DS_SO_DOC_BSRequest request, CancellationToken cancellationToken) =>
        _businesses.P_0211_LAY_DS_SO_DOC_BS(request.MA_BIEN_DOC, request.MA_XI_NGHIEP, request.THANG, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_0212_LAY_DS_SO_DOC_TRA_CUU")]
    public Task<ContentResult> P_0212_LAY_DS_SO_DOC_TRA_CUU([FromBody] P_0212_LAY_DS_SO_DOC_TRA_CUURequest request, CancellationToken cancellationToken) =>
        _businesses.P_0212_LAY_DS_SO_DOC_TRA_CUU(request.MA_BIEN_DOC, request.MA_XI_NGHIEP, request.THANG, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_022_NHAN_SO_DOC")]
    public Task<ContentResult> P_022_NHAN_SO_DOC([FromBody] P_022_NHAN_SO_DOCRequest request, CancellationToken cancellationToken) =>
        _businesses.P_022_NHAN_SO_DOC(request.DANH_SACH_MA_SO_DOC, request.MA_BIEN_DOC, request.THANG, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_03_LAY_DS_KHACH_HANG")]
    public Task<ContentResult> P_03_LAY_DS_KHACH_HANG([FromBody] P_03_LAY_DS_KHACH_HANGRequest request, CancellationToken cancellationToken) =>
        _businesses.P_03_LAY_DS_KHACH_HANG(request.ID_DOC_opt, request.STT_SO_DOC_opt, request.MA_KH_opt, request.TEN_KH_opt, request.DIA_CHI_DH_opt, request.PHONE_KH_opt, request.MA_SO_DOC, request.MA_BIEN_DOC, request.THANG, request.KIEU_LOC, request.SAP_XEP_THEO, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_03_LAY_DS_KHACH_HANG_SUB")]
    public Task<ContentResult> P_03_LAY_DS_KHACH_HANG_SUB([FromBody] P_03_LAY_DS_KHACH_HANG_SUBRequest request, CancellationToken cancellationToken) =>
        _businesses.P_03_LAY_DS_KHACH_HANG_SUB(request.ID_DOC_opt, request.STT_SO_DOC_opt, request.MA_KH_opt, request.TEN_KH_opt, request.DIA_CHI_DH_opt, request.PHONE_KH_opt, request.MA_SO_DOC, request.MA_BIEN_DOC, request.THANG, request.KIEU_LOC, request.SAP_XEP_THEO, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_03_LAY_DS_KHACH_HANG_SUB_BS")]
    public Task<ContentResult> P_03_LAY_DS_KHACH_HANG_SUB_BS([FromBody] P_03_LAY_DS_KHACH_HANG_SUB_BSRequest request, CancellationToken cancellationToken) =>
        _businesses.P_03_LAY_DS_KHACH_HANG_SUB_BS(request.ID_DOC_opt, request.STT_SO_DOC_opt, request.MA_KH_opt, request.TEN_KH_opt, request.DIA_CHI_DH_opt, request.PHONE_KH_opt, request.MA_SO_DOC, request.MA_BIEN_DOC, request.THANG, request.KIEU_LOC, request.SAP_XEP_THEO, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_0313_LAY_DS_KHACH_HANG_TRA_CUU")]
    public Task<ContentResult> P_0313_LAY_DS_KHACH_HANG_TRA_CUU([FromBody] P_0313_LAY_DS_KHACH_HANG_TRA_CUURequest request, CancellationToken cancellationToken) =>
        _businesses.P_0313_LAY_DS_KHACH_HANG_TRA_CUU(request.MA_SO_DOC, request.MA_BIEN_DOC, request.MA_XI_NGHIEP, request.THANG, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_043_LO_TRINH_DI_DOC_MAP")]
    public Task<ContentResult> P_043_LO_TRINH_DI_DOC_MAP([FromBody] P_043_LO_TRINH_DI_DOC_MAPRequest request, CancellationToken cancellationToken) =>
        _businesses.P_043_LO_TRINH_DI_DOC_MAP(request.MA_SO_DOC, request.MA_BIEN_DOC, request.MA_XI_NGHIEP, request.THANG, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_044_LUU_TEN_FILE_ANH")]
    public Task<ContentResult> P_044_LUU_TEN_FILE_ANH([FromBody] P_044_LUU_TEN_FILE_ANHRequest request, CancellationToken cancellationToken) =>
        _businesses.P_044_LUU_TEN_FILE_ANH(request.ID_DONG_HO, request.TEN_FILE_ANH, request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_032_LAY_SL_BINH_QUAN_3T")]
    public Task<ContentResult> P_032_LAY_SL_BINH_QUAN_3T([FromBody] P_032_LAY_SL_BINH_QUAN_3TRequest request, CancellationToken cancellationToken) =>
        _businesses.P_032_LAY_SL_BINH_QUAN_3T(request.MA_KHACH_HANG, request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_041_NHAP_XUAT_CS_LE_ONLINE")]
    public Task<ContentResult> P_041_NHAP_XUAT_CS_LE_ONLINE([FromBody] P_041_NHAP_XUAT_CS_LE_ONLINERequest request, CancellationToken cancellationToken) =>
        _businesses.P_041_NHAP_XUAT_CS_LE_ONLINE(request.ID_DONG_HO, request.MA_TINH_TRANG_DH, request.QUA_VONG, request.LOAI_CHI_SO, request.NGAY_DOC_CS, request.CHI_SO_MOI, request.SAN_LUONG_TT, request.TONG_SL, request.CONG_CHI_SO, request.SAN_LUONG_DUNG_IT, request.MA_GHI_CHU, request.GHI_CHU, request.VI_TRI_DOC, request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_045_CANH_BAO_SAN_LUONG_LON")]
    public Task<ContentResult> P_045_CANH_BAO_SAN_LUONG_LON([FromBody] P_045_CANH_BAO_SAN_LUONG_LONRequest request, CancellationToken cancellationToken) =>
        _businesses.P_045_CANH_BAO_SAN_LUONG_LON(request.ID_DONG_HO, request.MA_KHACH_HANG, request.MA_BIEN_DOC, request.TONG_SL, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_045_LUU_XEP_SO_DOC")]
    public Task<ContentResult> P_045_LUU_XEP_SO_DOC([FromBody] P_045_LUU_XEP_SO_DOCRequest request, CancellationToken cancellationToken) =>
        _businesses.P_045_LUU_XEP_SO_DOC(request.ID_DONG_HO, request.MA_KHACH_HANG, request.STT_CU, request.STT_MOI, request.MA_SO_DOC, request.MA_BIEN_DOC, request.THANG, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_041_NHAP_XUAT_CS_LE_ONLINE_SUB")]
    public Task<ContentResult> P_041_NHAP_XUAT_CS_LE_ONLINE_SUB([FromBody] P_041_NHAP_XUAT_CS_LE_ONLINE_SUBRequest request, CancellationToken cancellationToken) =>
        _businesses.P_041_NHAP_XUAT_CS_LE_ONLINE_SUB(request.ID_DONG_HO, request.MA_TINH_TRANG_DH, request.QUA_VONG, request.LOAI_CHI_SO, request.NGAY_DOC_CS, request.CHI_SO_MOI, request.SAN_LUONG_TT, request.TONG_SL, request.CONG_CHI_SO, request.SAN_LUONG_DUNG_IT, request.MA_GHI_CHU, request.GHI_CHU, request.VI_TRI_DOC, request.MA_KHACH_HANG, request.MA_SO_DOC, request.THANG, request.MA_XI_NGHIEP, request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_051_KIEM_TRA_BAN_GIAO_SD")]
    public Task<ContentResult> P_051_KIEM_TRA_BAN_GIAO_SD([FromBody] P_051_KIEM_TRA_BAN_GIAO_SDRequest request, CancellationToken cancellationToken) =>
        _businesses.P_051_KIEM_TRA_BAN_GIAO_SD(request.MA_SO_DOC, request.MA_BIEN_DOC, request.MA_XI_NGHIEP, request.THANG, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_05_BD_BAN_GIAO_CS_XONG")]
    public Task<ContentResult> P_05_BD_BAN_GIAO_CS_XONG([FromBody] P_05_BD_BAN_GIAO_CS_XONGRequest request, CancellationToken cancellationToken) =>
        _businesses.P_05_BD_BAN_GIAO_CS_XONG(request.DANH_SACH_MA_SO_DOC, request.MA_BIEN_DOC, request.THANG, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_81_LAY_TT_KHACH_HANG")]
    public Task<ContentResult> P_81_LAY_TT_KHACH_HANG([FromBody] P_81_LAY_TT_KHACH_HANGRequest request, CancellationToken cancellationToken) =>
        _businesses.P_81_LAY_TT_KHACH_HANG(request.MA_KHACH_HANG, request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_82_LAY_TT_HOA_DON")]
    public Task<ContentResult> P_82_LAY_TT_HOA_DON([FromBody] P_82_LAY_TT_HOA_DONRequest request, CancellationToken cancellationToken) =>
        _businesses.P_82_LAY_TT_HOA_DON(request.MA_KHACH_HANG, request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_83_LAY_TT_CHI_SO")]
    public Task<ContentResult> P_83_LAY_TT_CHI_SO([FromBody] P_83_LAY_TT_CHI_SORequest request, CancellationToken cancellationToken) =>
        _businesses.P_83_LAY_TT_CHI_SO(request.SO_HOA_DON, request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_84_LAY_TT_GIA_NUOC")]
    public Task<ContentResult> P_84_LAY_TT_GIA_NUOC([FromBody] P_84_LAY_TT_GIA_NUOCRequest request, CancellationToken cancellationToken) =>
        _businesses.P_84_LAY_TT_GIA_NUOC(request.SO_HOA_DON, request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_85_LAY_TT_GUI_SMS")]
    public Task<ContentResult> P_85_LAY_TT_GUI_SMS([FromBody] P_85_LAY_TT_GUI_SMSRequest request, CancellationToken cancellationToken) =>
        _businesses.P_85_LAY_TT_GUI_SMS(request.MA_KHACH_HANG, request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_88_LAY_TT_CAT_MO_NUOC")]
    public Task<ContentResult> P_88_LAY_TT_CAT_MO_NUOC([FromBody] P_88_LAY_TT_CAT_MO_NUOCRequest request, CancellationToken cancellationToken) =>
        _businesses.P_88_LAY_TT_CAT_MO_NUOC(request.MA_KHACH_HANG, request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_86_LAY_TT_GUI_EMAIL")]
    public Task<ContentResult> P_86_LAY_TT_GUI_EMAIL([FromBody] P_86_LAY_TT_GUI_EMAILRequest request, CancellationToken cancellationToken) =>
        _businesses.P_86_LAY_TT_GUI_EMAIL(request.MA_KHACH_HANG, request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_96_CC_DM_YEU_CAU")]
    public Task<ContentResult> P_96_CC_DM_YEU_CAU([FromBody] P_96_CC_DM_YEU_CAURequest request, CancellationToken cancellationToken) =>
        _businesses.P_96_CC_DM_YEU_CAU(request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_97_CC_DM_QUAN")]
    public Task<ContentResult> P_97_CC_DM_QUAN([FromBody] P_97_CC_DM_QUANRequest request, CancellationToken cancellationToken) =>
        _businesses.P_97_CC_DM_QUAN(request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_98_CC_DM_PHUONG")]
    public Task<ContentResult> P_98_CC_DM_PHUONG([FromBody] P_98_CC_DM_PHUONGRequest request, CancellationToken cancellationToken) =>
        _businesses.P_98_CC_DM_PHUONG(request.MA_BIEN_DOC, request.MA_QUAN, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_91_DM_XI_NGHIEP")]
    public Task<ContentResult> P_91_DM_XI_NGHIEP([FromBody] P_91_DM_XI_NGHIEPRequest request, CancellationToken cancellationToken) =>
        _businesses.P_91_DM_XI_NGHIEP(request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_92_DM_BIEN_DOC")]
    public Task<ContentResult> P_92_DM_BIEN_DOC([FromBody] P_92_DM_BIEN_DOCRequest request, CancellationToken cancellationToken) =>
        _businesses.P_92_DM_BIEN_DOC(request.MA_XI_NGHIEP, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_921_DM_SO_DOC")]
    public Task<ContentResult> P_921_DM_SO_DOC([FromBody] P_921_DM_SO_DOCRequest request, CancellationToken cancellationToken) =>
        _businesses.P_921_DM_SO_DOC(request.opt_MA_BIEN_DOC, request.MA_XI_NGHIEP, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_99_DM_GHI_CHU")]
    public Task<ContentResult> P_99_DM_GHI_CHU([FromBody] P_99_DM_GHI_CHURequest request, CancellationToken cancellationToken) =>
        _businesses.P_99_DM_GHI_CHU(request.MA_XI_NGHIEP, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_931_DM_DIEM_THU_HO")]
    public Task<ContentResult> P_931_DM_DIEM_THU_HO([FromBody] P_931_DM_DIEM_THU_HORequest request, CancellationToken cancellationToken) =>
        _businesses.P_931_DM_DIEM_THU_HO(request.MA_DIEM_THU, request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_93_DM_TINH_TRANG_DH")]
    public Task<ContentResult> P_93_DM_TINH_TRANG_DH([FromBody] P_93_DM_TINH_TRANG_DHRequest request, CancellationToken cancellationToken) =>
        _businesses.P_93_DM_TINH_TRANG_DH(request.MA_TINH_TRANG_DH, request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_94_DM_LOAI_CHI_SO")]
    public Task<ContentResult> P_94_DM_LOAI_CHI_SO([FromBody] P_94_DM_LOAI_CHI_SORequest request, CancellationToken cancellationToken) =>
        _businesses.P_94_DM_LOAI_CHI_SO(request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_95_DM_QUA_VONG")]
    public Task<ContentResult> P_95_DM_QUA_VONG([FromBody] P_95_DM_QUA_VONGRequest request, CancellationToken cancellationToken) =>
        _businesses.P_95_DM_QUA_VONG(request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_89_TINH_TIEN")]
    public Task<ContentResult> P_89_TINH_TIEN([FromBody] P_89_TINH_TIENRequest request, CancellationToken cancellationToken) =>
        _businesses.P_89_TINH_TIEN(request.MA_GIA, request.SAN_LUONG_SD, request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("P_07_CC_BAO_SU_CO")]
    public Task<ContentResult> P_07_CC_BAO_SU_CO([FromBody] P_07_CC_BAO_SU_CORequest request, CancellationToken cancellationToken) =>
        _businesses.P_07_CC_BAO_SU_CO(request, cancellationToken);

    [HttpPost("P_9E_SUA_THONG_TIN_KH")]
    public Task<ContentResult> P_9E_SUA_THONG_TIN_KH([FromBody] P_9E_SUA_THONG_TIN_KHRequest request, CancellationToken cancellationToken) =>
        _businesses.P_9E_SUA_THONG_TIN_KH(request, cancellationToken);

}
