using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicketsApp.Interfaces;
using TicketsApp.Models;
using TicketsApp.Views;

namespace TicketsApp.ViewModels;

public enum TicketPageType
{
    Assigned,
    Unassigned,
    User
}

public enum PriorityFilterType
{
    None,
    Low,
    Medium,
    High
}

public partial class HomePageViewModel : BaseViewModel
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

    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private bool? _isRefreshing;
    [ObservableProperty] private string _selectedAssignedPriority;

    [ObservableProperty] private string _selectedUnassignedPriority;

    [ObservableProperty] private int _unassignedCurrentPage;
    [ObservableProperty] private int _unassignedLastPage;
    [ObservableProperty] private ObservableCollection<Ticket>? _unassignedTickets;
    [ObservableProperty] private int _unassignedTotalPages;

    [ObservableProperty] private int _userTicketCurrentPage;
    [ObservableProperty] private int _userTicketLastPage;
    [ObservableProperty] private ObservableCollection<Ticket>? _userTickets;
    [ObservableProperty] private int _userTicketTotalPages;

    public HomePageViewModel(IAppState appState, IEngineerTicketService engineerTicketService,
        ITicketService ticketService, ITicketParser ticketParser, IMetaParser metaParser)
        : base(appState)
    {
        _engineerTicketService = engineerTicketService;
        _ticketService = ticketService;
        _ticketParser = ticketParser;
        _metaParser = metaParser;

        AssignedCurrentPage = FirstPage;
        UnassignedCurrentPage = FirstPage;
        UserTicketCurrentPage = FirstPage;
    }

    public async Task InitializeAsync()
    {
        IsBusy = true;
        try
        {
            await LoadTickets();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task LoadTickets()
    {
        if (IsEngineer ?? false)
        {
            await Task.WhenAll(
                LoadAssignedTickets(FirstPage),
                LoadUnassignedTickets(FirstPage)
            );
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
        catch (Exception ex)
        {
            Debug.WriteLine($"Error refreshing tickets: {ex.Message}");
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task TicketDetails(Ticket ticket)
    {
        if (ticket == null) return;

        var navigationParameters = new Dictionary<string, object>
        {
            { "ticket", ticket },
            { "isEngineer", IsEngineer ?? false }
        };

        await Shell.Current.GoToAsync(nameof(TicketDetailsPage), true, navigationParameters);
    }

    [RelayCommand]
    private async Task CreateTicket()
    {
        await Shell.Current.GoToAsync(nameof(CreateTicketPage));
    }

    [RelayCommand]
    private async Task LoadUserTickets(int page)
    {
        var reqParams = new TicketQueryParameter { Page = page };
        var response = await _ticketService.GetTickets(reqParams);
        UserTickets = await _ticketParser.ParseTickets(response);

        var meta = await _metaParser.Parse(response);
        UserTicketTotalPages = meta?.Total ?? 0;
        UserTicketLastPage = meta?.LastPage ?? FirstPage;
    }

    private async Task LoadAssignedTickets(int page, string? priority = null)
    {
        var requestParams = new TicketQueryParameter
        {
            Engineer = AppState.CurrentUser?.Id,
            Assigned = true,
            Page = page,
            Priority = priority
        };

        var response = await _ticketService.GetTickets(requestParams);
        AssignedTickets = await _ticketParser.ParseTickets(response);

        var meta = await _metaParser.Parse(response);
        AssignedTotalPages = meta?.Total ?? 0;
        AssignedLastPage = meta?.LastPage ?? FirstPage;
    }

    private async Task LoadUnassignedTickets(int page, string? priority = null)
    {
        var requestParams = new TicketQueryParameter
        {
            Assigned = false,
            Page = page,
            Priority = priority
        };

        var response = await _ticketService.GetTickets(requestParams);
        UnassignedTickets = await _ticketParser.ParseTickets(response);

        var meta = await _metaParser.Parse(response);
        UnassignedTotalPages = meta?.Total ?? 0;
        UnassignedLastPage = meta?.LastPage ?? FirstPage;
    }

    [RelayCommand]
    private async Task NextPage(TicketPageType type)
    {
        switch (type)
        {
            case TicketPageType.Assigned:
                if (AssignedCurrentPage == AssignedLastPage) return;
                AssignedCurrentPage++;
                await LoadAssignedTickets(AssignedCurrentPage, SelectedAssignedPriority);
                break;

            case TicketPageType.Unassigned:
                if (UnassignedCurrentPage == UnassignedLastPage) return;
                UnassignedCurrentPage++;
                await LoadUnassignedTickets(UnassignedCurrentPage, SelectedUnassignedPriority);
                break;

            case TicketPageType.User:
                if (UserTicketCurrentPage == UserTicketLastPage) return;
                UserTicketCurrentPage++;
                await LoadUserTickets(UserTicketCurrentPage);
                break;
        }
    }

    [RelayCommand]
    private async Task PreviousPage(TicketPageType type)
    {
        switch (type)
        {
            case TicketPageType.Assigned:
                if (AssignedCurrentPage == FirstPage) return;
                AssignedCurrentPage--;
                await LoadAssignedTickets(AssignedCurrentPage, SelectedAssignedPriority);
                break;

            case TicketPageType.Unassigned:
                if (UnassignedCurrentPage == FirstPage) return;
                UnassignedCurrentPage--;
                await LoadUnassignedTickets(UnassignedCurrentPage, SelectedUnassignedPriority);
                break;

            case TicketPageType.User:
                if (UserTicketCurrentPage == FirstPage) return;
                UserTicketCurrentPage--;
                await LoadUserTickets(UserTicketCurrentPage);
                break;
        }
    }

    [RelayCommand]
    private async Task FilterAssignedByPriority(PriorityFilterType type)
    {
        SelectedAssignedPriority = type.ToString();
        AssignedCurrentPage = FirstPage;
        if (SelectedAssignedPriority == "None") await LoadAssignedTickets(AssignedCurrentPage);
        await LoadAssignedTickets(AssignedCurrentPage, type.ToString());
    }

    [RelayCommand]
    private async Task FilterUnAssignedByPriority(PriorityFilterType type)
    {
        SelectedUnassignedPriority = type.ToString();
        UnassignedCurrentPage = FirstPage;
        if (SelectedUnassignedPriority == "None") await LoadUnassignedTickets(UnassignedCurrentPage);
        await LoadUnassignedTickets(UnassignedCurrentPage, type.ToString());
    }
}