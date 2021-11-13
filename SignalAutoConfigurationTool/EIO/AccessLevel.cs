using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalAutoConfigurationTool.EIO
{
    public class AccessLevel
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

        private bool rapid;
        /// <summary>
        /// Cfgname:Rapid
        /// </summary>
        public bool Rapid
        {
            get { return rapid; }
            set { rapid = value; }
        }

        private bool localClientinManualMode;
        /// <summary>
        /// Cfgname:LocalManual
        /// </summary>
        public bool LocalClientinManualMode
        {
            get { return localClientinManualMode; }
            set { localClientinManualMode = value; }
        }

        private bool localClientinAutoMode;
        /// <summary>
        /// Cfgname:LocalAuto
        /// </summary>
        public bool LocalClientinAutoMode
        {
            get { return localClientinAutoMode; }
            set { localClientinAutoMode = value; }
        }

        private bool remoteClientinManualMode;
        /// <summary>
        /// Cfgname:RemoteManual
        /// </summary>
        public bool RemoteClientinManualMode
        {
            get { return remoteClientinManualMode; }
            set { remoteClientinManualMode = value; }
        }

        private bool remoteClientinAutoMode;
        /// <summary>
        /// Cfgname:RemoteAuto
        /// </summary>
        public bool RemoteClientinAutoMode
        {
            get { return remoteClientinAutoMode; }
            set { remoteClientinAutoMode = value; }
        }

        private FieldBus fieldBus;

        public FieldBus FieldBus
        {
            get { return fieldBus; }
        }

        public AccessLevel(Instance instanceIndustrialNetwork, FieldBus fieldBus)
        {
            this.fieldBus = fieldBus;
            this.Name = (string)instanceIndustrialNetwork.GetAttribute("Name");
            this.Rapid = (bool)instanceIndustrialNetwork.GetAttribute("Rapid");
            this.LocalClientinManualMode = (bool)instanceIndustrialNetwork.GetAttribute("LocalManual");
            this.LocalClientinAutoMode = (bool)instanceIndustrialNetwork.GetAttribute("LocalAuto");
            this.RemoteClientinManualMode = (bool)instanceIndustrialNetwork.GetAttribute("RemoteManual");
            this.RemoteClientinAutoMode = (bool)instanceIndustrialNetwork.GetAttribute("RemoteAuto");
        }
    }
}
