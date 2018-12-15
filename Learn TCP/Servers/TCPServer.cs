using Kontrol.Components;
using Kontrol.Servers;
using Kontrol.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrol.Servers
{
    public class TCPServer : IServer
    {
        private IPAddress ipAddress;
        private TcpListener myListener;
        private bool started, shouldStop;
        private Thread runningThread, detectionThread;

        public TCPServer()
        {
            myListener = new TcpListener(IPAddress.Any, 8001);
        }

        public void Run()
        {
            if (runningThread != null)
            {
                throw new Exception("The server is already started");
            }
            runningThread = new Thread(new ThreadStart(startServer));
            runningThread.IsBackground = true;
            runningThread.Start();

            detectionThread = new Thread(new ThreadStart(sendDetectionSignal));
            detectionThread.IsBackground = true;
            detectionThread.Start();
        }

        public void Stop()
        {
            shouldStop = true;
            runningThread = null;
            started = false;
        }

        private void startServer()
        {
            /* Start Listeneting at the specified port */
            myListener.Start();
            Log.Info("Servers", "TCP Server started");

            while (!shouldStop)
            {
                bool shouldKeepConnectionAlive = true;
                Socket mySocket = null;

                //receiving data from the connection
                while (shouldKeepConnectionAlive && !shouldStop)
                {
                    //accepting connection
                    mySocket = myListener.AcceptSocket();

                    byte[] receivedBytes = new byte[200];
                    int amountOfReceivedBytes = 0;
                    try
                    {
                        amountOfReceivedBytes = mySocket.Receive(receivedBytes);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }

                    shouldKeepConnectionAlive = true;

                    Response response = KontrolBrain.Instance.CreateAndHandleRequest(receivedBytes, amountOfReceivedBytes, mySocket.RemoteEndPoint as IPEndPoint);
                    if (LogRequests)
                    {
                        //TODO maybe log requests
                    }

                    var responseText = response.ToString();
                    mySocket.Send(Encoding.ASCII.GetBytes(response.ToString() + "\n"), responseText.Length + 1, SocketFlags.None);
                }

                /* clean up */
                if (mySocket != null)
                {
                    mySocket.Close();
                }
            }
            myListener.Stop();
        }

        /// <summary>
        /// This is the thread that takes care of sending a signal to the phones, so that they can
        /// detect the server
        /// </summary>
        /// <example>
        ///
        /// 192. 168.0.100~DanielPC~1
        /// </example>
        public void sendDetectionSignal()
        {
            while (true)
            {
                UdpClient publisher = new UdpClient("234.6.7.2", HostInfo.PORT);

                byte[] sdata = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(
                    new
                    {
                        IPAddress = HostInfo.IpAddressString,
                        HostName = HostInfo.HostName,
                        Device = (int)HostInfo.Device,
                        HostId = HostInfo.HostId
                    }));
                publisher.Send(sdata, sdata.Length);

                Thread.Sleep(2000);
            }
        }

        public bool LogRequests { get; set; }
    }
}