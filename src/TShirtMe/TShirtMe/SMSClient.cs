using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TShirtMe
{
    public class SMSClient
    {
        private static async Task<TokenResponse> RequestToken(Uri uriAuthorizationServer, string clientId, string scope, string clientSecret)
        {
            HttpResponseMessage responseMessage;

            using (var client = new HttpClient())
            {
                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, uriAuthorizationServer);
                HttpContent httpContent = new FormUrlEncodedContent(
                    new[]
                    {
                        new KeyValuePair<string, string>("grant_type", "client_credentials"),
                        new KeyValuePair<string, string>("client_id", clientId),
                        new KeyValuePair<string, string>("scope", scope),
                        new KeyValuePair<string, string>("client_secret", clientSecret)
                    });
                tokenRequest.Content = httpContent;
                responseMessage = await client.SendAsync(tokenRequest);
            }

            return await responseMessage.Content.ReadAsAsync<TokenResponse>();
        }

        private static async Task<TResponse> PostAsync<TRequest, TResponse>(Uri url, TokenResponse token, TRequest request)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.access_token);

                var httpResponse = await httpClient.PostAsJsonAsync(url.ToString(), request);

                if (httpResponse.IsSuccessStatusCode)
                {
                    return await httpResponse.Content.ReadAsAsync<TResponse>();
                }

                var content = await httpResponse.Content.ReadAsStringAsync();
                throw new Exception(content);
            }
        }

        public class TokenResponse
        {
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public int ext_expires_in { get; set; }
            public string access_token { get; set; }
        }

        public class SmsSendRequest
        {
            public string toPhoneNumber { get; set; }
            public string body { get; set; }
            public string callbackUrl { get; set; }
            public Guid? providerConfigurationId { get; set; }
        }

        public class SmsSendResponse
        {
            public Guid smsMessageId { get; set; }
        }

        #region Secrets 

        private const string AuthorizationServerTokenIssuerUri = "";
        private const string ClientId = "";
        private const string ScopeUri = "";
        private const string ClientSecret = "";
        private static readonly Guid _smsProviderConfigId = Guid.Parse("");

        #endregion

        public static async Task SendSms(string toNumber, string entryCode)
        {
            var token = await RequestToken(new Uri(AuthorizationServerTokenIssuerUri), ClientId, ScopeUri, ClientSecret);
            var smsSendUri = new Uri(new Uri("https://gateway.kmdlogic.io/sms/v1/"), "subscriptions/x/sms");

            var smsRequest = new SmsSendRequest
            {
                toPhoneNumber = toNumber,
                body = $"Your entry code is {entryCode}",
                providerConfigurationId = _smsProviderConfigId,
                callbackUrl = string.Empty
            };

            var smsResponse = await PostAsync<SmsSendRequest, SmsSendResponse>(smsSendUri, token, smsRequest);
        }
    }
}
