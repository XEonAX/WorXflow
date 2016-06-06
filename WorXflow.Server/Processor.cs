using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AEonAX.Shared;
using WebSocketSharp;
using WebSocketSharp.Server;
namespace WorXflow.Server
{
    public class Processor : NotifyBase
    {
        private Viewmodel viewmodel;

        public WebSocketServer WsServer { get; set; }


        public Processor(Viewmodel viewmodel)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this.viewmodel = viewmodel;
        }

        internal void StartServer(string serverName, int portNumber)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (WsServer!=null)
            {
                WsServer.Stop();
            }
            WsServer = new WebSocketServer(portNumber);
            WsServer.Start();
            WsServer.AddWebSocketService("/Chat",CreateBehavior);

        }

        private WorXflowBehavior CreateBehavior()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            return new WorXflowBehavior(this,viewmodel);
        }
    }
}
