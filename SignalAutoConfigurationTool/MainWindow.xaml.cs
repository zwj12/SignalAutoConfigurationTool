using ABB.Robotics.Controllers.ConfigurationDomain;
using ABB.Robotics.Controllers.IOSystemDomain;
using CNAUSRobotics;
using SignalAutoConfigurationTool.EIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace SignalAutoConfigurationTool
{ 

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FieldBus fieldBus;

        public FieldBus FieldBus
        {
            get { return fieldBus; }
        }

        private Class_Controller myClass_Controller = new Class_Controller();

        private bool boolModified = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                myClass_Controller.ConnectbyGuid(Properties.Settings.Default.SystemGuid);
                myClass_Controller.Login(Properties.Settings.Default.UserName, Properties.Settings.Default.Password);
                this.label_ConnectingStatus.Content = string.Format("Controller: {0}({1})", myClass_Controller.controller.Name, myClass_Controller.controller.IPAddress);
                this.LoadSignalsFromControllerCFG();
            }
            catch
            {
                this.label_ConnectingStatus.Content = string.Format("Controller: default controller is not available");
            }
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.boolModified)
            {
                if (MessageBox.Show("You have modified the data, Do you want to save them?", "Save XML Data", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (this.myClass_Controller.controller != null)
            {
                this.myClass_Controller.controller.Logoff();
                this.myClass_Controller.controller.Dispose();
                this.myClass_Controller.controller = null;
            }
        }

        private void MenuItem_Connect_Click(object sender, RoutedEventArgs e)
        {
            NetworkScanningWindow myNetworkScanningWindow = new NetworkScanningWindow();
            if (myNetworkScanningWindow.ShowDialog() == true)
            {
                this.myClass_Controller.controller = myNetworkScanningWindow.myController;

                this.myClass_Controller.Login(Properties.Settings.Default.UserName, Properties.Settings.Default.Password);
                this.label_ConnectingStatus.Content = string.Format("Controller: {0}({1})", myClass_Controller.controller.Name, myClass_Controller.controller.IPAddress);
                Properties.Settings.Default.SystemGuid = this.myClass_Controller.controller.SystemId.ToString();
                Properties.Settings.Default.Save();
                this.LoadSignalsFromControllerCFG();
            }
        }

        private void MenuItem_SystemGuid_Click(object sender, RoutedEventArgs e)
        {
            SystemGuidWindow mySystemGuidWindow = new SystemGuidWindow();
            mySystemGuidWindow.systemGuid = Properties.Settings.Default.SystemGuid;
            mySystemGuidWindow.userName = Properties.Settings.Default.UserName;
            mySystemGuidWindow.password = Properties.Settings.Default.Password;
            if (mySystemGuidWindow.ShowDialog() == true)
            {
                this.myClass_Controller.controller = mySystemGuidWindow.myController;
                this.label_ConnectingStatus.Content = string.Format("Controller: {0}({1})", myClass_Controller.controller.Name, myClass_Controller.controller.IPAddress);
                Properties.Settings.Default.SystemGuid = mySystemGuidWindow.systemGuid;
                Properties.Settings.Default.UserName = mySystemGuidWindow.userName;
                if (!string.IsNullOrEmpty(mySystemGuidWindow.userName))
                {
                    Properties.Settings.Default.Password = mySystemGuidWindow.password;
                }
                else
                {
                    Properties.Settings.Default.Password = "";
                }
                Properties.Settings.Default.Save();
                this.LoadSignalsFromControllerCFG();
            }
        }

        private void LoadSignalsFromControllerCFG()
        {
            try
            {
                this.fieldBus = new EIO.FieldBus(this.myClass_Controller.controller);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            this.tree_Devices.Items.Clear();

            TreeViewItem myTreeViewItem_IOSystem = new TreeViewItem();
            myTreeViewItem_IOSystem.Header = "I/O System";
            myTreeViewItem_IOSystem.Tag = fieldBus;

            foreach (IndustrialNetwork industrialNetwork in fieldBus.GetIndustrialNetworks().Values)
            {
                TreeViewItem myTreeViewItem_Bus = new TreeViewItem();
                myTreeViewItem_Bus.Header = industrialNetwork.Name;
                myTreeViewItem_Bus.Tag = industrialNetwork;

                foreach (Device device in industrialNetwork.GetDevices().Values)
                {
                    TreeViewItem myTreeViewItem_Device = new TreeViewItem();
                    myTreeViewItem_Device.Header = device.Name;
                    myTreeViewItem_Device.Tag = device;
                    myTreeViewItem_Bus.Items.Add(myTreeViewItem_Device);
                }
                myTreeViewItem_IOSystem.Items.Add(myTreeViewItem_Bus);
            }

            this.tree_Devices.Items.Add(myTreeViewItem_IOSystem);

            DataGridComboBoxColumn dataGridComboBoxColumnDevice = new DataGridComboBoxColumn();
            dataGridComboBoxColumnDevice.Header = "AssignedtoDevice";
            dataGridComboBoxColumnDevice.SelectedItemBinding = new Binding("AssignedtoDevice");
            dataGridComboBoxColumnDevice.DisplayMemberPath = "Name";
            dataGridComboBoxColumnDevice.ItemsSource = fieldBus.GetDevices().Values.ToList();
            dataGridComboBoxColumnDevice.DisplayIndex = 7;
            this.dataGrid_signals.Columns.Add(dataGridComboBoxColumnDevice);

        }

        private void tree_Devices_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.tree_Devices.SelectedItem == null)
            {
                return;
            }
            TreeViewItem myTreeViewItem = (TreeViewItem)this.tree_Devices.SelectedItem;
            object obj = myTreeViewItem.Tag;
            List<SignalBase> signals=null;

            if (obj is FieldBus)
            {
                signals = ((FieldBus)obj).GetSignals().Values.ToList();
            }
            else if (obj is IndustrialNetwork)
            {
                signals = ((IndustrialNetwork)obj).GetSignals().Values.ToList();
            }
            else if (obj is Device)
            {
                signals = ((Device)obj).Signals.Values.ToList();
            }
            if (signals != null)
            {
                signals.Sort();
                this.dataGrid_signals.ItemsSource = signals;
                this.dataGrid_signals.Tag = obj;
           }

            if (this.dataGrid_signals.Items.Count > 1)
            {
                this.label_signalsTarget.Content = "Signals: " + this.dataGrid_signals.Items.Count + " records";
            }
            else
            {
                this.label_signalsTarget.Content = "Signals: " + this.dataGrid_signals.Items.Count + " record";
            }
        }

        private void menu_FrozenColumn_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGrid_signals.CurrentCell != null)
            {
                this.dataGrid_signals.FrozenColumnCount = this.dataGrid_signals.CurrentCell.Column.DisplayIndex;
            }
        }

        private void menu_ReArrangeSignalDeviceMappingbyIndex_Click(object sender, RoutedEventArgs e)
        {
            object obj = this.dataGrid_signals.Tag;
             if (obj is Device)
            {
                ((Device)obj).ReArrangeSignalDeviceMappingbyIndex();
            }
            else
            {
                MessageBox.Show("Please select a device!");
            }
        }

        private void menu_RefreshSignalIndex_Click(object sender, RoutedEventArgs e)
        {
            object obj = this.dataGrid_signals.Tag;
            if (obj is Device)
            {
                ((Device)obj).RefreshSignalIndex();
            }
            else
            {
                MessageBox.Show("Please select a device!");
            }
        }

        private void menu_SaveSignalstoCFG_Click(object sender, RoutedEventArgs e)
        {
            object obj = this.dataGrid_signals.Tag;
            if (obj is Device)
            {
                ((Device)obj).SaveSignalstoCFG();
            }
            else
            {
                MessageBox.Show("Please select a device!");
            }

        }
    }
}