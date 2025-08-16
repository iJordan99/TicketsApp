using TicketsApp.Models;

namespace TicketsApp.Interfaces;

public interface IQueryStringBuilder
{
    string BuildQueryString(TicketQueryParameters parameters);
}