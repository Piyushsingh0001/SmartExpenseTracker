using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
#if IOS
using UserNotifications;
using ObjCRuntime;
using UIKit;
using Foundation;
#endif
using SmartExpenseTracker.Models;
using SmartExpenseTracker.Services;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartExpenseTracker.ViewModels;

public partial class AddExpenseViewModel : BaseViewModel
{
    private readonly ExpenseService _service;

    [ObservableProperty] private decimal _amount;
    [ObservableProperty] private string _category = string.Empty;
    [ObservableProperty] private string _type = "Needs";
    [ObservableProperty] private string _paymentMode = "Cash";
    [ObservableProperty] private DateTime _date = DateTime.Now;
    [ObservableProperty] private string _notes = string.Empty;
    [ObservableProperty] private int? _editId;

    public List<string> Types { get; } = new() { "Needs", "Wants", "Investment" };
    public List<string> PaymentModes { get; } = new() { "Cash", "UPI", "Card" };
    public List<string> Categories { get; } = new()
        { "Food", "Transport", "Rent", "Utilities", "Entertainment",
          "Healthcare", "Shopping", "Education", "Investment", "Other" };

    public AddExpenseViewModel(ExpenseService service)
    {
        _service = service;
        Title = "Add Expense";
    }

    public void LoadForEdit(Expense expense)
    {
        EditId = expense.Id;
        Amount = expense.Amount;
        Category = expense.Category;
        Type = expense.Type;
        PaymentMode = expense.PaymentMode;
        Date = expense.Date;
        Notes = expense.Notes;
        Title = "Edit Expense";
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Amount <= 0 || string.IsNullOrWhiteSpace(Category))
        {
            await Shell.Current.DisplayAlert("Validation", "Amount and category are required.", "OK");
            return;
        }

        IsBusy = true;
        try
        {
            var expense = new Expense
            {
                Amount = Amount,
                Category = Category,
                Type = Type,
                PaymentMode = PaymentMode,
                Date = Date,
                Notes = Notes
            };

            if (EditId.HasValue)
            {
                expense.Id = EditId.Value;
                await _service.UpdateAsync(expense);
            }
            else
                await _service.AddAsync(expense);

            await Shell.Current.GoToAsync("..");
        }
        finally { IsBusy = false; }
    }
}
