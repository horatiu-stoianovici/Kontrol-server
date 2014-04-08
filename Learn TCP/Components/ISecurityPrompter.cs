using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kontrol.Components
{
    /// <summary>
    /// The interface for an object that prompts the user to authorize a request
    /// </summary>
    public interface ISecurityPrompter
    {
        bool PropmptUserToAuthorizeMAC(string mac);
    }
}