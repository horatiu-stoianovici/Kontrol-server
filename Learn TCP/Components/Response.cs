using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kontrol.Components
{
    public class Response
    {
        private Dictionary<string, object> responseDict = new Dictionary<string, object>();
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
            responseDict = new Dictionary<string, object>
            {
                {"status-code", (int)code},
                {"content", content}
            };
            StatusCode = code;
        }

        public void Write(string p)
        {
            responseContent.Append(p);
        }

        public override string ToString()
        {
            responseDict["content"] = responseContent.ToString();
            responseDict["status-code"] = (int)StatusCode;
            return JsonConvert.SerializeObject(responseDict);
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