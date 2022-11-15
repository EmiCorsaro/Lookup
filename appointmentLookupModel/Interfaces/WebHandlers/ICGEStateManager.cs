public interface ICGEStateManager
{
    public bool isLogged {get;set;}
    public DateTime lastHitNotification {get;set;}
    public void setSessionToken(string CGESessionToken);
    public string? getSessionToken();
}