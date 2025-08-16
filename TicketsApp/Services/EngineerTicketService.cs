using System.Text;
using System.Text.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;
using TicketsApp.Utilities;

namespace TicketsApp.Services;

/// <summary>
///     Provides services for fetching and managing engineer tickets.
/// </summary>
public class EngineerTicketService(
    HttpClient httpClient,
    JsonSerializerOptions serializerOptions)
    : IEngineerTicketService
{
    public Task<HttpResponseMessage> AssignEngineer(Ticket ticket, int engineer)
    {
        var payload = new
        {
            data = new
            {
                attributes = new
                {
                    engineer
                }
            }
        };
        var jsonPayload = JsonSerializer.Serialize(payload, serializerOptions);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var uri = $"https://tickets.test/api/v1/tickets/{ticket.Id}/engineer";

        return httpClient.PostAsync(uri, content);
    }

    public async Task<HttpResponseMessage> GetTickets(int? page)
    {
        return await httpClient.GetAsync(EngineerTicketApiRoutes.GetEngineerAssignedTickets(page));
    }
}