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
    }
}
