using Kontrol.Commands;
using Kontrol.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kontrol.Components
{
    public class KontrolRunner
    {
        private static KontrolRunner instance = new KontrolRunner();
        private Thread runningThread;
        private KontrolBrain brain = KontrolBrain.Instance;
        private List<IServer> servers = new List<IServer>();
        private IKontrolRunnerConfig config = new KontrolRunnerDefaultConfig();

        /// <summary>
        /// The configuration that is used for running
        /// The only thing you can change is the security prompter
        /// </summary>
        public IKontrolRunnerConfig Config
        {
            get
            {
                return config;
            }
        }

        private KontrolRunner()
        {
        }

        /// <summary>
        /// Gets the instance of this singleton
        /// </summary>
        public static KontrolRunner Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Runs the whole Kontrol app / server part using a custom configuration object
        /// </summary>
        /// <param name="config">The custom configuration object</param>
        public void Run(IKontrolRunnerConfig config)
        {
            config = new KontrolRunnerDefaultConfig();
            foreach (var server in config.Servers)
            {
                server.Run();
            }

            foreach (var command in config.Commands)
            {
                brain.AddCommand(command);
            }
        }

        /// <summary>
        /// Runs the whole Kontrol app / server part using standard configuration
        /// </summary>
        public void Run()
        {
            Run(new KontrolRunnerDefaultConfig());
        }
    }
}