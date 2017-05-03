using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalAutoConfigurationTool.EIO
{
    public class SignalGroupInput : SignalBase
    {
         public SignalGroupInput(Instance instanceSignal, Device assignedtoDevice) :base(instanceSignal, assignedtoDevice)
        {

        }
    }
}
