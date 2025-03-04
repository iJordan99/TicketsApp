using System.Collections.ObjectModel;
using TicketsApp.Models;

namespace TicketsApp.Interfaces;

public interface IEngineerTicketService
{
    Task<ObservableCollection<Ticket>> GetEngineerTickets();
}