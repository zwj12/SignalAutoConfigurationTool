﻿using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalAutoConfigurationTool.EIO
{
    public class Local:IndustrialNetwork
    {
        //private Device panelDevice;

        //public Device PanelDevice
        //{
        //    get { return panelDevice; }
        //}

        //private Device drv_1Device;

        //public Device Drv_1Device
        //{
        //    get { return drv_1Device; }
        //}

        //private Device drv_2Device;

        //public Device Drv_2Device
        //{
        //    get { return drv_2Device; }
        //}

        //private Device drv_3Device;

        //public Device Drv_3Device
        //{
        //    get { return drv_3Device; }
        //}

        //private Device drv_4Device;

        //public Device Drv_4Device
        //{
        //    get { return drv_4Device; }
        //}

        private Dictionary<string, LocalDevice> localDevices = new Dictionary<string, EIO.LocalDevice>();

        public Dictionary<string, LocalDevice> LocalDevices
        {
            get { return localDevices; }
        }

        public Local(Instance instanceIndustrialNetwork, FieldBus fieldBus, bool isRobotWare7 = false) :base(instanceIndustrialNetwork, fieldBus, isRobotWare7)
        {
            if (isRobotWare7)
            {
                this.Name = "IntBus";
                if (instanceIndustrialNetwork.Type.Domain.Types.IndexOf("INT_BUS_DEVICE") >= 0)
                {
                    //For RobotWare7
                    foreach (Instance instanceIntBusDevice in instanceIndustrialNetwork.Type.Domain["INT_BUS_DEVICE"].GetInstances())
                    {
                        LocalDevices.Add(instanceIntBusDevice.Name, new LocalDevice(instanceIntBusDevice, this, isRobotWare7));
                    }
                }
            }
            else
            {
                if (instanceIndustrialNetwork.Type.Domain.Types.IndexOf("LOCAL_DEVICE") >= 0)
                {
                    //For RobotWare6
                    foreach (Instance instanceLocalDevice in instanceIndustrialNetwork.Type.Domain["LOCAL_DEVICE"].GetInstances())
                    {
                        LocalDevices.Add(instanceLocalDevice.Name, new LocalDevice(instanceLocalDevice, this));
                    }
                }
            }

         

            //this.panelDevice = new Device(null, "PANEL", this);
            //this.drv_1Device = new Device(null, "DRV_1", this);
            //this.drv_2Device = new Device(null, "DRV_2", this);
            //this.drv_3Device = new Device(null, "DRV_3", this);
            //this.drv_4Device = new Device(null, "DRV_4", this);
        }

        public override Dictionary<string, Device> GetDevices()
        {
            Dictionary<string, Device> devices = new Dictionary<string, EIO.Device>();
            devices = devices.Concat(this.localDevices.Select(x => new KeyValuePair<string, Device>(x.Key, x.Value))).ToDictionary(x => x.Key, y => y.Value);
 
            //devices.Add(this.panelDevice.Name, this.panelDevice);
            //devices.Add(this.drv_1Device.Name, this.drv_1Device);
            //devices.Add(this.drv_2Device.Name, this.drv_2Device);
            //devices.Add(this.drv_3Device.Name, this.drv_3Device);
            //devices.Add(this.drv_4Device.Name, this.drv_4Device);
            return devices;
        }
    }
}
