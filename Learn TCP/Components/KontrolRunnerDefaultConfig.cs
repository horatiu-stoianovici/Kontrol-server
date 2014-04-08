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
                    new TCPServer(){
                        LogRequests = true
                    },
                    new UDPServer(){
                        LogRequests = true
                    }
                };
            }
        }

        public List<Commands.ICommand> Commands
        {
            get
            {
                return new List<ICommand>
                {
                    new LoginCommand(),
                    new MouseMoveCommand(),
                    new MouseClickCommand(),
                    new ScrollCommand(),
                    new KeyboardCommand(),
                    new PresentationCommand()
                };
            }
        }

        private ISecurityPrompter prompter = null;

        public ISecurityPrompter Prompter
        {
            get
            {
                if (prompter == null)
                    prompter = new DefaultSecurityPrompter();
                return prompter;
            }

            set
            {
                prompter = value;
            }
        }
    }
}