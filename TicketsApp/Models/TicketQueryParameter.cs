namespace TicketsApp.Models;

public class TicketQueryParameter
{
    public string? Sort { get; init; }
    public string? Status { get; init; }
    public int? Priority { get; init; }
    public string[]? Include { get; init; }
    public bool? Assigned { get; init; }

    public int? Page { get; init; }
}