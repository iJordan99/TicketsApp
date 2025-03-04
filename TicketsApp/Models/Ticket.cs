namespace TicketsApp.Models;

public class Ticket
{
    public Ticket(int id, string? errorCode, string? title, string? description, string? status,
        string? priority, string? reproductionStep, DateTime createdOn, DateTime updatedOn, string? type)
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

    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string Type { get; set; }
    public string Priority { get; set; }
    public string? ReproductionStep { get; set; }
    public string? ErrorCode { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}