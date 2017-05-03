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

        public DeviceNetInternalDevice(Instance instanceDeviceNetDevice, IndustrialNetwork connectedtoIndustrialNetwork) :base(instanceDeviceNetDevice,null, connectedtoIndustrialNetwork)
        {
            this.ConnectionOutputSize = (int)instanceDeviceNetDevice.GetAttribute("OutputSize");
            this.ConnectionInputSize = (int)instanceDeviceNetDevice.GetAttribute("InputSize");
            

        }

    }
}
