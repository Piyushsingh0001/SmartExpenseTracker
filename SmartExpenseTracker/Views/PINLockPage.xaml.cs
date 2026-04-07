using SmartExpenseTracker.ViewModels;
namespace SmartExpenseTracker.Views;

public partial class PINLockPage : ContentPage
{
    public PINLockPage(PINViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
