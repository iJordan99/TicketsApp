using System.Collections.ObjectModel;
using System.Text.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;

namespace TicketsApp.Parsers;

/// <summary>
///     Parses JSON data to create instances of the <c>User</c> model.
/// </summary>
/// <remarks>
///     This class implements the <c>IUserParser</c> interface and provides functionality to parse
///     JSON elements and map them to <c>User</c> objects. The parsing process handles the extraction
///     of relevant fields such as "id", "email", "name", and "is_engineer" from the JSON structure.
/// </remarks>
/// <param name="jsonHelper">
///     An instance of <c>IJsonParsingHelper</c> used to assist with JSON field extraction.
/// </param>
public class UserParser(IJsonParsingHelper jsonHelper) : IUserParser
{
    /// Parses a JSON element into a User object.
    /// <param name="element">
    ///     The JSON element containing the user data to be parsed.
    /// </param>
    /// <returns>
    ///     A User object if parsing is successful; otherwise, null.
    /// </returns>
    public User? Parse(JsonElement element)
    {
        if (!element.TryGetProperty("id", out var idElement) || idElement.ValueKind == JsonValueKind.Null) return null;

        var attributes = element.TryGetProperty("attributes", out var attrElement) ? attrElement : element;

        var isEngineer = false;
        if (attributes.TryGetProperty("is_engineer", out var isEngineerElement) &&
            (isEngineerElement.ValueKind == JsonValueKind.True || isEngineerElement.ValueKind == JsonValueKind.False))
            isEngineer = isEngineerElement.GetBoolean();

        var isAdmin = false;
        if (attributes.TryGetProperty("is_admin", out var isAdminElement) &&
            (isAdminElement.ValueKind == JsonValueKind.True || isAdminElement.ValueKind == JsonValueKind.False))
            isAdmin = isAdminElement.GetBoolean();

        return new User
        (
            jsonHelper.GetStringField(attributes, "email"),
            idElement.GetInt32(),
            isEngineer,
            isAdmin,
            jsonHelper.GetStringField(attributes, "name")
        );
    }

    public async Task<ObservableCollection<User>> ParseMany(HttpResponseMessage response)
    {
        var list = new ObservableCollection<User>();

        var json = await response.Content.ReadAsStringAsync();
        using var jsonDoc = JsonDocument.Parse(json);
        var root = jsonDoc.RootElement;

        if (root.TryGetProperty("data", out var data))
        {
            if (data.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in data.EnumerateArray())
                {
                    var user = Parse(item);
                    if (user is not null) list.Add(user);
                }

                return list;
            }

            if (data.ValueKind == JsonValueKind.Object)
            {
                var user = Parse(data);
                if (user is not null) list.Add(user);
                return list;
            }
        }

        if (root.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in root.EnumerateArray())
            {
                var user = Parse(item);
                if (user is not null) list.Add(user);
            }

            return list;
        }

        if (root.ValueKind == JsonValueKind.Object)
        {
            var user = Parse(root);
            if (user is not null) list.Add(user);
        }

        return list;
    }
}