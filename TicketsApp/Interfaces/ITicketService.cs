using TicketsApp.Models;

namespace TicketsApp.Interfaces;

public interface ITicketService
{
    Task<HttpResponseMessage> GetTicketWithIncludes(Ticket ticket);
    Task<HttpResponseMessage> AddComment(string comment, Ticket ticket);

    Task<HttpResponseMessage> GetTickets(TicketQueryParameters? parameters);
}