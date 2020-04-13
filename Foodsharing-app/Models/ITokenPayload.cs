using System.Text.Json.Serialization;

namespace Foodsharing_app.Models
{
    public interface ITokenPayload
    {
        public string Issuer { get; set; }

        public string Subject { get; set; }
        
        public string Audience { get; set; }
        
        public long Expires { get; set; }

        public long IssuedAt { get; set; }    }
}