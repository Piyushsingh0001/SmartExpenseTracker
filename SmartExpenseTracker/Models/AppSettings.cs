using SQLite;

namespace SmartExpenseTracker.Models;

public class AppSettings
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
