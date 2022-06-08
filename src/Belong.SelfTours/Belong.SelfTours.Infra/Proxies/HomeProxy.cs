using Belong.SelfTours.Infra.Proxies.Configs;
using Belong.SelfTours.Infra.Proxies.Interfaces;
using Belong.SelfTours.Infra.Proxies.Responses;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Net.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;

namespace Belong.SelfTours.Infra.Proxies
{
    public class HomeProxy : IHomeProxy
    {
        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly ILogger<HomeProxy> _Logger;
        private readonly HomeProxyConfig _Option;

        public HomeProxy(IHttpClientFactory httpClientFactory, IOptions<HomeProxyConfig> option, ILogger<HomeProxy> logger)
        {
            this._HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this._Logger = logger;
            this._Option = option?.Value ?? throw new ArgumentNullException(nameof(option));
        }


        public async Task<bool?> IsSelfServiceAllowedAsync(string externalHomeId)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{_Option.EndPointUrl}/{externalHomeId}");

            var httpClient = _HttpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                HomeResponse response = await JsonSerializer.DeserializeAsync<HomeResponse>(contentStream);
                if (response is null)
                {
                    _Logger.LogError($"{externalHomeId} not found on proxy");
                    return null;
                }

                return response.listingInfo.isSelfServeVisitsAllowed;
            }
            else
                return null;
        }
    }
}
