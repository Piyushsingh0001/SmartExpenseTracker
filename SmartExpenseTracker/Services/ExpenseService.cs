using SmartExpenseTracker.Models;
using SmartExpenseTracker.Repositories;

namespace SmartExpenseTracker.Services;

public class ExpenseService
{
    private readonly IExpenseRepository _repo;
    private readonly BudgetService _budgetService;
    private readonly NotificationService _notify;

    public ExpenseService(
        IExpenseRepository repo,
        BudgetService budgetService,
        NotificationService notify)
    {
        _repo = repo;
        _budgetService = budgetService;
        _notify = notify;
    }

    public Task<List<Expense>> GetAllAsync() => _repo.GetAllAsync();

    public Task<List<Expense>> GetTodayAsync()
    {
        var today = DateTime.Today;
        return _repo.GetByDateRangeAsync(today, today.AddDays(1).AddTicks(-1));
    }

    public Task<List<Expense>> GetThisMonthAsync()
    {
        var now = DateTime.Now;
        var from = new DateTime(now.Year, now.Month, 1);
        var to = from.AddMonths(1).AddTicks(-1);
        return _repo.GetByDateRangeAsync(from, to);
    }

    public async Task<int> AddAsync(Expense expense)
    {
        var result = await _repo.AddAsync(expense);
        // Check budget alerts after adding
        await _budgetService.CheckAndNotifyAsync(_notify);
        return result;
    }

    public Task<int> UpdateAsync(Expense expense) => _repo.UpdateAsync(expense);
    public Task<int> DeleteAsync(int id) => _repo.DeleteAsync(id);

    public async Task<Dictionary<string, decimal>> GetCategoryTotalsThisMonthAsync()
    {
        var expenses = await GetThisMonthAsync();
        return expenses
            .GroupBy(e => e.Category)
            .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));
    }
}
