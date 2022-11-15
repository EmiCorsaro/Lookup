public class CGEStateManager: ICGEStateManager
{
    public bool isLogged {get;set;}
    public DateTime lastHitNotification {get;set;}
    private string? _cgeSessionToken;

    public CGEStateManager()
    {
        isLogged = true;
        lastHitNotification = DateTime.Now.AddMinutes(-5);
        _cgeSessionToken = Environment.GetEnvironmentVariable("CGE_TOKEN");
    }

    public void setSessionToken(string CGESessionToken)
    {
        _cgeSessionToken = CGESessionToken;
    }

    public string? getSessionToken()
    {
        return _cgeSessionToken;
    }
}