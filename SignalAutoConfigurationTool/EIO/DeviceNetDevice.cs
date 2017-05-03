using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalAutoConfigurationTool.EIO
{
    public class DeviceNetDevice:Device
    {
        private int address;
        /// <summary>
        /// Cfgname:Address
        /// </summary>
        public int Address
        {
            get { return address; }
            set
            {
                if (value > 63 || value < 0)
                {
                    throw new Exception("DeviceNet Address: Allowed values are the integers 0-63");
                }
                address = value;
            }
        }

        private int vendorID;
        /// <summary>
        /// Cfgname:VendorID
        /// </summary>
        public int VendorID
        {
            get { return vendorID; }
            set {
                if (value > 65535 || value < 0)
                {
                    throw new Exception("DeviceNet VendorID: Allowed values are the integers 0-65535");
                }
                vendorID = value;
            }
        }

        private int productCode;
        /// <summary>
        /// Cfgname:ProductCode
        /// </summary>
        public int ProductCode
        {
            get { return productCode; }
            set
            {
                if (value > 65535 || value < 0)
                {
                    throw new Exception("DeviceNet ProductCode: Allowed values are the integers 0-65535");
                }
                productCode = value;
            }
        }

        private int deviceType;
        /// <summary>
        /// Cfgname:DeviceType
        /// </summary>
        public int DeviceType
        {
            get { return deviceType; }
            set
            {
                if (value > 65535 || value < 0)
                {
                    throw new Exception("DeviceNet DeviceType: Allowed values are the integers 0-65535");
                }
                deviceType = value;
            }
        }

        private int productionInhibitTime;
        /// <summary>
        /// Cfgname:ProductionInhibitTime
        /// </summary>
        public int ProductionInhibitTime
        {
            get { return productionInhibitTime; }
            set
            {
                if (value > 65535 || value < 0)
                {
                    throw new Exception("DeviceNet ProductionInhibitTime: Allowed values are the integers 0-65535");
                }
                productionInhibitTime = value;
            }
        }

        private string connectionType;
        /// <summary>
        /// Cfgname:ConnectionType
        /// </summary>
        public string ConnectionType
        {
            get { return connectionType; }
            set { connectionType = value; }
        }

        private int pollRate;
        /// <summary>
        /// Cfgname:PollRate
        /// </summary>
        public int PollRate
        {
            get { return pollRate; }
            set
            {
                if (value > 65535 || value < 0)
                {
                    throw new Exception("DeviceNet PollRate: Allowed values are the integers 0-65535");
                }
                pollRate = value;
            }
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
                if (value > 64 || value < -1)
                {
                    throw new Exception("DeviceNet ConnectionOutputSize: Allowed values are the integers -1-64");
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
                if (value > 64 || value < -1)
                {
                    throw new Exception("DeviceNet ConnectionInputSize: Allowed values are the integers -1-64");
                }
                connectionInputSize = value;
            }
        }

        private bool quickConnect;
        /// <summary>
        /// Cfgname:QuickConnect
        /// </summary>
        public bool QuickConnect
        {
            get { return quickConnect; }
            set { quickConnect = value; }
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

        public DeviceNetDevice(Instance instanceDeviceNetDevice, IndustrialNetwork connectedtoIndustrialNetwork) :base(instanceDeviceNetDevice,null, connectedtoIndustrialNetwork)
        {
            this.Address = (int)instanceDeviceNetDevice.GetAttribute("Address");
            this.VendorID = (int)instanceDeviceNetDevice.GetAttribute("VendorID");
            this.ProductCode = (int)instanceDeviceNetDevice.GetAttribute("ProductCode");
            this.DeviceType = (int)instanceDeviceNetDevice.GetAttribute("DeviceType");
            this.ProductionInhibitTime = (int)instanceDeviceNetDevice.GetAttribute("ProductionInhibitTime");
            this.ConnectionType = (string)instanceDeviceNetDevice.GetAttribute("ConnectionType");
            this.PollRate = (int)instanceDeviceNetDevice.GetAttribute("PollRate");
            this.ConnectionOutputSize = (int)instanceDeviceNetDevice.GetAttribute("OutputSize");
            this.ConnectionInputSize = (int)instanceDeviceNetDevice.GetAttribute("InputSize");
            this.QuickConnect = (bool)instanceDeviceNetDevice.GetAttribute("QuickConnect");
            this.Simulated = (bool)instanceDeviceNetDevice.GetAttribute("Simulated");
            this.TrustLevel = (string)instanceDeviceNetDevice.GetAttribute("TrustLevel");
            this.StatewhenSystemStartup = (string)instanceDeviceNetDevice.GetAttribute("StateWhenStartup");
            this.RecoveryTime = (int)instanceDeviceNetDevice.GetAttribute("RecoveryTime");


        }
    }
}
