using TicketsApp.Models;

namespace TicketsApp.Interfaces;

public interface ITicketService
{
    Task<TicketData?> GetTicketData(Ticket ticket);
    Task AddComment(Comment newComment);
}