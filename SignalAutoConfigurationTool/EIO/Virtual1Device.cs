using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalAutoConfigurationTool.EIO
{
    public class Virtual1Device:Device
    {
        public Virtual1Device(Instance instanceVirtual1Device, IndustrialNetwork connectedtoIndustrialNetwork) :base(instanceVirtual1Device,"Virtual1", connectedtoIndustrialNetwork)
        {
        }
    }
}
