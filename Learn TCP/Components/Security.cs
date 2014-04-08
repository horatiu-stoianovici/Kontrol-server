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
            var settings = Settings.Default;
            if (settings.authorizedMACs == null)
            {
                settings.authorizedMACs = new StringCollection();
                settings.Save();
            }
            //if the mac was not authorized before, check if it was denied before
            if (!settings.authorizedMACs.Contains(macAddress))
            {
                if (settings.notAuthorizedMACs == null)
                {
                    settings.notAuthorizedMACs = new StringCollection();
                    settings.Save();
                }
                if (!settings.notAuthorizedMACs.Contains(macAddress))
                {
                    return promptUserToAuthorizeMAC(macAddress);
                }
            }
            return Settings.Default.authorizedMACs.Contains(macAddress);
        }

        /// <summary>
        /// Prompts the user to authorize or revoke authorization for a phone
        /// </summary>
        /// <param name="macAddress">The mac address of the phone</param>
        private static bool promptUserToAuthorizeMAC(string macAddress)
        {
            if (KontrolRunner.Instance.Config.Prompter.PropmptUserToAuthorizeMAC(macAddress))
            {
                Settings.Default.authorizedMACs.Add(macAddress);
                Settings.Default.Save();
                return true;
            }
            else
            {
                Settings.Default.notAuthorizedMACs.Add(macAddress);
                Settings.Default.Save();
                return false;
            }
        }
    }
}