using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicketsApp.Interfaces;
using TicketsApp.Models;
using TicketsApp.Views;

namespace TicketsApp.ViewModels;

public partial class TicketDetailsViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IEngineerService _engineerService;
    private readonly IEngineerTicketService _engineerTicketService;
    private readonly IErrorParser _errorParser;

    private readonly ITicketParser _ticketParser;
    private readonly ITicketService _ticketService;
    private readonly IUserParser _userParser;
    [ObservableProperty] private ObservableCollection<User> _engineers = new();


    [ObservableProperty] private bool _isRefreshing;
    private Dictionary<string, object> _navigationParameters = new();
    [ObservableProperty] private string _newComment = string.Empty;
    [ObservableProperty] private Ticket? _ticket;
    [ObservableProperty] private TicketWithIncludes? _ticketData;

    public TicketDetailsViewModel(
        IAppState appState,
        ITicketParser ticketParser,
        IErrorParser errorParser,
        IUserParser userParser,
        ITicketService ticketService,
        IEngineerService engineerService,
        IEngineerTicketService engineerTicketService) : base(appState)
    {
        _ticketParser = ticketParser;
        _errorParser = errorParser;
        _userParser = userParser;
        _ticketService = ticketService;
        _engineerService = engineerService;
        _engineerTicketService = engineerTicketService;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null) return;

        if (query.TryGetValue("ticket", out var ticketValue))
            Ticket = ticketValue as Ticket;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await LoadDataAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Initialization error: {ex.Message}");
        }
    }

    private async Task LoadDataAsync()
    {
        try
        {
            var engineers = await _engineerService.GetEngineers();
            Engineers = await _userParser.ParseMany(engineers);
            await GetTicketData();
            _navigationParameters = new Dictionary<string, object> { { "refresh", true } };
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"LoadDataAsync error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task GetTicketData()
    {
        try
        {
            var includes = "comment,author,engineer";
            if (Ticket != null)
                TicketData =
                    await _ticketParser.ParseTicketWithIncludes(
                        await _ticketService.GetTicketWithIncludes(Ticket, includes));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"GetTicketData error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task AddComment()
    {
        if (Ticket == null || string.IsNullOrWhiteSpace(NewComment))
            return;

        var result = await _ticketService.AddComment(NewComment, Ticket);

        if (!result.IsSuccessStatusCode)
        {
            var errors = await _errorParser.Parse(result);
            return;
        }

        NewComment = string.Empty;
        await RefreshAsync();
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;

        try
        {
            await GetTicketData();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"RefreshAsync error: {ex.Message}");
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task AssignEngineer(User engineer)
    {
        if (Ticket == null || engineer == null) return;

        var response = await _engineerTicketService.AssignEngineer(Ticket, engineer);

        if (response.IsSuccessStatusCode)
            await RefreshAsync();
    }

    [RelayCommand]
    private async Task RemoveEngineer(User engineer)
    {
        if (Ticket == null || engineer == null) return;

        var remove = await _engineerTicketService.RemoveEngineer(Ticket, engineer);

        if (remove.IsSuccessStatusCode)
            await RefreshAsync();
    }

    [RelayCommand]
    private async Task UpdatePriority(string priority)
    {
        if (Ticket == null || string.IsNullOrWhiteSpace(priority)) return;

        var data = new[] { (Key: "priority", Value: priority) };

        var updated = await _ticketService.UpdateTicket(Ticket, AppState.CurrentUser, data);

        if (updated.IsSuccessStatusCode)
            await RefreshAsync();
    }

    [RelayCommand]
    private async Task UpdateStatus(string status)
    {
        if (Ticket == null || string.IsNullOrWhiteSpace(status)) return;

        var data = new[] { (Key: "status", Value: status) };

        var updated = await _ticketService.UpdateTicket(Ticket, AppState.CurrentUser, data);

        if (updated.IsSuccessStatusCode)
            await RefreshAsync();
    }

    [RelayCommand]
    private async Task DeleteTicket(Ticket ticket)
    {
        if (ticket == null) return;

        var deleted = await _ticketService.DeleteTicket(ticket);
        if (deleted.IsSuccessStatusCode)
            await Shell.Current.GoToAsync($"///{nameof(HomePage)}", true, _navigationParameters);
    }
}