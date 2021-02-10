using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BMDL.Console;
using Flurl.Http;
using static BMDL.API.APIAccess;

namespace BMDL.API
{
    public delegate void QueueUpdateEvent();

    public class APIDownloadQueue
    {
        private List<APIBeatmapset> Queue = new List<APIBeatmapset>();

        public event QueueUpdateEvent OnQueueUpdate;

        public async void Add(int mapsetID, APIBeatmapsetLookupType type)
        {
            try {
                var mapset = await App.API.GetBeatmapset(mapsetID, type);
                Add(mapset);
            } catch {}
        }

        public void Add(APIBeatmapset mapset)
        {
            Queue.Add(mapset);
            OnQueueUpdate?.Invoke();
            if(Queue.Count == 1)
                Download();
        }

        private async Task Download()
        {
            if(!Directory.Exists(@"./Temp"))
                Directory.CreateDirectory(@"./Temp/");
            var mapset = Queue[0];
            try {
                await $"https://osu.ppy.sh/api/v2/beatmapsets/{mapset.ID}/download"
                    .WithOAuthBearerToken(App.API.token.AccessToken)
                    .DownloadFileAsync(@"./Temp/", $"{mapset.ID}.osz");
                File.Move($@"./Temp/{mapset.ID}.osz", $@"{App.SONGS_PATH}/{mapset.ID}.osz");
            } catch {
                DebugLog.AddError($"Beatmapset download failed! ({mapset.ID})");
            }
            Queue = Queue.Skip(1).ToList();
            OnQueueUpdate?.Invoke();
            if(Queue.Count > 0)
                Download();
        }

        public void Remove(int i)
        {
            if(i <= 0 || i >= Queue.Count) return;
            Queue.RemoveAt(i);
            OnQueueUpdate?.Invoke();
        }

        public int Count {
            get {
                return Queue.Count;
            }
        }

        public List<APIBeatmapset> GetQueue() => Queue;
    }
}