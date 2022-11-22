using System.Net.Http.Headers;
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
        private readonly string? _accessToken;
        private readonly string? _refreshtoken;

        public SlackClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _accessToken = _configuration["slack_access_token"];
            _refreshtoken = _configuration["slack_refresh_token"];
        }

        public async Task AddEmojiAsync(string emojiName, string emojiUrl)
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://slack.com/api/emoji.add");
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
            throw new NotImplementedException();
        }
    }
}
