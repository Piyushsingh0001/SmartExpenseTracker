using SmartExpenseTracker.Models;
using SmartExpenseTracker.Repositories;

namespace SmartExpenseTracker.Services;

public class BudgetService
{
    private readonly IBudgetRepository _budgetRepo;
    private readonly IExpenseRepository _expenseRepo;

    public BudgetService(IBudgetRepository budgetRepo, IExpenseRepository expenseRepo)
    {
        _budgetRepo = budgetRepo;
        _expenseRepo = expenseRepo;
    }

    public async Task<Budget?> GetCurrentMonthBudgetAsync()
    {
        var now = DateTime.Now;
        return await _budgetRepo.GetByMonthYearAsync(now.Month, now.Year);
    }

    public async Task SetBudgetAsync(decimal amount)
    {
        var now = DateTime.Now;
        await _budgetRepo.UpsertAsync(new Budget
        {
            Month = now.Month,
            Year = now.Year,
            MonthlyLimit = amount
        });
    }

    public async Task<(decimal Spent, decimal Remaining, double Percent)> GetSummaryAsync()
    {
        var budget = await GetCurrentMonthBudgetAsync();
        var now = DateTime.Now;
        var from = new DateTime(now.Year, now.Month, 1);
        var to = from.AddMonths(1).AddTicks(-1);
        var spent = await _expenseRepo.GetTotalByDateRangeAsync(from, to);
        var limit = budget?.MonthlyLimit ?? 0;
        var remaining = limit - spent;
        var pct = limit > 0 ? (double)(spent / limit) * 100 : 0;
        return (spent, remaining, pct);
    }

    /// <summary>
    /// Check spend % and fire local notifications at 50%, 80%, 100%.
    /// </summary>
    public async Task CheckAndNotifyAsync(NotificationService notify)
    {
        var (_, _, pct) = await GetSummaryAsync();
        if (pct >= 100)
            await notify.SendAsync("Budget Exceeded!",
                "You have used 100% of your monthly budget.");
        else if (pct >= 80)
            await notify.SendAsync("Budget Warning",
                "You have used 80% of your monthly budget.");
        else if (pct >= 50)
            await notify.SendAsync("Budget Notice",
                "You have used 50% of your monthly budget.");
    }
}
