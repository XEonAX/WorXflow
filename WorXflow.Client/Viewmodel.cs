using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AEonAX.Shared;
using Newtonsoft.Json;
using WebSocketSharp;
using WorXflow.Common;
using WorXflow.Data;

/// <summary>
//     _____   _        _____   ______   _   _   _______ 
//    / ____| | |      |_   _| |  ____| | \ | | |__   __|
//   | |      | |        | |   | |__    |  \| |    | |   
//   | |      | |        | |   |  __|   | . ` |    | |   
//   | |____  | |____   _| |_  | |____  | |\  |    | |   
//    \_____| |______| |_____| |______| |_| \_|    |_|   
//                                                       
/// </summary>
namespace WorXflow.Client
{
    public class Viewmodel : NotifyBase
    {
        public Settings Settings { get; set; }
        public SimpleCommand Connect { get; set; }
        public SimpleCommand Send { get; set; }
        public Processor Processor { get; set; }
        private string _InputText;

        internal void WXOnClose(object sender, CloseEventArgs e)
        {
            rtbMessages.PrintMessage("Server", "Connection Closed.", Data.Type.Disconnect);

        }

        internal void WXOnOpen(object sender, EventArgs e)
        {
            rtbMessages.PrintMessage("Server", "Connection Opened.", Data.Type.Connect);
        }

        internal void WXOnError(object sender, ErrorEventArgs e)
        {
            rtbMessages.PrintMessage("Server", "Connection Errored.", Data.Type.Disconnect);
        }

        internal void WXOnMessage(object sender, MessageEventArgs e)
        {
            if (e.Data.IsValidJson())
            {
                var msg = JsonConvert.DeserializeObject<WebMessage>(e.Data);
                rtbMessages.PrintMessage(msg.User, msg.Text, msg.Type);
            }
        }

        public string InputText
        {
            get { return _InputText; }
            set
            {
                if (!Equals(_InputText, value))
                {
                    _InputText = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public RichTextBox rtbMessages { get; internal set; }

        public Viewmodel()
        {
            Settings = Settings.ReadFromfile(Application.ResourceAssembly.GetName().Name + ".xml", true);
            Settings = Settings ?? new Settings();
            Processor = new Processor(this);

            Connect = new SimpleCommand()
            {
                ExecuteDelegate = x =>
                {
                    try
                    {
                        Processor.Connect(Settings.ServerURL);
                    }
                    catch (Exception Ex)
                    {
                        ExceptionThrown(Processor, Ex);
                    }
                }
            };

            Send = new SimpleCommand()
            {
                ExecuteDelegate = x =>
                {
                    try
                    {
                        Processor.Send(InputText);
                        InputText = string.Empty;
                    }
                    catch (Exception Ex)
                    {
                        ExceptionThrown(Processor, Ex);
                    }
                }
            };



        }

        public event EventHandler<Exception> ExceptionThrown;
        protected virtual void OnExceptionThrown(Exception e)
        {
            EventHandler<Exception> eh = ExceptionThrown;
            if (eh != null)
            {
                eh(this, e);
            }
        }
    }
}
