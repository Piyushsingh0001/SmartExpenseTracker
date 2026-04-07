using SmartExpenseTracker.Services;
using SmartExpenseTracker.Views;

namespace SmartExpenseTracker;

public partial class App : Application
{
    private readonly SecurityService _securityService;
    private readonly RecurringExpenseService _recurringService;

    public App(SecurityService securityService, RecurringExpenseService recurringService)
    {
        InitializeComponent();
        _securityService = securityService;
        _recurringService = recurringService;
        MainPage = new AppShell();
    }

    protected override async void OnStart()
    {
        base.OnStart();
        // Auto-add recurring expenses
        await _recurringService.ProcessRecurringExpensesAsync();
        // Show PIN lock if enabled
        if (_securityService.IsPINEnabled())
        {
            await Shell.Current.GoToAsync(nameof(PINLockPage));
        }
    }
}
