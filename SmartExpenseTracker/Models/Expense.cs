using SQLite;

namespace SmartExpenseTracker.Models;

public class Expense
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Category { get; set; } = string.Empty;
    /// <summary>Needs | Wants | Investment</summary>
    public string Type { get; set; } = "Needs";
    /// <summary>Cash | UPI | Card</summary>
    public string PaymentMode { get; set; } = "Cash";
    public DateTime Date { get; set; } = DateTime.Now;
    public string Notes { get; set; } = string.Empty;
}
