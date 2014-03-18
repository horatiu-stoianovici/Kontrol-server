using Kontrol.Commands;
using Kontrol.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Kontrol.Components
{
    public class KontrolBrain
    {
        private static KontrolBrain instance = new KontrolBrain();

        private KontrolBrain()
        {
        }

        public static KontrolBrain Instance
        {
            get
            {
                return instance;
            }
        }

        private Dictionary<string, Command> commands = new Dictionary<string, Command>();

        /// <summary>
        /// Creates the request object from the received input (the input can be received from anywhere)
        /// </summary>
        /// <returns>A response</returns>
        public Response CreateAndHandleRequest(byte[] receivedBytes, int amountOfReceivedBytes, IPEndPoint ipEndPoint)
        {
            Status status;
            Request request = new Request(receivedBytes, amountOfReceivedBytes, ipEndPoint, out status);

            Response response = new Response();
            response.StatusCode = TCPStatusCodes.NotFound;

            if (status == Status.InvalidFormat)
            {
                response.StatusCode = TCPStatusCodes.WrongFormat;
            }
            else if (status == Status.TCPAuthorizationFailed)
            {
                response.StatusCode = TCPStatusCodes.NotAuthorized;
            }
            else
            {
                bool found = false;
                foreach (var command in commands)
                {
                    if (command.Key == request.RequestedCommandName)
                    {
                        response = command.Value.HandleRequest(request);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    response.StatusCode = TCPStatusCodes.NotFound;
                    response.Write("The command you requested does not exist");
                }
            }

            return response;
        }

        public void AddCommand(Command command)
        {
            commands.Add(command.GetCommandName(), command);
        }
    }
}