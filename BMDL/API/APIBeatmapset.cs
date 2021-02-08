using System;
using Newtonsoft.Json;

namespace BMDL.API
{
    [Serializable]
    public class APIBeatmapset
    {
        [JsonProperty(@"id")]
        public int ID;
        [JsonProperty(@"title")]
        public string Title;
        [JsonProperty(@"artist")]
        public string Artist;
        [JsonProperty(@"creator")]
        public string Creator;
        [JsonProperty(@"status")]
        public string Status;

        public override string ToString()
        {
            return $@"[{ID}] {Artist} - {Title} by {Creator}";
        }
    }
}