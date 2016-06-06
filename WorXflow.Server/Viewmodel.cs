using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AEonAX.Shared;
using WebSocketSharp;

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
        public SimpleCommand StartServer { get; set; }
        public Processor Processor { get; set; }
        public BindingList<WS_Session> OnlineSessions { get; set; }


        public Viewmodel(MainWindow mainWindow)
        {
            this.mW = mainWindow;
            Settings = Settings.ReadFromfile(Application.ResourceAssembly.GetName().Name + ".xml", true);
            Settings = Settings ?? new Settings();
            Processor = new Processor(this);
            OnlineSessions = new BindingList<WS_Session>();
            StartServer = new SimpleCommand()
            {
                ExecuteDelegate = x =>
                {
                    try
                    {
                        Processor.StartServer(Settings.ServerName, Settings.PortNumber);
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
            var conn = OnlineSessions.FirstOrDefault(x => x.ID == worXflowBehavior.ID);
            if (conn != null)
            {
                mW.WPFUIize(() =>
                  OnlineSessions.Remove(conn));
            }
        }

        internal void WSS_Message(MessageEventArgs e, WorXflowBehavior worXflowBehavior)
        {

            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        internal void WSS_Close(CloseEventArgs e, WorXflowBehavior worXflowBehavior)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            var conn = OnlineSessions.FirstOrDefault(x => x.ID == worXflowBehavior.ID);
            if (conn != null)
            {
                mW.WPFUIize(() =>
                  OnlineSessions.Remove(conn));
            }

        }

        internal void WSS_Open(WorXflowBehavior worXflowBehavior)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            mW.WPFUIize(() =>
            OnlineSessions.Add(new WS_Session { ID = worXflowBehavior.ID, Context = worXflowBehavior.Context }));
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
