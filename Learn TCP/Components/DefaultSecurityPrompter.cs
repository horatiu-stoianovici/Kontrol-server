using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kontrol.Components
{
    /// <summary>
    /// The default security prompter: uses console to prompt the user
    /// </summary>
    public class DefaultSecurityPrompter : ISecurityPrompter
    {
        public bool PropmptUserToAuthorizeMAC(string mac)
        {
            lock (typeof(Console))
            {
                Console.WriteLine("A user with mac address '{0}' wants to access this computer. Do you want to allow that?\nY/N");
                String answer = Console.ReadLine();
                return answer.ToLower() == "y";
            }
        }
    }
}