using Kontrol.Components;
using Kontrol.Security;
using Kontrol.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrol.Commands
{
    public class LoginCommand : Command
    {
        public string GetCommandName()
        {
            return "authorize";
        }

        public Response HandleRequest(Request request)
        {
            bool success;
            Response response = new Response();
            response.StatusCode = TCPStatusCodes.Ok;

            Guid authToken = SecurityManager.AuthorizeClient(request.AuthToken, request.ClientEndPoint, out success);

            if (!success)
            {
                response.StatusCode = TCPStatusCodes.NotAuthorized;
                response.Write("wrong password");
                Log.Info("Security", "Authorization attempt failed from ip " + request.ClientEndPoint.Address.ToString());
            }
            else
            {
                response.StatusCode = TCPStatusCodes.Ok;
                response.Write(authToken.ToString());
                Log.Info("Security", "Use authorised successfully from ip " + request.ClientEndPoint.Address.ToString());
            }

            return response;
        }
    }
}