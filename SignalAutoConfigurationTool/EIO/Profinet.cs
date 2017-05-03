using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalAutoConfigurationTool.EIO
{
    public class Profinet : IndustrialNetwork
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
        
        private string configurationFile;
        /// <summary>
        /// Cfgname:CfgPath
        /// </summary>
        public string ConfigurationFile
        {
            get { return configurationFile; }
            set { configurationFile = value; }
        }

        private string profinetStationName;
        /// <summary>
        /// Cfgname:StationName
        /// </summary>
        public string ProfinetStationName
        {
            get { return profinetStationName; }
            set { profinetStationName = value; }
        }

        private bool nestedDiagnosis;
        /// <summary>
        /// Cfgname:Nesteddiagnosis
        /// </summary>
        public bool NestedDiagnosis
        {
            get { return nestedDiagnosis; }
            set { nestedDiagnosis = value; }
        }

        private Dictionary<string, ProfinetDevice> profinetDevices= new Dictionary<string, EIO.ProfinetDevice>();

        public Dictionary<string, ProfinetDevice> ProfinetDevices
        {
            get { return profinetDevices; }
        }

        private ProfinetInternalDevice profinetInternalDevice;

        public ProfinetInternalDevice ProfinetInternalDevice
        {
            get { return profinetInternalDevice; }
        }

        public Profinet(Instance instanceIndustrialNetwork, FieldBus fieldBus) : base(instanceIndustrialNetwork, fieldBus)
        {
            this.Connection = (string)instanceIndustrialNetwork.GetAttribute("Connection");
            this.ConfigurationFile = (string)instanceIndustrialNetwork.GetAttribute("CfgPath");
            this.ProfinetStationName = (string)instanceIndustrialNetwork.GetAttribute("StationName");
            this.NestedDiagnosis = (bool)instanceIndustrialNetwork.GetAttribute("Nesteddiagnosis");
            foreach (Instance instanceProfinetDevice in instanceIndustrialNetwork.Type.Domain["PROFINET_DEVICE"].GetInstances())
            {
                profinetDevices.Add(instanceProfinetDevice.Name, new ProfinetDevice(instanceProfinetDevice, this));
            }
            profinetInternalDevice = new ProfinetInternalDevice(instanceIndustrialNetwork.Type.Domain["PROFINET_INTERNAL_DEVICE"].GetInstance("PN_Internal_Device"), this);

        }
        public override Dictionary<string, Device> GetDevices()
        {
            Dictionary<string, Device> devices = new Dictionary<string, EIO.Device>();
            devices = devices.Concat(this.profinetDevices.Select(x => new KeyValuePair<string, Device>(x.Key, x.Value))).ToDictionary(x => x.Key, y => y.Value);
            devices.Add(this.profinetInternalDevice.Name, this.profinetInternalDevice);
            return devices;
        }
    }
}
