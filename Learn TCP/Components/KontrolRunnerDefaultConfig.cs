using Kontrol.Commands;
using Kontrol.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kontrol.Components
{
    /// <summary>
    /// The default configuration for the kontrol runner
    /// </summary>
    internal class KontrolRunnerDefaultConfig : IKontrolRunnerConfig
    {
        public List<Servers.IServer> Servers
        {
            get
            {
                return new List<IServer>()
                {
                    new TCPServer(),
                    new UDPServer()
                };
            }
        }

        public List<Commands.Command> Commands
        {
            get
            {
                return new List<Command>
                {
                    new LoginCommand(),
                    new MouseMoveCommand(),
                    new MouseClickCommand(),
                    new ScrollCommand()
                };
            }
        }
    }
}