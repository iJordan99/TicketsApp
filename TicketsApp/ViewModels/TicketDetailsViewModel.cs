using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicketsApp.Interfaces;
using TicketsApp.Models;

namespace TicketsApp.ViewModels;

public partial class TicketDetailsViewModel(IAppState appState, ITicketService ticketService)
    : BaseViewModel(appState), IQueryAttributable
{
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private Ticket? _ticket;
    [ObservableProperty] private TicketData? _ticketData;
    [ObservableProperty] private Comment _newComment;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Ticket = query["ticket"] as Ticket;
        LoadDataAsync();
    }

    private async void LoadDataAsync()
    {
        await GetTicketData();
    }

    [RelayCommand]
    private async Task GetTicketData()
    {
        if (Ticket != null) TicketData = await ticketService.GetTicketData(Ticket);
    }

    [RelayCommand]
    private async Task AddComment()
    {
        await ticketService.AddComment(NewComment);
    }
}