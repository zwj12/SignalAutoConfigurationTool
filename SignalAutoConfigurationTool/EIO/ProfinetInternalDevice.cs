using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalAutoConfigurationTool.EIO
{
    public class ProfinetInternalDevice:Device
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
                if (value == 8 || value ==16 || value == 32 || value == 64 || value == 128 || value == 256)
                {
                    connectionOutputSize = value;
                }
                else
                {
                    throw new Exception("Profinet OutputSize: Allowed values are 8, 16, 32, 64, 128 or 256");
                }
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
                if (value == 8 || value == 16 || value == 32 || value == 64 || value == 128 || value == 256)
                {
                    connectionInputSize = value;
                }
                else
                {
                    throw new Exception("Profinet InputSize: Allowed values are 8, 16, 32, 64, 128 or 256");
                }
            }
        }
        public int DefaultConnectionInputSize
        {
            get { return 0; }
        }

        public ProfinetInternalDevice(Instance instanceProfinetDevice, IndustrialNetwork connectedtoIndustrialNetwork) :base(instanceProfinetDevice,null, connectedtoIndustrialNetwork)
        {
            this.ConnectionOutputSize = (int)instanceProfinetDevice.GetAttribute("OutputSize");
            this.ConnectionInputSize = (int)instanceProfinetDevice.GetAttribute("InputSize");
        }

        public override string GetDeviceCFG()
        {
            List<string> strPreLines = new List<string>();
            strPreLines.Add(string.Format("      -Name \"{0}\"", this.Name));
            FillCfgLines(strPreLines, "Label", this.IdentificationLabel, this.DefaultIdentificationLabel);
            FillCfgLines(strPreLines, "VendorName", this.VendorName, this.DefaultVendorName);
            FillCfgLines(strPreLines, "ProductName", this.ProductName, this.DefaultProductName);
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
