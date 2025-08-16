using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicketsApp.Interfaces;
using TicketsApp.Models;

namespace TicketsApp.ViewModels;

public partial class TicketDetailsViewModel(
    IAppState appState,
    ITicketParser ticketParser,
    IErrorParser errorParser,
    ITicketService ticketService,
    IEngineerTicketService engineerTicketService)
    : BaseViewModel(appState), IQueryAttributable
{
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private bool _isRefreshing;
    [ObservableProperty] private string _newComment;
    [ObservableProperty] private Ticket? _ticket;
    [ObservableProperty] private TicketWithIncludes? _ticketData;

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
        if (Ticket != null)
            TicketData = await ticketParser.ParseTicketWithIncludes(await ticketService.GetTicketWithIncludes(Ticket));
    }

    [RelayCommand]
    private async Task AddComment()
    {
        if (Ticket == null)
            return;

        var result = await ticketService.AddComment(NewComment, Ticket);

        if (!result.IsSuccessStatusCode)
        {
            var errors = await errorParser.Parse(result);
        }

        NewComment = string.Empty;
        await RefreshAsync();
        await Shell.Current.DisplayAlert("Success", "Comment added.", "OK");
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;

        await GetTicketData();

        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task AssignEngineer()
    {
        if (Ticket != null)
        {
            var assigned = await engineerTicketService.AssignEngineer(Ticket, 11);
        }
    }
}