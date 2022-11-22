using System.Net.Http.Headers;
using System.Text.Json;
using AutoSlackEmoji.Core.Models;
using Microsoft.Extensions.Configuration;

namespace AutoSlackEmoji.Core
{
    public interface ISlackClient
    {
        Task AddEmojiAsync(string emojiName, string emojiUrl);
        Task RefreshAccessTokenAsync();
    }

    public class SlackClient : ISlackClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private string? _accessToken;
        private string? _refreshToken;

        public SlackClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _accessToken = _configuration["slack_access_token"];
            _refreshToken = _configuration["slack_refresh_token"];
        }

        public async Task AddEmojiAsync(string emojiName, string emojiUrl)
        {
            using var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://slack.com/api/emoji.add");
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"name", emojiName},
                {"url", emojiUrl}
            });
            request.Content = content;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            var result = await client.SendAsync(request);
            var resultContent = await result.Content.ReadAsStringAsync();
        }

        public async Task RefreshAccessTokenAsync()
        {
            using var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://slack.com/api/tooling.tokens.rotate");
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"refresh_token", _refreshToken}
            });
            request.Content = content;

            var result = await client.SendAsync(request);
            var resultContent = await result.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<TokenRefreshResponse>(resultContent);
            if (response != null)
            {
                _accessToken = response.AccessToken;
                _refreshToken = response.RefreshToken;
            }
        }
    }
}
