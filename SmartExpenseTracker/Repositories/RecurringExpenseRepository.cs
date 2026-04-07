using SmartExpenseTracker.Models;
using SmartExpenseTracker.Services;
namespace SmartExpenseTracker.Repositories;

public class RecurringExpenseRepository : IRecurringExpenseRepository
{
    private readonly DatabaseService _db;
    public RecurringExpenseRepository(DatabaseService db) => _db = db;

    public async Task<List<RecurringExpense>> GetAllActiveAsync()
    {
        var conn = await _db.GetConnectionAsync();
        return await conn.Table<RecurringExpense>()
            .Where(r => r.IsActive).ToListAsync();
    }
    public async Task<int> AddAsync(RecurringExpense r)
    {
        var conn = await _db.GetConnectionAsync();
        return await conn.InsertAsync(r);
    }
    public async Task<int> UpdateAsync(RecurringExpense r)
    {
        var conn = await _db.GetConnectionAsync();
        return await conn.UpdateAsync(r);
    }
    public async Task<int> DeleteAsync(int id)
    {
        var conn = await _db.GetConnectionAsync();
        return await conn.DeleteAsync<RecurringExpense>(id);
    }
}
