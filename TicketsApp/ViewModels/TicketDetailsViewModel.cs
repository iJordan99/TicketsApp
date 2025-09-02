using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicketsApp.Interfaces;
using TicketsApp.Models;
using TicketsApp.Views;

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
    [ObservableProperty] private bool? _isEngineer;

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private bool? _isNotEngineer;
    [ObservableProperty] private bool _isRefreshing;
    private Dictionary<string, object> _navigationParameters;
    [ObservableProperty] private string _newComment;
    [ObservableProperty] private User? _selectedEngineer;
    [ObservableProperty] private Ticket? _ticket;
    [ObservableProperty] private TicketWithIncludes? _ticketData;


    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Ticket = query["ticket"] as Ticket;
        IsEngineer = query["isEngineer"] as bool?;
        IsNotEngineer = !IsEngineer;
        LoadDataAsync();
    }

    private async void LoadDataAsync()
    {
        var engineers = await engineerService.GetEngineers();
        Engineers = await userParser.ParseMany(engineers);
        await GetTicketData();
        _navigationParameters = new Dictionary<string, object> { { "refresh", true } };
    }

    [RelayCommand]
    private async Task GetTicketData()
    {
        var includes = "comment,author,engineer";
        if (Ticket != null)
            TicketData =
                await ticketParser.ParseTicketWithIncludes(await ticketService.GetTicketWithIncludes(Ticket, includes));
        ;
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
        var response = await engineerTicketService.AssignEngineer(Ticket, engineer);

        if (response.IsSuccessStatusCode) await RefreshAsync();
    }

    [RelayCommand]
    private async Task RemoveEngineer(User engineer)
    {
        var remove = await engineerTicketService.RemoveEngineer(Ticket, engineer);

        if (remove.IsSuccessStatusCode) await RefreshAsync();
    }

    [RelayCommand]
    private async Task UpdatePriority(string priority)
    {
        var data = new[] { (Key: "priority", Value: priority) };

        var updated = await ticketService.UpdateTicket(Ticket, AppState.CurrentUser, data);

        if (updated.IsSuccessStatusCode) await RefreshAsync();
    }

    [RelayCommand]
    private async Task UpdateStatus(string status)
    {
        var data = new[] { (Key: "status", Value: status) };

        var updated = await ticketService.UpdateTicket(Ticket, AppState.CurrentUser, data);

        if (updated.IsSuccessStatusCode) await RefreshAsync();
    }

    [RelayCommand]
    private async Task DeleteTicket(Ticket ticket)
    {
        var deleted = await ticketService.DeleteTicket(ticket);
        if (deleted.IsSuccessStatusCode)
            await Shell.Current.GoToAsync($"///{nameof(HomePage)}", true, _navigationParameters);
    }
}