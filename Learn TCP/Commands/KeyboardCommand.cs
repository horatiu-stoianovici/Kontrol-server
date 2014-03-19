using Kontrol.Components;
using Kontrol.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kontrol.Commands
{
    internal class KeyboardCommand : ICommand
    {
        public Response HandleRequest(Request request)
        {
            KeyboardController.PressKey(Convert.ToInt32(request.RequestedParameters[0]));

            return new Response(TCPStatusCodes.Ok);
        }

        public string GetCommandName()
        {
            return "keyboard";
        }

        public String Nume { get; set; }
    }
}