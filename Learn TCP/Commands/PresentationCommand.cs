using Kontrol.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace Kontrol.Commands
{
    /// <summary>
    /// Command for the presentation mode
    /// </summary>
    public class PresentationCommand : ICommand
    {
        public Response HandleRequest(Request request)
        {
            PresentationControls control = (PresentationControls)Convert.ToInt32(request.RequestedParameters[0]);

            InputSimulator sim = new InputSimulator();

            switch (control)
            {
                //start the presentation with F5
                case PresentationControls.StartPresentation:
                    sim.Keyboard.KeyPress(new VirtualKeyCode[] { VirtualKeyCode.F5 });
                    break;

                //stop the presentation with ESC
                case PresentationControls.StopPresentation:
                    sim.Keyboard.KeyPress(new VirtualKeyCode[] { VirtualKeyCode.ESCAPE });
                    break;

                //next slide is arrow right
                case PresentationControls.Next:
                    sim.Keyboard.KeyPress(new VirtualKeyCode[] { VirtualKeyCode.RIGHT });
                    break;

                //previous is arrow left
                case PresentationControls.Previous:
                    sim.Keyboard.KeyPress(new VirtualKeyCode[] { VirtualKeyCode.LEFT });
                    break;

                default:
                    break;
            }

            return new Response(TCPStatusCodes.Ok);
        }

        public string GetCommandName()
        {
            return "presentation";
        }
    }

    public enum PresentationControls
    {
        StartPresentation = 0,
        StopPresentation = 1,
        Next = 2,
        Previous = 3
    }
}