using CommunityToolkit.Mvvm.ComponentModel;
namespace TicketsApp.Models;

public partial class User(string? email, int id, bool isEngineer, string? name)
    : ObservableObject
{
    //ObservableProperty so that models with fields of type User receives updates when binding
    [ObservableProperty] private string? _email = email;
    [ObservableProperty] private int _id = id;
    [ObservableProperty] private bool _isEngineer = isEngineer;
    [ObservableProperty] private string? _name = name;

}