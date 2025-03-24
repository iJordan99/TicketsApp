using System.Collections.ObjectModel;
namespace TicketsApp.Models;

public class TicketWithIncludes(Ticket ticket, Collection<User> engineers, User? author, Collection<Comment> comments)
    : Ticket(ticket.Id, ticket.ErrorCode, ticket.Title, ticket.Description, ticket.Type, ticket.Status,
        ticket.Priority,
        ticket.ReproductionStep, ticket.CreatedOn, ticket.UpdatedOn)
{

    public Collection<User> Engineers { get; set; } = engineers;
    public User? Author { get; set; } = author;
    public Collection<Comment> Comments { get; set; } = comments;
}