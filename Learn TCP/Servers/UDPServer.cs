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

            IPEndPoint ep = null;

            int receivedCount = 0;

            while (shouldRun)
            {
                String rdata = Encoding.ASCII.GetString(udpc.Receive(ref ep));

                receivedCount++;

                var response = KontrolBrain.Instance.CreateAndHandleRequest(Encoding.ASCII.GetBytes(rdata), rdata.Length, ep);

                var responseBytes = Encoding.ASCII.GetBytes(response.ToString());

                udpc.Send(responseBytes, responseBytes.Length, ep);
            }
        }
    }
}