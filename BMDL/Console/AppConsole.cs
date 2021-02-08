using System;
using System.Collections.Generic;
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

        public AppConsole()
        {
            windows = new List<Window>()
            {
                new LoadingWindow(this),
                new LoginWindow(this),
                new MenuWindow(this),
                new SearchWindow(this)
            };
            currentWindow = windows.First();
        }

        public void Run()
        {
            System.Console.CursorVisible = false;
            SetTitle("BMDL | Logging in...");
            currentWindow.OnSwitch();
            Output();

            var keyThread = new Thread((c) => {
                var AC = c as AppConsole;
                while(AC.Running)
                {
                    while(System.Console.KeyAvailable == false)
                        Thread.Sleep(100);
                    var key = System.Console.ReadKey();
                    AC.currentWindow.ProcessInput(key);
                    AC.Output();
                }
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
        }

        public void Stop()
        {
            Running = false;
        }
        
        private void WriteLine(int row, WindowText left, WindowText right)
        {
            left ??= EmptyText;
            right ??= EmptyText;
            var Width = System.Console.BufferWidth;
            var maxLeftLength = Width - right.Length - 3;
            System.Console.SetCursorPosition(0, row);
            if(left.Length > maxLeftLength)
                left.Crop(maxLeftLength);
            left.Write();

            System.Console.SetCursorPosition(Width - right.Length - 1, row);
            right.Write();
            System.Console.WriteLine();
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