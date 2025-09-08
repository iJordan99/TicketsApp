using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicketsApp.Interfaces;
using TicketsApp.Models;

namespace TicketsApp.ViewModels;

public partial class SearchPageViewModel(
    IAppState appState,
    ITicketService ticketService,
    ITicketParser ticketParser,
    ITicketNavigationService ticketNavigationService
) : BaseViewModel(appState)
{
    [ObservableProperty] private ObservableCollection<Ticket> _tickets;

    [RelayCommand]
    private async Task SearchTickets(string searchTerm)
    {
        var parameter = new TicketQueryParameter
        {
            Title = searchTerm
        };

        var response = await ticketService.GetTickets(parameter);
        var tickets = await ticketParser.ParseTickets(response);

        if (tickets.Count > 0) Tickets = tickets;
    }

    [RelayCommand]
    private async Task TicketDetails(Ticket ticket)
    {
        if (ticket == null) return;

        await ticketNavigationService.ShowTicketDetailsAsync(ticket, IsEngineer ?? false);
    }
}