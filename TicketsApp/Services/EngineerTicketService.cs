using System.Collections.ObjectModel;
using System.Text.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;
using TicketsApp.Parsers;
using TicketsApp.Utilities;

namespace TicketsApp.Services;

/// <summary>
/// Provides services for fetching and managing engineer tickets.
/// </summary>
public class EngineerTicketService(HttpClient httpClient, ITicketParser ticketParser, GlobalParsingConfig globalParsingConfig)
    : IEngineerTicketService
{
    /// <summary>
    /// Retrieves all engineer tickets by iterating through multiple pages of results.
    /// </summary>
    /// <returns>
    /// An observable collection of <see cref="Ticket"/> objects containing engineer tickets from all pages.
    /// </returns>
    public async Task<ObservableCollection<Ticket>> GetEngineerTickets()
    {
        var tickets = new ObservableCollection<Ticket>();
        var page = 1;
        int totalPages;

        do
        {
            var (pageTickets, pages) = await FetchTicketsPageAsync(page);
            totalPages = pages;

            foreach (var ticket in pageTickets)
            {
                tickets.Add(ticket);
            }

            page++;
        } while (page <= totalPages);

        return tickets;
    }

    /// <summary>
    /// Fetches a single page of tickets assigned to an engineer and determines the total number of pages available.
    /// </summary>
    /// <param name="page">The page number to fetch tickets for.</param>
    /// <returns>
    /// A tuple containing an observable collection of tickets for the specified page
    /// and the total number of pages available.
    /// </returns>
    /// <exception cref="HttpRequestException">
    /// Thrown when the HTTP request fails or the response is not successful.
    /// </exception>
    private async Task<(ObservableCollection<Ticket> Tickets, int TotalPages)> FetchTicketsPageAsync(int page)
    {
        var httpResponse = await httpClient.GetAsync(EngineerTicketApiRoutes.GetEngineerAssignedTickets(page));

        if (!httpResponse.IsSuccessStatusCode)
            throw new HttpRequestException($"Failed to fetch tickets for page {page}.");

        var json = await httpResponse.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var metaElement = doc.RootElement.GetProperty(globalParsingConfig.MetaProperty);


        var totalPages = metaElement.GetProperty(globalParsingConfig.PaginationMappings["LastPage"]).GetInt32();

        var tickets = await ticketParser.ParseTickets(httpResponse);

        return (tickets, totalPages);
    }
}