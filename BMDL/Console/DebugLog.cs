using System;
using System.Collections.Generic;
using System.Linq;

namespace BMDL.Console
{
    public static class DebugLog
    {
        private static List<WindowText> log = new List<WindowText>();

        public static List<string> errors = new List<string>();

        private static AppConsole console;

        public static void Initialize(AppConsole c)
        {
            if(console == null)
                console = c;
        }

        public static void AddLog(string text, ConsoleColor color)
        {
            var windowText = new WindowText(text, ConsoleColor.Black, color);
            log.Add(windowText);
            console.Output();
        }
        public static List<WindowText> GetLast(int count)
        {
            return log.TakeLast(count).ToList();
        }

        public static int Count {
            get => log.Count;
        }

        public static void AddError(string error)
        {
            errors.Add(error);
        }
    }
}