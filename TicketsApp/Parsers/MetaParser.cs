using System.Text.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;

namespace TicketsApp.Parsers;

public class MetaParser : IMetaParser
{
    public async Task<MetaData> Parse(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var root = doc.RootElement;

        if (!root.TryGetProperty("meta", out var metaElement) || metaElement.ValueKind != JsonValueKind.Object)
            return null;

        return new MetaData(
            metaElement.TryGetProperty("current_page", out var currentPage) ? currentPage.GetInt32() : 0,
            metaElement.TryGetProperty("last_page", out var lastPage) ? lastPage.GetInt32() : 0,
            metaElement.TryGetProperty("total", out var total) ? total.GetInt32() : 0
        );
    }
}