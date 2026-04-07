using CommunityToolkit.Maui;
using Microcharts.Maui;
using Plugin.LocalNotification;
using SmartExpenseTracker.Repositories;
using SmartExpenseTracker.Services;
using SmartExpenseTracker.ViewModels;
using SmartExpenseTracker.Views;

namespace SmartExpenseTracker;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMicrocharts()
            .UseLocalNotification()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Services
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<IExpenseRepository, ExpenseRepository>();
        builder.Services.AddSingleton<IGoalRepository, GoalRepository>();
        builder.Services.AddSingleton<IBudgetRepository, BudgetRepository>();
        builder.Services.AddSingleton<IRecurringExpenseRepository, RecurringExpenseRepository>();
        builder.Services.AddSingleton<AppSettingsRepository>();
        builder.Services.AddSingleton<ExpenseService>();
        builder.Services.AddSingleton<BudgetService>();
        builder.Services.AddSingleton<GoalService>();
        builder.Services.AddSingleton<RecurringExpenseService>();
        builder.Services.AddSingleton<NotificationService>();
        builder.Services.AddSingleton<SecurityService>();
        builder.Services.AddSingleton<BackupService>();

        // ViewModels
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<AddExpenseViewModel>();
        builder.Services.AddTransient<ExpenseListViewModel>();
        builder.Services.AddTransient<GoalsViewModel>();
        builder.Services.AddTransient<BudgetViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();
        builder.Services.AddTransient<PINViewModel>();

        // Views
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<AddExpensePage>();
        builder.Services.AddTransient<ExpenseListPage>();
        builder.Services.AddTransient<GoalsPage>();
        builder.Services.AddTransient<BudgetPage>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<PINLockPage>();

        var app = builder.Build();

        // Ensure DatabaseService is created without blocking the UI thread
        var _ = app.Services.GetRequiredService<DatabaseService>();

        return app;
    }
}

