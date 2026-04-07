using SQLite;

namespace SmartExpenseTracker.Models;

public class RecurringExpense
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Category { get; set; } = string.Empty;
    public string PaymentMode { get; set; } = "Cash";
    public int DayOfMonth { get; set; }    // Day to auto-add each month
    public DateTime LastProcessed { get; set; }
    public bool IsActive { get; set; } = true;
}
