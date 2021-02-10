using System;
using System.Collections.Generic;
using static BMDL.API.APIAccess;

namespace BMDL.Console.Windows
{
    public class MenuWindow : Window
    {
        private int Cursor = 0;

        private bool CursorLocked = false;

        private string SearchString = "";

        public MenuWindow(AppConsole c)
            : base(c)
        {
            rawOutput.left = new List<WindowText> 
            { 
                new WindowText("Search"),
                new WindowText("By set ID"), 
                new WindowText("By map ID"),
                new WindowText("Queue"),
                new WindowText(App.DEBUG ? "Errors" : "Exit")
            };

            if(App.DEBUG)
                rawOutput.left.Add(new WindowText("Exit"));

            rawOutput.right = new List<WindowText> 
            {
                new WindowText("Queue is empty"),
                new WindowText("", ConsoleColor.Black, ConsoleColor.Blue),
                new WindowText("", ConsoleColor.Black, ConsoleColor.Blue)
            };

            App.DownloadQueue.OnQueueUpdate += OnQueueUpdate;
        }

        public override void ProcessInput(ConsoleKeyInfo keyInfo)
        {
            var key = keyInfo.Key;
            switch(key)
            {
                case ConsoleKey.DownArrow:
                    if(CursorLocked) break;
                    Cursor += 1;
                    if(Cursor == rawOutput.left.Count)
                        Cursor = 0;
                    break;

                case ConsoleKey.UpArrow:
                    if(CursorLocked) break;
                    Cursor -= 1;
                    if(Cursor < 0)
                        Cursor = rawOutput.left.Count - 1;
                    break;
                case ConsoleKey.Escape:
                    if(CursorLocked)
                    {
                        CursorLocked = false;
                        SearchString = "";
                        rawOutput.left[Cursor].RemoveTempText();
                    }
                    break;
                case ConsoleKey.Backspace:
                    if(CursorLocked && SearchString.Length > 0)
                        SearchString = SearchString.Substring(0, SearchString.Length - 1);
                    break;
                case ConsoleKey.Enter:
                    OnItemSelect();
                    break;
                default:
                    if(char.IsDigit(keyInfo.KeyChar))
                        OnCharInput(keyInfo.KeyChar);
                    break;
            }
        }

        public void OnCharInput(char Char)
        {
            if(CursorLocked)
                SearchString += Char;
        }

        public void OnItemSelect()
        {
            if(CursorLocked)
            {
                App.DownloadQueue.Add(int.Parse(SearchString), Cursor == 1 ? APIBeatmapsetLookupType.SetID : APIBeatmapsetLookupType.MapID);
                CursorLocked = false;
                SearchString = "";
                rawOutput.left[Cursor].RemoveTempText();
            }
            else
                switch(Cursor)
                {
                    case 0: // Search
                        console.SetWindow<SearchWindow>();
                        break;
                    case 1: // 1 = Set ID; 2 = Map ID
                    case 2:
                        CursorLocked = true;
                        break;
                    case 3: // Queue
                        console.SetWindow<QueueWindow>();
                        break;
                    case 4:
                        if(App.DEBUG)
                            console.SetWindow<ErrorsWindow>();
                        else
                            console.Stop();
                        break;
                    case 5:
                        console.Stop();
                        break;
                }
        }

        public void OnQueueUpdate()
        {
            console.Output();
        }

        public override WindowOutput GetOutput()
        {
            var output = rawOutput;
            output.ResetColors();
            output.left[Cursor].SetColors(ConsoleColor.White, ConsoleColor.Black);
            output.right[0].SetText(App.DownloadQueue.Count > 0 ? $"{App.DownloadQueue.Count} remaining" : "Queue is empty");
            if(CursorLocked)
                output.left[Cursor].SetTempText($"> {SearchString}");
            if(App.DEBUG)
            {
                output.right[1].SetText($"DEBUG MODE");
                output.right[2].SetText($"Token expires in: {App.API.token.ExpiresIn}");
            }
            return output;
        }
    }
}