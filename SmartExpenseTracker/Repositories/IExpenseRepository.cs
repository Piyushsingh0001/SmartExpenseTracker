using SmartExpenseTracker.Models;

namespace SmartExpenseTracker.Repositories;

public interface IExpenseRepository
{
    Task<List<Expense>> GetAllAsync();
    Task<List<Expense>> GetByDateRangeAsync(DateTime from, DateTime to);
    Task<List<Expense>> GetByCategoryAsync(string category);
    Task<Expense?> GetByIdAsync(int id);
    Task<int> AddAsync(Expense expense);
    Task<int> UpdateAsync(Expense expense);
    Task<int> DeleteAsync(int id);
    Task<decimal> GetTotalByDateRangeAsync(DateTime from, DateTime to);
}
