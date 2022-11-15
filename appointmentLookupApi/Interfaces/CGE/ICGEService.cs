using appointmentLookupModel;
public interface ICGEService
{
    public LookupResult lookupAppointment();
    public IEnumerable<DTOLookupResult> getLastestsLookupResults();
    public DTOLookupResultWithDetails getLastLookupResultsDetailed();
    public DTOLookupResult getLastLookupResult();
}