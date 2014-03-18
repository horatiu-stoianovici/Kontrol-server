using Kontrol.Components;
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
        private static Dictionary<string, Guid> _authorized = new Dictionary<string, Guid>();

        /// <summary>
        /// Certifies that the client has "signed in" the server
        /// </summary>
        /// <param name="authorizationKey">the authorization key provided on log in</param>
        /// <param name="theEndPoint">an object representing</param>
        /// <returns></returns>
        public static bool IsAuthorized(string authorizationKey, IPEndPoint theEndPoint)
        {
            if (!Kontrol.Properties.Settings.Default.requiresPassword)
            {
                return true;
            }

            string ipAddress = theEndPoint.Address.ToString();

            if (!_authorized.ContainsKey(ipAddress))
            {
                return false;
            }

            Guid guid = Guid.Parse(authorizationKey);

            if (_authorized[ipAddress].CompareTo(guid) != 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Authorizes a client if it tries to login with the right password for the server
        /// </summary>
        /// <param name="password">The password</param>
        /// <param name="theEndPoint">IP of the client</param>
        /// <param name="success">Output parameter that specifies if the authorization process was successfull</param>
        /// <returns>The guid that will authorize the client in the future</returns>
        public static Guid AuthorizeClient(string password, IPEndPoint theEndPoint, out bool success)
        {
            var savedPasswordHash = Kontrol.Properties.Settings.Default.passwordHash;

            Guid g = Guid.NewGuid();
            if (savedPasswordHash == HashPassword(password) || !Kontrol.Properties.Settings.Default.requiresPassword)
            {
                if (!_authorized.ContainsKey(theEndPoint.Address.ToString()))
                    _authorized.Add(theEndPoint.Address.ToString(), g);
                else
                    _authorized[theEndPoint.Address.ToString()] = g;
                success = true;
            }
            else
            {
                success = false;
            }

            return g;
        }

        /// <summary>
        /// Changes the password for the server and clears all the authorized users
        /// </summary>
        /// <param name="oldPassword">The old password</param>
        /// <param name="newPassword">The new password</param>
        /// <returns>Success</returns>
        public static bool ChangePassword(string oldPassword, string newPassword)
        {
            var savedPasswordHash = Kontrol.Properties.Settings.Default.passwordHash;

            if (HashPassword(oldPassword) == savedPasswordHash)
            {
                Kontrol.Properties.Settings.Default.passwordHash = HashPassword(newPassword);
                Kontrol.Properties.Settings.Default.Save();
                _authorized.Clear();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Disables the password
        /// </summary>
        /// <param name="password">the current password</param>
        /// <returns>Success</returns>
        public static bool DisablePassword(string password)
        {
            var savedPasswordHash = Kontrol.Properties.Settings.Default.passwordHash;

            if (HashPassword(password) == savedPasswordHash)
            {
                Kontrol.Properties.Settings.Default.requiresPassword = false;
                Kontrol.Properties.Settings.Default.Save();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Enables password protection
        /// </summary>
        /// <param name="password">The password</param>
        public static void EnablePassword(string password)
        {
            if (!Kontrol.Properties.Settings.Default.requiresPassword)
            {
                Kontrol.Properties.Settings.Default.requiresPassword = true;
                Kontrol.Properties.Settings.Default.passwordHash = HashPassword(password);
                Kontrol.Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Generates the hash of the password
        /// </summary>
        /// <param name="password">The password</param>
        /// <returns>Hashed value</returns>
        private static String HashPassword(string password)
        {
            var hashAlgorithm = SHA512Cng.Create();

            return Encoding.ASCII.GetString(hashAlgorithm.ComputeHash(Encoding.ASCII.GetBytes(password)));
        }
    }
}