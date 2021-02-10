using System;

namespace BMDL.Console
{
    public class WindowText
    {
        private string text;
        private string tempText;
        private ConsoleColor bg = ConsoleColor.Black;
        private ConsoleColor fg = ConsoleColor.White;

        public bool Temporary { private set; get; } = false;

        public int Length { 
            get {
                return text?.Length ?? 0;
            }
        }

        public WindowText(string text, bool temporary = false) {
            SetText(text);
            Temporary = temporary;
        }
        public WindowText(string text, ConsoleColor background, ConsoleColor foreground, bool temporary = false)
        {
            SetText(text);
            SetColors(background, foreground);
            Temporary = temporary;
        }

        public void SetText(string text)
        {
            this.text = text;
        }

        public void SetTempText(string text)
        {
            tempText = text;
        }

        public void RemoveTempText()
        {
            tempText = null;
        }

        public void SetColors(ConsoleColor background, ConsoleColor foreground) { SetBG(background); SetFG(foreground); }

        public void SetBG(ConsoleColor background)
        {
            bg = background;
        }

        public void SetFG(ConsoleColor foreground)
        {
            fg = foreground;
        }

        public void ResetColors()
        {
            SetColors(ConsoleColor.Black, ConsoleColor.White);
        }

        public void Write()
        {
            System.Console.BackgroundColor = bg;
            System.Console.ForegroundColor = fg;
            System.Console.Write(tempText ?? text);
        }

        public void Crop(int len)
        {
            SetText((tempText ?? text).Substring(0, len) + "..");
        }
    }
}