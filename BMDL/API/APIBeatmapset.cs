using System;
using Newtonsoft.Json;

namespace BMDL.API
{
    [Serializable]
    public class APIBeatmapset
    {
        [JsonProperty(@"id")]
        public int ID;
        [JsonProperty(@"status")]
        public APIBeatmapsetOnlineStatus Status;
        [JsonProperty(@"title")]
        public string Title;
        [JsonProperty(@"artist")]
        public string Artist;
        [JsonProperty(@"creator")]
        public string Creator;

        public override string ToString()
        {
            return $@"<{Status.ToString()}> {Artist.Crop(20)} - {Title.Crop(20)} by {Creator} [{ID}]";
        }
    }
}