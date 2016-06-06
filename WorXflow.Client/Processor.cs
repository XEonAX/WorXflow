using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AEonAX.Shared;
using WebSocketSharp;
namespace WorXflow.Client
{
    public class Processor : NotifyBase
    {
        public WebSocket WorXSocket { get; set; }
        public Processor()
        {
           

        }

        internal void Connect(string serverURL)
        {

            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            WorXSocket = new WebSocket(serverURL);
            WorXSocket.ConnectAsync();
        }
    }
}
