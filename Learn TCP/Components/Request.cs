using Kontrol.Security;
using Kontrol.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Helpers;

namespace Kontrol.Components
{
    /// <summary>
    /// This class represents a request (In the context of the Kontrol app)
    /// </summary>
    public class Request
    {
        private string _requestText;
        private EndPoint theEndPoint;
        private String macAddress;
        private List<String> parameters;
        private String commandName;

        /// <summary>
        /// Helper class to pass json data
        /// </summary>
        private struct JsonDataHolder
        {
            public string MACAddress;
            public List<string> Parameters;
            public string CommandName;
        }

        /// <summary>
        /// Creates a TCP Request object
        /// </summary>
        /// <param name="receivedBytes">The actual received bytes</param>
        /// <param name="nrOfReceivedBytes">the number of bytes that were received in the array</param>
        /// <param name="endPoint">The end point that represents the sender</param>
        /// <param name="status">
        /// An output parameter that repesents the success or insuccess of accepting the request
        /// </param>
        public Request(byte[] receivedBytes, int nrOfReceivedBytes, EndPoint endPoint, out Status status)
        {
            _requestText = Encoding.ASCII.GetString(receivedBytes, 0, nrOfReceivedBytes).Trim();

            try
            {
                JsonDataHolder req = Json.Decode<JsonDataHolder>(_requestText);

                macAddress = req.MACAddress;
                parameters = new List<String>(req.Parameters);
                commandName = req.CommandName;
            }
            catch (Exception e)
            {
                status = Status.InvalidFormat;
                return;
            }

            if (!isAuthorised())
                status = Status.TCPAuthorizationFailed;
            else
            {
                status = Status.OK;
            }
        }

        internal Request(byte[] receivedBytes, int nrOfReceivedBytes, EndPoint endPoint)
        {
            _requestText = Encoding.ASCII.GetString(receivedBytes, 0, nrOfReceivedBytes);
            dynamic req = Json.Decode(_requestText);

            macAddress = req.MACAddress;
            parameters = req.Parameters;
            commandName = req.CommandName;
        }

        /// <summary>
        /// The raw request text
        /// </summary>
        public string RequestText
        {
            get
            {
                return _requestText;
            }
        }

        /// <summary>
        /// The name of the requested command
        /// </summary>
        public string RequestedCommandName
        {
            get
            {
                return commandName;
            }
        }

        /// <summary>
        /// The parameters sent in the request
        /// </summary>
        public List<string> RequestedParameters
        {
            get
            {
                return parameters;
            }
        }

        internal string MACAddress
        {
            get
            {
                return macAddress;
            }
        }

        /// <summary>
        /// Determines if the request is authorised or not
        /// </summary>
        /// <returns>true if it is authorised, false if not</returns>
        private bool isAuthorised()
        {
            if (RequestedCommandName.ToUpper() != "AUTHORIZE")
            {
                bool auth = SecurityManager.Instance.IsAuthorized(macAddress);
                if (!auth)
                    Log.Info("Security", "Attempt to execute a command without being authorized : MAC = " + macAddress);
                return auth;
            }
            return true;
        }

        /// <summary>
        /// The ip address of the client
        /// </summary>
        public IPEndPoint ClientEndPoint
        {
            get
            {
                return theEndPoint as IPEndPoint;
            }
        }
    }
}