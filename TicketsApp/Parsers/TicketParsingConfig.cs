namespace TicketsApp.Parsers;

public class TicketParsingConfig(
    string rootProperty = "data",
    string attributesProperty = "attributes")
{
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

    public string RootProperty { get; } = rootProperty;

    public string AttributesProperty { get; } = attributesProperty;

    public IReadOnlyDictionary<string, string> FieldMappings => _fieldMappings;
}