using System.Globalization;
namespace SmartExpenseTracker.Converters;

public class BoolToColorConverter : IValueConverter
{
    public object Convert(object? value, Type t, object? param, CultureInfo c)
        => value is true ? Colors.Green : Colors.Red;
    public object ConvertBack(object? value, Type t, object? param, CultureInfo c)
        => throw new NotImplementedException();
}
