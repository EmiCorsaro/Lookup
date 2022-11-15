namespace appointmentLookupModel;
public class LookupResultDbHandler: ILookupResultsDbHandler
{
    private AppDbContext DbContext;

    public LookupResultDbHandler(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }
    public IEnumerable<LookupResult> GetLookupResults()
    {
        var lookupResults = DbContext.LookupResults;
        if(lookupResults is null)
        {
            return new List<LookupResult>();
        }
        List<LookupResult> aList = lookupResults.ToList();
        if(aList.Count == 0)
            return new List<LookupResult>();
        return aList.OrderByDescending(l => l.LookupTime);
    }
    public IEnumerable<LookupResult> GetLookupResultsByDate(DateTime dateTime)
    {
        var lookupResults = DbContext.LookupResults;
        if(lookupResults is null)
        {
            return new List<LookupResult>();
        }
        return lookupResults.Where(l => l.LookupTime == dateTime);
    }
    public LookupResult? GetLastLookupResult()
    {
        var lookupResults = DbContext.LookupResults;
        if(lookupResults is null)
        {
            return null;
        }
        List<LookupResult> aList = lookupResults.ToList();
        if(aList.Count == 0)
            return null;
        return aList.OrderByDescending(l => l.LookupTime).First();
    }
    public async Task<bool> AddNewLookupResult(LookupResult lookupResult)
    {
        int changes = 0;
        if(DbContext.LookupResults is not null)
            DbContext.LookupResults.Add(lookupResult);
            changes = await DbContext.SaveChangesAsync().ConfigureAwait(false);
        return changes == 1;
    }

    public async Task<bool> CleanOldEntries()
    {
        int changes = 0;
        if(DbContext.LookupResults is not null)
            DbContext.LookupResults.RemoveRange(DbContext.LookupResults.Where(l => (DateTime.Now - l.LookupTime).Minutes > 30));
            changes = await DbContext.SaveChangesAsync().ConfigureAwait(false);
        return true;
    }
}