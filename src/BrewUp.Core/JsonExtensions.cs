using System.Text.Json;

namespace BrewUp.Core;

public static class JsonExtensions
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    public static T Deserialized<T>(this string s) => JsonSerializer.Deserialize<T>(s)!;

    public static string Serialized<T>(this T o) =>
        JsonSerializer.Serialize(o, JsonSerializerOptions);
}
