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
    public Task<HttpResponseMessage> AssignEngineer(Ticket ticket, User engineer)
    {
        var payload = new
        {
            data = new
            {
                attributes = new
                {
                    engineer = engineer.Id
                }
            }
        };
        var jsonPayload = JsonSerializer.Serialize(payload, serializerOptions);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        return httpClient.PostAsync(EngineerTicketApiRoutes.AssignEngineer(ticket), content);
    }

    public Task<HttpResponseMessage> RemoveEngineer(Ticket ticket, User engineer)
    {
        return httpClient.DeleteAsync(EngineerTicketApiRoutes.RemoveEngineer(ticket, engineer));
    }

    public async Task<HttpResponseMessage> GetTickets(int? page)
    {
        return await httpClient.GetAsync(EngineerTicketApiRoutes.EngineerAssignedTickets(page));
    }
}