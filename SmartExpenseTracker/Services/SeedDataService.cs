using SmartExpenseTracker.Models;
using SmartExpenseTracker.Repositories;
using SmartExpenseTracker.Services;

namespace SmartExpenseTracker.Services;

/// <summary>
/// Call once on first launch to pre-populate demo data.
/// Check a settings flag to avoid re-seeding.
/// </summary>
public class SeedDataService
{
    private readonly IExpenseRepository _expenseRepo;
    private readonly IGoalRepository _goalRepo;
    private readonly IBudgetRepository _budgetRepo;
    private readonly IRecurringExpenseRepository _recurringRepo;
    private readonly AppSettingsRepository _settings;

    public SeedDataService(
        IExpenseRepository expenseRepo,
        IGoalRepository goalRepo,
        IBudgetRepository budgetRepo,
        IRecurringExpenseRepository recurringRepo,
        AppSettingsRepository settings)
    {
        _expenseRepo = expenseRepo;
        _goalRepo = goalRepo;
        _budgetRepo = budgetRepo;
        _recurringRepo = recurringRepo;
        _settings = settings;
    }

    public async Task SeedIfFirstRunAsync()
    {
        var seeded = await _settings.GetAsync("seeded");
        if (seeded == "true") return;

        var now = DateTime.Now;

        // Sample expenses
        await _expenseRepo.AddAsync(new Expense { Amount = 450, Category = "Food", Type = "Needs", PaymentMode = "UPI", Date = now.AddDays(-1), Notes = "Lunch" });
        await _expenseRepo.AddAsync(new Expense { Amount = 1200, Category = "Transport", Type = "Needs", PaymentMode = "Card", Date = now.AddDays(-2), Notes = "Flight" });
        await _expenseRepo.AddAsync(new Expense { Amount = 599, Category = "Entertainment", Type = "Wants", PaymentMode = "UPI", Date = now.AddDays(-3), Notes = "Netflix" });
        await _expenseRepo.AddAsync(new Expense { Amount = 5000, Category = "Investment", Type = "Investment", PaymentMode = "UPI", Date = now.AddDays(-5), Notes = "SIP" });
        await _expenseRepo.AddAsync(new Expense { Amount = 2300, Category = "Shopping", Type = "Wants", PaymentMode = "Card", Date = now.AddDays(-7), Notes = "Clothes" });

        // Budget
        await _budgetRepo.UpsertAsync(new Budget { Month = now.Month, Year = now.Year, MonthlyLimit = 30000 });

        // Goals
        await _goalRepo.AddAsync(new Goal { GoalName = "Emergency Fund", TargetAmount = 100000, SavedAmount = 35000, Deadline = now.AddYears(1) });
        await _goalRepo.AddAsync(new Goal { GoalName = "Laptop Upgrade", TargetAmount = 80000, SavedAmount = 15000, Deadline = now.AddMonths(8) });

        // Recurring
        await _recurringRepo.AddAsync(new RecurringExpense { Name = "Rent", Amount = 12000, Category = "Rent", PaymentMode = "UPI", DayOfMonth = 1, LastProcessed = DateTime.MinValue, IsActive = true });
        await _recurringRepo.AddAsync(new RecurringExpense { Name = "Internet Bill", Amount = 699, Category = "Utilities", PaymentMode = "UPI", DayOfMonth = 5, LastProcessed = DateTime.MinValue, IsActive = true });

        await _settings.SetAsync("seeded", "true");
    }
}
