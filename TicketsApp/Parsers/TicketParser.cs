using System.Collections.ObjectModel;
using System.Text.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;
namespace TicketsApp.Parsers;

/// <summary>
/// The TicketParser class is responsible for parsing ticket-related data from HTTP responses.
/// It processes raw JSON data and converts it into strongly-typed ticket models with optional included data like authors, engineers, and comments.
/// </summary>
public class TicketParser(IJsonParsingHelper jsonHelper, TicketParsingConfig config, IUserParser userParser, ICommentParser commentParser) : ITicketParser
{
    /// <summary>
    /// Represents the key used to identify the author property in the parsed JSON structure.
    /// </summary>
    /// <remarks>
    /// This constant is used as a reference to a specific mapping within a JSON data structure,
    /// where the key corresponds to the "Author" element in the includes section. It plays a
    /// critical role in extracting author-related information during ticket data parsing.
    /// </remarks>
    private const string AuthorKey = "Author";

    /// <summary>
    /// Represents the key used to identify engineers within the ticket parsing process.
    /// </summary>
    /// <remarks>
    /// The key corresponds to the inclusion mapping for engineers in the JSON structure
    /// processed by the TicketParser. It is used to locate and parse engineer-related data.
    /// </remarks>
    private const string EngineerKey = "Engineer";

    /// <summary>
    /// Represents the JSON key used to identify the comments section within the parsed data structure.
    /// This constant is utilized within the ticket parsing logic to extract comment-related information
    /// from the 'includes' property defined in the configuration.
    /// </summary>
    private const string CommentsKey = "Comments";

    /// <summary>
    /// Parses a given HTTP response containing ticket data and returns an observable collection of tickets.
    /// </summary>
    /// <param name="response">The HTTP response message containing the ticket data in JSON format.</param>
    /// <returns>An ObservableCollection of <see cref="Ticket"/> objects parsed from the JSON data. If parsing fails or no tickets are found, an empty collection is returned.</returns>
    public async Task<ObservableCollection<Ticket>> ParseTickets(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        using var jsonDoc = JsonDocument.Parse(json);
        var rootElement = jsonDoc.RootElement;
        
        if (!rootElement.TryGetProperty(config.RootProperty, out var ticketArray) || ticketArray.ValueKind != JsonValueKind.Array)
            return new ObservableCollection<Ticket>();

        var tickets = new ObservableCollection<Ticket>();
        foreach (var ticketElement in ticketArray.EnumerateArray())
        {
            var ticket = ParseTicket(ticketElement);
            if (ticket != null)
                tickets.Add(ticket);
        }
        return tickets;
    }

    /// <summary>
    /// Parses a response to extract a ticket with related includes such as engineers, author, and comments.
    /// </summary>
    /// <param name="response">The HTTP response containing the ticket data to be parsed.</param>
    /// <returns>An instance of <see cref="TicketWithIncludes"/> containing the parsed ticket and its associated data,
    /// or null if the parsing fails.</returns>
    public async Task<TicketWithIncludes?> ParseTicketWithIncludes(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        using var jsonDocument = JsonDocument.Parse(json);
        var rootElement = jsonDocument.RootElement;

        if (!rootElement.TryGetProperty(config.RootProperty, out var dataElement) || dataElement.ValueKind != JsonValueKind.Object)
            return null;

        var ticket = ParseTicket(dataElement);
        if (ticket == null) return null;

        var author = ParseAuthor(dataElement);
        var engineers = ParseEngineers(dataElement);
        var comments = ParseComments(dataElement);

        return new TicketWithIncludes(ticket, engineers, author, comments);
    }

    /// Parses the author information from the provided JSON element.
    /// <param name="dataElement">
    /// The JSON element containing data from which the author should be extracted.
    /// This is expected to be a part of the root JSON structure associated with the ticket details.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="User"/> class if the author information is successfully parsed;
    /// otherwise, null if the data does not contain valid author information or parsing fails.
    /// </returns>
    private User? ParseAuthor(JsonElement dataElement)
    {
        if (dataElement.TryGetProperty(config.IncludesProperty, out var includesElement) &&
            includesElement.ValueKind == JsonValueKind.Object &&
            includesElement.TryGetProperty(config.IncludesMappings[AuthorKey], out var authorElement))
        {
            return userParser.Parse(authorElement);
        }
        return null;
    }

    /// <summary>
    /// Extracts and parses engineers from the given JSON element.
    /// </summary>
    /// <param name="dataElement">
    /// The JSON element containing ticket data, including the includes property which might hold the engineers to be parsed.
    /// </param>
    /// <returns>
    /// An observable collection of parsed users marked as engineers, or an empty collection if engineers cannot be parsed.
    /// </returns>
    private ObservableCollection<User> ParseEngineers(JsonElement dataElement)
    {
        var engineers = new ObservableCollection<User>();
        if (dataElement.TryGetProperty(config.IncludesProperty, out var includesElement) &&
            includesElement.ValueKind == JsonValueKind.Object &&
            includesElement.TryGetProperty(config.IncludesMappings[EngineerKey], out var engineersArray) &&
            engineersArray.ValueKind == JsonValueKind.Array)
        {
            foreach (var engineer in engineersArray.EnumerateArray())
            {
                var user = userParser.Parse(engineer);
                if (user != null)
                    engineers.Add(user);
            }
        }
        return engineers;
    }

    /// <summary>
    /// Parses comments from a JSON element and returns a collection of Comment objects.
    /// </summary>
    /// <param name="dataElement">The JSON element containing the data structure from which the comments will be parsed.</param>
    /// <returns>An observable collection of parsed comments as <see cref="Comment"/> objects. If no comments are found, an empty collection is returned.</returns>
    private ObservableCollection<Comment> ParseComments(JsonElement dataElement)
    {
        var comments = new ObservableCollection<Comment>();
        if (dataElement.TryGetProperty(config.IncludesProperty, out var includesElement) &&
            includesElement.ValueKind == JsonValueKind.Object &&
            includesElement.TryGetProperty(config.IncludesMappings[CommentsKey], out var commentsArray) &&
            commentsArray.ValueKind == JsonValueKind.Array)
        {
            foreach (var comment in commentsArray.EnumerateArray())
            {
                var parsedComment = commentParser.Parse(comment);
                if (parsedComment != null)
                    comments.Add(parsedComment);
            }
        }
        return comments;
    }

    /// <summary>
    /// Parses a single ticket from a given JSON element.
    /// </summary>
    /// <param name="ticketElement">The JSON element containing ticket details to parse.</param>
    /// <returns>
    /// A <see cref="Ticket"/> object if the ticket is successfully parsed, or null if required fields are missing.
    /// </returns>
    internal Ticket? ParseTicket(JsonElement ticketElement)
    {
        if (!ticketElement.TryGetProperty(config.FieldMappings["Id"], out var idElement) || idElement.ValueKind == JsonValueKind.Null)
            return null; // ID is required

        var id = idElement.GetInt32();
        var attributes = string.IsNullOrEmpty(config.AttributesProperty)
            ? ticketElement
            : ticketElement.TryGetProperty(config.AttributesProperty, out var attrElement)
                ? attrElement
                : default;

        return new Ticket(
            id,
            jsonHelper.GetStringField(attributes, config.FieldMappings["ErrorCode"]),
            jsonHelper.GetStringField(attributes, config.FieldMappings["Title"]),
            jsonHelper.GetStringField(attributes, config.FieldMappings["Description"]),
            jsonHelper.GetStringField(attributes, config.FieldMappings["Type"]),
            jsonHelper.GetStringField(attributes, config.FieldMappings["Status"]),
            jsonHelper.GetStringField(attributes, config.FieldMappings["Priority"]),
            jsonHelper.GetStringField(attributes, config.FieldMappings["ReproductionStep"]),
            jsonHelper.GetDateTimeField(attributes, config.FieldMappings["CreatedOn"]),
            jsonHelper.GetDateTimeField(attributes, config.FieldMappings["UpdatedOn"])
        );
    }
}