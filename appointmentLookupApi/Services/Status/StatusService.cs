using appointmentLookupModel;

public class StatusService: IStatusService
{
    private readonly ICGEStateManager CGEStateManager;

    public StatusService(ICGEStateManager cGEStateManager)
    {
        CGEStateManager = cGEStateManager;
    }
    public Status GetStatus()
    {
        Status status = new Status(CGEStateManager.lastHitNotification);
        return status;
    }
}