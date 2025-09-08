using TicketsApp.Interfaces;
using TicketsApp.Models;
using TicketsApp.Views;

namespace TicketsApp.Services;

public class TicketNavigationService : ITicketNavigationService
{
    public async Task ShowTicketDetailsAsync(Ticket ticket, bool isEngineer)
    {
        if (ticket == null) return;

        var navigationParameters = new Dictionary<string, object>
        {
            { "ticket", ticket },
            { "isEngineer", isEngineer }
        };

        await Shell.Current.GoToAsync(nameof(TicketDetailsPage), true, navigationParameters);
    }
}