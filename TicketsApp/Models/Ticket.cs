using CommunityToolkit.Mvvm.ComponentModel;
namespace TicketsApp.Models;

public partial class Ticket : ObservableObject
{

    [ObservableProperty] private DateTime _createdOn;

    [ObservableProperty] private string? _description;

    [ObservableProperty] private string? _errorCode;

    [ObservableProperty] private int _id;

    [ObservableProperty] private string? _priority;

    [ObservableProperty] private string? _reproductionStep;

    [ObservableProperty] private string? _status;

    [ObservableProperty] private string? _title;

    [ObservableProperty] private string? _type;

    [ObservableProperty] private DateTime _updatedOn;

    public Ticket(int id, string? errorCode, string? title, string? description, string? type, string? status,
        string? priority, string? reproductionStep, DateTime createdOn, DateTime updatedOn)
    {
        Id = id;
        ErrorCode = errorCode;
        Title = title;
        Description = description;
        Type = type;
        Status = status;
        Priority = priority;
        ReproductionStep = reproductionStep;
        CreatedOn = createdOn;
        UpdatedOn = updatedOn;
    }
}