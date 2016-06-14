using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AEonAX.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketSharp;
using WorXflow.Common;
using WorXflow.Data;

/// <summary>
//     _____   ______   _____   __      __  ______   _____  
//    / ____| |  ____| |  __ \  \ \    / / |  ____| |  __ \ 
//   | (___   | |__    | |__) |  \ \  / /  | |__    | |__) |
//    \___ \  |  __|   |  _  /    \ \/ /   |  __|   |  _  / 
//    ____) | | |____  | | \ \     \  /    | |____  | | \ \ 
//   |_____/  |______| |_|  \_\     \/     |______| |_|  \_\
//                                                          
/// </summary>
namespace WorXflow.Server
{
    public class Viewmodel : NotifyBase
    {
        private MainWindow mW;

        public Settings Settings { get; set; }
        public SimpleCommand Restart { get; set; }
        public SimpleCommand Send { get; set; }
        public Processor Processor { get; set; }
        public BindingList<WS_Session> OnlineSessions { get; set; }
        public RichTextBox rtbMessages { get; internal set; }
        private string _InputText;
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


        public Viewmodel(MainWindow mainWindow)
        {
            this.mW = mainWindow;
            Settings = Settings.ReadFromfile(Application.ResourceAssembly.GetName().Name + ".xml", true);
            Settings = Settings ?? new Settings();
            Processor = new Processor(this);
            OnlineSessions = new BindingList<WS_Session>();
            Restart = new SimpleCommand()
            {
                ExecuteDelegate = x =>
                {
                    try
                    {
                        Processor.RestartServer(Settings.ServerName, Settings.PortNumber);
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
                        rtbMessages.PrintMessage("Server", InputText, Data.Type.Broadcast);
                        InputText = string.Empty;
                    }
                    catch (Exception Ex)
                    {
                        ExceptionThrown(Processor, Ex);
                    }
                }
            };

        }

        internal void WSS_Error(ErrorEventArgs e, WorXflowBehavior worXflowBehavior)
        {

            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            lock (OnlineSessions)
            {
                var conn = OnlineSessions.FirstOrDefault(x => x.ID == worXflowBehavior.ID);
                if (conn != null)
                {
                    mW.WPFUIize(() =>
                      OnlineSessions.Remove(conn));
                    rtbMessages.PrintMessage(worXflowBehavior.Name, worXflowBehavior.Name + " left hastily.", Data.Type.Disconnect);

                    var msg = JsonConvert.SerializeObject(new WebMessage { User = "Server", Text = worXflowBehavior.Name + " left hastily.", Type = Data.Type.Disconnect });
                    foreach (var session in OnlineSessions)
                    {
                        session.Context.WebSocket.Send(msg);
                    }
                }
            }

        }

        internal void WSS_Message(MessageEventArgs e, WorXflowBehavior worXflowBehavior)
        {
            if (e.Data.IsValidJson())
            {
                var msg = JsonConvert.DeserializeObject<WebMessage>(e.Data);
                rtbMessages.PrintMessage(worXflowBehavior.Name, msg.Text, msg.Type);
                msg.User = worXflowBehavior.Name;
                var jMsg = JsonConvert.SerializeObject(msg);
                foreach (var session in OnlineSessions)
                {
                    session.Context.WebSocket.Send(jMsg);
                }
            }
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        internal void WSS_Close(CloseEventArgs e, WorXflowBehavior worXflowBehavior)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            lock (OnlineSessions)
            {
                var conn = OnlineSessions.FirstOrDefault(x => x.ID == worXflowBehavior.ID);
                if (conn != null)
                {
                    mW.WPFUIize(() =>
                      OnlineSessions.Remove(conn));
                    rtbMessages.PrintMessage(worXflowBehavior.Name, worXflowBehavior.Name + " left.", Data.Type.Disconnect);
                    var msg = JsonConvert.SerializeObject(new WebMessage { User = "Server", Text = worXflowBehavior.Name + " left.", Type = Data.Type.Disconnect });
                    foreach (var session in OnlineSessions)
                    {
                        session.Context.WebSocket.Send(msg);
                    }
                }
            }

        }

        internal void WSS_Open(WorXflowBehavior worXflowBehavior)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            mW.WPFUIize(
                () =>
                     OnlineSessions.Add(
                            new WS_Session
                            {
                                ID = worXflowBehavior.ID,
                                Context = worXflowBehavior.Context,
                                Name = worXflowBehavior.Context.QueryString["Name"]
                            })
                );
            rtbMessages.PrintMessage(worXflowBehavior.Name, worXflowBehavior.Name + " joined.", Data.Type.Connect);
            var msg = JsonConvert.SerializeObject(new WebMessage { User = "Server", Text = worXflowBehavior.Name + " joined.", Type = Data.Type.Connect });
            foreach (var session in OnlineSessions)
            {
                session.Context.WebSocket.Send(msg);
            }
        }

        public event EventHandler<Exception> ExceptionThrown;
        protected virtual void OnExceptionThrown(Exception e)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            EventHandler<Exception> eh = ExceptionThrown;
            if (eh != null)
            {
                eh(this, e);
            }
        }
    }
}
