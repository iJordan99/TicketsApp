namespace TicketsApp.Parsers;

/// <summary>
///     Provides configuration for parsing API responses by mapping C# property names
///     to their corresponding JSON response field names. This enables consistent
///     parsing across different response types while maintaining flexibility in
///     API response structure.
/// </summary>
public class GlobalParsingConfig
{
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

    // Default key mappings for paginated responses
    private readonly Dictionary<string, string> _paginationMappings = new()
    {
        { "LastPage", "last_page" },
        { "CurrentPage", "current_page" },
        { "TotalItems", "total" },
        { "ItemsPerPage", "per_page" }
    };

    // Default property for metadata, used across multiple API responses
    public string MetaProperty { get; } = "meta";

    public IReadOnlyDictionary<string, string> PaginationMappings => _paginationMappings;
    public IReadOnlyDictionary<string, string> ErrorMappings => _errorMappings;
    public IReadOnlyDictionary<string, string> GlobalMappings => _globalMappings;
}