using System.Globalization;
namespace SmartExpenseTracker.Converters;

/// <summary>Converts 0-100 double to 0.0-1.0 for ProgressBar.</summary>
public class PercentageConverter : IValueConverter
{
    public object Convert(object? value, Type t, object? param, CultureInfo c)
        => value is double d ? Math.Min(d / 100.0, 1.0) : 0.0;
    public object ConvertBack(object? value, Type t, object? param, CultureInfo c)
        => throw new NotImplementedException();
}
