﻿using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Dictionary<string, IndustrialNetwork> GetIndustrialNetworks()
        {
            Dictionary<string, IndustrialNetwork> industrialNetworks = new Dictionary<string, IndustrialNetwork>();
            industrialNetworks.Add(this.DeviceNet.Name, this.DeviceNet);
            industrialNetworks.Add(this.Profinet.Name, this.Profinet);
            industrialNetworks.Add(this.EtherNetIP.Name, this.EtherNetIP);
            industrialNetworks.Add(this.Local.Name, this.Local);
            industrialNetworks.Add(this.Virtual.Name, this.Virtual);
            return industrialNetworks;
        }



        public Dictionary<string, Device> GetDevices()
        {
            Dictionary<string, Device> devices = new Dictionary<string, Device>();
            devices = devices.Concat(this.DeviceNet.GetDevices().Select(x => new KeyValuePair<string, Device>(x.Key, x.Value))).ToDictionary(x => x.Key, y => y.Value);
            devices = devices.Concat(this.Profinet.GetDevices().Select(x => new KeyValuePair<string, Device>(x.Key, x.Value))).ToDictionary(x => x.Key, y => y.Value);
            devices = devices.Concat(this.EtherNetIP.GetDevices().Select(x => new KeyValuePair<string, Device>(x.Key, x.Value))).ToDictionary(x => x.Key, y => y.Value);
            devices = devices.Concat(this.Local.GetDevices().Select(x => new KeyValuePair<string, Device>(x.Key, x.Value))).ToDictionary(x => x.Key, y => y.Value);
            devices = devices.Concat(this.Virtual.GetDevices().Select(x => new KeyValuePair<string, Device>(x.Key, x.Value))).ToDictionary(x => x.Key, y => y.Value);
            return devices;
        }

        public Dictionary<string, SignalBase> GetSignals()
        {
            Dictionary<string, SignalBase> signals = new Dictionary<string, SignalBase>();
            foreach (Device device in this.GetDevices().Values)
            {
                signals = signals.Concat(device.Signals.Select(x => new KeyValuePair<string, SignalBase>(x.Key, x.Value))).ToDictionary(x => x.Key, y => y.Value);
            }
            return signals;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public FieldBus(Controller controller)
        {
            try
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
                Dictionary<string, Device> devices = new Dictionary<string, Device>();
                devices = this.GetDevices();
                foreach (Instance instanceSignal in controller.Configuration.Domains["EIO"]["EIO_SIGNAL"].GetInstances())
                {
                    Device device = null;
                    if (devices.ContainsKey((string)instanceSignal.GetAttribute("Device")))
                    {
                        device = devices[(string)instanceSignal.GetAttribute("Device")];
                    }
                    else
                    {
                        device = devices["Virtual1"];
                    }
                    SignalBase signalBase = new SignalBase(instanceSignal, device);
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
                foreach (Device device in devices.Values)
                {                    
                    device.RefreshSignalIndex();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }   
        }

    }
}
