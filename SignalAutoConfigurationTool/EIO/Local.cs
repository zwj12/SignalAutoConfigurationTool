using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalAutoConfigurationTool.EIO
{
    public class Local:IndustrialNetwork
    {
        private Device panelDevice;

        public Device PanelDevice
        {
            get { return panelDevice; }
        }

        private Device drv_1Device;

        public Device Drv_1Device
        {
            get { return drv_1Device; }
        }

        private Device drv_2Device;

        public Device Drv_2Device
        {
            get { return drv_2Device; }
        }

        private Device drv_3Device;

        public Device Drv_3Device
        {
            get { return drv_3Device; }
        }

        private Device drv_4Device;

        public Device Drv_4Device
        {
            get { return drv_4Device; }
        }

        public Local(Instance instanceIndustrialNetwork, FieldBus fieldBus) :base(instanceIndustrialNetwork, fieldBus)
        {
            this.panelDevice = new Device(null, "PANEL", this);
            this.drv_1Device = new Device(null, "DRV_1", this);
            this.drv_2Device = new Device(null, "DRV_2", this);
            this.drv_3Device = new Device(null, "DRV_3", this);
            this.drv_4Device = new Device(null, "DRV_4", this);
        }
        public override Dictionary<string, Device> GetDevices()
        {
            Dictionary<string, Device> devices = new Dictionary<string, EIO.Device>();
            devices.Add(this.panelDevice.Name, this.panelDevice);
            devices.Add(this.drv_1Device.Name, this.drv_1Device);
            devices.Add(this.drv_2Device.Name, this.drv_2Device);
            devices.Add(this.drv_3Device.Name, this.drv_3Device);
            devices.Add(this.drv_4Device.Name, this.drv_4Device);
            return devices;
        }
    }
}
