namespace SmartExpenseTracker.Views;

public partial class BudgetPage : ContentPage
{
    public BudgetPage(ViewModels.BudgetViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        Resources["PctConverter"] = new Converters.PercentageConverter();
    }
}