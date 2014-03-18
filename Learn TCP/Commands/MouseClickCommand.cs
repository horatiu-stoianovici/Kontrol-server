using Kontrol.Components;
using Kontrol.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kontrol.Commands
{
    internal enum ClickCommand
    {
        LeftClickDown = 0,
        LeftClickUp = 1,
        RightClickDown = 2,
        RightClickUp = 3
    }

    public class MouseClickCommand : Command
    {
        public Response HandleRequest(Request request)
        {
            ClickCommand action = (ClickCommand)Convert.ToInt32(request.RequestedParameters[0]);

            switch (action)
            {
                case ClickCommand.LeftClickDown:
                    MouseController.LeftClickDown();
                    break;

                case ClickCommand.LeftClickUp:
                    MouseController.LeftClickUp();
                    break;

                case ClickCommand.RightClickDown:
                    MouseController.RightClickDown();
                    break;

                case ClickCommand.RightClickUp:
                    MouseController.RightClickUp();
                    break;
            }

            Response response = new Response();
            response.StatusCode = TCPStatusCodes.Ok;
            return response;
        }

        public string GetCommandName()
        {
            return "mouse-click";
        }
    }
}