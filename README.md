## SmartExpenseTracker

SmartExpenseTracker is a simple personal finance app built with .NET MAUI.  
It helps you track your monthly budget and day‑to‑day expenses so you can clearly see how your money is being used.

### Features

- **Dashboard overview**
  - Today’s total spending
  - This month’s total spending
  - Monthly budget limit, remaining amount, and percentage used
  - Category breakdown of this month’s expenses

- **Budgets**
  - Set a **monthly budget limit**
  - See how much you’ve **spent vs remaining**
  - Visual progress bar for budget usage

- **Expenses**
  - Add expenses with:
    - Amount
    - Category (Food, Transport, Rent, etc.)
    - Type (Needs, Wants, Investment)
    - Payment mode (Cash, UPI, Card)
    - Date and notes
  - Edit or delete expenses (via the list page)

- **Recurring expenses**
  - Define recurring monthly expenses (e.g. rent, subscriptions)
  - App auto‑adds them each month once, on or after the configured day

- **PIN lock (optional)**
  - Protect your data with a simple PIN screen on startup (if enabled in settings)

### Technology

- **Framework**: .NET MAUI (.NET 9, multi‑platform)
- **Database**: SQLite via `SQLite-net` (async APIs)
- **Architecture**:
  - MVVM with `CommunityToolkit.Mvvm`
  - Dependency injection for services and repositories
  - Shell navigation

### Project structure (high level)

- `SmartExpenseTracker/SmartExpenseTracker/`
  - `App.xaml`, `App.xaml.cs` – application startup
  - `AppShell.xaml`, `AppShell.xaml.cs` – tabbed navigation (Dashboard, Expenses, Budget, Goals, Settings)
  - `Models/` – `Expense`, `Budget`, `Goal`, `RecurringExpense`, `AppSettings`
  - `Services/` – `DatabaseService`, `ExpenseService`, `BudgetService`, `GoalService`, `RecurringExpenseService`, `NotificationService`, `SecurityService`, `BackupService`
  - `Repositories/` – SQLite data access for each model
  - `ViewModels/` – `DashboardViewModel`, `AddExpenseViewModel`, `ExpenseListViewModel`, `BudgetViewModel`, `GoalsViewModel`, `SettingsViewModel`, `PINViewModel`
  - `Views/` – XAML pages such as `DashboardPage`, `AddExpensePage`, `ExpenseListPage`, `BudgetPage`, `GoalsPage`, `SettingsPage`, `PINLockPage`
  - `Resources/` – styles, colors, images

### Database

- The app uses a local SQLite database file named **`smart_expense.db3`**.
- You **do not need to create this manually**:
  - On first run, `DatabaseService` creates the file inside the app’s data folder.
  - All required tables (`Expense`, `Budget`, `Goal`, `RecurringExpense`, `AppSettings`) are created automatically.

### Running the app (development)

1. **Prerequisites**
   - Visual Studio 2022 (latest) with:
     - **.NET Multi‑platform App UI development** workload
     - Android SDKs and emulators installed
   - .NET SDK 9 (preview/installed as required by the project)

2. **Open the solution**
   - Open the folder `SmartExpenseTracker` in Visual Studio.
   - Ensure the startup project is `SmartExpenseTracker` (the .NET MAUI project).

3. **Restore and build**
   - Visual Studio will restore NuGet packages automatically.
   - Build the project (Debug / Any CPU).

4. **Run on Android emulator**
   - Select an Android emulator from the device dropdown.
   - Press **F5** (Debug) or **Ctrl+F5** (Run without debugging).
   - On first run, the app will:
     - Create the SQLite database
     - Create tables

### Installing on your Android phone

1. Build the app in **Release** mode for Android from Visual Studio.
2. Locate the generated APK / app bundle in the project’s `bin/Release/net9.0-android/` output.
3. Copy the APK to your Android device (USB, cloud, etc.).
4. On your phone:
   - Enable installation from unknown sources (if needed).
   - Open the APK and install the app.
5. After installation, you can use it normally:
   - Set a monthly budget in the **Budget** tab.
   - Add expenses from the **Dashboard** or **Expenses** tab.

### Notes / Limitations

- All data is stored **locally on the device** in SQLite.
- There is currently **no cloud sync or backup** unless you implement it (there is a `BackupService` placeholder to extend).
- This app is designed for **personal monthly money tracking**, not for multi‑user or enterprise scenarios.

### Contributing

This repository mainly serves as a **personal finance helper app**.  
If you fork it for your own use:

- Adjust categories, UI, or flows to match your needs.
- Feel free to add features like:
  - Export to CSV
  - Cloud backup/restore
  - More detailed analytics and charts.

