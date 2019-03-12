using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalAutoConfigurationTool.EIO
{
    public class EtherNetIP: IndustrialNetwork
    {
        private string connection;
        /// <summary>
        /// Cfgname:Connection
        /// </summary>
        public string Connection
        {
            get { return connection; }
            set { connection = value; }
        }

        public EtherNetIP(Instance instanceIndustrialNetwork, FieldBus fieldBus) : base(instanceIndustrialNetwork, fieldBus)
        {
            this.Connection = (string)instanceIndustrialNetwork.GetAttribute("Connection");
        }

        public override Dictionary<string, Device> GetDevices()
        {
            Dictionary<string, Device> devices = new Dictionary<string, EIO.Device>();
            return devices;
        }

        public override string GetIndustrialNetworkCFG()
        {
            List<string> strPreLines = new List<string>
            {
                string.Format("      -Name \"{0}\"", this.Name),
                string.Format(" -Connection \"{0}\"\\\n", this.Connection),
                string.Format("      -Label \"{0}\"", this.IdentificationLabel),
            };
            
            StringBuilder strBuilder = new StringBuilder();
            foreach (string str in strPreLines)
            {
                strBuilder.Append(str);
            }
            return strBuilder.ToString();
        }
    }
}
