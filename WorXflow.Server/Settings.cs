using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AEonAX.Shared;

namespace WorXflow.Server
{
    public class Settings : NotifyBase
    {
        private int _PortNumber;
        public int PortNumber
        {
            get { return _PortNumber; }
            set
            {
                if (!Equals(_PortNumber, value))
                {
                    _PortNumber = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _ServerName;
        public string ServerName
        {
            get { return _ServerName; }
            set
            {
                if (!Equals(_ServerName, value))
                {
                    _ServerName = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public Settings()
        {
            ServerName = "WorXflow Server";
        }
        internal void Save()
        {
            this.WriteTofile(Application.ResourceAssembly.GetName().Name + ".xml");
        }
    }
}
