using Kontrol.Components;
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
            throw new NotImplementedException();
        }

        public string GetCommandName()
        {
            return "keyboard";
        }

        public String Nume { get; set; }
    }
}