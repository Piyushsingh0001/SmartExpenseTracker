using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartExpenseTracker.Models;
using SmartExpenseTracker.Services;
using static System.Net.WebRequestMethods;

namespace SmartExpenseTracker.ViewModels;

public partial class ExpenseListViewModel : BaseViewModel
{
    private readonly ExpenseService _service;
    private List<Expense> _allExpenses = new();

    [ObservableProperty] private List<Expense> _expenses = new();
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private string _selectedCategory = "All";

    public List<string> FilterCategories { get; } = new()
        { "All", "Food", "Transport", "Rent", "Utilities",
          "Entertainment", "Healthcare", "Shopping", "Education", "Investment", "Other" };

    public ExpenseListViewModel(ExpenseService service)
    {
        _service = service;
        Title = "Expenses";
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            _allExpenses = await _service.GetAllAsync();
            ApplyFilter();
        }
        finally { IsBusy = false; }
    }

    partial void OnSearchTextChanged(string value) => ApplyFilter();
    partial void OnSelectedCategoryChanged(string value) => ApplyFilter();

    private void ApplyFilter()
    {
        var filtered = _allExpenses.AsEnumerable();
        if (SelectedCategory != "All")
            filtered = filtered.Where(e => e.Category == SelectedCategory);
        if (!string.IsNullOrWhiteSpace(SearchText))
            filtered = filtered.Where(e =>
                e.Notes.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                e.Category.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        Expenses = filtered.ToList();
    }

    [RelayCommand]
    private async Task DeleteAsync(Expense expense)
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Delete", "Delete this expense?", "Yes", "No");
        if (!confirm) return;
        await _service.DeleteAsync(expense.Id);
        await LoadAsync();
    }

    [RelayCommand]
    private async Task EditAsync(Expense expense)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(Views.AddExpensePage)}?id={expense.Id}");
    }
}
