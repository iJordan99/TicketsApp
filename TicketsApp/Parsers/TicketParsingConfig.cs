namespace TicketsApp.Parsers;

public class TicketParsingConfig(
    string rootProperty = "data",
    string attributesProperty = "attributes",
    string includesProperty = "includes")
{
    private readonly Dictionary<string, string> _commentFieldMappings = new()
    {
        { "id", "id" },
        { "ticket", "ticket" },
        { "comment", "comment" },
        { "Created_at", "created_at" }
    };

    private readonly Dictionary<string, string> _fieldMappings = new()
    {
        { "id", "id" },
        { "error_code", "error_code" },
        { "title", "title" },
        { "description", "description" },
        { "status", "status" },
        { "type", "type" },
        { "priority", "priority" },
        { "reproduction_step", "reproduction_step" },
        { "created_at", "created_at" },
        { "updated_at", "updated_at" }
    };

    private readonly Dictionary<string, string> _includesMappings = new()
    {
        { "author", "author" },
        { "engineers", "engineers" },
        { "comments", "comments" }
    };

    private readonly Dictionary<string, string> _userFieldMappings = new()
    {
        { "id", "id" },
        { "name", "name" },
        { "email", "email" },
        { "is_engineer", "is_engineer" },
        { "assigned_at", "assigned_at" }
    };

    public string RootProperty { get; } = rootProperty;
    public string AttributesProperty { get; } = attributesProperty;
    public string IncludesProperty { get; } = includesProperty;

    public IReadOnlyDictionary<string, string> FieldMappings => _fieldMappings;
    public IReadOnlyDictionary<string, string> IncludesMappings => _includesMappings;
    public IReadOnlyDictionary<string, string> UserFieldMappings => _userFieldMappings;
    public IReadOnlyDictionary<string, string> CommentFieldMappings => _commentFieldMappings;
}