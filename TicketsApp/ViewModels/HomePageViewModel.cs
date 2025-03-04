using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicketsApp.Interfaces;
using TicketsApp.Models;

namespace TicketsApp.ViewModels;

public partial class HomePageViewModel : BaseViewModel
{
    private readonly IEngineerTicketService _engineerTicketService;
    [ObservableProperty] private bool? _engineer;
    [ObservableProperty] private bool? _isRefreshing;
    [ObservableProperty] private ObservableCollection<Ticket> _tickets;
    [ObservableProperty] private string? _username;


    public HomePageViewModel(IAppState appState, IEngineerTicketService engineerTicketService) : base(appState)
    {
        _engineerTicketService = engineerTicketService;
        Username = AppState.CurrentUser?.Name;
        Engineer = AppState.CurrentUser?.IsEngineer;
        LoadDataAsync();
    }

    private async void LoadDataAsync()
    {
        await GetTickets();
    }

    [RelayCommand]
    private async Task GetTickets()
    {
        Tickets = await _engineerTicketService.GetEngineerTickets();
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;

        await GetTickets();

        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task TicketDetails(Ticket ticket)
    {
        var navigationParameters = new Dictionary<string, object> { { "ticket", ticket } };

        await Shell.Current.GoToAsync("TicketDetailsPage?ticket=", true, navigationParameters);
    }
}