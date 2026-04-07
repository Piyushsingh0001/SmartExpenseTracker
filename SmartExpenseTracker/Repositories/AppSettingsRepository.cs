using SmartExpenseTracker.Models;
using SmartExpenseTracker.Services;
namespace SmartExpenseTracker.Repositories;

public class AppSettingsRepository
{
    private readonly DatabaseService _db;
    public AppSettingsRepository(DatabaseService db) => _db = db;

    public async Task<string?> GetAsync(string key)
    {
        var conn = await _db.GetConnectionAsync();
        var s = await conn.Table<AppSettings>().FirstOrDefaultAsync(x => x.Key == key);
        return s?.Value;
    }

    public async Task SetAsync(string key, string value)
    {
        var conn = await _db.GetConnectionAsync();
        var existing = await conn.Table<AppSettings>().FirstOrDefaultAsync(x => x.Key == key);
        if (existing is null)
            await conn.InsertAsync(new AppSettings { Key = key, Value = value });
        else
        {
            existing.Value = value;
            await conn.UpdateAsync(existing);
        }
    }
}
