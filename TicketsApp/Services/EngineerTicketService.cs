using System.Collections.ObjectModel;
using System.Text.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;
using TicketsApp.Parsers;

namespace TicketsApp.Services;

public class EngineerTicketService(HttpClient httpClient, ITicketParser ticketParser, TicketParsingConfig config)
    : IEngineerTicketService
{
    public async Task<ObservableCollection<Ticket>> GetEngineerTickets()
    {
        var tickets = new ObservableCollection<Ticket>();
        var currentPage = 1;
        var apiUrl = $"https://tickets.test/api/v1/engineer/tickets?page={currentPage}";

        while (true)
        {
            var response = await httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
                break;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var root = doc.RootElement.GetProperty("meta");
            var totalPages = root.GetProperty("last_page").GetInt32();

            foreach (var ticket in await ticketParser.ParseTickets(response, config)) tickets.Add(ticket);

            if (currentPage >= totalPages)
                break;

            currentPage++;
            apiUrl = $"https://tickets.test/api/v1/engineer/tickets?page={currentPage}";
        }

        return tickets;
    }
}