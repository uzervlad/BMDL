using System;
using System.Collections.Generic;
using System.IO;

namespace BMDL.Console.Windows
{
    public class SetupWindow : Window
    {
        private string path = @"";

        private bool Locked = false;

        public SetupWindow(AppConsole c)
            : base(c)
        {
            rawOutput.left = new List<WindowText>() { 
                new WindowText("Path to /Songs: ", ConsoleColor.White, ConsoleColor.Black),
                new WindowText("", ConsoleColor.Black, ConsoleColor.Red)
            };
        }

        public override void ProcessInput(ConsoleKeyInfo keyInfo)
        {
            if(Locked) return;
            switch(keyInfo.Key)
            {
                case ConsoleKey.Escape:
                    console.Stop();
                    break;
                case ConsoleKey.Backspace:
                    if(path.Length > 0)
                        path = path.Substring(0, path.Length - 1);
                    break;
                case ConsoleKey.Enter:
                    Locked = true;
                    if(!Directory.Exists(path))
                        rawOutput.left[1].SetTempText("This path doesn't exist!");
                    else
                    {
                        App.SONGS_PATH = path;
                        File.WriteAllText(@"./songs_path.txt", path);
                        console.SetWindow<LoadingWindow>();
                        Locked = false;
                    }
                    break;
                default:
                    path += keyInfo.KeyChar;
                    break;
            }
        }

        public override WindowOutput GetOutput()
        {
            var output = rawOutput;
            output.left[0].SetTempText($"Path to /Songs: {path}");
            return output;
        }
    }
}