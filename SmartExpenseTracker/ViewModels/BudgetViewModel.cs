using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartExpenseTracker.Services;
using static System.Net.WebRequestMethods;

namespace SmartExpenseTracker.ViewModels;

public partial class BudgetViewModel : BaseViewModel
{
    private readonly BudgetService _service;

    [ObservableProperty] private decimal _monthlyLimit;
    [ObservableProperty] private decimal _spent;
    [ObservableProperty] private decimal _remaining;
    [ObservableProperty] private double _percent;

    public BudgetViewModel(BudgetService service)
    {
        _service = service;
        Title = "Budget";
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        IsBusy = true;
        try
        {
            var budget = await _service.GetCurrentMonthBudgetAsync();
            MonthlyLimit = budget?.MonthlyLimit ?? 0;
            var (spent, rem, pct) = await _service.GetSummaryAsync();
            Spent = spent; Remaining = rem; Percent = pct;
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task SaveBudgetAsync()
    {
        if (MonthlyLimit <= 0)
        {
            await Shell.Current.DisplayAlert("Validation", "Enter a valid budget amount.", "OK");
            return;
        }
        await _service.SetBudgetAsync(MonthlyLimit);
        await LoadAsync();
        await Shell.Current.DisplayAlert("Saved", "Monthly budget updated.", "OK");
    }
}
