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
            App.API.OnAPILogin += OnLoggedIn;
        }

        public override void ProcessInput(ConsoleKeyInfo keyInfo) {}

        private void OnLoggedIn()
        {
            console.SetTitle("BMDL");
            console.SetWindow<MenuWindow>();
        }

        public override void OnSwitch()
        {
            if(App.SONGS_PATH.Length <= 0 || !Directory.Exists(App.SONGS_PATH))
            {
                console.SetWindow<SetupWindow>();
                return;
            }
            var tokenExists = File.Exists(@"./refresh.txt");
            if(!tokenExists)
                console.SetWindow<LoginWindow>();
            else
            {
                var refresh = File.ReadAllText(@"./refresh.txt");
                App.API.LoginWithRefreshToken(refresh);
            }
        }
    }
}