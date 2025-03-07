using System.Collections.ObjectModel;
using TicketsApp.Models;
using TicketsApp.Parsers;

namespace TicketsApp.Interfaces;

public interface ITicketParser
{
    Task<ObservableCollection<Ticket>> ParseTickets(HttpResponseMessage response, TicketParsingConfig config);
}