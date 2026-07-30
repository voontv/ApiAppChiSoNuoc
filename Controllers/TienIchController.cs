using Microsoft.AspNetCore.Mvc;
using ReadMeter.Api.Businesses;
using ReadMeter.Api.Contracts.Requests;

namespace ReadMeter.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TienIchController : ControllerBase
{
    private readonly IReadMeterBusinesses _businesses;

    public TienIchController(IReadMeterBusinesses businesses) => _businesses = businesses;

    [HttpPost("PF_01_GetSysDateString")]
    public Task<ContentResult> PF_01_GetSysDateString([FromBody] PF_01_GetSysDateStringRequest request, CancellationToken cancellationToken) =>
        _businesses.PF_01_GetSysDateString(request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("PF_01_GetSysDate_Thang")]
    public Task<ContentResult> PF_01_GetSysDate_Thang([FromBody] PF_01_GetSysDate_ThangRequest request, CancellationToken cancellationToken) =>
        _businesses.PF_01_GetSysDate_Thang(request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

    [HttpPost("PF_02_LAY_CHI_SO_THAO_LAP")]
    public Task<ContentResult> PF_02_LAY_CHI_SO_THAO_LAP([FromBody] PF_02_LAY_CHI_SO_THAO_LAPRequest request, CancellationToken cancellationToken) =>
        _businesses.PF_02_LAY_CHI_SO_THAO_LAP(request.MA_KHACH_HANG, request.MA_BIEN_DOC, request.SO_IMEI, request.PASSWORD_K, cancellationToken);

}
