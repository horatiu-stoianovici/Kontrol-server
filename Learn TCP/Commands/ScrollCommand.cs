using Kontrol.Components;
using Kontrol.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kontrol.Commands
{
    public class ScrollCommand : ICommand
    {
        public Response HandleRequest(Request request)
        {
            ScrollCommands command = (ScrollCommands)Convert.ToInt32(request.RequestedParameters[0]);
            switch (command)
            {
                case ScrollCommands.ScrollWithVelocity:
                    MouseController.ScrollWithInitialVelocity(-float.Parse(request.RequestedParameters[1]), float.Parse(request.RequestedParameters[2]));
                    break;

                case ScrollCommands.StopScrolling:
                    MouseController.StopScrolling();
                    break;

                case ScrollCommands.Scroll:
                    MouseController.ScrollHorizontally(-(int)Math.Round(float.Parse(request.RequestedParameters[1])));
                    MouseController.ScrollVertically((int)Math.Round(float.Parse(request.RequestedParameters[2])));
                    break;
            }

            return new Response(TCPStatusCodes.Ok);
        }

        public string GetCommandName()
        {
            return "scroll";
        }

        private enum ScrollCommands
        {
            Scroll = 1,
            ScrollWithVelocity = 2,
            StopScrolling = 3
        }
    }
}