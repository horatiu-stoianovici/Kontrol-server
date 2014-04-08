using Kontrol.Components;
using Kontrol.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Kontrol.Servers
{
    public class UDPServer : IServer
    {
        private const int PORT = 4672;
        private bool shouldRun = false;
        private Thread runningThread;

        public UDPServer()
        {
            runningThread = new Thread(new ThreadStart(startServer));
            runningThread.IsBackground = true;
        }

        public void Run()
        {
            if (!shouldRun)
            {
                shouldRun = true;
                runningThread.Start();
            }
        }

        public void Stop()
        {
            shouldRun = false;
        }

        private void startServer()
        {
            UdpClient udpc = new UdpClient(PORT);
            Log.Info("Servers", "UDP Server started");

            while (shouldRun)
            {
                IPEndPoint ep = null;
                var rdata = udpc.Receive(ref ep);
                new Thread(new ParameterizedThreadStart(processRequest)).Start(new object[] { rdata, ep });
            }
        }

        /// <summary>
        /// Processing the request in a different thread
        /// </summary>
        /// <param name="data"></param>
        private void processRequest(object data)
        {
            var parameters = (object[])data;
            var response = KontrolBrain.Instance.CreateAndHandleRequest((byte[])parameters[0], ((byte[])parameters[0]).Length, (IPEndPoint)parameters[1]);

            if (LogRequests)
            {
                //TODO maybe log requests
            }
        }

        public bool LogRequests { get; set; }
    }
}