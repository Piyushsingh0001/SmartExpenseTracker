using SmartExpenseTracker.Models;
using SmartExpenseTracker.Repositories;

namespace SmartExpenseTracker.Services;

public class RecurringExpenseService
{
    private readonly IRecurringExpenseRepository _recurringRepo;
    private readonly IExpenseRepository _expenseRepo;

    public RecurringExpenseService(
        IRecurringExpenseRepository recurringRepo,
        IExpenseRepository expenseRepo)
    {
        _recurringRepo = recurringRepo;
        _expenseRepo = expenseRepo;
    }

    /// <summary>
    /// Called on app start – auto-inserts recurring expenses that haven't
    /// been processed for the current month yet.
    /// </summary>
    public async Task ProcessRecurringExpensesAsync()
    {
        var recurring = await _recurringRepo.GetAllActiveAsync();
        var now = DateTime.Now;

        foreach (var r in recurring)
        {
            // Skip if already processed this month
            if (r.LastProcessed.Month == now.Month && r.LastProcessed.Year == now.Year)
                continue;

            // Only add if today >= the scheduled day
            if (now.Day < r.DayOfMonth) continue;

            await _expenseRepo.AddAsync(new Expense
            {
                Amount = r.Amount,
                Category = r.Category,
                Type = "Needs",
                PaymentMode = r.PaymentMode,
                Date = new DateTime(now.Year, now.Month, r.DayOfMonth),
                Notes = $"Auto: {r.Name}"
            });

            r.LastProcessed = now;
            await _recurringRepo.UpdateAsync(r);
        }
    }

    public Task<List<RecurringExpense>> GetAllAsync() => _recurringRepo.GetAllActiveAsync();
    public Task<int> AddAsync(RecurringExpense r) => _recurringRepo.AddAsync(r);
    public Task<int> DeleteAsync(int id) => _recurringRepo.DeleteAsync(id);
}
