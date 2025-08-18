using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicketsApp.Interfaces;
using TicketsApp.Models;

namespace TicketsApp.ViewModels;

public partial class TicketDetailsViewModel(
    IAppState appState,
    ITicketParser ticketParser,
    IErrorParser errorParser,
    IUserParser userParser,
    ITicketService ticketService,
    IEngineerService engineerService,
    IEngineerTicketService engineerTicketService)
    : BaseViewModel(appState), IQueryAttributable
{
    [ObservableProperty] private ObservableCollection<User> _engineers;

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private bool _isRefreshing;
    [ObservableProperty] private string _newComment;
    [ObservableProperty] private User? _selectedEngineer;
    [ObservableProperty] private Ticket? _ticket;
    [ObservableProperty] private TicketWithIncludes? _ticketData;


    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Ticket = query["ticket"] as Ticket;
        LoadDataAsync();
    }

    private async void LoadDataAsync()
    {
        var engineers = await engineerService.GetEngineers();
        Engineers = await userParser.ParseMany(engineers);
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
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;

        await GetTicketData();

        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task AssignEngineer(User engineer)
    {
        if (Ticket != null)
        {
            var assigned = await engineerTicketService.AssignEngineer(Ticket, engineer);
            // var response = await errorParser.Parse(assigned);
            await RefreshAsync();
        }
    }

    [RelayCommand]
    private async Task RemoveEngineer(User engineer)
    {
        var remove = await engineerTicketService.RemoveEngineer(Ticket, engineer);

        if (remove.IsSuccessStatusCode) await RefreshAsync();
    }
}