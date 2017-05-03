using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalAutoConfigurationTool.EIO
{
    public class SignalAnalogOutput : SignalBase
    {
     

        public SignalAnalogOutput(Instance instanceSignal, Device assignedtoDevice) :base(instanceSignal, assignedtoDevice)
        {
        }
    }
}
