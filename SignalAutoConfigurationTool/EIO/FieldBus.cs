using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace SignalAutoConfigurationTool.EIO
{
    /// <summary>
    /// Field Bus has all IndustrialNetworks, Devices and Signals.
    /// Every IndustrialNetworks has its Devices and Signals.
    /// Every Device has its Signals and IndustrialNetwork.
    /// Every Signal has its Device.
    /// </summary>
    public class FieldBus
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private Controller controller;

        public Controller Controller
        {
            get { return controller; }
            set { controller = value; }
        }

        private Profinet profinet;

        public Profinet Profinet
        {
            get { return profinet; }
        }

        private DeviceNet deviceNet;

        public DeviceNet DeviceNet
        {
            get { return deviceNet; }
        }

        private EtherNetIP etherNetIP;

        public EtherNetIP EtherNetIP
        {
            get { return etherNetIP; }
        }

        private Virtual1 _virtual;

        public Virtual1 Virtual
        {
            get { return _virtual; }
        }

        private Local local;

        public Local Local
        {
            get { return local; }
        }

        private Dictionary<string, AccessLevel> accessLevels;

        public Dictionary<string, AccessLevel> AccessLevels
        {
            get { return accessLevels; }
            set { accessLevels = value; }
        }

        private Dictionary<string, SystemOutput> systemOutputs;

        public Dictionary<string, SystemOutput> SystemOutputs
        {
            get { return systemOutputs; }
            set { systemOutputs = value; }
        }
        
        private Dictionary<string, CrossConnection> crossConnections;

        public Dictionary<string, CrossConnection> CrossConnections
        {
            get { return crossConnections; }
            set { crossConnections = value; }
        }

        private Dictionary<string, SystemInput> systemInputs;

        public Dictionary<string, SystemInput> SystemInputs
        {
            get { return systemInputs; }
            set { systemInputs = value; }
        }

        public Dictionary<string, IndustrialNetwork> GetIndustrialNetworks()
        {
            Dictionary<string, IndustrialNetwork> industrialNetworks = new Dictionary<string, IndustrialNetwork>();
            if (this.DeviceNet != null)
            {
                industrialNetworks.Add(this.DeviceNet.Name, this.DeviceNet);
            }
            if (this.Profinet != null)
            {
                industrialNetworks.Add(this.Profinet.Name, this.Profinet);
            }
            if (this.EtherNetIP != null)
            {
                industrialNetworks.Add(this.EtherNetIP.Name, this.EtherNetIP);
            }
            industrialNetworks.Add(this.Local.Name, this.Local);
            industrialNetworks.Add(this.Virtual.Name, this.Virtual);
            return industrialNetworks;
        }



        public Dictionary<string, Device> GetDevices()
        {
            Dictionary<string, Device> devices = new Dictionary<string, Device>();
            if (this.DeviceNet != null)
            {
                devices = devices.Concat(this.DeviceNet.GetDevices().Select(x => new KeyValuePair<string, Device>(x.Key, x.Value))).ToDictionary(x => x.Key, y => y.Value);
            }
            if (this.Profinet != null)
            {
                devices = devices.Concat(this.Profinet.GetDevices().Select(x => new KeyValuePair<string, Device>(x.Key, x.Value))).ToDictionary(x => x.Key, y => y.Value);
            }
            if (this.EtherNetIP != null)
            {
                devices = devices.Concat(this.EtherNetIP.GetDevices().Select(x => new KeyValuePair<string, Device>(x.Key, x.Value))).ToDictionary(x => x.Key, y => y.Value);
            }
            if (this.Local != null)
            {
                devices = devices.Concat(this.Local.GetDevices().Select(x => new KeyValuePair<string, Device>(x.Key, x.Value))).ToDictionary(x => x.Key, y => y.Value);
            }
            if (this.Virtual != null)
            {
                devices = devices.Concat(this.Virtual.GetDevices().Select(x => new KeyValuePair<string, Device>(x.Key, x.Value))).ToDictionary(x => x.Key, y => y.Value);
            }
            return devices;
        }

        public Dictionary<string, Signal> GetSignals()
        {
            Dictionary<string, Signal> signals = new Dictionary<string, Signal>();
            foreach (Device device in this.GetDevices().Values)
            {
                signals = signals.Concat(device.Signals.Select(x => new KeyValuePair<string, Signal>(x.Key, x.Value))).ToDictionary(x => x.Key, y => y.Value);
            }
            return signals;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public FieldBus(Controller controller)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.Controller = controller;
            try
            {
                this.accessLevels = new Dictionary<string, EIO.AccessLevel>();
                foreach (Instance instanceAccessLevel in controller.Configuration.Domains["EIO"]["EIO_ACCESS"].GetInstances())
                {
                    AccessLevel accessLevel = new EIO.AccessLevel(instanceAccessLevel, this);
                    this.accessLevels.Add(accessLevel.Name, accessLevel);
                    logger.Debug("EIO_ACCESS: " + stopwatch.ElapsedMilliseconds.ToString());
                }
                logger.Debug("EIO_ACCESS: " + stopwatch.ElapsedMilliseconds.ToString());

                this.systemOutputs = new Dictionary<string, EIO.SystemOutput>();
                foreach (Instance instanceSystemOutput in controller.Configuration.Domains["EIO"]["SYSSIG_OUT"].GetInstances())
                {
                    SystemOutput systemOutput = new EIO.SystemOutput(instanceSystemOutput, this);
                    this.systemOutputs.Add(systemOutput.ID, systemOutput);
                }
                logger.Debug("SYSSIG_OUT: " + stopwatch.ElapsedMilliseconds.ToString());

                this.systemInputs = new Dictionary<string, EIO.SystemInput>();
                foreach (Instance instanceSystemInput in controller.Configuration.Domains["EIO"]["SYSSIG_IN"].GetInstances())
                {
                    SystemInput systemInput = new EIO.SystemInput(instanceSystemInput, this);
                    this.systemInputs.Add(systemInput.ID, systemInput);
                }
                logger.Debug("SYSSIG_IN: " + stopwatch.ElapsedMilliseconds.ToString());

                this.crossConnections = new Dictionary<string, EIO.CrossConnection>();
                foreach (Instance instanceCrossConnection in controller.Configuration.Domains["EIO"]["EIO_CROSS"].GetInstances())
                {
                    CrossConnection crossConnection = new EIO.CrossConnection(instanceCrossConnection, this);
                    this.crossConnections.Add(crossConnection.Name, crossConnection);
                }
                logger.Debug("EIO_CROSS: " + stopwatch.ElapsedMilliseconds.ToString());

                if (controller.IsRobotWare7)
                {
                    Domain domain = controller.Configuration.Domains["EIO"];
                    ABB.Robotics.Controllers.ConfigurationDomain.Type domainType = domain.Types.FirstOrDefault(item => item.Name == "ETHERNETIP_NETWORK");
                    if (domainType != null)
                    {
                        this.etherNetIP = new EtherNetIP(domainType.GetInstance("EtherNetIP"), this, controller.IsRobotWare7);
                    }
                    this._virtual = new Virtual1(null, this, controller.IsRobotWare7);
                    this.local = new Local(null, this,controller.IsRobotWare7);
                }
                else
                {
                    foreach (Instance instanceIndustrialNetwork in controller.Configuration.Domains["EIO"]["INDUSTRIAL_NETWORK"].GetInstances())
                    {
                        switch (instanceIndustrialNetwork.Name)
                        {
                            case "PROFINET":
                                this.profinet = new Profinet(instanceIndustrialNetwork, this);
                                break;
                            case "DeviceNet":
                                this.deviceNet = new DeviceNet(instanceIndustrialNetwork, this);
                                break;
                            case "EtherNetIP":
                                this.etherNetIP = new EtherNetIP(instanceIndustrialNetwork, this);
                                break;
                            case "Local":
                                this.local = new Local(instanceIndustrialNetwork, this);
                                break;
                            case "Virtual":
                                this._virtual = new Virtual1(instanceIndustrialNetwork, this);
                                break;
                        }
                    }
                }             
                logger.Debug("INDUSTRIAL_NETWORK: " + stopwatch.ElapsedMilliseconds.ToString());

                Dictionary<string, Device> devices = new Dictionary<string, Device>();
                devices = this.GetDevices();
                logger.Debug("GetDevices: " + stopwatch.ElapsedMilliseconds.ToString());

                Instance[] instances = controller.Configuration.Domains["EIO"]["EIO_SIGNAL"].GetInstances();
                logger.Debug("EIO_SIGNAL: " + stopwatch.ElapsedMilliseconds.ToString());
                foreach (Instance instanceSignal in instances)
                {                    
                    Device device = null;
                    object o = instanceSignal.GetAttribute("Device");
                    if (devices.ContainsKey(instanceSignal.GetAttribute("Device").ToString()))
                    {
                        device = devices[instanceSignal.GetAttribute("Device").ToString()];
                    }
                    else
                    {
                        device = devices["Virtual1"];
                    }
                    Signal signalBase = new Signal(instanceSignal, device);                    
                    //log.Debug(signalBase.Name);
                    //switch ((string)instanceSignal.GetAttribute("SignalType"))
                    //{
                    //    case "DI":
                    //        SignalDigitalInput signalDigitalInput = new EIO.SignalDigitalInput(instanceSignal, device);
                    //        break;
                    //    case "DO":
                    //        SignalDigitalOutput signalDigitalOutput = new EIO.SignalDigitalOutput(instanceSignal, device);
                    //        break;
                    //    case "GI":
                    //        SignalGroupInput signalGroupInput = new EIO.SignalGroupInput(instanceSignal, device);
                    //        break;
                    //    case "GO":
                    //        SignalGroupOutput signalGroupOutput = new EIO.SignalGroupOutput(instanceSignal, device);
                    //        break;
                    //    case "AI":
                    //        SignalAnalogInput signalAnalogInput = new EIO.SignalAnalogInput(instanceSignal, device);
                    //        break;
                    //    case "AO":
                    //        SignalAnalogOutput signalAnalogOutput = new EIO.SignalAnalogOutput(instanceSignal, device);
                    //        break;
                    //}
                }
                logger.Debug("EIO_SIGNAL: " + stopwatch.ElapsedMilliseconds.ToString());

                foreach (Device device in devices.Values)
                {                    
                    device.RefreshSignalIndex();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            stopwatch.Stop();
            logger.Debug("FieldBus: " + stopwatch.ElapsedMilliseconds.ToString());
        }

        public void SaveIndustrialNetworkstoCFG()
        {
            SaveFileDialog mySaveFileDialog = new SaveFileDialog();
            mySaveFileDialog.FileName = "EIO_INDUSTRIAL_NETWORK.cfg";
            mySaveFileDialog.Filter = "EIO files (*.cfg)|*.cfg";
            mySaveFileDialog.RestoreDirectory = true;
            Nullable<bool> result = mySaveFileDialog.ShowDialog();
            if (result == false)
            {
                return;
            }
            FileStream fs = new FileStream(mySaveFileDialog.FileName, FileMode.Create);
            StreamWriter myStreamWriter = new StreamWriter(fs);
            myStreamWriter.Write("EIO:CFG_1.0::\n");
            myStreamWriter.WriteLine("");

            myStreamWriter.Write("#\nINDUSTRIAL_NETWORK:\n");
            
            foreach (IndustrialNetwork industrialNetwork in this.GetIndustrialNetworks().Values)
            {
                myStreamWriter.WriteLine("");
                myStreamWriter.WriteLine(industrialNetwork.GetIndustrialNetworkCFG());
            }     

            myStreamWriter.Close();
            fs.Close();
        }
    }
}
