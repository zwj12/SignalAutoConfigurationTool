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
        public int DefaultAddress
        {
            get { return 63; }
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
        public int DefaultVendorID
        {
            get { return 0; }
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
        public int DefaultProductCode
        {
            get { return 0; }
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
        public int DefaultDeviceType
        {
            get { return 0; }
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
        public int DefaultProductionInhibitTime
        {
            get { return 10; }
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
        public string DefaultConnectionType
        {
            get { return "POLLED"; }
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
        public int DefaultPollRate
        {
            get { return 1000; }
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
                if (value > 64 || value < -1)
                {
                    throw new Exception("DeviceNet ConnectionInputSize: Allowed values are the integers -1-64");
                }
                connectionInputSize = value;
            }
        }
        public int DefaultConnectionInputSize
        {
            get { return 0; }
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
        public bool DefaultQuickConnect
        {
            get { return false; }
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

        private bool simulated;
        /// <summary>
        /// Cfgname:Simulated
        /// </summary>
        public bool Simulated
        {
            get { return simulated; }
            set { simulated = value; }
        }
        public bool DefaultSimulated
        {
            get { return false; }
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
            this.StateWhenSystemStartup = (string)instanceDeviceNetDevice.GetAttribute("StateWhenStartup");
            this.RecoveryTime = (int)instanceDeviceNetDevice.GetAttribute("RecoveryTime");
        }

        public override string GetDeviceCFG()
        {
            List<string> strPreLines = new List<string>();
            strPreLines.Add(string.Format("      -Name \"{0}\"", this.Name));
            FillCfgLines(strPreLines, "TrustLevel", this.TrustLevel,this.DefaultTrustLevel);
            FillCfgLines(strPreLines, "VendorName", this.VendorName, this.DefaultVendorName);
            FillCfgLines(strPreLines, "ProductName", this.ProductName, this.DefaultProductName);
            FillCfgLines(strPreLines, "Address", this.Address, this.DefaultAddress);
            FillCfgLines(strPreLines, "VendorId", this.VendorID, this.DefaultVendorID);
            FillCfgLines(strPreLines, "ProductCode", this.ProductCode, this.DefaultProductCode);
            FillCfgLines(strPreLines, "DeviceType", this.DeviceType, this.DefaultDeviceType);
            FillCfgLines(strPreLines, "PollRate", this.PollRate, this.DefaultPollRate);
            FillCfgLines(strPreLines, "OutputSize", this.ConnectionOutputSize, this.DefaultConnectionOutputSize);
            FillCfgLines(strPreLines, "InputSize", this.ConnectionInputSize, this.DefaultConnectionInputSize);
            FillCfgLines(strPreLines, "Label", this.IdentificationLabel, this.DefaultIdentificationLabel);
            FillCfgLines(strPreLines, "ConnectionType", this.ConnectionType, this.DefaultConnectionType);
            FillCfgLines(strPreLines, "ProductionInhibitTime", this.ProductionInhibitTime, this.DefaultProductionInhibitTime);
            FillCfgLines(strPreLines, "QuickConnect", this.QuickConnect, this.DefaultQuickConnect);
            FillCfgLines(strPreLines, "RecoveryTime", this.RecoveryTime, this.DefaultRecoveryTime);
            FillCfgLines(strPreLines, "Simulated", this.Simulated, this.DefaultSimulated);
            FillCfgLines(strPreLines, "StateWhenStartup", this.StateWhenSystemStartup, this.DefaultStateWhenSystemStartup);
            StringBuilder strBuilder = new StringBuilder();
            foreach(string str in strPreLines)
            {
                strBuilder.Append(str);
            }
            return strBuilder.ToString();
        }

        static public void FillCfgLines(List<string> strPreLines, string strParameter, int intParameterValue, int intDefaultParameterValue)
        {
            if (intParameterValue == intDefaultParameterValue)
            {
                return ;
            }
            string strIndentation = "     ";
            strParameter = string.Format(" -{0} {1}", strParameter, intParameterValue);
            if (strPreLines[strPreLines.Count-1].Length + strParameter.Length < 80)
            {
                strPreLines[strPreLines.Count - 1]= strPreLines[strPreLines.Count - 1] + strParameter;
            }
            else
            {
                strPreLines[strPreLines.Count - 1] = strPreLines[strPreLines.Count - 1] + "\\\n";
                strPreLines.Add (strIndentation + strParameter);
            }
        }
        static public void FillCfgLines(List<string> strPreLines, string strParameter, float floatParameterValue, float floatDefaultParameterValue)
        {
            if (floatParameterValue == floatDefaultParameterValue)
            {
                return;
            }
            string strIndentation = "     ";
            strParameter = string.Format(" -{0} {1}", strParameter, floatParameterValue);
            if (strPreLines[strPreLines.Count - 1].Length + strParameter.Length < 80)
            {
                strPreLines[strPreLines.Count - 1] = strPreLines[strPreLines.Count - 1] + strParameter;
            }
            else
            {
                strPreLines[strPreLines.Count - 1] = strPreLines[strPreLines.Count - 1] + "\\\n";
                strPreLines.Add(strIndentation + strParameter);
            }
        }

        static public void FillCfgLines(List<string> strPreLines, string strParameter, string strParameterValue, string strDefaultParameterValue)
        {
            if (strParameterValue == strDefaultParameterValue)
            {
                return;
            }
            string strIndentation = "     ";
            strParameter = string.Format(" -{0} \"{1}\"", strParameter, strParameterValue);
            if (strPreLines[strPreLines.Count - 1].Length + strParameter.Length < 80)
            {
                strPreLines[strPreLines.Count - 1] = strPreLines[strPreLines.Count - 1] + strParameter;
            }
            else
            {
                strPreLines[strPreLines.Count - 1] = strPreLines[strPreLines.Count - 1] + "\\\n";
                strPreLines.Add(strIndentation + strParameter);
            }
        }

        static public void FillCfgLines(List<string> strPreLines, string strParameter, bool boolParameterValue, bool boolDefaultParameterValue)
        {
            if (boolParameterValue == boolDefaultParameterValue)
            {
                return;
            }
            string strIndentation = "     ";
            strParameter = string.Format(" -{0}", strParameter);
            if (strPreLines[strPreLines.Count - 1].Length + strParameter.Length < 80)
            {
                strPreLines[strPreLines.Count - 1] = strPreLines[strPreLines.Count - 1] + strParameter;
            }
            else
            {
                strPreLines[strPreLines.Count - 1] = strPreLines[strPreLines.Count - 1] + "\\\n";
                strPreLines.Add(strIndentation + strParameter);
            }
        }
    }
}
