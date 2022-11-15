using Microsoft.AspNetCore.Mvc;
using appointmentLookupModel;

namespace appointmentLookupApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StatusCheckController : ControllerBase
{   
    private readonly ILogger<StatusCheckController> _logger;
    private readonly IStatusService StatusService;
    private readonly IVersionInfo VersionInfoService;

    public StatusCheckController(ILogger<StatusCheckController> logger, IStatusService statusService, IVersionInfo versionInfo)
    {
        _logger = logger;
        StatusService = statusService;
        VersionInfoService = versionInfo;
    }

    [HttpGet(Name = "GetStatusCheck")]
    public Status Get()
    {
        Status status = StatusService.GetStatus();
        status.VersionInfo = VersionInfoService.Version;
        return status;
    }
}
