namespace appointmentLookupModel;
public class Status {
    public DateTime SystemTime => DateTime.Now;
    public DateTime LastHitNotification {get;set;}
    public double TimeSinceLastNotification {get;set;}
    public string VersionInfo {get;set;}

    public Status(DateTime lastHitNotification)
    {
        LastHitNotification = lastHitNotification;
        TimeSinceLastNotification = (DateTime.Now - lastHitNotification).TotalMinutes;
        VersionInfo = string.Empty;
    }
}