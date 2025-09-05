using CommunityToolkit.Mvvm.ComponentModel;
using TicketsApp.Interfaces;

namespace TicketsApp.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    protected readonly IAppState AppState;

    [ObservableProperty] private bool? _isAdmin;
    [ObservableProperty] private bool? _isEngineer;
    [ObservableProperty] private bool? _isNotEngineer;
    [ObservableProperty] private string? _username;

    protected BaseViewModel(IAppState appState)
    {
        AppState = appState;
        IsEngineer = AppState.CurrentUser?.IsEngineer ?? false;
        IsNotEngineer = !(IsEngineer ?? false);
        IsAdmin = AppState.CurrentUser?.IsAdmin;
        Username = AppState.CurrentUser?.Name;
    }
}