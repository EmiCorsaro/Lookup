using RestSharp;
using Microsoft.Extensions.Configuration;
namespace appointmentLookupModel;
public class CGEWebHandler: ICGEWebHandler
{
    private readonly IConfiguration Config;
    private readonly ICGEStateManager CGEStateManager;

    public CGEWebHandler(IConfiguration configuration, ICGEStateManager cGEStateManager)
    {
        Config = configuration;
        CGEStateManager = cGEStateManager;
    }
    public LookupResult verifyAppointments()
    {
        string? appointmentURL = Config["CGE:AppointmentURL"];
        string? CGEToken = CGEStateManager.getSessionToken();
        string content = string.Empty;
        bool hasAppointment = false;
        bool isAuthenticated = false;
        if(!String.IsNullOrEmpty(appointmentURL) && !String.IsNullOrEmpty(CGEToken))
        {
            var client = new RestClient(Config["CGE:AppointmentURL"]);
            var request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("Cookie", "PHPSESSID="+CGEToken+";");
            var response = client.Get(request);
            content = response.Content is null? string.Empty : response.Content;
            if(!string.IsNullOrEmpty(content))
            {
                isAuthenticated = content.Contains(Config["CGE:AppointmentAuthenticationDetectionPattern"]);
                hasAppointment = isAuthenticated && !content.Contains(Config["CGE:AppointmentAvailabilityDetectionPattern"]);
            }
        }
        return new LookupResult() { LookupTime = DateTime.Now, StillAuthenticated = isAuthenticated, FoundAppointment = hasAppointment, Raw = content is null? string.Empty:content};
    }
}