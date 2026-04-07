using SQLite;
using SmartExpenseTracker.Models;

namespace SmartExpenseTracker.Services;

/// <summary>
/// Singleton service that manages the SQLite connection.
/// All repositories obtain the connection from here.
/// </summary>
public class DatabaseService
{
    private SQLiteAsyncConnection? _connection;
    private readonly SemaphoreSlim _initLock = new(1, 1);
    private const string DB_NAME = "smart_expense.db3";

    public async Task<SQLiteAsyncConnection> GetConnectionAsync()
    {
        if (_connection is not null) return _connection;

        await _initLock.WaitAsync();
        try
        {
            if (_connection is not null) return _connection;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, DB_NAME);
            var conn = new SQLiteAsyncConnection(
                dbPath,
                SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);

            // Create all tables before exposing the connection to callers
            await conn.CreateTableAsync<Expense>();
            await conn.CreateTableAsync<Goal>();
            await conn.CreateTableAsync<Budget>();
            await conn.CreateTableAsync<RecurringExpense>();
            await conn.CreateTableAsync<AppSettings>();

            _connection = conn;
            return _connection;
        }
        finally
        {
            _initLock.Release();
        }
    }

    public string GetDatabasePath()
        => Path.Combine(FileSystem.AppDataDirectory, DB_NAME);
}
