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
        private int outputSize;
        /// <summary>
        /// Cfgname:OutputSize
        /// </summary>
        public int OutputSize
        {
            get { return outputSize; }
            set
            {
                if (value == 8 || value ==16 || value == 32 || value == 64 || value == 128 || value == 256)
                {
                    outputSize = value;
                }
                else
                {
                    throw new Exception("Profinet OutputSize: Allowed values are 8, 16, 32, 64, 128 or 256");
                }
            }
        }

        private int inputSize;
        /// <summary>
        /// Cfgname:InputSize
        /// </summary>
        public int InputSize
        {
            get { return inputSize; }
            set
            {
                if (value == 8 || value == 16 || value == 32 || value == 64 || value == 128 || value == 256)
                {
                    inputSize = value;
                }
                else
                {
                    throw new Exception("Profinet InputSize: Allowed values are 8, 16, 32, 64, 128 or 256");
                }
            }
        }

        public ProfinetInternalDevice(Instance instanceProfinetDevice, IndustrialNetwork connectedtoIndustrialNetwork) :base(instanceProfinetDevice,null, connectedtoIndustrialNetwork)
        {
            this.OutputSize = (int)instanceProfinetDevice.GetAttribute("OutputSize");
            this.InputSize = (int)instanceProfinetDevice.GetAttribute("InputSize");
        }
    }
}
