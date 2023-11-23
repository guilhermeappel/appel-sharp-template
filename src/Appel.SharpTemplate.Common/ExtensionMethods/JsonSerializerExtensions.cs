using System.Text.Json;

namespace Appel.SharpTemplate.Common.ExtensionMethods;

public static class JsonSerializerExtensions
{
    public static T? DeserializeCaseInsensitive<T>(this string jsonString)
    {
        return JsonSerializer.Deserialize<T>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
