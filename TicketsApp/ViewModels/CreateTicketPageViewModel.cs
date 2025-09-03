using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicketsApp.Interfaces;
using TicketsApp.Models;

namespace TicketsApp.ViewModels;

public partial class CreateTicketPageViewModel(
    IAppState appState,
    ITicketService ticketService,
    IPostApiResponseService postApiResponseService
) : BaseViewModel(appState)
{
    [ObservableProperty] private string _description;
    [ObservableProperty] private string _error;
    [ObservableProperty] private string _reproduction;
    [ObservableProperty] private string _title;

    [RelayCommand]
    private async Task CreateTicket()
    {
        var ticket = new Ticket
        {
            Title = Title,
            Description = Description,
            Type = "incident",
            Status = "N",
            Priority = "low",
            ErrorCode = Error,
            ReproductionStep = Reproduction
        };

        if (appState.CurrentUser != null)
        {
            var response = await ticketService.CreateTicket(ticket, appState.CurrentUser);
            var error = await postApiResponseService.ProcessResponse(response);
        }
    }
}