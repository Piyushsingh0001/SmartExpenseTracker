// IGoalRepository.cs
using SmartExpenseTracker.Models;
using SmartExpenseTracker.Services;
namespace SmartExpenseTracker.Repositories;

public interface IGoalRepository
{
    Task<List<Goal>> GetAllAsync();
    Task<Goal?> GetByIdAsync(int id);
    Task<int> AddAsync(Goal goal);
    Task<int> UpdateAsync(Goal goal);
    Task<int> DeleteAsync(int id);
}

// GoalRepository.cs
