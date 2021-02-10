using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using BMDL.API.Responses;
using BMDL.Console;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;

namespace BMDL.API
{
    public delegate void APILoginEvent();
    public delegate void SearchResultEvent(APIBeatmapset[] mapsets);

    public class APIAccess
    {
        public OAuthToken token { private set; get; } = new OAuthToken();

        public event APILoginEvent OnAPILogin;
        public event SearchResultEvent OnSearchResult;
        public event QueueUpdateEvent OnQueueUpdate;

        public APIAccess()
        {
            //
        }

        public async Task Login(string username, string password)
        {
            DebugLog.AddLog("Logging in...", ConsoleColor.Yellow);
            try {
                var newToken = await "https://osu.ppy.sh/oauth/token"
                    .PostJsonAsync(new
                    {
                        username,
                        password,
                        grant_type = "password",
                        client_id = 5,
                        client_secret = "FGc9GAtyHzeQDshWP5Ah7dega8hJACAJpQtw6OXk",
                        scope = "*"
                    })
                    .ReceiveJson<OAuthToken>();

                DebugLog.AddLog("Logged in!", ConsoleColor.Green);

                token = newToken;

                File.WriteAllText(@"./refresh.txt", token.RefreshToken);

                OnAPILogin?.Invoke();
            } 
            catch(Exception) 
            {
                DebugLog.AddLog("Login failed!", ConsoleColor.Red);
            }
        }

        public async Task LoginWithRefreshToken(string RefreshToken)
        {
            DebugLog.AddLog("Logging in...", ConsoleColor.Yellow);
            await Refresh(RefreshToken);
            DebugLog.AddLog("Logged in!", ConsoleColor.Green);
            OnAPILogin?.Invoke();
        }

        public async Task Refresh(string RefreshToken)
        {
            DebugLog.AddLog("Refreshing token...", ConsoleColor.Blue);
            try {
                var newToken = await "https://osu.ppy.sh/oauth/token"
                    .PostJsonAsync(new
                    {
                        grant_type = "refresh_token",
                        client_id = 5,
                        client_secret = "FGc9GAtyHzeQDshWP5Ah7dega8hJACAJpQtw6OXk",
                        refresh_token = RefreshToken,
                        scope = "*"
                    })
                    .ReceiveJson<OAuthToken>();

                token = newToken;

                DebugLog.AddLog("Token refreshed!", ConsoleColor.Green);

                File.WriteAllText(@"./refresh.txt", token.RefreshToken);
            } 
            catch(Exception)
            {
                DebugLog.AddLog("Refresh failed!", ConsoleColor.Red);
            }
        }

        public async Task<APIBeatmapset> GetBeatmapset(int id, APIBeatmapsetLookupType lookupType = APIBeatmapsetLookupType.SetID)
        {
            try {
                if(!token.IsValid)
                    await Refresh(token.RefreshToken);
                
                var mapset = await ("https://osu.ppy.sh/api/v2/beatmapsets/"
                    + (lookupType == APIBeatmapsetLookupType.SetID ? $"{id}" : $"lookup?beatmap_id={id}"))
                    .WithOAuthBearerToken(token.AccessToken)
                    .GetJsonAsync<APIBeatmapset>();

                return mapset;
            }
            catch
            {
                throw new Exception("oh shit oh fuck");
            }
        }

        public async Task SearchBeatmapsets(string query)
        {
            try {
                if(!token.IsValid)
                    await Refresh(token.RefreshToken);
                
                DebugLog.AddLog($"Searching: {query}", ConsoleColor.Blue);

                var mapsets = await "https://osu.ppy.sh/api/v2/beatmapsets/search"
                    .SetQueryParams(new
                    {
                        q = query
                    })
                    .WithOAuthBearerToken(token.AccessToken)
                    .GetJsonAsync<APIBeatmapsetSearchResponse>();

                DebugLog.AddLog($"Search successful! Results: {mapsets.Beatmapsets.Length}", ConsoleColor.Green);

                OnSearchResult?.Invoke(mapsets.Beatmapsets);
            } 
            catch(Exception e)
            {
                DebugLog.AddError(e.ToString());
            }
        }

        public enum APIBeatmapsetLookupType
        {
            SetID,
            MapID
        }

        [Serializable]
        public class OAuthToken
        {
            [JsonProperty(@"access_token")]
            public string AccessToken;
            [JsonProperty(@"refresh_token")]
            public string RefreshToken;
            [JsonProperty(@"expires_in")]
            public long ExpiresIn
            {
                get => AccessTokenExpiry - DateTimeOffset.Now.ToUnixTimeSeconds();
                set => AccessTokenExpiry = DateTimeOffset.Now.AddSeconds(value).ToUnixTimeSeconds();
            }

            public long AccessTokenExpiry;

            public bool IsValid => !string.IsNullOrEmpty(AccessToken) && ExpiresIn > 30;
        }
    }
}