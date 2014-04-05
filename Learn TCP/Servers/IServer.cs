using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kontrol.Servers
{
    public interface IServer
    {
        void Run();

        void Stop();

        bool LogRequests { get; set; }
    }
}