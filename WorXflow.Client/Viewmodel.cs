using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AEonAX.Shared;

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
        public Processor Processor { get; set; }
        public Viewmodel()
        {
            Settings = Settings.ReadFromfile(Application.ResourceAssembly.GetName().Name + ".xml", true);
            Settings = Settings ?? new Settings();
            Processor = new Processor();
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
