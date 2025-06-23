using System.Globalization;
namespace TicketsApp.Converters;

public class PriorityToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string priority)
        {
            switch (priority.ToLower())
            {
                case "low":
                    return Color.FromRgb(0, 128, 0);
                case "medium":
                    return Color.FromRgb(255, 140, 0);
                case "high":
                    return Color.FromRgb(255, 0, 0);
                default:
                    return Color.FromRgb(26, 45, 66);
            }
        }

        return Color.FromRgb(26, 45, 66);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}