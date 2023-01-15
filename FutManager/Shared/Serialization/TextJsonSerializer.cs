using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.Serialization;

internal sealed class TextJsonSerializer : ISerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = {new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)}
    };

    public string Serialize<T>(T value) where T : class
    {
       return JsonSerializer.Serialize(value, Options);
    }

    public T? Deserialize<T>(string value) where T : class
    {
        return JsonSerializer.Deserialize<T>(value, Options);
    }
}