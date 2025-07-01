using System.Text.RegularExpressions;
using LifeNotifications.Data;

namespace LifeNotifications.DataParser;

public class DataParser
{
    public Notification GetNearestLifeNotification(DateTime now, TimeZoneInfo timeZone, string filePath)
    {
        DateTime nowLocal = TimeZoneInfo.ConvertTime(now, timeZone);
        DateOnly nowLocalDate = DateOnly.FromDateTime(nowLocal);
        string[] lines = File.ReadAllLines(filePath);
        Notification? lastValid = null;

        for (int i = 0; i < lines.Length; i++)
        {
            if (!lines[i].StartsWith("📅"))
                continue;

            Match match = Regex.Match(lines[i], @"(\d{2}\.\d{2}\.\d{4})");

            if (!match.Success)
                continue;

            if (!DateOnly.TryParseExact(match.Value, "dd.MM.yyyy", out DateOnly date))
                continue;

            if (date != nowLocalDate)
                continue;

            if (i + 2 >= lines.Length)
                continue;

            string text = string.Join(Environment.NewLine, lines[i], lines[i + 1], lines[i + 2]);
            lastValid = new() { Date = date, Text = text, };
        }

        return lastValid ?? throw new("Сообщение не найдено");
    }
}