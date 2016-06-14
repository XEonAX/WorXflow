using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WorXflow.Server
{
    class WorXflowBehavior : WebSocketBehavior
    {
        private Processor processor;
        private Viewmodel viewmodel;
        public string Name { get; set; }

        public WorXflowBehavior(Processor processor, Viewmodel viewmodel)
        {
            this.processor = processor;
            this.viewmodel = viewmodel;
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            Name = Context.QueryString["Name"];
            viewmodel.WSS_Open(this);
        }
        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
            viewmodel.WSS_Close(e, this);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
            viewmodel.WSS_Message(e, this);
        }
        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
            viewmodel.WSS_Error(e, this);
        }

    }
}
