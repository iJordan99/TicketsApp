namespace TicketsApp.Parsers;

public class TicketParsingConfig(
    string rootProperty = "data",
    string attributesProperty = "attributes",
    string includesProperty = "includes")
{

    private readonly Dictionary<string, string> _commentFieldMappings = new()
    {
        { "CommentId", "id" },
        { "TicketId", "ticket" },
        { "CommentText", "comment" },
        { "CreatedAt", "created_at" }
    };

    private readonly Dictionary<string, string> _fieldMappings = new()
    {
        { "Id", "id" },
        { "ErrorCode", "error_code" },
        { "Title", "title" },
        { "Description", "description" },
        { "Status", "status" },
        { "Type", "type" },
        { "Priority", "priority" },
        { "ReproductionStep", "reproduction_step" },
        { "CreatedOn", "created_at" },
        { "UpdatedOn", "updated_at" }
    };

    private readonly Dictionary<string, string> _includesMappings = new()
    {
        { "Author", "author" },
        { "Engineer", "engineer" },
        { "Comments", "comments" }
    };

    private readonly Dictionary<string, string> _userFieldMappings = new()
    {
        { "UserId", "id" },
        { "Name", "name" },
        { "Email", "email" },
        { "IsEngineer", "is_engineer" },
        { "AssignedAt", "assigned_at" }
    };

    public string RootProperty { get; } = rootProperty;
    public string AttributesProperty { get; } = attributesProperty;
    public string IncludesProperty { get; } = includesProperty;

    public IReadOnlyDictionary<string, string> FieldMappings => _fieldMappings;
    public IReadOnlyDictionary<string, string> IncludesMappings => _includesMappings;
    public IReadOnlyDictionary<string, string> UserFieldMappings => _userFieldMappings;
    public IReadOnlyDictionary<string, string> CommentFieldMappings => _commentFieldMappings;
}