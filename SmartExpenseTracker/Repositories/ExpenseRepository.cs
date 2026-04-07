using SmartExpenseTracker.Models;
using SmartExpenseTracker.Services;

namespace SmartExpenseTracker.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly DatabaseService _db;

    public ExpenseRepository(DatabaseService db) => _db = db;

    public async Task<List<Expense>> GetAllAsync()
    {
        var conn = await _db.GetConnectionAsync();
        return await conn.Table<Expense>().OrderByDescending(e => e.Date).ToListAsync();
    }

    public async Task<List<Expense>> GetByDateRangeAsync(DateTime from, DateTime to)
    {
        var conn = await _db.GetConnectionAsync();
        return await conn.Table<Expense>()
            .Where(e => e.Date >= from && e.Date <= to)
            .OrderByDescending(e => e.Date)
            .ToListAsync();
    }

    public async Task<List<Expense>> GetByCategoryAsync(string category)
    {
        var conn = await _db.GetConnectionAsync();
        return await conn.Table<Expense>()
            .Where(e => e.Category == category)
            .ToListAsync();
    }

    public async Task<Expense?> GetByIdAsync(int id)
    {
        var conn = await _db.GetConnectionAsync();
        return await conn.Table<Expense>().FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<int> AddAsync(Expense expense)
    {
        var conn = await _db.GetConnectionAsync();
        return await conn.InsertAsync(expense);
    }

    public async Task<int> UpdateAsync(Expense expense)
    {
        var conn = await _db.GetConnectionAsync();
        return await conn.UpdateAsync(expense);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var conn = await _db.GetConnectionAsync();
        return await conn.DeleteAsync<Expense>(id);
    }

    public async Task<decimal> GetTotalByDateRangeAsync(DateTime from, DateTime to)
    {
        var conn = await _db.GetConnectionAsync();
        var expenses = await GetByDateRangeAsync(from, to);
        return expenses.Sum(e => e.Amount);
    }
}
