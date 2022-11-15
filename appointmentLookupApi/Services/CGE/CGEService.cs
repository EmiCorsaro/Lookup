using appointmentLookupModel;
using AutoMapper;
namespace appointmentLookupApi;
public class CGEService: ICGEService
{
    private readonly ILogger<CGEService> _logger;
    private readonly IMapper _mapper;
    private ILookupResultsDbHandler LookupResultsDbHandler;
    private ICGEWebHandler CGEWebHandler;

    public CGEService(ILogger<CGEService> logger, IMapper mapper, ICGEWebHandler cGEWebHandler, ILookupResultsDbHandler lookupResultsDbHandler)
    {
        _logger = logger;
        _mapper = mapper;
        LookupResultsDbHandler = lookupResultsDbHandler;
        CGEWebHandler = cGEWebHandler;
    }
    public LookupResult lookupAppointment()
    {
        LookupResultsDbHandler.CleanOldEntries();
        LookupResult alookupResult = CGEWebHandler.verifyAppointments();
        LookupResultsDbHandler.AddNewLookupResult(alookupResult);
        return alookupResult;
    }

    public IEnumerable<DTOLookupResult> getLastestsLookupResults()
    {
        List<DTOLookupResult> aList = _mapper.Map<List<DTOLookupResult>>(LookupResultsDbHandler.GetLookupResults().ToList());
        return aList;
    }

    public DTOLookupResultWithDetails getLastLookupResultsDetailed()
    {
        DTOLookupResultWithDetails aLookupResult = _mapper.Map<DTOLookupResultWithDetails>(LookupResultsDbHandler.GetLastLookupResult());
        return aLookupResult;
    }
    public DTOLookupResult getLastLookupResult()
    {
        DTOLookupResult aLookupResult = _mapper.Map<DTOLookupResult>(LookupResultsDbHandler.GetLastLookupResult());
        return aLookupResult;
    }
}