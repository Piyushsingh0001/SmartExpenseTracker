using SQLite;

namespace SmartExpenseTracker.Models;

public class Goal
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string GoalName { get; set; } = string.Empty;
    public decimal TargetAmount { get; set; }
    public decimal SavedAmount { get; set; }
    public DateTime Deadline { get; set; }

    [Ignore]
    public double ProgressPercentage =>
        TargetAmount > 0 ? (double)(SavedAmount / TargetAmount) * 100 : 0;
}
