using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Kontrol.Components
{
    public class HostInfo
    {
        /// <summary>
        /// Gets the current IP address of the machine(as a string)
        /// </summary>
        /// <returns>The string representation of ip address (xxx.xxx.xxx.xxx)</returns>
        public static String IpAddressString
        {
            get
            {
                string name = Dns.GetHostName();
                try
                {
                    IPAddress[] addrs = Dns.Resolve(name).AddressList;
                    foreach (IPAddress addr in addrs)
                        return addr.ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the current IP address of the machine
        /// </summary>
        /// <returns>The ip address object</returns>
        public static IPAddress IpAddress
        {
            get
            {
                string name = Dns.GetHostName();
                try
                {
                    IPAddress[] addrs = Dns.Resolve(name).AddressList;
                    foreach (IPAddress addr in addrs)
                        return addr;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return null;
            }
        }

        /// <summary>
        /// The port number used
        /// </summary>
        public static int PORT
        {
            get
            {
                return 4672;
            }
        }

        /// <summary>
        /// the name of the PC
        /// </summary>
        public static string HostName
        {
            get
            {
                return System.Environment.MachineName;
            }
        }

        public static NetworkDevice Device
        {
            get
            {
                var interfaces = NetworkInterface.GetAllNetworkInterfaces();

                foreach (var interf in interfaces)
                {
                    if (interf.OperationalStatus == OperationalStatus.Up)
                        switch (interf.NetworkInterfaceType)
                        {
                            case NetworkInterfaceType.Wireless80211:
                                return NetworkDevice.WiFi;

                            default:
                                return NetworkDevice.Cable;
                        }
                }

                return NetworkDevice.Cable;
            }
        }

        public static Guid HostId
        {
            get
            {
                if (Kontrol.Properties.Settings.Default.hostID.Length == 0)
                {
                    Kontrol.Properties.Settings.Default.hostID = Guid.NewGuid().ToString();
                    Kontrol.Properties.Settings.Default.Save();
                }

                return Guid.Parse(Kontrol.Properties.Settings.Default.hostID);
            }
        }
    }

    public enum NetworkDevice
    {
        Cable = 1,
        WiFi = 2
    }
}