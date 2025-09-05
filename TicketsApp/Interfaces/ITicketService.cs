using TicketsApp.Models;

namespace TicketsApp.Interfaces;

public interface ITicketService
{
    Task<HttpResponseMessage> GetTicketWithIncludes(Ticket ticket, string includes);
    Task<HttpResponseMessage> AddComment(string comment, Ticket ticket);
    Task<HttpResponseMessage> GetTickets(TicketQueryParameter? parameters);
    Task<HttpResponseMessage> UpdateTicket(Ticket ticket, User user, (string Key, string Value)[] data);
    Task<HttpResponseMessage> DeleteTicket(Ticket ticket);
    Task<HttpResponseMessage> CreateTicket(Ticket ticket, User user);
}