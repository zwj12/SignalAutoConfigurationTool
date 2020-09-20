using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalAutoConfigurationTool.EIO
{
    public class ProfinetDevice : Device
    {
        private string profinetStationName;
        /// <summary>
        /// Cfgname:StationName
        /// </summary>
        public string ProfinetStationName
        {
            get { return profinetStationName; }
            set
            {
                profinetStationName = value;
            }
        }
        public string DefaultProfinetStationName
        {
            get { return ""; }
        }

        private string fastDeviceStartup;
        /// <summary>
        /// Cfgname:FastDeviceStartup
        /// </summary>
        public string FastDeviceStartup
        {
            get { return fastDeviceStartup; }
            set { fastDeviceStartup = value; }
        }
        public string DefaultFastDeviceStartup
        {
            get { return "Deactivated"; }
        }

        private bool energySaving;
        /// <summary>
        /// Cfgname:EnergySaving
        /// PROFIenergy must be installed
        /// </summary>
        public bool EnergySaving
        {
            get { return energySaving; }
            set { energySaving = value; }
        }
        public bool DefaultEnergySaving
        {
            get { return true; }
        }

        private int connectionOutputSize;
        /// <summary>
        /// Cfgname:OutputSize
        /// </summary>
        public int ConnectionOutputSize
        {
            get { return connectionOutputSize; }
            set
            {
                if (value > 65535 || value < 0)
                {
                    throw new Exception("Profinet ConnectionOutputSize: Allowed values are the integers 0-65535");
                }
                connectionOutputSize = value;
            }
        }
        public int DefaultConnectionOutputSize
        {
            get { return 0; }
        }

        private int connectionInputSize;
        /// <summary>
        /// Cfgname:InputSize
        /// </summary>
        public int ConnectionInputSize
        {
            get { return connectionInputSize; }
            set
            {
                if (value > 65535 || value < 0)
                {
                    throw new Exception("Profinet ConnectionInputSize: Allowed values are the integers 0-65535");
                }
                connectionInputSize = value;
            }
        }
        public int DefaultConnectionInputSize
        {
            get { return 0; }
        }

        private string trustLevel;
        /// <summary>
        /// Cfgname:TrustLevel
        /// </summary>
        public string TrustLevel
        {
            get { return trustLevel; }
            set { trustLevel = value; }
        }
        public string DefaultTrustLevel
        {
            get { return "DefaultTrustLevel"; }
        }

        private string stateWhenSystemStartup;
        /// <summary>
        /// Cfgname:StateWhenStartup
        /// </summary>
        public string StateWhenSystemStartup
        {
            get { return stateWhenSystemStartup; }
            set { stateWhenSystemStartup = value; }
        }
        public string DefaultStateWhenSystemStartup
        {
            get { return "Activated"; }
        }

        private int recoveryTime;
        /// <summary>
        /// Cfgname:RecoveryTime
        /// </summary>
        public int RecoveryTime
        {
            get { return recoveryTime; }
            set { recoveryTime = value; }
        }
        public int DefaultRecoveryTime
        {
            get { return 5000; }
        }

        private string port1;
        /// <summary>
        /// Cfgname:FastDeviceStartup_Port1
        /// </summary>
        public string Port1
        {
            get { return port1; }
            set { port1 = value; }
        }
        public string DefaultPort1
        {
            get { return "Deactivated"; }
        }

        private string port2;
        /// <summary>
        /// Cfgname:FastDeviceStartup_Port2
        /// </summary>
        public string Port2
        {
            get { return port2; }
            set { port2 = value; }
        }
        public string DefaultPort2
        {
            get { return "Deactivated"; }
        }

        private string port3;
        /// <summary>
        /// Cfgname:FastDeviceStartup_Port3
        /// </summary>
        public string Port3
        {
            get { return port3; }
            set { port3 = value; }
        }
        public string DefaultPort3
        {
            get { return "Deactivated"; }
        }

        private string port4;
        /// <summary>
        /// Cfgname:FastDeviceStartup_Port4
        /// </summary>
        public string Port4
        {
            get { return port4; }
            set { port4 = value; }
        }
        public string DefaultPort4
        {
            get { return "Deactivated"; }
        }

        public ProfinetDevice(Instance instanceProfinetDevice, IndustrialNetwork connectedtoIndustrialNetwork) :base(instanceProfinetDevice,null, connectedtoIndustrialNetwork)
        {
            this.ProfinetStationName = (string)instanceProfinetDevice.GetAttribute("StationName");
            //this.EnergySaving = (bool)instanceProfinetDevice.GetAttribute("EnergySaving");
            this.FastDeviceStartup = (string)instanceProfinetDevice.GetAttribute("FastDeviceStartup");
            this.Simulated = (bool)instanceProfinetDevice.GetAttribute("Simulated");
            this.TrustLevel = (string)instanceProfinetDevice.GetAttribute("TrustLevel");
            this.StateWhenSystemStartup = (string)instanceProfinetDevice.GetAttribute("StateWhenStartup");
            this.RecoveryTime = (int)instanceProfinetDevice.GetAttribute("RecoveryTime");
            if (this.Simulated)
            {
                this.ConnectionOutputSize = (int)instanceProfinetDevice.GetAttribute("OutputSize");
                this.ConnectionInputSize = (int)instanceProfinetDevice.GetAttribute("InputSize");
            }
            if(this.FastDeviceStartup== "Activated" || this.FastDeviceStartup == "Support")
            {
                this.Port1 = (string)instanceProfinetDevice.GetAttribute("FastDeviceStartup_Port1");
                this.Port2 = (string)instanceProfinetDevice.GetAttribute("FastDeviceStartup_Port2");
                this.Port3 = (string)instanceProfinetDevice.GetAttribute("FastDeviceStartup_Port3");
                this.Port4 = (string)instanceProfinetDevice.GetAttribute("FastDeviceStartup_Port4");
            }
        }
        public override string GetDeviceCFG()
        {
            List<string> strPreLines = new List<string>();
            strPreLines.Add(string.Format("      -Name \"{0}\"", this.Name));
            FillCfgLines(strPreLines, "Label", this.IdentificationLabel, this.DefaultIdentificationLabel);
            FillCfgLines(strPreLines, "VendorName", this.VendorName, this.DefaultVendorName);
            FillCfgLines(strPreLines, "ProductName", this.ProductName, this.DefaultProductName);
            FillCfgLines(strPreLines, "TrustLevel", this.TrustLevel, this.DefaultTrustLevel);
            FillCfgLines(strPreLines, "OutputSize", this.ConnectionOutputSize, this.DefaultConnectionOutputSize);
            FillCfgLines(strPreLines, "InputSize", this.ConnectionInputSize, this.DefaultConnectionInputSize);
            FillCfgLines(strPreLines, "RecoveryTime", this.RecoveryTime, this.DefaultRecoveryTime);
            FillCfgLines(strPreLines, "Simulated", this.Simulated, this.DefaultSimulated);
            FillCfgLines(strPreLines, "StateWhenStartup", this.StateWhenSystemStartup, this.DefaultStateWhenSystemStartup);
//            FillCfgLines(strPreLines, "EnergySaving", this.EnergySaving, this.DefaultEnergySaving);
            FillCfgLines(strPreLines, "FastDeviceStartup", this.FastDeviceStartup, this.DefaultFastDeviceStartup);
            FillCfgLines(strPreLines, "StationName", this.ProfinetStationName, this.DefaultProfinetStationName);
            FillCfgLines(strPreLines, "FastDeviceStartup_Port1", this.Port1, this.DefaultPort1);
            FillCfgLines(strPreLines, "FastDeviceStartup_Port2", this.Port2, this.DefaultPort2);
            FillCfgLines(strPreLines, "FastDeviceStartup_Port3", this.Port3, this.DefaultPort3);
            FillCfgLines(strPreLines, "FastDeviceStartup_Port4", this.Port4, this.DefaultPort4);
            StringBuilder strBuilder = new StringBuilder();
            foreach (string str in strPreLines)
            {
                strBuilder.Append(str);
            }
            return strBuilder.ToString();
        }
    }
}
