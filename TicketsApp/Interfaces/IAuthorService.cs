using TicketsApp.Models;

namespace TicketsApp.Interfaces;

public interface IAuthorService
{
    public Task<HttpResponseMessage> GetAuthors(UserQueryParameter parameters);

    public Task<HttpResponseMessage> CreateTicket(Ticket ticket, User author);
}