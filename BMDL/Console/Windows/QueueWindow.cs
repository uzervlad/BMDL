using System;
using System.Collections.Generic;
using System.Linq;

namespace BMDL.Console.Windows
{
    public class QueueWindow : Window
    {
        public QueueWindow(AppConsole c)
            : base(c)
        {
            rawOutput.left = new List<WindowText>()
            {
                new WindowText("Queue:", ConsoleColor.White, ConsoleColor.Black)
            };
        }

        public override void ProcessInput(ConsoleKeyInfo keyInfo)
        {
            if(keyInfo.Key == ConsoleKey.Escape)
                console.SetWindow<MenuWindow>();
        }

        public override WindowOutput GetOutput()
        {
            var output = rawOutput;
            output.RemoveTempText();
            var queue = App.DownloadQueue.GetQueue().Take(10);
            output.left.AddRange(queue.Select(m => new WindowText(m.ToString(), true)));
            return output;
        }
    }
}