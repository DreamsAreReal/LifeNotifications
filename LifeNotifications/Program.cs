using LifeNotifications.Data;
using LifeNotifications.DataParser;
using LifeNotifications.Telegram;

string filePath = "./LifeNotifications/data.txt";
string telegramToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN") ?? throw new Exception("Token not found");

string? rawIds = Environment.GetEnvironmentVariable("TELEGRAM_CHAT_IDS");
if (string.IsNullOrWhiteSpace(rawIds))
    throw new InvalidOperationException("TELEGRAM_CHAT_IDS not found or empty");

long[] telegramOwnerIds = rawIds
                          .Split(',', StringSplitOptions.RemoveEmptyEntries)
                          .Select(id => long.Parse(id.Trim()))
                          .ToArray();
DataParser parser = new();
TelegramSender sender = new(telegramToken);

DateTime utcNow = DateTime.UtcNow;
TimeZoneInfo timeZone = TimeZoneInfo.Utc;


Console.WriteLine($"Now: {utcNow}");
Notification lifeNotification =
    parser.GetNearestLifeNotification(utcNow, timeZone, filePath);

foreach (long id in telegramOwnerIds)
    await sender.SendMessageAsync(id, lifeNotification.Text);
    
    
    