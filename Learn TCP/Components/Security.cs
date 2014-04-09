using Kontrol.Components;
using Kontrol.Properties;
using Kontrol.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private static object lockObj = new object();
        private static SecurityManager instance = null;

        private SecurityManager()
        {
            initMACs();
        }

        private void initMACs()
        {
            if (Settings.Default.authorizedMACs == null)
            {
                Settings.Default.authorizedMACs = new StringCollection();
                Settings.Default.Save();
            }

            if (Settings.Default.notAuthorizedMACs == null)
            {
                Settings.Default.notAuthorizedMACs = new StringCollection();
                Settings.Default.Save();
            }
            AcceptedMACs = new ObservableCollection<string>(Settings.Default.authorizedMACs.Cast<String>());
            NotAcceptedMACs = new ObservableCollection<string>(Settings.Default.notAuthorizedMACs.Cast<String>());
        }

        /// <summary>
        /// The instance of the security manager
        /// </summary>
        public static SecurityManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                            instance = new SecurityManager();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Certifies that the client has "signed in" the server
        /// </summary>
        /// <param name="macAddress">the authorization key provided on log in</param>
        /// <returns></returns>
        public bool IsAuthorized(string macAddress)
        {
            var settings = Settings.Default;
            if (settings.authorizedMACs == null)
            {
                settings.authorizedMACs = new StringCollection();
                settings.Save();
                initMACs();
            }
            //if the mac was not authorized before, check if it was denied before
            if (!settings.authorizedMACs.Contains(macAddress))
            {
                if (settings.notAuthorizedMACs == null)
                {
                    settings.notAuthorizedMACs = new StringCollection();
                    settings.Save();
                    initMACs();
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
        private bool promptUserToAuthorizeMAC(string macAddress)
        {
            if (KontrolRunner.Instance.Config.Prompter.PropmptUserToAuthorizeMAC(macAddress))
            {
                AllowMAC(macAddress);
                return true;
            }
            else
            {
                DenyMAC(macAddress);
                return false;
            }
        }

        /// <summary>
        /// Authorizes a mac address
        /// </summary>
        public void AllowMAC(string mac)
        {
            if (!Settings.Default.authorizedMACs.Contains(mac))
            {
                Settings.Default.authorizedMACs.Add(mac);
                AcceptedMACs.Add(mac);
            }

            if (Settings.Default.notAuthorizedMACs.Contains(mac))
            {
                Settings.Default.notAuthorizedMACs.Remove(mac);
                NotAcceptedMACs.Remove(mac);
            }

            Settings.Default.Save();
        }

        /// <summary>
        /// Denies authorization for a mac address
        /// </summary>
        public void DenyMAC(string mac)
        {
            if (!Settings.Default.notAuthorizedMACs.Contains(mac))
            {
                Settings.Default.notAuthorizedMACs.Add(mac);
                NotAcceptedMACs.Add(mac);
            }

            if (Settings.Default.authorizedMACs.Contains(mac))
            {
                Settings.Default.authorizedMACs.Remove(mac);
                AcceptedMACs.Remove(mac);
            }

            Settings.Default.Save();
        }

        public ObservableCollection<string> AcceptedMACs { get; private set; }

        public ObservableCollection<string> NotAcceptedMACs { get; private set; }
    }
}