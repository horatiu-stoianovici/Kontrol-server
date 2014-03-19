using Kontrol.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrol.Commands
{
    public interface ICommand
    {
        Response HandleRequest(Request request);

        string GetCommandName();
    }
}