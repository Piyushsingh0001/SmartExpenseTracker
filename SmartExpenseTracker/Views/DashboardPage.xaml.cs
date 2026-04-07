using SmartExpenseTracker.Converters;
using SmartExpenseTracker.ViewModels;
using SmartExpenseTracker.Views;

namespace SmartExpenseTracker.Views;

public partial class DashboardPage : ContentPage
{
    private readonly DashboardViewModel _vm;

    public DashboardPage(DashboardViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
        Resources["PctConverter"] = new PercentageConverter();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.LoadCommand.Execute(null);
    }

    // Navigate to Add Expense (FAB workaround via code-behind)
    private async void OnAddExpenseTapped(object sender, EventArgs e)
        => await Shell.Current.GoToAsync(nameof(AddExpensePage));
}
