using System.Text.Json.Serialization;
namespace TicketsApp.Models;

public class ApiResponseMetaData
{
    [JsonPropertyName("current_page")] public int CurrentPage { get; set; }

    [JsonPropertyName("last_page")] public int LastPage { get; set; }

    [JsonPropertyName("total")] public int TotalItems { get; set; }

    [JsonPropertyName("per_page")] public int ItemsPerPage { get; set; }
}