using SmartExpenseTracker.Models;
using SmartExpenseTracker.Services;
namespace SmartExpenseTracker.Repositories;

public class GoalRepository : IGoalRepository
{
    private readonly DatabaseService _db;
    public GoalRepository(DatabaseService db) => _db = db;

    public async Task<List<Goal>> GetAllAsync()
    {
        var conn = await _db.GetConnectionAsync();
        return await conn.Table<Goal>().ToListAsync();
    }
    public async Task<Goal?> GetByIdAsync(int id)
    {
        var conn = await _db.GetConnectionAsync();
        return await conn.Table<Goal>().FirstOrDefaultAsync(g => g.Id == id);
    }
    public async Task<int> AddAsync(Goal goal)
    {
        var conn = await _db.GetConnectionAsync();
        return await conn.InsertAsync(goal);
    }
    public async Task<int> UpdateAsync(Goal goal)
    {
        var conn = await _db.GetConnectionAsync();
        return await conn.UpdateAsync(goal);
    }
    public async Task<int> DeleteAsync(int id)
    {
        var conn = await _db.GetConnectionAsync();
        return await conn.DeleteAsync<Goal>(id);
    }
}
