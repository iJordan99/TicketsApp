using TicketsApp.Interfaces;
using TicketsApp.Models;

namespace TicketsApp.Parsers;

public class QueryStringBuilder : IQueryStringBuilder
{
    public string BuildQueryString(TicketQueryParameters parameters)
    {
        var queryParams = new List<string>();

        if (!string.IsNullOrEmpty(parameters.Sort))
            queryParams.Add($"sort={Uri.EscapeDataString(parameters.Sort)}");

        if (!string.IsNullOrEmpty(parameters.Status))
            queryParams.Add($"status={Uri.EscapeDataString(parameters.Status)}");

        if (parameters.Priority.HasValue)
            queryParams.Add($"priority={parameters.Priority.Value}");

        if (parameters.Include?.Length > 0)
            queryParams.Add($"include={Uri.EscapeDataString(string.Join(",", parameters.Include))}");

        if (parameters.Assigned.HasValue)
            queryParams.Add($"assigned={parameters.Assigned.Value.ToString().ToLower()}");

        if (parameters.Page.HasValue)
            queryParams.Add($"page={parameters.Page.Value}");

        return string.Join("&", queryParams);
    }
}