using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsInput;
using WindowsInput.Native;

namespace Kontrol.Controllers
{
    public class KeyboardController
    {
        private static InputSimulator input = new InputSimulator();

        public static void PressKey(int keyCode)
        {
            if (keyCode == 8)
            {
                input.Keyboard.KeyPress(VirtualKeyCode.BACK);
            }
            else if (keyCode == 13)
            {
                input.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            }
            else
            {
                input.Keyboard.TextEntry((char)keyCode);
            }
        }
    }
}