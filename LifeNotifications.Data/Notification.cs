namespace LifeNotifications.Data;

public class Notification
{
    public DateOnly Date { get; set; }
    public string Text { get; set; } = string.Empty;
}