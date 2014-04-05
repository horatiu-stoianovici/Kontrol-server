using Kontrol.Components;
using Kontrol.Properties;
using Kontrol.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Kontrol.Security
{
    public class SecurityManager
    {
        /// <summary>
        /// Certifies that the client has "signed in" the server
        /// </summary>
        /// <param name="macAddress">the authorization key provided on log in</param>
        /// <returns></returns>
        public static bool IsAuthorized(string macAddress)
        {
            if (Settings.Default.authorizedMACs == null)
            {
                Settings.Default.authorizedMACs = new StringCollection();
                Settings.Default.Save();
            }
            return Settings.Default.authorizedMACs.Contains(macAddress);
        }

        /// <summary>
        /// Adds a mac address to the list of authorized devices
        /// </summary>
        public static void AuthorizeClient(string macAddress)
        {
            Settings.Default.authorizedMACs.Add(macAddress);
            Settings.Default.Save();
        }
    }
}