using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace BMDL.API
{
    public delegate void APILoginEvent();
    public delegate void SearchResultEvent(APIBeatmapset[] mapsets);

    public class APIAccess
    {
        private HttpClient client = new HttpClient();
        private OAuthToken token = new OAuthToken();

        public event APILoginEvent OnAPILogin;
        public event SearchResultEvent OnSearchResult;

        public APIAccess()
        {
            //
        }

        public async Task Login(string username, string password)
        {
            var json = JsonConvert.SerializeObject(new {
                username,
                password,
                grant_type = "password",
                client_id = 5,
                client_secret = "FGc9GAtyHzeQDshWP5Ah7dega8hJACCAJpQtw6OXk",
                score = "*"
            });
            var body = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("https://osu.ppy.sh/oauth/token", body);

                var newToken = JsonConvert.DeserializeObject<OAuthToken>(response.Content.ReadAsStringAsync().Result);
                
                token = newToken;

                File.WriteAllText(@"./refresh.txt", token.RefreshToken);

                OnAPILogin?.Invoke();
            }
        }

        public async Task LoginWithRefreshToken(string RefreshToken)
        {
            await Refresh(RefreshToken);
            OnAPILogin?.Invoke();
        }

        public async Task Refresh(string RefreshToken)
        {
            var json = JsonConvert.SerializeObject(new {
                grant_type = "refresh_token",
                client_id = 5,
                client_secret = "FGc9GAtyHzeQDshWP5Ah7dega8hJACCAJpQtw6OXk",
                refresh_token = RefreshToken,
                score = "*"
            });
            var body = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("https://osu.ppy.sh/oauth/token", body);

                var newToken = JsonConvert.DeserializeObject<OAuthToken>(response.Content.ReadAsStringAsync().Result);
                
                token = newToken;

                File.WriteAllText(@"./refresh.txt", token.RefreshToken);
            }
        }

        public async Task SearchBeatmapsets(string query)
        {
            if(!token.IsValid)
                await Refresh(token.RefreshToken);
            var url = new Uri("https://osu.ppy.sh/api/v2/beatmapsets/search")
                .AddQuery("q", query);

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url.ToString());
                
                var mapsets = JsonConvert.DeserializeObject<APIBeatmapset[]>(response.Content.ReadAsStringAsync().Result);

                OnSearchResult?.Invoke(mapsets);
            }   
        }

        [Serializable]
        private class OAuthToken
        {
            [JsonProperty(@"access_token")]
            public string AccessToken;
            [JsonProperty(@"refresh_token")]
            public string RefreshToken;
            public long ExpiresIn
            {
                get => AccessTokenExpiry - DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                set => DateTimeOffset.Now.AddSeconds(value).ToUnixTimeSeconds();
            }

            public long AccessTokenExpiry;

            public bool IsValid => !string.IsNullOrEmpty(AccessToken) && ExpiresIn > 30;
        }
    }
}