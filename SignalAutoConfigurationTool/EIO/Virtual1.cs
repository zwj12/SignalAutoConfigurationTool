using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalAutoConfigurationTool.EIO
{
    /// <summary>
    /// Use name Virtual1 instead of Virtual
    /// </summary>
    public class Virtual1 : IndustrialNetwork
    {
        private Virtual1Device virtual1Device;

        public Virtual1Device Virtual1Device
        {
            get { return virtual1Device; }
        }

        public Virtual1(Instance instanceIndustrialNetwork, FieldBus fieldBus, bool isRobotWare7 = false) : base(instanceIndustrialNetwork, fieldBus)
        {
            if (isRobotWare7)
            {
                this.Name = "Virtual";
            }
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
