using System;
using System.Collections.Generic;
using System.IO;
using BMDL.Console.Windows;

namespace BMDL.Console
{
    public class LoadingWindow : Window
    {
        public LoadingWindow(AppConsole c)
            : base(c)
        {
            rawOutput.left = new List<WindowText>() { new WindowText("Logging in...") };
        }

        public override void ProcessInput(ConsoleKeyInfo keyInfo) {}

        private void OnLoggedIn()
        {
            console.SetTitle("BMDL");
            console.SetWindow<MenuWindow>();
        }

        public override void OnSwitch()
        {
            var credentials = File.ReadAllLines(@"./credentials.txt");
            App.API.Login(credentials[0], credentials[1]);
            App.API.OnAPILogin += OnLoggedIn;
        }
    }
}