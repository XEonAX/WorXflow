using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AEonAX.Shared;

namespace WorXflow.Client
{
    public class Settings : NotifyBase
    {

        private string _ServerURL;
        public string ServerURL
        {
            get { return _ServerURL; }
            set
            {
                if (!Equals(_ServerURL, value))
                {
                    _ServerURL = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public Settings()
        {
            ServerURL = "ws://localhost:9876/Chat";
        }
        internal void Save()
        {
            this.WriteTofile(Application.ResourceAssembly.GetName().Name + ".xml");
        }
    }
}
