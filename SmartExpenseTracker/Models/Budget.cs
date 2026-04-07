using SQLite;

namespace SmartExpenseTracker.Models;

public class Budget
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int Month { get; set; }        // 1-12
    public int Year { get; set; }
    public decimal MonthlyLimit { get; set; }
}
