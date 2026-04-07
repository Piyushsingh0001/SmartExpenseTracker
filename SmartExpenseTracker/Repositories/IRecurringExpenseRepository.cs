using SmartExpenseTracker.Models;
namespace SmartExpenseTracker.Repositories;

public interface IRecurringExpenseRepository
{
    Task<List<RecurringExpense>> GetAllActiveAsync();
    Task<int> AddAsync(RecurringExpense r);
    Task<int> UpdateAsync(RecurringExpense r);
    Task<int> DeleteAsync(int id);
}
