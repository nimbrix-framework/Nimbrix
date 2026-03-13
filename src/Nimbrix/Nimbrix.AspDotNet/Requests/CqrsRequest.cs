using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nimbrix.AspDotNet.Requests;

public class CqrsRequest
{
    [JsonPropertyName("type")] public string Type { get; set; } = null!;
    [JsonPropertyName("payload")] public JsonElement Payload { get; set; }
}