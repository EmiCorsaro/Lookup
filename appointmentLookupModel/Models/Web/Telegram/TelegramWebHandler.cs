using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Configuration;
namespace appointmentLookupModel;
public class TelegramWebHandler: ITelegramWebHandler
{
    private readonly IConfiguration Config;
    private readonly ICGEStateManager CGEStateManager;
    private TelegramBotClient? telegramBot;

    public TelegramWebHandler(IConfiguration configuration, ICGEStateManager cGEStateManager)
    {
        Config = configuration;
        CGEStateManager = cGEStateManager;
        string? TelegramToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");
        if (!String.IsNullOrEmpty(TelegramToken))
        {
            telegramBot = new TelegramBotClient(TelegramToken);
            using var cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };
            telegramBot.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
        }else{
            throw new Exception("Telegram token hasn't been provided");
        }
    }
    public async Task notifyAuthenticationError()
    {
        if(telegramBot is not null)
        {
            List<String> subscriptors = Config.GetSection("Telegram:AuthenticationError:Subscriptors").Get<List<String>>();
            var stickerURL = Config["Telegram:AuthenticationError:StickerURL"];
            foreach(string sub in subscriptors)
            {
                var t = await telegramBot.SendTextMessageAsync(chatId:sub, text:Config["Telegram:AuthenticationError:Message"]).ConfigureAwait(false);
             if(!String.IsNullOrEmpty(stickerURL))
                {
                    var s = await telegramBot.SendStickerAsync(chatId: sub, sticker: stickerURL);
                }
            }
        }
    }
    public async Task notifyAvailableAppointment()
    {
        if(telegramBot is not null)
        {
            List<String> subscriptors = Config.GetSection("Telegram:AppointmentAvailability:Subscriptors").Get<List<String>>();
            var stickerURL = Config["Telegram:AppointmentAvailability:StickerURL"];
            foreach(string sub in subscriptors)
            {
                var t = await telegramBot.SendTextMessageAsync(chatId:sub, text:Config["Telegram:AppointmentAvailability:Message"]).ConfigureAwait(false);
                if(!String.IsNullOrEmpty(stickerURL))
                {
                    var s = await telegramBot.SendStickerAsync(chatId: sub, sticker: stickerURL);
                }
            }
        }
    }

    public async Task greenTitles()
    {
        if(telegramBot is not null)
        {
            List<String> subscriptors = Config.GetSection("Telegram:AuthenticationError:Subscriptors").Get<List<String>>();
            foreach(string sub in subscriptors)
            {
                 var t = await telegramBot.GetChatAsync(chatId: sub).ConfigureAwait(false);
                 if(!string.IsNullOrEmpty(t.Title)){
                    var title = t.Title.Replace("🟢",string.Empty).Replace("🔴",string.Empty).Trim();
                    await telegramBot.SetChatTitleAsync(chatId:sub, title:"🟢 " + title).ConfigureAwait(false);
                 }
            }

        }
    }
    public async Task redTitles()
    {
        if(telegramBot is not null)
        {
            List<String> subscriptors = Config.GetSection("Telegram:AuthenticationError:Subscriptors").Get<List<String>>();
            foreach(string sub in subscriptors)
            {
                 var t = await telegramBot.GetChatAsync(chatId: sub).ConfigureAwait(false);
                 if(!string.IsNullOrEmpty(t.Title)){
                    var title = t.Title.Replace("🔴",string.Empty).Replace("🟢",string.Empty).Trim();
                    await telegramBot.SetChatTitleAsync(chatId:sub, title:"🔴 " + title).ConfigureAwait(false);
                 }
            }
        }
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
        {
            return;
        }
        if (message.Text is not { } messageText)
        {
            return;
        }
        if(telegramBot is not null)
        {
            List<String> subscriptors = Config.GetSection("Telegram:AuthenticationError:Subscriptors").Get<List<String>>();
            if(subscriptors.Exists(s => s == "@"+message.Chat.Username ))
            {
                if(messageText.StartsWith("Token="))
                {
                    CGEStateManager.setSessionToken(messageText.Replace("Token=",string.Empty));
                    var t = await telegramBot.SendTextMessageAsync(chatId:"@"+message.Chat.Username, text:"Token actualizado.\nCuando el icono en el titulo se vuelva verde el sistema quedará nuevamente conectado.").ConfigureAwait(false);
                }else{
                    var t = await telegramBot.SendTextMessageAsync(chatId:"@"+message.Chat.Username, text:"No entendí bien lo que necesitas. Por el momento podes realizar las siguientes acciones:\n1. Actualizar el token enviandome Token= seguido del codigo correspondiente.").ConfigureAwait(false);
                }
            }
        }
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}