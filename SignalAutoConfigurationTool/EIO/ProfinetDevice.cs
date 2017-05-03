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

        private string fastDeviceStartup;
        /// <summary>
        /// Cfgname:FastDeviceStartup
        /// </summary>
        public string FastDeviceStartup
        {
            get { return fastDeviceStartup; }
            set { fastDeviceStartup = value; }
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

        private string trustLevel;
        /// <summary>
        /// Cfgname:TrustLevel
        /// </summary>
        public string TrustLevel
        {
            get { return trustLevel; }
            set { trustLevel = value; }
        }

        private string statewhenSystemStartup;
        /// <summary>
        /// Cfgname:StateWhenStartup
        /// </summary>
        public string StatewhenSystemStartup
        {
            get { return statewhenSystemStartup; }
            set { statewhenSystemStartup = value; }
        }

        private bool simulated;
        /// <summary>
        /// Cfgname:Simulated
        /// </summary>
        public bool Simulated
        {
            get { return simulated; }
            set { simulated = value; }
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

        private string port1;
        /// <summary>
        /// Cfgname:FastDeviceStartup_Port1
        /// </summary>
        public string Port1
        {
            get { return port1; }
            set { port1 = value; }
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

        private string port3;
        /// <summary>
        /// Cfgname:FastDeviceStartup_Port3
        /// </summary>
        public string Port3
        {
            get { return port3; }
            set { port3 = value; }
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

        public ProfinetDevice(Instance instanceProfinetDevice, IndustrialNetwork connectedtoIndustrialNetwork) :base(instanceProfinetDevice,null, connectedtoIndustrialNetwork)
        {
            this.ProfinetStationName = (string)instanceProfinetDevice.GetAttribute("StationName");
            //this.EnergySaving = (bool)instanceProfinetDevice.GetAttribute("EnergySaving");
            this.FastDeviceStartup = (string)instanceProfinetDevice.GetAttribute("FastDeviceStartup");
            this.Simulated = (bool)instanceProfinetDevice.GetAttribute("Simulated");
            this.TrustLevel = (string)instanceProfinetDevice.GetAttribute("TrustLevel");
            this.StatewhenSystemStartup = (string)instanceProfinetDevice.GetAttribute("StateWhenStartup");
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
    }
}
