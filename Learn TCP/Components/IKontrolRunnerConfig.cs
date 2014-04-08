using Kontrol.Commands;
using Kontrol.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kontrol.Components
{
    public interface IKontrolRunnerConfig
    {
        /// <summary>
        /// The servers that can receive requests
        /// </summary>
        List<IServer> Servers { get; }

        /// <summary>
        /// The commands that can be executed
        /// </summary>
        List<ICommand> Commands { get; }

        /// <summary>
        /// The prompter used to prompt the user when someone tries to connect to the server and is not yet authorized
        /// </summary>
        ISecurityPrompter Prompter { get; set; }
    }
}