using SmartExpenseTracker.Models;
namespace SmartExpenseTracker.Repositories;

public interface IBudgetRepository
{
    Task<Budget?> GetByMonthYearAsync(int month, int year);
    Task<int> UpsertAsync(Budget budget);
}
