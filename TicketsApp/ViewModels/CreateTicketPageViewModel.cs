using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicketsApp.Interfaces;
using TicketsApp.Models;
using TicketsApp.Views;

namespace TicketsApp.ViewModels;

public partial class CreateTicketPageViewModel : BaseViewModel
{
    private readonly IAuthorService _authorService;
    private readonly IEngineerService _engineerService;
    private readonly IPostApiResponseService _postApiResponseService;
    private readonly ITicketService _ticketService;
    private readonly IUserParser _userParser;
    [ObservableProperty] private string _description;
    [ObservableProperty] private ObservableCollection<User> _engineers = new();
    [ObservableProperty] private string _error;
    [ObservableProperty] private string _priority;

    [ObservableProperty] private string _reproduction;

    [ObservableProperty] private ObservableCollection<User> _searchedAuthors;

    [ObservableProperty] private User _selectedAuthor;

    [ObservableProperty] private ObservableCollection<User> _selectedEngineers = new();

    [ObservableProperty] private string _status;
    [ObservableProperty] private string _title;
    [ObservableProperty] private string _type;

    [ObservableProperty] private string searchTerm;


    public CreateTicketPageViewModel(
        IAppState appState,
        ITicketService ticketService,
        IPostApiResponseService postApiResponseService,
        IAuthorService authorService,
        IEngineerService engineerService,
        IUserParser userParser) : base(appState)
    {
        _ticketService = ticketService;
        _postApiResponseService = postApiResponseService;
        _authorService = authorService;
        _engineerService = engineerService;
        _userParser = userParser;
    }

    [RelayCommand]
    private async Task CreateTicket()
    {
        if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Description))
        {
            Error = "Title and Description are required.";
            return;
        }

        var ticket = new Ticket
        {
            Title = Title,
            Description = Description,
            Type = Type ?? "incident",
            Status = Status ?? "N",
            Priority = Priority ?? "low",
            ErrorCode = Error ?? "",
            ReproductionStep = Reproduction ?? ""
        };

        if (AppState.CurrentUser != null)
        {
            HttpResponseMessage? response;
            PostApiResponse error;

            switch (AppState.CurrentUser.IsEngineer)
            {
                case false:
                    response = await _ticketService.CreateTicket(ticket, AppState.CurrentUser);
                    break;
                case true:
                    response = await _ticketService.CreateTicket(ticket, SelectedAuthor, SelectedEngineers);
                    error = await _postApiResponseService.ProcessResponse(response);
                    break;
            }

            if (response.IsSuccessStatusCode)
                await Shell.Current.GoToAsync($"///{nameof(HomePage)}", true);
        }
    }


    [RelayCommand]
    private async Task SearchAuthors()
    {
        var searchParams = new UserQueryParameter { Name = SearchTerm };
        var response = await _authorService.GetAuthors(searchParams);
        SearchedAuthors = await _userParser.ParseMany(response);
    }

    [RelayCommand]
    public async Task LoadEngineers()
    {
        var engineers = await _engineerService.GetEngineers();
        Engineers = await _userParser.ParseMany(engineers);
    }
}