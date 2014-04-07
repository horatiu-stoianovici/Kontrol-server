using Kontrol.Commands;
using Kontrol.Components;
using Kontrol.Security;
using Kontrol.Servers;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Kontrol
{
    public class KontrolProgram
    {
        private static KontrolBrain brain = KontrolBrain.Instance;

        public static void Main()
        {
            KontrolRunner.Instance.Run();

            Console.WriteLine("Kontrol server started on {0}", HostInfo.IpAddressString);
            Thread.Sleep(TimeSpan.FromDays(1));
        }
    }
}