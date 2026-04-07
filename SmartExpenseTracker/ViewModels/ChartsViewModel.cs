#if IOS
using UserNotifications;
using ObjCRuntime;
using UIKit;
using Foundation;
#endif
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microcharts;
using SkiaSharp;
using SmartExpenseTracker.Services;
using System.Runtime.InteropServices;
using static System.Net.WebRequestMethods;

namespace SmartExpenseTracker.ViewModels;

public partial class ChartsViewModel : BaseViewModel
{
    private readonly ExpenseService _service;

    [ObservableProperty] private Chart? _categoryChart;
    [ObservableProperty] private Chart? _monthlyChart;

    public ChartsViewModel(ExpenseService service)
    {
        _service = service;
        Title = "Charts";
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        IsBusy = true;
        try
        {
            await BuildCategoryChartAsync();
            await BuildMonthlyChartAsync();
        }
        finally { IsBusy = false; }
    }

    private async Task BuildCategoryChartAsync()
    {
        var catTotals = await _service.GetCategoryTotalsThisMonthAsync();
        var colors = new[] {
            SKColor.Parse("#2E75B6"), SKColor.Parse("#E74C3C"), SKColor.Parse("#27AE60"),
            SKColor.Parse("#F39C12"), SKColor.Parse("#8E44AD"), SKColor.Parse("#17A589")
        };
        int i = 0;
        var entries = catTotals.Select(kv => new ChartEntry((float)kv.Value)
        {
            Label = kv.Key,
            ValueLabel = kv.Value.ToString("N0"),
            Color = colors[i++ % colors.Length]
        }).ToList();

        CategoryChart = new DonutChart
        {
            Entries = entries,
            LabelTextSize = 32,
            BackgroundColor = SKColors.White
        };
    }

    private async Task BuildMonthlyChartAsync()
    {
        // Last 6 months
        var entries = new List<ChartEntry>();
        for (int m = 5; m >= 0; m--)
        {
            var date = DateTime.Now.AddMonths(-m);
            var from = new DateTime(date.Year, date.Month, 1);
            var to = from.AddMonths(1).AddTicks(-1);
            var expenses = await _service.GetAllAsync();
            var total = expenses
                .Where(e => e.Date >= from && e.Date <= to)
                .Sum(e => (float)e.Amount);
            entries.Add(new ChartEntry(total)
            {
                Label = date.ToString("MMM"),
                ValueLabel = total.ToString("N0"),
                Color = SKColor.Parse("#2E75B6")
            });
        }
        MonthlyChart = new BarChart
        {
            Entries = entries,
            LabelTextSize = 28,
            BackgroundColor = SKColors.White
        };
    }
}
