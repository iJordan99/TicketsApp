using System.Globalization;
using TicketsApp.Models;
namespace TicketsApp.Converters;

public class EngineerToColourConverter : IValueConverter
{

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is User user)
        {
            return user.IsEngineer ? Colors.Blue : Colors.Black;
        }

        return Colors.Black;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}