using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ABB.Robotics.Controllers.ConfigurationDomain;

namespace SignalAutoConfigurationTool.EIO
{
    public class LocalDevice : Device
    {
        public LocalDevice(Instance instanceProfinetDevice, IndustrialNetwork connectedtoIndustrialNetwork) : base(instanceProfinetDevice, null, connectedtoIndustrialNetwork)
        {
        }

        public override string GetDeviceCFG()
        {
            List<string> strPreLines = new List<string>();
            strPreLines.Add(string.Format("      -Name \"{0}\" -Network \"Local\"", this.Name));
            FillCfgLines(strPreLines, "Label", this.IdentificationLabel, this.DefaultIdentificationLabel);
            FillCfgLines(strPreLines, "Simulated", this.Simulated, this.DefaultSimulated);
            StringBuilder strBuilder = new StringBuilder();
            foreach (string str in strPreLines)
            {
                strBuilder.Append(str);
            }
            return strBuilder.ToString();
        }
    }
}
