using Kontrol.Components;
using Kontrol.Controllers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Kontrol.Commands
{
    public class MouseMoveCommand : Command
    {
        private string currentTouchUid = "";
        private Point clientStartPoint, serverStartPoint;

        public Response HandleRequest(Request request)
        {
            int clientX = Convert.ToInt32(request.RequestedParameters[0]), clientY = Convert.ToInt32(request.RequestedParameters[1]);
            string uid = request.RequestedParameters[2];

            if (uid != currentTouchUid)
            {
                clientStartPoint = new Point(clientX, clientY);
                serverStartPoint = MouseController.GetCurrentMousePosition();
                currentTouchUid = uid;
            }
            else
            {
                var delta = new Point(clientX - clientStartPoint.X, clientY - clientStartPoint.Y);
                var newPos = new Point(serverStartPoint.X + delta.X, serverStartPoint.Y + delta.Y);
                MouseController.SetMousePosition(newPos);
            }
            return new Response(TCPStatusCodes.Ok);
        }

        public string GetCommandName()
        {
            return "mouse-move";
        }
    }
}