using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace WorXflow.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public Viewmodel VM { get; set; }

        public MainWindow()
        {
            VM = new Viewmodel();
            VM.ExceptionThrown += VM_ExceptionThrown;
            DataContext = VM;
            InitializeComponent();
            VM.rtbMessages = rtbMessages;
        }

        void VM_ExceptionThrown(object sender, Exception e)
        {
            this.ShowMessageAsync("Error", e.Message, MessageDialogStyle.Affirmative);
        }
        private void MWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            VM.Settings.Save();
        }

    }
}
