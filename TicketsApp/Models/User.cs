using CommunityToolkit.Mvvm.ComponentModel;

namespace TicketsApp.Models;

public partial class User : ObservableObject
{
    //ObservableProperty so that models with fields of type User receives updates when binding
    [ObservableProperty] private string? _email;
    [ObservableProperty] private int _id;
    [ObservableProperty] private bool _isEngineer;
    [ObservableProperty] private string? _name;
}