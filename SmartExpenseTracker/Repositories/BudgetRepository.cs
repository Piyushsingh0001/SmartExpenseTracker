using SmartExpenseTracker.Models;
using SmartExpenseTracker.Services;
namespace SmartExpenseTracker.Repositories;

public class BudgetRepository : IBudgetRepository
{
    private readonly DatabaseService _db;
    public BudgetRepository(DatabaseService db) => _db = db;

    public async Task<Budget?> GetByMonthYearAsync(int month, int year)
    {
        var conn = await _db.GetConnectionAsync();
        return await conn.Table<Budget>()
            .FirstOrDefaultAsync(b => b.Month == month && b.Year == year);
    }

    public async Task<int> UpsertAsync(Budget budget)
    {
        var conn = await _db.GetConnectionAsync();
        var existing = await GetByMonthYearAsync(budget.Month, budget.Year);
        if (existing is null)
            return await conn.InsertAsync(budget);
        budget.Id = existing.Id;
        return await conn.UpdateAsync(budget);
    }
}
