using Kontrol.Components;
using Kontrol.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace Kontrol_Server_Tests
{
    [TestClass]
    public class SecurityUnitTests
    {
        [TestMethod]
        public void TestSecurityWithoutPasswords()
        {
            IPEndPoint hostEndPoint = new IPEndPoint(HostInfo.IpAddress, 300);
            bool success = false;
            SecurityManager.AuthorizeClient("baba face mere", hostEndPoint, out success);
            Assert.IsTrue(success, "Could not authorize withour requiring password");
            Assert.IsTrue(SecurityManager.IsAuthorized("", hostEndPoint), "Could not authorize without requiring password");
        }

        [TestMethod]
        public void TestSecurityWithPasswords()
        {
            IPEndPoint hostEndPoint = new IPEndPoint(HostInfo.IpAddress, 300);
            bool success = false;

            SecurityManager.EnablePassword("nbancba");
            Assert.IsTrue(HostInfo.RequiresPassword, "Could not enable password");

            SecurityManager.AuthorizeClient("cucurigu", hostEndPoint, out success);
            Assert.IsFalse(success, "Authorised with wrong password");

            var authGuid = SecurityManager.AuthorizeClient("nbancba", hostEndPoint, out success);
            Assert.IsTrue(success);

            Assert.IsTrue(SecurityManager.IsAuthorized(authGuid.ToString(), hostEndPoint), "Could not authorize using valid authorization key");
            Console.WriteLine("Using bad authorization key: authorise success = \t\t{0}", !SecurityManager.IsAuthorized(Guid.NewGuid().ToString(), hostEndPoint));

            Assert.IsFalse(SecurityManager.ChangePassword("nbancb", "cucurigu"), "Changed the password using an invalid old password!");
            Assert.IsTrue(SecurityManager.ChangePassword("nbancba", "cucurigu"), "Could not change the password using a valid old password");

            Assert.IsFalse(SecurityManager.IsAuthorized(authGuid.ToString(), hostEndPoint), "Used old authorization token with success! => Nok");

            SecurityManager.AuthorizeClient("nbancba", hostEndPoint, out success);
            Assert.IsFalse(success, "Tried to get authorized with old password worked!");
            authGuid = SecurityManager.AuthorizeClient("cucurigu", hostEndPoint, out success);
            Assert.IsTrue(success, "Failed to authorize valid client");

            Assert.IsFalse(SecurityManager.DisablePassword("dasjd"), "Could disable password using a wrong one!");
            Assert.IsTrue(SecurityManager.DisablePassword("cucurigu"), "Could not disable password using the correct old password");
        }
    }
}