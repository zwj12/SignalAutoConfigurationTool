﻿using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalAutoConfigurationTool.EIO
{
    public class EtherNetIP: IndustrialNetwork
    {
        private string connection;
        /// <summary>
        /// Cfgname:Connection
        /// </summary>
        public string Connection
        {
            get { return connection; }
            set { connection = value; }
        }

        private Dictionary<string, EtherNetIPDevice> etherNetIPDevices = new Dictionary<string, EIO.EtherNetIPDevice>();

        public Dictionary<string, EtherNetIPDevice> EtherNetIPDevices
        {
            get { return etherNetIPDevices; }
        }

        private EtherNetIPInternalDevice etherNetIPInternalDevice;

        public EtherNetIPInternalDevice EtherNetIPInternalDevice
        {
            get { return etherNetIPInternalDevice; }
        }

        public EtherNetIP(Instance instanceIndustrialNetwork, FieldBus fieldBus,bool isRobotWare7 = false) : base(instanceIndustrialNetwork, fieldBus, isRobotWare7)
        {
            if (!isRobotWare7)
            {
                this.Connection = instanceIndustrialNetwork.GetAttribute("Connection").ToString();
            }
            else
            {
                this.Name = "EtherNetIP";
            }

            foreach (Instance instanceEtherNetIPDevice in instanceIndustrialNetwork.Type.Domain["ETHERNETIP_DEVICE"].GetInstances())
            {
                etherNetIPDevices.Add(instanceEtherNetIPDevice.Name, new EtherNetIPDevice(instanceEtherNetIPDevice, this));
            }
            //If there is no option of "ETHERNETIP"
            if (instanceIndustrialNetwork.Type.Domain.Types.IndexOf("ETHERNETIP_INTERNAL_DEVICE") >= 0)
            {
                etherNetIPInternalDevice = new EtherNetIPInternalDevice(instanceIndustrialNetwork.Type.Domain["ETHERNETIP_INTERNAL_DEVICE"].GetInstance("EN_Internal_Device"), this, isRobotWare7);
            }
        }
        
        public override Dictionary<string, Device> GetDevices()
        {
            Dictionary<string, Device> devices = new Dictionary<string, EIO.Device>();
            devices = devices.Concat(this.EtherNetIPDevices.Select(x => new KeyValuePair<string, Device>(x.Key, x.Value))).ToDictionary(x => x.Key, y => y.Value);
            if (this.EtherNetIPInternalDevice != null)
            {
                devices.Add(this.EtherNetIPInternalDevice.Name, this.EtherNetIPInternalDevice);
            }            
            return devices;
        }

        public override string GetIndustrialNetworkCFG()
        {
            List<string> strPreLines = new List<string>
            {
                string.Format("      -Name \"{0}\"", this.Name),
                string.Format(" -Connection \"{0}\"\\\n", this.Connection),
                string.Format("      -Label \"{0}\"", this.IdentificationLabel),
            };
            
            StringBuilder strBuilder = new StringBuilder();
            foreach (string str in strPreLines)
            {
                strBuilder.Append(str);
            }
            return strBuilder.ToString();
        }
    }
}
