using Telegram.Bot;

namespace LifeNotifications.Telegram;

public class TelegramSender(string token)
{
    private readonly TelegramBotClient _bot = new(token);

    public async Task SendMessageAsync(long chatId, string message)
    {
        await _bot.SendMessage(chatId, message);
    }
}