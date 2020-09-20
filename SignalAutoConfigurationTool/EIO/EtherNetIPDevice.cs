using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABB.Robotics.Controllers.ConfigurationDomain;

namespace SignalAutoConfigurationTool.EIO
{
    public class EtherNetIPDevice : Device
    {

        private string address;
        /// <summary>
        /// Cfgname:Address
        /// </summary>
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
            }
        }
        public string DefaultAddress
        {
            get { return ""; }
        }

        private int vendorID;
        /// <summary>
        /// Cfgname:VendorID
        /// </summary>
        public int VendorID
        {
            get { return vendorID; }
            set
            {
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

        private int outputAssembly;
        /// <summary>
        /// Cfgname:OutputAssembly
        /// </summary>
        public int OutputAssembly
        {
            get { return outputAssembly; }
            set
            {
                if (value > 65535 || value < 0)
                {
                    throw new Exception("EtherNetIP OutputAssembly: Allowed values are the integers 0-65535");
                }
                outputAssembly = value;
            }
        }
        public int DefaultOutputAssembly
        {
            get { return 0; }
        }

        private int inputAssembly;
        /// <summary>
        /// Cfgname:InputAssembly
        /// </summary>
        public int InputAssembly
        {
            get { return inputAssembly; }
            set
            {
                if (value > 65535 || value < 0)
                {
                    throw new Exception("EtherNetIP InputAssembly: Allowed values are the integers 0-65535");
                }
                inputAssembly = value;
            }
        }
        public int DefaultInputAssembly
        {
            get { return 0; }
        }

        private int configurationAssembly;
        /// <summary>
        /// Cfgname:ConfigurationAssembly
        /// </summary>
        public int ConfigurationAssembly
        {
            get { return configurationAssembly; }
            set
            {
                if (value > 65535 || value < 0)
                {
                    throw new Exception("EtherNetIP ConfigurationAssembly: Allowed values are the integers 0-65535");
                }
                configurationAssembly = value;
            }
        }
        public int DefaultConfigurationAssembly
        {
            get { return 0; }
        }

        private string inputConnectionType;
        /// <summary>
        /// Cfgname:InputConnectionType
        /// </summary>
        public string InputConnectionType
        {
            get { return inputConnectionType; }
            set
            {
                inputConnectionType = value;
            }
        }
        public string DefaultInputConnectionType
        {
            get { return "Multicast"; }
        }

        private string connectionPriority;
        /// <summary>
        /// Cfgname:ConnectionPriority
        /// </summary>
        public string ConnectionPriority
        {
            get { return connectionPriority; }
            set
            {
                connectionPriority = value;
            }
        }
        public string DefaultConnectionPriority
        {
            get { return "Low"; }
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
                if (value > 505 || value < 0)
                {
                    throw new Exception("EtherNetIP ConnectionOutputSize: Allowed values are the integers 0-505");
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
                if (value > 509 || value < 0)
                {
                    throw new Exception("EtherNetIP ConnectionInputSize: Allowed values are the integers 0-509");
                }
                connectionInputSize = value;
            }
        }
        public int DefaultConnectionInputSize
        {
            get { return 0; }
        }

        private int configurationSize;
        /// <summary>
        /// Cfgname:ConfigurationSize
        /// </summary>
        public int ConfigurationSize
        {
            get { return configurationSize; }
            set
            {
                if (value > 400 || value < 0)
                {
                    throw new Exception("EtherNetIP ConfigurationSize: Allowed values are the integers 0-400");
                }
                configurationSize = value;
            }
        }
        public int DefaultConfigurationSize
        {
            get { return 0; }
        }

        private string configurationData00;
        /// <summary>
        /// Cfgname:ConfigurationData00
        /// </summary>
        public string ConfigurationData00
        {
            get { return configurationData00; }
            set
            {
                configurationData00 = value;
            }
        }
        public string DefaultConfigurationData00
        {
            get { return "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"; }
        }

        private int outputRPI;
        /// <summary>
        /// Cfgname:O2T_RPI
        /// </summary>
        public int OutputRPI
        {
            get { return outputRPI; }
            set
            {
                if (value < 1)
                {
                    throw new Exception("EtherNetIP OutputRPI: Allowed values are the integers 1-4,294,967,296");
                }
                outputRPI = value;
            }
        }
        public int DefaultOutputRPI
        {
            get { return 50000; }
        }

        private int inputRPI;
        /// <summary>
        /// Cfgname:T2O_RPI
        /// </summary>
        public int InputRPI
        {
            get { return inputRPI; }
            set
            {
                if (value < 1)
                {
                    throw new Exception("EtherNetIP InputRPI: Allowed values are the integers 1-4,294,967,296");
                }
                inputRPI = value;
            }
        }
        public int DefaultInputRPI
        {
            get { return 50000; }
        }

        public EtherNetIPDevice(Instance instanceEtherNetIPDevice, IndustrialNetwork connectedtoIndustrialNetwork) : base(instanceEtherNetIPDevice, null, connectedtoIndustrialNetwork)
        {
            this.Address = (string)instanceEtherNetIPDevice.GetAttribute("Address");           
            this.VendorID = (int)instanceEtherNetIPDevice.GetAttribute("VendorID");
            this.ProductCode = (int)instanceEtherNetIPDevice.GetAttribute("ProductCode");
            this.DeviceType = (int)instanceEtherNetIPDevice.GetAttribute("DeviceType");
            this.OutputAssembly = (int)instanceEtherNetIPDevice.GetAttribute("OutputAssembly");
            this.InputAssembly = (int)instanceEtherNetIPDevice.GetAttribute("InputAssembly");
            this.ConfigurationAssembly = (int)instanceEtherNetIPDevice.GetAttribute("ConfigurationAssembly");
            this.InputConnectionType = (string)instanceEtherNetIPDevice.GetAttribute("InputConnectionType");
            this.ConnectionPriority = (string)instanceEtherNetIPDevice.GetAttribute("ConnectionPriority");
            this.ConnectionOutputSize = (int)instanceEtherNetIPDevice.GetAttribute("OutputSize");
            this.ConnectionInputSize = (int)instanceEtherNetIPDevice.GetAttribute("InputSize");
            this.ConfigurationSize = (int)instanceEtherNetIPDevice.GetAttribute("ConfigurationSize");
            this.ConfigurationData00 = (string)instanceEtherNetIPDevice.GetAttribute("ConfigurationData00");
            this.OutputRPI =int.Parse(instanceEtherNetIPDevice.GetAttribute("O2T_RPI").ToString());
            this.InputRPI = int.Parse(instanceEtherNetIPDevice.GetAttribute("T2O_RPI").ToString());
        }

        public override string GetDeviceCFG()
        {
            List<string> strPreLines = new List<string>();
            strPreLines.Add(string.Format("      -Name \"{0}\"", this.Name));
            FillCfgLines(strPreLines, "VendorName", this.VendorName, this.DefaultVendorName);
            FillCfgLines(strPreLines, "ProductName", this.ProductName, this.DefaultProductName);
            FillCfgLines(strPreLines, "Label", this.IdentificationLabel, this.DefaultIdentificationLabel);
            FillCfgLines(strPreLines, "Address", this.Address, this.DefaultAddress);
            FillCfgLines(strPreLines, "VendorId", this.VendorID, this.DefaultVendorID);
            FillCfgLines(strPreLines, "ProductCode", this.ProductCode, this.DefaultProductCode);
            FillCfgLines(strPreLines, "DeviceType", this.DeviceType, this.DefaultDeviceType);
            FillCfgLines(strPreLines, "OutputAssembly", this.OutputAssembly, this.DefaultOutputAssembly);
            FillCfgLines(strPreLines, "InputAssembly", this.InputAssembly, this.DefaultInputAssembly);
            FillCfgLines(strPreLines, "ConfigurationAssembly", this.ConfigurationAssembly, this.DefaultConfigurationAssembly);
            FillCfgLines(strPreLines, "InputConnectionType", this.InputConnectionType, this.DefaultInputConnectionType);
            FillCfgLines(strPreLines, "ConnectionPriority", this.ConnectionPriority, this.DefaultConnectionPriority);
            FillCfgLines(strPreLines, "OutputSize", this.ConnectionOutputSize, this.DefaultConnectionOutputSize);
            FillCfgLines(strPreLines, "InputSize", this.ConnectionInputSize, this.DefaultConnectionInputSize);
            FillCfgLines(strPreLines, "ConfigurationSize", this.ConfigurationSize, this.DefaultConfigurationSize);
            FillCfgLines(strPreLines, "ConfigurationData00", this.ConfigurationData00, this.DefaultConfigurationData00);
            FillCfgLines(strPreLines, "O2T_RPI", this.OutputRPI, this.DefaultOutputRPI);
            FillCfgLines(strPreLines, "T2O_RPI", this.InputRPI, this.DefaultInputRPI);
            StringBuilder strBuilder = new StringBuilder();
            foreach (string str in strPreLines)
            {
                strBuilder.Append(str);
            }
            return strBuilder.ToString();
        }
    }
}
