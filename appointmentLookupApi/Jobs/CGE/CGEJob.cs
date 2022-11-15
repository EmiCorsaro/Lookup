using Quartz;
using appointmentLookupModel;
namespace appointmentLookupApi;

public class CGEJob:IJob
{
    private ICGEService _CGEService;
    private ITelegramWebHandler _TelegramWebHandler;
    private ICGEStateManager _CGEStateManager;
    public CGEJob(ICGEService cGEService, ITelegramWebHandler telegramWebHandler, ICGEStateManager cGEStateManager)
    {
        _CGEService = cGEService;
        _TelegramWebHandler = telegramWebHandler;
        _CGEStateManager = cGEStateManager;
    }
    public Task Execute(IJobExecutionContext context)
    {
        LookupResult aLookupResult = _CGEService.lookupAppointment();
        if(!aLookupResult.StillAuthenticated)
        {
            if(_CGEStateManager.isLogged)
            {
                _TelegramWebHandler.notifyAuthenticationError();
                _TelegramWebHandler.redTitles();
                _CGEStateManager.isLogged = false;
            }
            return Task.CompletedTask;
        }
        else{
            if(!_CGEStateManager.isLogged)
            {
                _TelegramWebHandler.greenTitles();
            }
            _CGEStateManager.isLogged = true;
        }
        if(aLookupResult.FoundAppointment && (DateTime.Now - _CGEStateManager.lastHitNotification).TotalMinutes >= 5)
        {
            _TelegramWebHandler.notifyAvailableAppointment();
            _CGEStateManager.lastHitNotification = DateTime.Now;
            return Task.CompletedTask;
        }
        return Task.CompletedTask;
    }
}