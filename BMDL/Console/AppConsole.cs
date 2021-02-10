using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using BMDL.Console.Windows;

namespace BMDL.Console
{
    public class AppConsole
    {
        private List<Window> windows;

        private Window currentWindow;

        private bool Running = true;

        private readonly WindowText EmptyText = new WindowText("");

        private readonly int DebugLength = 10;

        public AppConsole()
        {
            DebugLog.Initialize(this);
            
            windows = new List<Window>()
            {
                new LoadingWindow(this),
                new LoginWindow(this),
                new MenuWindow(this),
                new SearchWindow(this),
                new QueueWindow(this),
                new ErrorsWindow(this),
                new SetupWindow(this)
            };
            currentWindow = windows.First();
        }

        public void Run()
        {
            System.Console.CursorVisible = false;
            SetTitle("BMDL | Logging in...");
            currentWindow.OnSwitch();
            Output();

            System.Console.CancelKeyPress += ForceStop;

            var keyThread = new Thread((c) => {
                var AC = c as AppConsole;
                while(AC.Running)
                {
                    while(System.Console.KeyAvailable == false)
                        Thread.Sleep(100);
                    var key = System.Console.ReadKey();
                    DebugLog.AddLog($"Key pressed: {key.Key}", ConsoleColor.Cyan);
                    AC.currentWindow.ProcessInput(key);
                    AC.Output();
                }
                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.Clear();
            });
            keyThread.Start(this);
        }

        public void SetTitle(string title)
        {
            System.Console.Title = title;
        }

        public void Output()
        {
            var Width = System.Console.BufferWidth;
            System.Console.Clear();

            var output = currentWindow.GetOutput();

            for(var i = 0; i < Math.Max(output.left.Count, output.right.Count); i++)
            {
                var left = i < output.left.Count ? output.left[i] : EmptyText;
                var right = i < output.right.Count ? output.right[i] : EmptyText;
                WriteLine(i, left, right);
            }

            if(App.DEBUG)
            {
                var log = DebugLog.GetLast(DebugLength);
                var Height = System.Console.WindowHeight;
                for(var i = 0; i < log.Count; i++)
                {
                    WriteLine(Height - (log.Count - i), EmptyText, log[i]);
                }
            }

            System.Console.SetCursorPosition(0, System.Console.BufferHeight - 1);
        }

        public void ForceStop(object sender, ConsoleCancelEventArgs args) => Stop();

        public void Stop()
        {
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.Clear();
            
            var files = Directory.GetFiles(@"./Temp/");
            foreach(var file in files)
                File.Delete(file);

            Running = false;
        }
        
        private void WriteLine(int row, WindowText left, WindowText right)
        {
            left ??= EmptyText;
            right ??= EmptyText;
            int Width;
            try {
                Width = System.Console.BufferWidth;
            } catch {
                Width = 40;
            }
            var maxLeftLength = Width - right.Length - 3;
            try {
                System.Console.SetCursorPosition(0, row);
            } catch {}
            if(left.Length > maxLeftLength)
                left.Crop(maxLeftLength);
            left.Write();

            try {
                System.Console.SetCursorPosition(Width - right.Length - 1, row);
            } catch {}
            right.Write();
        }

        public bool IsWindowOpened<T>() where T : Window
        {
            return currentWindow is T;
        }

        public void SetWindow<T>() where T : Window
        {
            var window = windows.OfType<T>().First();
            currentWindow = window;
            currentWindow.OnSwitch();
            Output();
        }
    }
}