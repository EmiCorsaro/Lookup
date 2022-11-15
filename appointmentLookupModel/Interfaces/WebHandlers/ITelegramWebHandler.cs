public interface ITelegramWebHandler
{
    Task notifyAuthenticationError();
    Task notifyAvailableAppointment();
    Task greenTitles();
    Task redTitles();
}