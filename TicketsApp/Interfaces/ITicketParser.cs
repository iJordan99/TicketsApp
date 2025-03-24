using System.Collections.ObjectModel;
using TicketsApp.Models;
namespace TicketsApp.Interfaces;

public interface ITicketParser
{
    Task<ObservableCollection<Ticket>> ParseTickets(HttpResponseMessage response);
    Task<TicketWithIncludes?> ParseTicketWithIncludes(HttpResponseMessage response);
}