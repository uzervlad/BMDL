using System;
using System.Collections.Generic;

namespace BMDL.Console
{
    public class WindowOutput
    {
        public List<WindowText> left = new List<WindowText>();
        public List<WindowText> right = new List<WindowText>();

        public void ResetColors()
        {
            foreach(var t in left)
                t.ResetColors();
            foreach(var t in right)
                t.ResetColors();
        }
    }

    public abstract class Window
    {
        protected readonly AppConsole console;

        protected WindowOutput rawOutput = new WindowOutput();

        public Window(AppConsole c)
        {
            console = c;
        }

        public abstract void ProcessInput(ConsoleKeyInfo keyInfo);

        public virtual WindowOutput GetOutput()
        {
            return rawOutput;
        }

        public virtual void OnSwitch() {}
    }
}