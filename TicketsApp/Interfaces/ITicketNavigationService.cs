using TicketsApp.Models;

namespace TicketsApp.Interfaces;

public interface ITicketNavigationService
{
    Task ShowTicketDetailsAsync(Ticket ticket, bool isEngineer);
}