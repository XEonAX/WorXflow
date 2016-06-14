using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AEonAX.Shared;
using WebSocketSharp;
using WorXflow.Data;
using Newtonsoft.Json;

namespace WorXflow.Client
{
    public class Processor : NotifyBase
    {
        public WebSocket WorXSocket { get; set; }
        public Viewmodel VM { get; set; }
        private string Username = string.Empty;
        public Processor(Viewmodel vm)
        {
            VM = vm;
        }

        internal void Connect(string serverURL)
        {

            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Username = Faker.Internet.UserName();
            WorXSocket = new WebSocket(serverURL.Contains("?")
                ? serverURL + "&Name=" + Username
                : serverURL + "?Name=" + Username);
            WorXSocket.OnMessage += VM.WXOnMessage;
            WorXSocket.OnClose += VM.WXOnClose;
            WorXSocket.OnError += VM.WXOnError;
            WorXSocket.OnOpen += VM.WXOnOpen;
            WorXSocket.ConnectAsync();

        }
        

        internal void Send(string inputText)
        {
            if (WorXSocket != null && WorXSocket.IsAlive)
            {
                WebMessage msg = new WebMessage() { Text = inputText, Type = Data.Type.Broadcast, User = Username };
                WorXSocket.SendAsync(JsonConvert.SerializeObject(msg), _ => { });
            }
        }
    }
}
