using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicketsApp.Interfaces;
using TicketsApp.Models;

namespace TicketsApp.ViewModels;

public partial class LoginPageViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    
    public LoginPageViewModel(IAppState appState,IAuthService authService) : base(appState)
    {
        _authService = authService;
    }
    
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    string _entryEmail;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    string _entryPassword;
    
    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task Login()
    {
        try
        {
            var authenticate = await _authService.LoginAsync<object>(new LoginRequest(EntryEmail, EntryPassword));
            if (authenticate != null && authenticate.Errors.Any())
            {
                var errorMessages = string.Join(Environment.NewLine, authenticate.Errors.Select(e => e.Message));
                await Shell.Current.DisplayAlert("Login Failed", errorMessages, "OK");
                return;
            }
            
            await Shell.Current.GoToAsync("//HomePage");
        }
        catch (Exception ex)
        {
            await Shell.Current
                .DisplayAlert("Error", $"Login failed: {ex.Message}", "OK");
        }
    }
    
    private bool CanLogin()
    {
        return !string.IsNullOrEmpty(EntryEmail) && !string.IsNullOrEmpty(EntryPassword);
    }
}