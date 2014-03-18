using Kontrol.Security;
using Kontrol.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Kontrol.Components
{
    /// <summary>
    /// This class represents a request (In the context of the Kontrol app)
    /// </summary>
    public class Request
    {
        private string _requestText;
        private List<string> splitted;
        private EndPoint theEndPoint;

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
            splitted = new List<string>(_requestText.Split('~'));
            theEndPoint = endPoint;

            if (!isValid())
                status = Status.InvalidFormat;
            else if (!isAuthorised())
                status = Status.TCPAuthorizationFailed;
            else
            {
                status = Status.OK;
            }
        }

        internal Request(byte[] receivedBytes, int nrOfReceivedBytes, EndPoint endPoint)
        {
            _requestText = Encoding.ASCII.GetString(receivedBytes, 0, nrOfReceivedBytes);
            splitted = new List<string>(_requestText.Split('~'));
            theEndPoint = endPoint;
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
                return splitted[0];
            }
        }

        /// <summary>
        /// The parameters sent in the request
        /// </summary>
        public List<string> RequestedParameters
        {
            get
            {
                return splitted.GetRange(2, splitted.Count - 2);
            }
        }

        internal string AuthToken
        {
            get
            {
                return splitted[1];
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
                bool auth = SecurityManager.IsAuthorized(AuthToken, theEndPoint as IPEndPoint);
                if (!auth)
                    Log.Info("Security", "Attepmpt to execute a command without being authorized from ip " + ClientEndPoint.Address.ToString());
                return auth;
            }
            return true;
        }

        /// <summary>
        /// If the request is in a valid format
        /// </summary>
        /// <returns>true if it is in valid format; false if it is not</returns>
        private bool isValid()
        {
            if (splitted[0].ToUpper().Trim() == "AUTHORIZE")
            {
                return splitted.Count == 2;
            }
            else
            {
                Guid guid;
                return splitted.Count >= 2 && Guid.TryParse(splitted[1].ToLower().Trim(), out guid);
            }
        }

        public IPEndPoint ClientEndPoint
        {
            get
            {
                return theEndPoint as IPEndPoint;
            }
        }
    }
}