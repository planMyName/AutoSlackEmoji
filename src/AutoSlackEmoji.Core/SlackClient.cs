using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSlackEmoji.Core
{
    public class SlackClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SlackClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
