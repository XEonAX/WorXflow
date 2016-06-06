using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AEonAX.Shared;
using WebSocketSharp.Net.WebSockets;

namespace WorXflow.Server
{
    public class WS_Session : NotifyBase
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public WebSocketContext Context { get; internal set; }
    }
}
