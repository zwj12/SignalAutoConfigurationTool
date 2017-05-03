using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalAutoConfigurationTool.EIO
{
    public class Virtual1 : IndustrialNetwork
    {
        private Virtual1Device virtual1Device;

        public Virtual1Device Virtual1Device
        {
            get { return virtual1Device; }
        }

        public Virtual1(Instance instanceIndustrialNetwork, FieldBus fieldBus) : base(instanceIndustrialNetwork, fieldBus)
        {
            this.virtual1Device = new Virtual1Device(null, this);

        }
        public override Dictionary<string, Device> GetDevices()
        {
            Dictionary<string, Device> devices = new Dictionary<string, EIO.Device>();
            devices.Add(this.virtual1Device.Name, this.virtual1Device);
            return devices;
        }
    }
}
