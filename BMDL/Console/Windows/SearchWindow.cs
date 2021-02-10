using System;
using System.Collections.Generic;
using System.Linq;
using BMDL.API;

namespace BMDL.Console.Windows
{
    public class SearchWindow : Window
    {
        private string SearchString = "";

        private int Cursor = 0;

        private bool Searching = false;

        private List<APIBeatmapset> SearchResult = new List<APIBeatmapset>();

        public SearchWindow(AppConsole c)
            : base(c)
        {
            ResetOutput();
            App.API.OnSearchResult += OnSearchResult;
        }

        private void OnSearchResult(APIBeatmapset[] mapsets)
        {
            Cursor = mapsets.Length == 0 ? 0 : 1;
            SearchResult = mapsets.Take(10).ToList();
            console.Output();
            Searching = false;
        }

        private void ResetOutput()
        {
            rawOutput.left = new List<WindowText>()
            {
                new WindowText("Search: ", ConsoleColor.White, ConsoleColor.Black)
            };
        }

        public override void ProcessInput(ConsoleKeyInfo keyInfo)
        {
            if(Searching) return;
            var key = keyInfo.Key;
            switch(key)
            {
                case ConsoleKey.Escape:
                    SearchString = "";
                    SearchResult.Clear();
                    console.SetWindow<MenuWindow>();
                    break;
                case ConsoleKey.Backspace:
                    if(SearchString.Length > 0)
                        SearchString = SearchString.Substring(0, SearchString.Length - 1);
                    break;
                case ConsoleKey.Enter:
                    if(Cursor == 0)
                    {
                        Searching = true;
                        DebugLog.AddLog("About to perform search", ConsoleColor.White);
                        App.API.SearchBeatmapsets(SearchString);
                    }
                    else
                        App.DownloadQueue.Add(SearchResult[Cursor - 1]);
                    break;
                case ConsoleKey.UpArrow:
                    Cursor = Math.Max(0, Cursor - 1);
                    break;
                case ConsoleKey.DownArrow:
                    Cursor = Math.Min(Cursor + 1, SearchResult.Count);
                    break;
                default:
                    OnCharInput(keyInfo.KeyChar);
                    break;
            }
        }

        private void OnCharInput(char Char)
        {
            SearchString += Char;
        }

        public override WindowOutput GetOutput()
        {
            var output = rawOutput;
            output.left[0].SetTempText($"Search: {SearchString}");
            output.RemoveTempText();
            output.left.AddRange(SearchResult.Select(m => new WindowText(m.ToString(), true)));
            output.left[Cursor].SetColors(ConsoleColor.White, ConsoleColor.Black);
            foreach(var queue in App.DownloadQueue.GetQueue())
            {
                var i = SearchResult.FindIndex(m => queue.ID == m.ID);
                if(i != -1)
                    output.left[i + 1].SetFG(ConsoleColor.Green);
            }
            return output;
        }
    }
}