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
using System.Windows.Shapes;
using System.Data;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.RapidDomain;

namespace CNAUSRobotics
{
    /// <summary>
    /// Interaction logic for NetworkScanningWindow.xaml
    /// </summary>
    public partial class NetworkScanningWindow : Window
    {
        private NetworkScanner scanner = new NetworkScanner();
        private NetworkWatcher networkwatcher;
        public Controller myController;

        public NetworkScanningWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            refreshControllers();
            this.networkwatcher = new NetworkWatcher();
            this.networkwatcher.Found += HandleFoundEvent;
            this.networkwatcher.Lost += HandleLostEvent;
            this.networkwatcher.EnableRaisingEvents = true;

        }

        void HandleFoundEvent(object sender, NetworkWatcherEventArgs e)
        {
            if (this.checkBox_showVirtual.IsChecked == true)
            {
                this.listView_controllerInfo.Items.Add(e.Controller);
            }
            else
            {
                if (! e.Controller.IsVirtual)
                {
                    this.listView_controllerInfo.Items.Add(e.Controller);
                }
            }
        }

        void HandleLostEvent(object sender, NetworkWatcherEventArgs e)
        {
            this.listView_controllerInfo.Items.Remove(e.Controller);
        }

        private void refreshControllers()
        {
            ControllerInfo[] controllerInfos;
            this.listView_controllerInfo.Items.Clear();
            if (this.checkBox_showVirtual.IsChecked == true)
            {
                controllerInfos = this.scanner.GetControllers();
            }
            else
            {
                controllerInfos = this.scanner.GetControllers(NetworkScannerSearchCriterias.Real);
            }
            foreach (ControllerInfo controllerInfo in controllerInfos)
            {
                this.listView_controllerInfo.Items.Add(controllerInfo);
            }
        }

        private void listView_controllerInfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.saveSystemGuid();
        }

        private void button_OK_Click(object sender, RoutedEventArgs e)
        {
            this.saveSystemGuid();
        }

        private void saveSystemGuid()
        {
            if (this.listView_controllerInfo.SelectedItem == null)
            {
                MessageBox.Show("Please select an item");
                return;            
            }
            ControllerInfo myControllerInfo = (ControllerInfo)this.listView_controllerInfo.SelectedItem;
            this.myController = new Controller(myControllerInfo);
            this.DialogResult = true;
        }

        private void button_refresh_Click(object sender, RoutedEventArgs e)
        {
            this.refreshControllers();
        }

        private void checkBox_showVirtual_Click(object sender, RoutedEventArgs e)
        {
            this.refreshControllers();
        }

        private void button_addRemote_Click(object sender, RoutedEventArgs e)
        {
            System.Net.IPAddress ipAddress;
            try
            {
                ipAddress = System.Net.IPAddress.Parse(this.textBox_remoteIPAddress.Text);
                NetworkScanner.AddRemoteController(ipAddress);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Wrong IP address format: " + ex.Message);
            }
            this.refreshControllers();
        }

    }
}
