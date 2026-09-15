using System.Text.Json;
using System.Text.Json.Serialization;

namespace SyllabusApp.Web.Services;

public static class ApiJson
{
    // Musi być spójne z API (Program.cs: JsonStringEnumConverter) - enumy jadą przez sieć jako
    // stringi (np. "Draft"), więc bez tego konwertera deserializacja każdego DTO z polem enum rzuca wyjątek.
    public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() },
    };
}
