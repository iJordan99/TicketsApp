namespace TicketsApp.Models;

public class TicketQueryParameter
{
    public string? Sort { get; init; }
    public string? Status { get; init; }
    public string? Priority { get; init; }
    public string[]? Include { get; init; }
    public bool? Assigned { get; init; }

    public int? Page { get; init; }

    public int? Engineer { get; init; }

    public int? Author { get; init; }
}