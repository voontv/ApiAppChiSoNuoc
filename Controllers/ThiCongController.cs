using Microsoft.AspNetCore.Mvc;
using ReadMeter.Api.Businesses;
using ReadMeter.Api.Contracts.Requests;

namespace ReadMeter.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ThiCongController : ControllerBase
{
    private readonly IReadMeterBusinesses _businesses;

    public ThiCongController(IReadMeterBusinesses businesses) => _businesses = businesses;

}
