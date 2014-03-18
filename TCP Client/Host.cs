using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Kontrol
{
    class Host
    {
        /// <summary>
        /// Gets the current IP address of the machine
        /// </summary>
        /// <returns>The string representation of ip address (xxx.xxx.xxx.xxx)</returns>
        public static string GetIp()
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
}
