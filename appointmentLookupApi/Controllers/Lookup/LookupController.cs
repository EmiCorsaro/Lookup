using Microsoft.AspNetCore.Mvc;
using appointmentLookupModel;

namespace appointmentLookupApi;

[ApiController]
[Route("[controller]")]
public class LookupController
{
    private readonly ILogger<LookupController> _logger;
    private ICGEService CGEService;

    public LookupController(ILogger<LookupController> logger, ICGEService cGEService)
    {
        _logger = logger;
        CGEService = cGEService;
    }

    [HttpGet]
    public ResponseData<List<DTOLookupResult>> GetAll()
    {
        List<DTOLookupResult> aDTOLookupResultList = CGEService.getLastestsLookupResults().ToList();
        return new ResponseData<List<DTOLookupResult>>() {Data = aDTOLookupResultList};
    }

    [HttpGet]
    [Route("Lastest")]
    public ResponseData<DTOLookupResult> GetLastRun()
    {
        DTOLookupResult aDTOLookupResult = CGEService.getLastLookupResult();
        return new ResponseData<DTOLookupResult>() {Data = aDTOLookupResult};
    }

    [HttpGet]
    [Route("Lastest/Detailed")]
    public ResponseData<DTOLookupResultWithDetails> GetLastRunDetailed()
    {
        DTOLookupResultWithDetails aDTOLookupResult = CGEService.getLastLookupResultsDetailed();
        return new ResponseData<DTOLookupResultWithDetails>() {Data = aDTOLookupResult};
    }
}