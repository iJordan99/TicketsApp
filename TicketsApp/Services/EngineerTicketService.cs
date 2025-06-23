using System.Collections.ObjectModel;
using System.Text.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;
using TicketsApp.Parsers;
using TicketsApp.Utilities;
namespace TicketsApp.Services;

/// <summary>
///     Provides services for fetching and managing engineer tickets.
/// </summary>
public class EngineerTicketService(HttpClient httpClient, ITicketParser ticketParser, GlobalParsingConfig globalParsingConfig)
    : IEngineerTicketService
{
    /// <summary>
    ///     Retrieves engineer tickets. If a specific page is provided, returns tickets from that page only.
    ///     Otherwise, iterates through multiple pages to retrieve all tickets.
    /// </summary>
    /// <param name="page">Optional. When specified, retrieves tickets only from the specified page number.</param>
    /// <returns>
    ///     An observable collection of <see cref="Ticket" /> objects containing engineer tickets.
    ///     If page parameter is provided, returns tickets from that specific page only;
    ///     otherwise, returns tickets from all available pages.
    /// </returns>
    public async Task<ObservableCollection<Ticket>> GetEngineerTickets(int? page = null)
    {
        var tickets = new ObservableCollection<Ticket>();

        if (page.HasValue)
        {
            var (pageTickets, _) = await FetchTicketsPageAsync(page.Value);
            foreach (var ticket in pageTickets)
            {
                tickets.Add(ticket);
            }

            return tickets;
        }

        const int currentPage = 1;
        int totalPages;

        do
        {
            var (pageTickets, pages) = await FetchTicketsPageAsync(currentPage);
            totalPages = pages;

            foreach (var ticket in pageTickets)
            {
                tickets.Add(ticket);
            }

            page++;
        } while (currentPage <= totalPages);

        return tickets;
    }

    /// <summary>
    ///     Retrieves metadata information from the API's engineer assigned tickets endpoint.
    ///     This method specifically fetches the first page to extract metadata about pagination
    ///     and other response properties.
    /// </summary>
    /// <returns>
    ///     An ApiResponseMetaData object containing metadata information from the API response,
    ///     or null if the deserialization fails.
    /// </returns>
    /// <exception cref="HttpRequestException">
    ///     Thrown when the HTTP request fails or returns a non-success status code.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the API response doesn't contain the expected 'meta' property.
    /// </exception>
    /// <exception cref="JsonException">
    ///     Thrown when the JSON deserialization fails due to invalid format or unexpected data structure.
    /// </exception>
    public async Task<ApiResponseMetaData?> GetResponseMetaData()
    {
        var httpResponse = await httpClient.GetAsync(EngineerTicketApiRoutes.GetEngineerAssignedTickets(1));

        if (!httpResponse.IsSuccessStatusCode)
            throw new HttpRequestException("Failed to fetch tickets for page 1.");

        var json = await httpResponse.Content.ReadAsStringAsync();
        var response = JsonSerializer.Deserialize<JsonElement>(json);

        if (!response.TryGetProperty("meta", out var metaElement))
        {
            throw new InvalidOperationException("Response is missing required 'meta' property");
        }

        return JsonSerializer.Deserialize<ApiResponseMetaData>(metaElement.GetRawText());
    }


    /// <summary>
    ///     Fetches a single page of tickets and extracts pagination information from the response.
    /// </summary>
    /// <param name="page">The page number to fetch (1-based index)</param>
    /// <returns>
    ///     A tuple containing:
    ///     - Tickets: Collection of tickets from the requested page
    ///     - TotalPages: Total number of available pages from pagination metadata
    /// </returns>
    /// <exception cref="HttpRequestException">
    ///     Thrown when the HTTP request fails or returns a non-success status code
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the response is missing required metadata
    /// </exception>
    private async Task<(ObservableCollection<Ticket> Tickets, int TotalPages)> FetchTicketsPageAsync(int page)
    {
        var httpResponse = await httpClient.GetAsync(EngineerTicketApiRoutes.GetEngineerAssignedTickets(page));

        if (!httpResponse.IsSuccessStatusCode)
            throw new HttpRequestException($"Failed to fetch tickets for page {page}.");

        var json = await httpResponse.Content.ReadAsStringAsync();
        var response = JsonSerializer.Deserialize<JsonElement>(json);

        if (!response.TryGetProperty("meta", out var metaElement))
        {
            throw new InvalidOperationException("Response is missing required 'meta' property");
        }

        var metadata = JsonSerializer.Deserialize<ApiResponseMetaData>(metaElement.GetRawText());
        var tickets = await ticketParser.ParseTickets(httpResponse);


        return (tickets, metadata.LastPage);
    }
}