using System.Text.Json.Serialization;

namespace AutoSlackEmoji.Core.Models
{
    public class TokenRefreshResponse
    {
        [JsonPropertyName("ok")]
        public bool IsSuccessful { get; set; }
        [JsonPropertyName("token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonPropertyName("team_id")]
        public string TeamId { get; set; }
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
        [JsonPropertyName("iat")]
        public int Iat { get; set; }
        [JsonPropertyName("exp")]
        public int Exp { get; set; }
        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}
