using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalAutoConfigurationTool.EIO
{
    public  enum DeviceNetCommunicationSpeed { BaudRate125Kbps=125, BaudRate250Kbps=250, BaudRate500Kbps=500 }

    public class DeviceNet: IndustrialNetwork
    {
        private int address;
        /// <summary>
        /// Cfgname:Address
        /// </summary>
        public int Address
        {
            get { return address; }
            set {
                if(value>63 || value<0)
                {
                    throw new Exception("DeviceNet Address: Allowed values are the integers 0-63");
                }
                address = value;
            }
        }

        private DeviceNetCommunicationSpeed deviceNetCommunicationSpeed;
        /// <summary>
        /// Cfgname:BaudRate
        /// </summary>
        public DeviceNetCommunicationSpeed DeviceNetCommunicationSpeed
        {
            get { return deviceNetCommunicationSpeed; }
            set { deviceNetCommunicationSpeed = value; }
        }

        private Dictionary<string,DeviceNetDevice> deviceNetDevices = new Dictionary<string, EIO.DeviceNetDevice>();

        public Dictionary<string,DeviceNetDevice> DeviceNetDevices
        {
            get { return deviceNetDevices; }
        }

        private DeviceNetInternalDevice deviceNetInternalDevice;

        public DeviceNetInternalDevice DeviceNetInternalDevice
        {
            get { return deviceNetInternalDevice; }
        }
                
        public DeviceNet(Instance instanceIndustrialNetwork, FieldBus fieldBus) : base(instanceIndustrialNetwork, fieldBus)
        {
            this.Address = int.Parse((string)instanceIndustrialNetwork.GetAttribute("Address"));
            this.DeviceNetCommunicationSpeed = (DeviceNetCommunicationSpeed)instanceIndustrialNetwork.GetAttribute("BaudRate");

            foreach (Instance instanceDeviceNetDevice in instanceIndustrialNetwork.Type.Domain["DEVICENET_DEVICE"].GetInstances())
            {
                DeviceNetDevices.Add(instanceDeviceNetDevice.Name, new DeviceNetDevice(instanceDeviceNetDevice,this));
            }
            deviceNetInternalDevice = new DeviceNetInternalDevice(instanceIndustrialNetwork.Type.Domain["DEVICENET_INTERNAL_DEVICE"].GetInstance("DN_Internal_Device"), this);
        }

        public override Dictionary<string, Device> GetDevices()
        {
            Dictionary<string, Device> devices = new Dictionary<string, EIO.Device>();
            devices = devices.Concat(this.DeviceNetDevices.Select(x => new KeyValuePair<string, Device>(x.Key, x.Value))).ToDictionary(x => x.Key, y => y.Value);
            devices.Add(this.DeviceNetInternalDevice.Name, this.DeviceNetInternalDevice);
            return devices;
        }
    }
}
