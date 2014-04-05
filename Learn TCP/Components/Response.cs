using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Helpers;

namespace Kontrol.Components
{
    public class Response
    {
        private StringBuilder responseContent = new StringBuilder();

        public TCPStatusCodes StatusCode { get; set; }

        public Response()
            : this(TCPStatusCodes.Ok, "")
        {
        }

        public Response(TCPStatusCodes code)
            : this(code, "")
        {
        }

        public Response(TCPStatusCodes code, string content)
        {
            StatusCode = code;
        }

        /// <summary>
        /// Writes content to the response
        /// </summary>
        public void Write(string content)
        {
            responseContent.Append(content);
        }

        /// <summary>
        /// Get string representation of the response (JSON)
        /// </summary>
        public override string ToString()
        {
            return Json.Encode(new
            {
                StatusCode = (int)StatusCode,
                Content = responseContent.ToString()
            });
        }
    }

    public enum TCPStatusCodes
    {
        Ok = 200,
        NotAuthorized = 303,
        WrongFormat = 405,
        NotFound = 404
    }
}