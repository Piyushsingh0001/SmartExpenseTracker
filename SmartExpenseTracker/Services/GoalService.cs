using SmartExpenseTracker.Models;
using SmartExpenseTracker.Repositories;

namespace SmartExpenseTracker.Services;

public class GoalService
{
    private readonly IGoalRepository _repo;
    public GoalService(IGoalRepository repo) => _repo = repo;

    public Task<List<Goal>> GetAllAsync() => _repo.GetAllAsync();
    public Task<int> AddAsync(Goal goal) => _repo.AddAsync(goal);
    public Task<int> UpdateAsync(Goal goal) => _repo.UpdateAsync(goal);
    public Task<int> DeleteAsync(int id) => _repo.DeleteAsync(id);

    public async Task AddSavingsAsync(int goalId, decimal amount)
    {
        var goal = await _repo.GetByIdAsync(goalId);
        if (goal is null) return;
        goal.SavedAmount += amount;
        await _repo.UpdateAsync(goal);
    }
}
