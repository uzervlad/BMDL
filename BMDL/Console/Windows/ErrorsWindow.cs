using System;
using System.Collections.Generic;

namespace BMDL.Console.Windows
{
    public class ErrorsWindow : Window
    {
        public ErrorsWindow(AppConsole c) : base(c) {}

        public override void ProcessInput(ConsoleKeyInfo keyInfo)
        {
            if(keyInfo.Key == ConsoleKey.Escape)
                console.SetWindow<MenuWindow>();
        }

        public override WindowOutput GetOutput()
        {
            rawOutput.left = new List<WindowText>()
            {
                new WindowText("Errors:", ConsoleColor.White, ConsoleColor.Black)
            };

            foreach(var error in DebugLog.errors)
            {
                rawOutput.left.Add(new WindowText(error));
            }

            return rawOutput;
        }
    }
}