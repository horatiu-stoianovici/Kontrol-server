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
    public class LoginCommand : ICommand
    {
        public string GetCommandName()
        {
            return "authorize";
        }

        public Response HandleRequest(Request request)
        {
            Response response = new Response();
            response.StatusCode = TCPStatusCodes.Ok;

            if (!SecurityManager.IsAuthorized(request.MACAddress))
            {
                //TODO: remove this in release
                SecurityManager.AuthorizeClient(request.MACAddress);
                response.StatusCode = TCPStatusCodes.Ok;
                Log.Info("Security", "Use authorised successfully with MAC " + request.MACAddress);
                return response;
                response.StatusCode = TCPStatusCodes.NotAuthorized;
                Log.Info("Security", "Authorization attempt failed from MAC " + request.MACAddress);
            }
            else
            {
                response.StatusCode = TCPStatusCodes.Ok;
                Log.Info("Security", "Use authorised successfully from MAC " + request.MACAddress);
            }

            return response;
        }
    }
}