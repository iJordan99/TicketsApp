namespace TicketsApp.Parsers;

public class GlobalParsingConfig
{
    // Default property for metadata, used across multiple API responses
    public string MetaProperty { get; } = "meta";

    // Default key mappings for paginated responses
    private readonly Dictionary<string, string> _paginationMappings = new()
    {
        { "LastPage", "last_page" },
        { "CurrentPage", "current_page" },
        { "TotalItems", "total_items" },
        { "PerPage", "per_page" }
    };

    // Mappings for generic error responses
    private readonly Dictionary<string, string> _errorMappings = new()
    {
        { "ErrorCode", "error_code" },
        { "Message", "message" }
    };

    // Include additional global mappings if needed
    private readonly Dictionary<string, string> _globalMappings = new()
    {
        { "Id", "id" },
        { "CreatedAt", "created_at" },
        { "UpdatedAt", "updated_at" }
    };

    public IReadOnlyDictionary<string, string> PaginationMappings => _paginationMappings;
    public IReadOnlyDictionary<string, string> ErrorMappings => _errorMappings;
    public IReadOnlyDictionary<string, string> GlobalMappings => _globalMappings;
}