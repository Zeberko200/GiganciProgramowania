using System.Text.Json;

namespace Shared;

public static class ObjectExtensions
{
    public static string ToCamelJson(this object obj)
    {
        return JsonSerializer.Serialize(obj, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    }
}