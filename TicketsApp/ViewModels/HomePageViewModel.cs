using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicketsApp.Interfaces;
using TicketsApp.Models;
using TicketsApp.Views;

namespace TicketsApp.ViewModels;

public partial class HomePageViewModel : BaseViewModel, IQueryAttributable
{
    private const int FirstPage = 1;

    private readonly IEngineerTicketService _engineerTicketService;
    private readonly IMetaParser _metaParser;
    private readonly ITicketParser _ticketParser;
    private readonly ITicketService _ticketService;
    [ObservableProperty] private int _assignedCurrentPage;
    [ObservableProperty] private int _assignedLastPage;

    [ObservableProperty] private ObservableCollection<Ticket>? _assignedTickets;
    [ObservableProperty] private int _assignedTotalPages;

    [ObservableProperty] private bool? _isAdmin;
    [ObservableProperty] private bool? _isEngineer;
    [ObservableProperty] private bool? _isNotEngineer;
    [ObservableProperty] private bool? _isRefreshing;
    [ObservableProperty] private int _unassignedCurrentPage;
    [ObservableProperty] private int _unassignedLastPage;

    [ObservableProperty] private ObservableCollection<Ticket>? _unassignedTickets;
    [ObservableProperty] private int _unassignedTotalPages;

    [ObservableProperty] private string? _username;
    [ObservableProperty] private int _userTicketCurrentPage;
    [ObservableProperty] private int _userTicketLastPage;

    [ObservableProperty] private ObservableCollection<Ticket>? _userTickets;
    [ObservableProperty] private int _userTicketTotalPages;

    public HomePageViewModel(IAppState appState, IEngineerTicketService engineerTicketService,
        ITicketService ticketService, ITicketParser ticketParser,
        IMetaParser metaParser) : base(appState)
    {
        _engineerTicketService = engineerTicketService;
        _ticketService = ticketService;
        _ticketParser = ticketParser;
        _metaParser = metaParser;


        Username = AppState.CurrentUser?.Name;
        IsEngineer = AppState.CurrentUser?.IsEngineer ?? false;
        IsAdmin = AppState.CurrentUser?.IsAdmin;
        IsNotEngineer = !IsEngineer;

        AssignedCurrentPage = FirstPage;
        UnassignedCurrentPage = FirstPage;
        UserTicketCurrentPage = FirstPage;

        _ = LoadTickets();
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("refresh") && query["refresh"] is bool refresh && refresh) await RefreshAsync();
    }

    [RelayCommand]
    private async Task LoadTickets()
    {
        if (IsEngineer ?? false)
        {
            await LoadAssignedTickets(FirstPage);
            await LoadUnAssignedTickets(FirstPage);
            return;
        }

        await LoadUserTickets(FirstPage);
    }

    [RelayCommand]
    public async Task RefreshAsync()
    {
        IsRefreshing = true;
        try
        {
            await LoadTickets();
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task TicketDetails(Ticket ticket)
    {
        if (ticket is null)
            return;

        var navigationParameters = new Dictionary<string, object>
        {
            { "ticket", ticket },
            { "isEngineer", _isEngineer }
        };

        await Shell.Current.GoToAsync(nameof(TicketDetailsPage), true, navigationParameters);
    }

    [RelayCommand]
    private async Task LoadUserTickets(int page)
    {
        var reqParams = new TicketQueryParameters
        {
            Page = page
        };
        var response = await _ticketService.GetTickets(reqParams);
        UserTickets = await _ticketParser.ParseTickets(response);


        var meta = await _metaParser.Parse(response);
        UserTicketTotalPages = meta.Total;
        UserTicketLastPage = meta?.LastPage ?? FirstPage;
    }

    [RelayCommand]
    private async Task UserTicketPreviousPage()
    {
        if (UserTicketCurrentPage == FirstPage)
            return;

        UserTicketCurrentPage -= 1;
        await LoadUserTickets(UserTicketCurrentPage);
    }

    [RelayCommand]
    private async Task UserTicketNextPage()
    {
        if (UserTicketCurrentPage == UserTicketLastPage)
            return;

        UserTicketCurrentPage += 1;
        await LoadUserTickets(UserTicketCurrentPage);
    }

    private async Task LoadAssignedTickets(int page)
    {
        var response = await _engineerTicketService.GetTickets(page);
        AssignedTickets = await _ticketParser.ParseTickets(response);

        var meta = await _metaParser.Parse(response);

        AssignedTotalPages = meta.Total;
        AssignedLastPage = meta?.LastPage ?? FirstPage;
    }

    [RelayCommand]
    private async Task AssignedPreviousPage()
    {
        if (AssignedCurrentPage == FirstPage)
            return;

        AssignedCurrentPage -= 1;
        await LoadAssignedTickets(AssignedCurrentPage);
    }

    [RelayCommand]
    private async Task AssignedNextPage()
    {
        if (AssignedCurrentPage == AssignedLastPage)
            return;

        AssignedCurrentPage += 1;
        await LoadAssignedTickets(AssignedCurrentPage);
    }


    private async Task LoadUnAssignedTickets(int page)
    {
        var requestParams = new TicketQueryParameters
        {
            Assigned = false,
            Page = page
        };

        var response = await _ticketService.GetTickets(requestParams);
        UnassignedTickets = await _ticketParser.ParseTickets(response);

        var meta = await _metaParser.Parse(response);

        UnassignedTotalPages = meta.Total;
        UnassignedLastPage = meta?.LastPage ?? FirstPage;
    }

    [RelayCommand]
    private async Task UnassignedNextPage()
    {
        if (UnassignedCurrentPage == UnassignedLastPage)
            return;

        UnassignedCurrentPage += 1;
        await LoadUnAssignedTickets(UnassignedCurrentPage);
    }

    [RelayCommand]
    private async Task UnassignedPreviousPage()
    {
        if (UnassignedCurrentPage == FirstPage)
            return;

        UnassignedCurrentPage -= 1;
        await LoadUnAssignedTickets(UnassignedCurrentPage);
    }
}