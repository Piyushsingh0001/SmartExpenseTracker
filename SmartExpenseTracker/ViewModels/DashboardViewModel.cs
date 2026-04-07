using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartExpenseTracker.Services;
using static System.Net.WebRequestMethods;

namespace SmartExpenseTracker.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    private readonly ExpenseService _expenseService;
    private readonly BudgetService _budgetService;

    [ObservableProperty] private decimal _todayTotal;
    [ObservableProperty] private decimal _monthTotal;
    [ObservableProperty] private decimal _budgetLimit;
    [ObservableProperty] private decimal _budgetRemaining;
    [ObservableProperty] private double _budgetPercent;
    [ObservableProperty] private List<CategorySummary> _categorySummaries = new();

    public DashboardViewModel(ExpenseService expenseService, BudgetService budgetService)
    {
        _expenseService = expenseService;
        _budgetService = budgetService;
        Title = "Dashboard";
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var today = await _expenseService.GetTodayAsync();
            TodayTotal = today.Sum(e => e.Amount);

            var month = await _expenseService.GetThisMonthAsync();
            MonthTotal = month.Sum(e => e.Amount);

            var (spent, remaining, pct) = await _budgetService.GetSummaryAsync();
            var budget = await _budgetService.GetCurrentMonthBudgetAsync();
            BudgetLimit = budget?.MonthlyLimit ?? 0;
            BudgetRemaining = remaining;
            BudgetPercent = pct;

            var catTotals = await _expenseService.GetCategoryTotalsThisMonthAsync();
            CategorySummaries = catTotals
                .Select(kv => new CategorySummary { Category = kv.Key, Amount = kv.Value })
                .OrderByDescending(c => c.Amount)
                .ToList();
        }
        finally { IsBusy = false; }
    }
}

public class CategorySummary
{
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
