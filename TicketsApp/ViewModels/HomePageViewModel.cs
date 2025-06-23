using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicketsApp.Interfaces;
using TicketsApp.Models;
namespace TicketsApp.ViewModels;

public partial class HomePageViewModel : BaseViewModel
{
    private readonly IEngineerTicketService _engineerTicketService;
    [ObservableProperty] private int _currentPage;
    [ObservableProperty] private bool? _engineer;
    [ObservableProperty] private ApiResponseMetaData? _engineerTicketsApiResponseMeta;
    [ObservableProperty] private bool? _isRefreshing;
    [ObservableProperty] private int _lastPage;
    [ObservableProperty] private ObservableCollection<Ticket> _tickets;
    [ObservableProperty] private string? _username;

    public HomePageViewModel(IAppState appState, IEngineerTicketService engineerTicketService) : base(appState)
    {
        _engineerTicketService = engineerTicketService;
        Username = AppState.CurrentUser?.Name;
        Engineer = AppState.CurrentUser?.IsEngineer;
        CurrentPage = 1;
        LoadDataAsync();
    }

    private async void LoadDataAsync()
    {
        await LoadInitialTickets();
        EngineerTicketsApiResponseMeta = await _engineerTicketService.GetResponseMetaData();

        if (EngineerTicketsApiResponseMeta != null)
            LastPage = EngineerTicketsApiResponseMeta.LastPage;
    }

    [RelayCommand]
    private async Task LoadInitialTickets()
    {
        Tickets = await _engineerTicketService.GetEngineerTickets(1);
        //ApiResponseMetaData contains the pagination info
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;

        await LoadInitialTickets();

        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task TicketDetails(Ticket ticket)
    {
        var navigationParameters = new Dictionary<string, object> { { "ticket", ticket } };

        await Shell.Current.GoToAsync("TicketDetailsPage?ticket=", true, navigationParameters);
    }

    [RelayCommand]
    private async Task PreviousPage()
    {
        if (CurrentPage != 1)
        {
            Tickets.Clear();
            CurrentPage -= 1;
        }

        Tickets = await _engineerTicketService.GetEngineerTickets(CurrentPage);
    }


    [RelayCommand]
    private async Task NextPage()
    {
        if (CurrentPage != LastPage)
        {
            Tickets.Clear();
            CurrentPage += 1;
            Tickets = await _engineerTicketService.GetEngineerTickets(CurrentPage);
        }
    }
}