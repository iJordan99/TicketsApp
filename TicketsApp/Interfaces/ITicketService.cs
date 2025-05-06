using TicketsApp.Models;
namespace TicketsApp.Interfaces;

public interface ITicketService
{
    Task<TicketWithIncludes?> GetTicketWithIncludes(Ticket ticket);
    Task<PostApiResponse> AddComment(string comment, Ticket ticket);
}