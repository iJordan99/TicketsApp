using System.Collections.ObjectModel;

namespace TicketsApp.Models;

public class TicketData : Ticket
{
    public TicketData(Ticket ticket, Collection<User> engineers, User? author, Collection<Comment> comments)
        : base(ticket.Id, ticket.ErrorCode, ticket.Title, ticket.Description, ticket.Type, ticket.Status,
            ticket.Priority,
            ticket.ReproductionStep, ticket.CreatedOn, ticket.UpdatedOn)
    {
        Engineers = engineers;
        Author = author;
        Comments = comments;
    }

    public Collection<User> Engineers { get; set; }
    public User? Author { get; set; }
    public Collection<Comment> Comments { get; set; }
}