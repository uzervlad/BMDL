using System;
using Newtonsoft.Json;

namespace BMDL.API.Responses
{
    [Serializable]
    public class APIBeatmapsetSearchResponse
    {
        [JsonProperty("beatmapsets")]
        public APIBeatmapset[] Beatmapsets;

        [JsonProperty("total")]
        public int Total;
    }
}