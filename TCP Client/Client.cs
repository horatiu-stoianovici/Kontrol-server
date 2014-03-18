using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;


namespace Kontrol
{
    public class clnt
    {

        public static void Main()
        {

            try
            {
                TcpClient tcpclnt = new TcpClient();
                Console.WriteLine("Connecting.....");

                tcpclnt.Connect(Host.GetIp(), 8001);
                // use the ipaddress as in the server program

                Console.WriteLine("Connected");
                Console.Write("Enter the string to be transmitted : ");

                String str = Console.ReadLine();
                Stream stm = tcpclnt.GetStream();

                while (true)
                {
                    ASCIIEncoding asen = new ASCIIEncoding();
                    byte[] ba = asen.GetBytes(str);
                    Console.WriteLine("Transmitting.....");

                    stm.Write(ba, 0, ba.Length);

                    byte[] bb = new byte[100];
                    int k = stm.Read(bb, 0, 100);
                    string receivedStatus = Encoding.ASCII.GetString(bb, 0, k);
                    Console.WriteLine(receivedStatus);

                    Console.WriteLine();
                    Console.Write("Enter the string to be transmitted : ");
                    str = Console.ReadLine();

                    if (!str.StartsWith("keep"))
                        break;
                }
                tcpclnt.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }

    }
}