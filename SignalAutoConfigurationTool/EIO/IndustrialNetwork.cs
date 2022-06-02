using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalAutoConfigurationTool.EIO
{
    public class IndustrialNetwork
    {
        private string name;
        /// <summary>
        /// Cfgname:Name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string identificationLabel;
        /// <summary>
        /// Cfgname:Label
        /// </summary>
        public string IdentificationLabel
        {
            get { return identificationLabel; }
            set { identificationLabel = value; }
        }

        private bool simulated;
        /// <summary>
        /// Cfgname:Simulated
        /// </summary>
        public bool Simulated
        {
            get { return simulated; }
            set { simulated = value; }
        }

        private FieldBus fieldBus;

        public FieldBus FieldBus
        {
            get { return fieldBus; }
        }

        public virtual Dictionary<string, Device> GetDevices()
        {
            throw new Exception("Please override this function");
        }

        public Dictionary<string, Signal> GetSignals()
        {
            Dictionary<string, Device> devices = this.GetDevices();
            Dictionary<string, Signal> signals = new Dictionary<string, Signal>();
            foreach (Device device in devices.Values)
            {
                signals = signals.Concat(device.Signals.Select(x => new KeyValuePair<string, Signal>(x.Key, x.Value))).ToDictionary(x => x.Key, y => y.Value);
            }
            return signals;
        }

        /// <summary>
        /// For RobotWare6
        /// </summary>
        /// <param name="instanceIndustrialNetwork"></param>
        /// <param name="fieldBus"></param>
        public IndustrialNetwork(Instance instanceIndustrialNetwork, FieldBus fieldBus, bool isRobotWare7 = false)
        {
            if (!isRobotWare7 && instanceIndustrialNetwork!=null)
            {
                this.Name = (string)instanceIndustrialNetwork.GetAttribute("Name");
                this.IdentificationLabel = (string)instanceIndustrialNetwork.GetAttribute("Label");
                this.Simulated = (bool)instanceIndustrialNetwork.GetAttribute("Simulated");
            }
            this.fieldBus = fieldBus;
        }
        
        public virtual string GetIndustrialNetworkCFG()
        {
            return null;
        }

    }
}
