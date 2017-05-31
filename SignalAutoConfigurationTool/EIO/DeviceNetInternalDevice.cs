using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalAutoConfigurationTool.EIO
{
    public class DeviceNetInternalDevice : Device
    {
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

        public DeviceNetInternalDevice(Instance instanceDeviceNetDevice, IndustrialNetwork connectedtoIndustrialNetwork) :base(instanceDeviceNetDevice,null, connectedtoIndustrialNetwork)
        {
            this.ConnectionOutputSize = (int)instanceDeviceNetDevice.GetAttribute("OutputSize");
            this.ConnectionInputSize = (int)instanceDeviceNetDevice.GetAttribute("InputSize");
            

        }
        public override string GetDeviceCFG()
        {
            List<string> strPreLines = new List<string>();
            strPreLines.Add(string.Format("      -Name \"{0}\"", this.Name));
            FillCfgLines(strPreLines, "VendorName", this.VendorName, this.DefaultVendorName);
            FillCfgLines(strPreLines, "ProductName", this.ProductName, this.DefaultProductName);
            FillCfgLines(strPreLines, "Label", this.IdentificationLabel, this.DefaultIdentificationLabel);
            FillCfgLines(strPreLines, "OutputSize", this.ConnectionOutputSize, this.DefaultConnectionOutputSize);
            FillCfgLines(strPreLines, "InputSize", this.ConnectionInputSize, this.DefaultConnectionInputSize);
            StringBuilder strBuilder = new StringBuilder();
            foreach (string str in strPreLines)
            {
                strBuilder.Append(str);
            }
            return strBuilder.ToString();
        }
    }
}
