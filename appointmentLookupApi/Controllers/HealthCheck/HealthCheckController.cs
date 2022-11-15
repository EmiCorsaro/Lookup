using Microsoft.AspNetCore.Mvc;
using appointmentLookupModel;

namespace appointmentLookupApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthCheckController : ControllerBase
{   
    private readonly ILogger<HealthCheckController> _logger;
    private readonly IVersionInfo VersionInfoService;

    public HealthCheckController(ILogger<HealthCheckController> logger, IVersionInfo versionInfo)
    {
        _logger = logger;
        VersionInfoService = versionInfo;
    }

    [HttpGet(Name = "GetHealthCheck")]
    public Health Get()
    {
        Health status = new Healthy();
        status.versionInfo = VersionInfoService.Version;
        return status;
    }
}

