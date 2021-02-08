using System;
using System.Collections.Generic;

namespace BMDL.Console.Windows
{
    public class LoginWindow : Window
    {
        private string Username = "";
        private string Password = "";

        private bool UsernameEntered = false;
        private bool Locked = false;

        public LoginWindow(AppConsole c)
            : base(c)
        {
            rawOutput.left = new List<WindowText>()
            {
                new WindowText("Log in:", ConsoleColor.White, ConsoleColor.Black),
                new WindowText("Username: ", ConsoleColor.Gray, ConsoleColor.Black),
                new WindowText("Password: ")
            };
        }

        public override void ProcessInput(ConsoleKeyInfo keyInfo)
        {
            var key = keyInfo.Key;
            if(Locked) return;
            switch(key)
            {
                case ConsoleKey.Backspace:
                    if(UsernameEntered && Password.Length > 0)
                        Password = Password.Substring(0, Password.Length - 1);
                    else if(!UsernameEntered && Username.Length > 0)
                        Username = Username.Substring(0, Username.Length - 1);
                    break;
                case ConsoleKey.Enter:
                    if(!UsernameEntered)
                    {
                        UsernameEntered = true;
                        rawOutput.left[1].ResetColors();
                        rawOutput.left[2].SetColors(ConsoleColor.Gray, ConsoleColor.Black);
                    }
                    else
                    {
                        rawOutput.left[2].ResetColors();
                        Locked = true;
                        App.API.Login(Username, Password);
                    }
                    break;
                default:
                    if(UsernameEntered)
                        Password += keyInfo.KeyChar;
                    else
                        Username += keyInfo.KeyChar;
                    break;
            }
        }

        public override WindowOutput GetOutput()
        {
            var output = rawOutput;
            output.left[1].SetTempText($"Username: {Username}");
            output.left[2].SetTempText($"Password: {new String('*', Password.Length)}");
            if(Locked)
            {
                output.left[0].SetTempText("Logging in...");
                output.left[1].SetColors(ConsoleColor.Black, ConsoleColor.Gray);
                output.left[2].SetColors(ConsoleColor.Black, ConsoleColor.Gray);
            }
            return output;
        }
    }
}