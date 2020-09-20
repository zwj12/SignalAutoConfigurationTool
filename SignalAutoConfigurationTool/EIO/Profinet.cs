using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

            //If the option is "888-3 PROFINET Device", there will be no section called "PROFINET_DEVICE"
            if (instanceIndustrialNetwork.Type.Domain.Types.IndexOf("PROFINET_DEVICE") >= 0)
            {
                foreach (Instance instanceProfinetDevice in instanceIndustrialNetwork.Type.Domain["PROFINET_DEVICE"].GetInstances())
                {
                    profinetDevices.Add(instanceProfinetDevice.Name, new ProfinetDevice(instanceProfinetDevice, this));
                }
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

        public override string GetIndustrialNetworkCFG()
        {
            List<string> strPreLines = new List<string>
            {
                string.Format("      -Name \"{0}\"", this.Name),
                string.Format(" -Connection \"{0}\"\\\n", this.Connection),
                string.Format("      -Label \"{0}\"", this.IdentificationLabel),
                string.Format(" -CfgPath \"{0}\"\\\n", this.ConfigurationFile),
                string.Format("      -StationName \"{0}\"", this.ProfinetStationName),
            };

            if (this.NestedDiagnosis)
            {
                strPreLines.Add(" -NestedDiagnosis");
            }

            StringBuilder strBuilder = new StringBuilder();
            foreach (string str in strPreLines)
            {
                strBuilder.Append(str);
            }
            return strBuilder.ToString();
        }
    }
}
