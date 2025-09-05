using System.Reflection;
using TicketsApp.Interfaces;

namespace TicketsApp.Parsers;

public class QueryStringBuilder : IQueryStringBuilder
{
    public string BuildQueryString<T>(T? parameter) where T : class
    {
        if (parameter == null)
            return string.Empty;

        var queryParams = new List<string>();

        // Get all public properties of the parameter object
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in properties)
        {
            var value = prop.GetValue(parameter);

            if (value == null)
                continue;

            switch (value)
            {
                case string s when !string.IsNullOrEmpty(s):
                    queryParams.Add($"{prop.Name.ToLower()}={Uri.EscapeDataString(s)}");
                    break;
                case bool b:
                    queryParams.Add($"{prop.Name.ToLower()}={b.ToString().ToLower()}");
                    break;
                case Array arr when arr.Length > 0:
                    queryParams.Add(
                        $"{prop.Name.ToLower()}={Uri.EscapeDataString(string.Join(",", arr.Cast<object>()))}");
                    break;
                default:
                    queryParams.Add($"{prop.Name.ToLower()}={value}");
                    break;
            }
        }

        return string.Join("&", queryParams);
    }
}