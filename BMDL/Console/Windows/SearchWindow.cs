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

        public SearchWindow(AppConsole c)
            : base(c)
        {
            ResetOutput();
            App.API.OnSearchResult += OnSearchResult;
        }

        private void OnSearchResult(APIBeatmapset[] mapsets)
        {
            rawOutput.left.AddRange(mapsets.Select(m => new WindowText(m.ToString())));
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
            var key = keyInfo.Key;
            switch(key)
            {
                case ConsoleKey.Escape:
                    rawOutput.left[0].RemoveTempText();
                    console.SetWindow<MenuWindow>();
                    break;
                case ConsoleKey.Backspace:
                    if(SearchString.Length > 0)
                        SearchString = SearchString.Substring(0, SearchString.Length - 1);
                    break;
                case ConsoleKey.Enter:
                    App.API.SearchBeatmapsets("honesty");
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
            return output;
        }
    }
}