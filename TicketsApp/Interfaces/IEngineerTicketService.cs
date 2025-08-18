using TicketsApp.Models;

namespace TicketsApp.Interfaces;

public interface IEngineerTicketService
{
    Task<HttpResponseMessage> GetTickets(int? page = null);
    Task<HttpResponseMessage> AssignEngineer(Ticket ticket, User engineer);
    Task<HttpResponseMessage> RemoveEngineer(Ticket ticket, User engineer);
}