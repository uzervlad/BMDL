using System;

namespace BMDL.Console
{
    public class WindowText
    {
        private string text;
        private string tempText;
        private ConsoleColor bg = ConsoleColor.Black;
        private ConsoleColor fg = ConsoleColor.White;

        public int Length { 
            get {
                return text.Length;
            }
        }

        public WindowText(string text)
        {
            this.text = text;
        }
        
        public WindowText(string text, ConsoleColor background, ConsoleColor foreground)
        {
            SetText(text);
            SetColors(background, foreground);
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

        public void SetColors(ConsoleColor background, ConsoleColor foreground)
        {
            bg = background;
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