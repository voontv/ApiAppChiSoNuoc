using Microsoft.AspNetCore.Mvc;
using ReadMeter.Api.Businesses;
using ReadMeter.Api.Contracts.Requests;

namespace ReadMeter.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CheckInController : ControllerBase
{
    private readonly IReadMeterBusinesses _businesses;

    public CheckInController(IReadMeterBusinesses businesses) => _businesses = businesses;

    [HttpPost("A_011_CHECKIN_DS_KH_CAT_NUOC")]
    public Task<ContentResult> A_011_CHECKIN_DS_KH_CAT_NUOC([FromBody] A_011_CHECKIN_DS_KH_CAT_NUOCRequest request, CancellationToken cancellationToken) =>
        _businesses.A_011_CHECKIN_DS_KH_CAT_NUOC(request.MA_NHAN_VIEN, request.TU_NGAY, request.DEN_NGAY, request.MA_XI_NGHIEP, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("A_012_CHECKIN_DS_KH_MO_NUOC")]
    public Task<ContentResult> A_012_CHECKIN_DS_KH_MO_NUOC([FromBody] A_012_CHECKIN_DS_KH_MO_NUOCRequest request, CancellationToken cancellationToken) =>
        _businesses.A_012_CHECKIN_DS_KH_MO_NUOC(request.MA_NHAN_VIEN, request.TU_NGAY, request.DEN_NGAY, request.MA_XI_NGHIEP, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("A_013_CHECKIN_DS_KH_GUI_GB")]
    public Task<ContentResult> A_013_CHECKIN_DS_KH_GUI_GB([FromBody] A_013_CHECKIN_DS_KH_GUI_GBRequest request, CancellationToken cancellationToken) =>
        _businesses.A_013_CHECKIN_DS_KH_GUI_GB(request.MA_NHAN_VIEN, request.TU_NGAY, request.DEN_NGAY, request.MA_XI_NGHIEP, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("A_99_CHECKIN_DM_KIEU_CAT")]
    public Task<ContentResult> A_99_CHECKIN_DM_KIEU_CAT([FromBody] A_99_CHECKIN_DM_KIEU_CATRequest request, CancellationToken cancellationToken) =>
        _businesses.A_99_CHECKIN_DM_KIEU_CAT(request.MA_XI_NGHIEP, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("A_02_CHECKIN_LUU_KH_CAT_NUOC")]
    public Task<ContentResult> A_02_CHECKIN_LUU_KH_CAT_NUOC([FromBody] A_02_CHECKIN_LUU_KH_CAT_NUOCRequest request, CancellationToken cancellationToken) =>
        _businesses.A_02_CHECKIN_LUU_KH_CAT_NUOC(request.ID_XAC_NHAN, request.MA_NHAN_VIEN, request.MA_XI_NGHIEP, request.NGUOI_THI_CONG, request.MA_KIEU_CAT_MO, request.NGAY_HOAN_THANH, request.VI_TRI_XAC_NHAN, request.MA_GHI_CHU, request.GHI_CHU, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("A_03_CHECKIN_LUU_KH_MO_NUOC")]
    public Task<ContentResult> A_03_CHECKIN_LUU_KH_MO_NUOC([FromBody] A_03_CHECKIN_LUU_KH_MO_NUOCRequest request, CancellationToken cancellationToken) =>
        _businesses.A_03_CHECKIN_LUU_KH_MO_NUOC(request.ID_XAC_NHAN, request.MA_NHAN_VIEN, request.MA_XI_NGHIEP, request.NGUOI_THI_CONG, request.MA_KIEU_CAT_MO, request.NGAY_HOAN_THANH, request.VI_TRI_XAC_NHAN, request.MA_GHI_CHU, request.GHI_CHU, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("A_00_CHECKIN_LUU_TEN_FILE_ANH")]
    public Task<ContentResult> A_00_CHECKIN_LUU_TEN_FILE_ANH([FromBody] A_00_CHECKIN_LUU_TEN_FILE_ANHRequest request, CancellationToken cancellationToken) =>
        _businesses.A_00_CHECKIN_LUU_TEN_FILE_ANH(request.ID_XAC_NHAN, request.LOAI_CV, request.TEN_FILE_ANH, request.MA_NHAN_VIEN, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

}
