using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartExpenseTracker.Models;
using SmartExpenseTracker.Services;
using static System.Net.WebRequestMethods;

namespace SmartExpenseTracker.ViewModels;

public partial class GoalsViewModel : BaseViewModel
{
    private readonly GoalService _service;

    [ObservableProperty] private List<Goal> _goals = new();
    [ObservableProperty] private string _goalName = string.Empty;
    [ObservableProperty] private decimal _targetAmount;
    [ObservableProperty] private decimal _savedAmount;
    [ObservableProperty] private DateTime _deadline = DateTime.Now.AddMonths(6);

    public GoalsViewModel(GoalService service)
    {
        _service = service;
        Title = "Goals";
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        IsBusy = true;
        try { Goals = await _service.GetAllAsync(); }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task AddGoalAsync()
    {
        if (string.IsNullOrWhiteSpace(GoalName) || TargetAmount <= 0)
        {
            await Shell.Current.DisplayAlert("Validation", "Name and target amount are required.", "OK");
            return;
        }
        await _service.AddAsync(new Goal
        {
            GoalName = GoalName,
            TargetAmount = TargetAmount,
            SavedAmount = SavedAmount,
            Deadline = Deadline
        });
        GoalName = string.Empty; TargetAmount = 0; SavedAmount = 0;
        await LoadAsync();
    }

    [RelayCommand]
    private async Task DeleteGoalAsync(Goal goal)
    {
        bool ok = await Shell.Current.DisplayAlert("Delete Goal", "Are you sure?", "Yes", "No");
        if (ok) { await _service.DeleteAsync(goal.Id); await LoadAsync(); }
    }
}
