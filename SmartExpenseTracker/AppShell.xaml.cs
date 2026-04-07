using SmartExpenseTracker.Views;
namespace SmartExpenseTracker;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        // Register routes for non-tab pages
        Routing.RegisterRoute(nameof(AddExpensePage), typeof(AddExpensePage));
        Routing.RegisterRoute(nameof(PINLockPage), typeof(PINLockPage));
    }
}
