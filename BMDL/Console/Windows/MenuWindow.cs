using System;
using System.Collections.Generic;

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
                new WindowText("By map ID"), 
                new WindowText("By set ID"), 
                new WindowText("Search"), 
                new WindowText("Queue"),
                new WindowText("Exit")
            };

            rawOutput.right = new List<WindowText> 
            {
                new WindowText("Queue is empty"),
                new WindowText("")
            };
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
                CursorLocked = false;
                SearchString = "";
                rawOutput.left[Cursor].RemoveTempText();
            }
            else
                switch(Cursor)
                {
                    case 0:
                        // Map ID
                        CursorLocked = true;
                        break;
                    case 1:
                        // Set ID
                        CursorLocked = true;
                        break;
                    case 2:
                        // Search
                        console.SetWindow<SearchWindow>();
                        break;
                    case 3:
                        // Queue
                        break;
                    case 4:
                        console.Stop();
                        break;
                }
        }

        public override WindowOutput GetOutput()
        {
            var output = rawOutput;
            output.ResetColors();
            output.left[Cursor].SetColors(ConsoleColor.White, ConsoleColor.Black);
            if(CursorLocked)
                output.left[Cursor].SetTempText($"> {SearchString}");
            return output;
        }
    }
}