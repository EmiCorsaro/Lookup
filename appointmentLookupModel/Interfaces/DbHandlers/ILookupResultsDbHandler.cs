namespace appointmentLookupModel;
public interface ILookupResultsDbHandler
{
    public IEnumerable<LookupResult> GetLookupResults();
    public IEnumerable<LookupResult> GetLookupResultsByDate(DateTime dateTime);
    public LookupResult? GetLastLookupResult();
    public Task<bool> AddNewLookupResult(LookupResult lookupResult);
    public Task<bool> CleanOldEntries();
}