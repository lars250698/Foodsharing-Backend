using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Foodsharing_app.Models
{
    public class RefreshTokenPayload : ITokenPayload
    {
        [JsonPropertyName("iss")] public string Issuer { get; set; }

        [JsonPropertyName("sub")] public string Subject { get; set; }
        
        [JsonPropertyName("aud")] public string Audience { get; set; }
        
        [JsonPropertyName("exp")] public long Expires { get; set; }

        [JsonPropertyName("iat")] public long IssuedAt { get; set; }
    }
}