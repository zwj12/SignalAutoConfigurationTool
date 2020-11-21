using ABB.Robotics.Controllers;
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
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace SignalAutoConfigurationTool
{ 

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

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
 //               this.LoadSignalsFromControllerCFG();
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
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                this.fieldBus = new EIO.FieldBus(this.myClass_Controller.controller);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            stopwatch.Stop();
            logger.Debug(stopwatch.ElapsedMilliseconds.ToString());

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

            DataGridComboBoxColumn dataGridComboBoxColumnAccessLevel = new DataGridComboBoxColumn();
            dataGridComboBoxColumnAccessLevel.Header = "AccessLevel";
            dataGridComboBoxColumnAccessLevel.SelectedItemBinding = new Binding("AccessLevel");
            dataGridComboBoxColumnAccessLevel.DisplayMemberPath = "Name";
            dataGridComboBoxColumnAccessLevel.ItemsSource = fieldBus.AccessLevels.Values.ToList();
            dataGridComboBoxColumnAccessLevel.DisplayIndex = 10;
            this.dataGrid_signals.Columns.Add(dataGridComboBoxColumnAccessLevel);
            
        }

        private void tree_Devices_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.tree_Devices.SelectedItem == null)
            {
                return;
            }
            TreeViewItem myTreeViewItem = (TreeViewItem)this.tree_Devices.SelectedItem;
            object obj = myTreeViewItem.Tag;
            List<EIO.Signal> signals =null;

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
            if (this.dataGrid_signals.CurrentItem != null)
            {
                this.dataGrid_signals.FrozenColumnCount = this.dataGrid_signals.CurrentCell.Column.DisplayIndex;
            }
        }

        private void menu_ReArrangeSignalDeviceMappingbyIndex_Click(object sender, RoutedEventArgs e)
        {
            this.tree_Devices.Focus();
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
            this.tree_Devices.Focus();
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

        private void menu_SaveDeviceSignalstoCFG_Click(object sender, RoutedEventArgs e)
        {
            this.tree_Devices.Focus();
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

        private void menu_ResetSignalTypeByName_Click(object sender, RoutedEventArgs e)
        {
            this.tree_Devices.Focus();
            object obj = this.dataGrid_signals.Tag;
            if (obj is Device)
            {
                ((Device)obj).ResetSignalTypeByName();
            }
            else
            {
                MessageBox.Show("Please select a device!");
            }
        }

        private void menu_InitAnalogEncoding_Click(object sender, RoutedEventArgs e)
        {
            this.tree_Devices.Focus();
            EIO.Signal signal = this.dataGrid_signals.SelectedItem as EIO.Signal;
            if (signal !=null)
            {
                signal.InitAnalogEncoding();
            }
       
        }

        private void menu_GetSignalValues_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGrid_signals.ItemsSource == null)
            {
                return;
            }
            List<EIO.Signal> signals = null;
            signals = (List<EIO.Signal>)this.dataGrid_signals.ItemsSource ;
            foreach(EIO.Signal signal in signals)
            {
                signal.GetSignalValue();
            }
        }

        private void MenuItem_Test_Click(object sender, RoutedEventArgs e)
        {
            this.myClass_Controller.TestFileSystem();
            return;
            this.myClass_Controller.TestSpeedDataArray();
            return;
            this.myClass_Controller.TestSpeedData();
            return;
            ABB.Robotics.Controllers.IOSystemDomain.Signal signal = this.myClass_Controller.controller.IOSystem.GetSignal("diTest");
            if (signal != null)
            {
                MessageBox.Show(signal.Value.ToString());
                if (signal.State.Simulated == true)
                {
                signal.Value = 1;
                }
                signal.Value = 1;
            }
        }

        private void menu_ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGrid_signals.ItemsSource == null)
            {
                return;
            }
            List<EIO.Signal> signals = null;
            signals = (List<EIO.Signal>)this.dataGrid_signals.ItemsSource;

            SaveFileDialog mySaveFileDialog = new SaveFileDialog();
            mySaveFileDialog.FileName = this.myClass_Controller.controller.Name + ".csv";
            mySaveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            mySaveFileDialog.RestoreDirectory = true;
            Nullable<bool> result = mySaveFileDialog.ShowDialog();
            if (result == false)
            {
                return;
            }
            FileStream fs = new FileStream(mySaveFileDialog.FileName, FileMode.Create);
            StreamWriter myStreamWriter = new StreamWriter(fs);
            string strTab= ",";
            myStreamWriter.Write("Name,SignalType,AssignedtoDevice,DeviceMapping,Category,SignalIdentificationLabel,AccessLevel,SafeLevel,DefaultValue,NumberOfBits,FilterTimePassive,FilterTimeActive,InvertPhysicalValue,AnalogEncodingType, MaximumLogicalValue, MaximumPhysicalValue, MaximumPhysicalValueLimit, MaximumBitValue, MinimumLogicalValue, MinimumPhysicalValue, MinimumPhysicalValueLimit, MinimumBitValue\n");

            foreach (EIO.Signal signal in signals)
            {
                myStreamWriter.Write(signal.Name);
                myStreamWriter.Write(strTab + signal.SignalType.ToString());
                myStreamWriter.Write(strTab + signal.AssignedtoDevice.Name);
                myStreamWriter.Write(strTab + signal.DeviceMapping);
                myStreamWriter.Write(strTab + signal.Category);
                myStreamWriter.Write(strTab + signal.SignalIdentificationLabel);
                myStreamWriter.Write(strTab + signal.AccessLevel.Name);
                myStreamWriter.Write(strTab + signal.SafeLevel);
                myStreamWriter.Write(strTab + signal.DefaultValue);
                myStreamWriter.Write(strTab + signal.NumberOfBits);

                switch (signal.SignalType)
                {
                    case EIO.SignalType.DI:
                    case EIO.SignalType.DO:
                    case EIO.SignalType.GI:
                    case EIO.SignalType.GO:
                        myStreamWriter.Write(strTab + signal.FilterTimePassive);
                        myStreamWriter.Write(strTab + signal.FilterTimeActive);
                        myStreamWriter.Write(strTab + signal.InvertPhysicalValue);

                        myStreamWriter.Write(strTab + "");
                        myStreamWriter.Write(strTab + "");
                        myStreamWriter.Write(strTab + "");
                        myStreamWriter.Write(strTab + "");
                        myStreamWriter.Write(strTab + "");
                        myStreamWriter.Write(strTab + "");
                        myStreamWriter.Write(strTab + "");
                        myStreamWriter.Write(strTab + "");
                        myStreamWriter.Write(strTab + "");
                        break;
                    case EIO.SignalType.AI:
                    case EIO.SignalType.AO:
                        myStreamWriter.Write(strTab + "");
                        myStreamWriter.Write(strTab + "");
                        myStreamWriter.Write(strTab + "");
                        myStreamWriter.Write(strTab + signal.AnalogEncodingType);
                        myStreamWriter.Write(strTab + signal.MaximumLogicalValue);
                        myStreamWriter.Write(strTab + signal.MaximumPhysicalValue);
                        myStreamWriter.Write(strTab + signal.MaximumPhysicalValueLimit);
                        myStreamWriter.Write(strTab + signal.MaximumBitValue);
                        myStreamWriter.Write(strTab + signal.MinimumLogicalValue);
                        myStreamWriter.Write(strTab + signal.MinimumPhysicalValue);
                        myStreamWriter.Write(strTab + signal.MinimumPhysicalValueLimit);
                        myStreamWriter.Write(strTab + signal.MinimumBitValue);
                        break;
                }
                myStreamWriter.Write("\n");
            }

            myStreamWriter.Close();
            fs.Close();
        }

        private void menu_QueryFilter_Click(object sender, RoutedEventArgs e)
        {
            //this.tree_Devices.Focus();
            //object obj = this.dataGrid_signals.Tag;
            //if (obj is Device)
            //{
            //    QueryFilterWindow queryFilterWindow = new QueryFilterWindow();
            //    queryFilterWindow.ShowDialog();
            //}
            //else
            //{
            //    MessageBox.Show("Please select a device!");
            //}
        }

        private void Menu_SaveIndustrialNetworkCFG_Click(object sender, RoutedEventArgs e)
        {
            this.tree_Devices.Focus();
            object obj = this.dataGrid_signals.Tag;
            if (obj is Device)
            {
                ((Device)obj).ConnectedtoIndustrialNetwork.FieldBus.SaveIndustrialNetworkstoCFG();
            }
            else if (obj is IndustrialNetwork)
            {
                ((IndustrialNetwork)obj).FieldBus.SaveIndustrialNetworkstoCFG();
            }
            else if (obj is FieldBus)
            {
                ((FieldBus)obj).SaveIndustrialNetworkstoCFG();
            }
            else
            {
                MessageBox.Show("Please select a device!");             
            }
        }

        private void MenuItem_RequestWriteAcces_Click(object sender, RoutedEventArgs e)
        {
           this.myClass_Controller.RequestWriteAccess();

        }

        private void MenuItem_ReleaseWriteAcces_Click(object sender, RoutedEventArgs e)
        {
            this.myClass_Controller.ReleaseWriteAccess();
        }
    }
}