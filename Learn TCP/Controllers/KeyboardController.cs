using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsInput;

namespace Kontrol.Controllers
{
    public class KeyboardController
    {
        private static InputSimulator input = new InputSimulator();

        public static void PressKey(int keyCode)
        {
            input.Keyboard.TextEntry((char)keyCode);
        }
    }
}