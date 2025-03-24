using TicketsApp.Interfaces;
using TicketsApp.Models;
namespace TicketsApp.Services;

public class TicketService(HttpClient httpClient, ITicketParser ticketParser) : ITicketService
{
    public async Task<TicketWithIncludes?> GetTicketWithIncludes(Ticket ticket)
    {
        var response =
            await httpClient.GetAsync(
                $"https://tickets.test/api/v1/tickets/{ticket.Id}?include=comment,author,engineer");

        if (response.IsSuccessStatusCode)
        {
            return await ticketParser.ParseTicketWithIncludes(response);
        }

        return null;
    }

    public Task AddComment(Comment newComment)
    {
        throw new NotImplementedException();
    }
}