using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalAutoConfigurationTool.EIO
{
    public class SignalBase: IComparable<SignalBase>
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
        public string DefaultName
        {
            get { return ""; }
        }

        private SignalType signalType;
        /// <summary>
        /// Cfgname:SignalType
        /// Allowed Value:DI, DO, GI, GO, AI, AO
        /// </summary>
        public SignalType SignalType
        {
            get { return signalType; }
            set { signalType = value; }
        }
        public string DefaultSignalType
        {
            get { return ""; }
        }

        private Device assignedtoDevice;

        public Device AssignedtoDevice
        {
            get { return assignedtoDevice; }
            set {
                if(assignedtoDevice!= value)
                {
                    if (assignedtoDevice != null)
                    {
                        assignedtoDevice.Signals.Remove(this.name);
                    }
                    assignedtoDevice = value;
                    assignedtoDevice.Signals.Add(this.name, this);
                }
            }
        }
        
        private string signalIdentificationLabel;
        /// <summary>
        /// Cfgname:Label
        /// </summary>
        public string SignalIdentificationLabel
        {
            get { return signalIdentificationLabel; }
            set { signalIdentificationLabel = value; }
        }
        public string DefaultSignalIdentificationLabel
        {
            get { return ""; }
        }

        private string deviceMapping;
        /// <summary>
        /// Cfgname:DeviceMap
        /// </summary>
        public string DeviceMapping
        {
            get { return deviceMapping; }
            set {
                deviceMapping = value;
                if(!string.IsNullOrWhiteSpace(value))
                {
                    List<int> deviceMappings = SignalBase.GetDeviceMappings(value);
                    this.numberOfBits= deviceMappings.Count;
                    this.deviceMappingFirst = deviceMappings[0];
                }else
                {
                    this.deviceMappingFirst = -1;
                }
            }
        }
        public string DefaultDeviceMapping
        {
            get { return ""; }
        }

        private int deviceMappingFirst;
        public int DeviceMappingFirst
        {
            get { return deviceMappingFirst; }
        }

        private int numberOfBits;
        /// <summary>
        /// Cfgname:Size
        /// </summary>
        public int NumberOfBits
        {
            get
            {
                return numberOfBits;
            }
            set
            {
                if(numberOfBits != value && numberOfBits>=1 && numberOfBits<=32)
                {
                    numberOfBits = value;
                    this.deviceMapping = null;
                    this.deviceMappingFirst = -1;
                }
            }
        }
        public int DefaultNumberOfBits
        {
            get { return 23; }
        }

        private int index;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        private string category;
        /// <summary>
        /// Cfgname:Category
        /// </summary>
        public string Category
        {
            get { return category; }
            set { category = value; }
        }
        public string DefaultCategory
        {
            get { return ""; }
        }

        private string accessLevel;
        /// <summary>
        /// Cfgname:Access
        /// </summary>
        public string AccessLevel
        {
            get { return accessLevel; }
            set { accessLevel = value; }
        }
        public string DefaultAccessLevel
        {
            get { return "Default"; }
        }

        private string safeLevel;
        /// <summary>
        /// Cfgname:SafeLevel
        /// </summary>
        public string SafeLevel
        {
            get { return safeLevel; }
            set { safeLevel = value; }
        }
        public string DefaultSafeLevel
        {
            get { return "DefaultSafeLevel"; }
        }

        private int filterTimePassive;
        /// <summary>
        /// Cfgname:FiltPas
        /// </summary>
        public int FilterTimePassive
        {
            get { return filterTimePassive; }
            set
            {
                if (value > 32000 || value < 0)
                {
                    throw new Exception("Filter Time Passive: Allowed values are the integers 0-32000");
                }
                filterTimePassive = value;
            }
        }

        private int filterTimeActive;
        /// <summary>
        /// Cfgname:FiltAct
        /// </summary>
        public int FilterTimeActive
        {
            get { return filterTimeActive; }
            set
            {
                if (value > 32000 || value < 0)
                {
                    throw new Exception("Filter Time Active: Allowed values are the integers 0-32000");
                }
                filterTimeActive = value;
            }
        }

        private bool invertPhysicalValue;
        /// <summary>
        /// Cfgname:Invert
        /// </summary>
        public bool InvertPhysicalValue
        {
            get { return invertPhysicalValue; }
            set { invertPhysicalValue = value; }
        }

        private float defaultValue;
        /// <summary>
        /// Cfgname:Default
        /// </summary>
        public float DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        private string analogEncodingType;
        /// <summary>
        /// Cfgname:EncType
        /// </summary>
        public string AnalogEncodingType
        {
            get { return analogEncodingType; }
            set { analogEncodingType = value; }
        }

        private float maximumLogicalValue;
        /// <summary>
        /// Cfgname:MaxLog
        /// </summary>
        public float MaximumLogicalValue
        {
            get { return maximumLogicalValue; }
            set { maximumLogicalValue = value; }
        }

        private float maximumPhysicalValue;
        /// <summary>
        /// Cfgname:MaxPhys
        /// </summary>
        public float MaximumPhysicalValue
        {
            get { return maximumPhysicalValue; }
            set { maximumPhysicalValue = value; }
        }

        private float maximumPhysicalValueLimit;
        /// <summary>
        /// Cfgname:MaxPhysLimit
        /// </summary>
        public float MaximumPhysicalValueLimit
        {
            get { return maximumPhysicalValueLimit; }
            set { maximumPhysicalValueLimit = value; }
        }

        private int maximumBitValue;
        /// <summary>
        /// Cfgname:MaxBitVal
        /// </summary>
        public int MaximumBitValue
        {
            get { return maximumBitValue; }
            set { maximumBitValue = value; }
        }

        private float minimumLogicalValue;
        /// <summary>
        /// Cfgname:MinLog
        /// </summary>
        public float MinimumLogicalValue
        {
            get { return minimumLogicalValue; }
            set { minimumLogicalValue = value; }
        }

        private float minimumPhysicalValue;
        /// <summary>
        /// Cfgname:MinPhys
        /// </summary>
        public float MinimumPhysicalValue
        {
            get { return minimumPhysicalValue; }
            set { minimumPhysicalValue = value; }
        }

        private float minimumPhysicalValueLimit;
        /// <summary>
        /// Cfgname:MinPhysLimit
        /// </summary>
        public float MinimumPhysicalValueLimit
        {
            get { return minimumPhysicalValueLimit; }
            set { minimumPhysicalValueLimit = value; }
        }

        private int minimumBitValue;
        /// <summary>
        /// Cfgname:MinBitVal
        /// </summary>
        public int MinimumBitValue
        {
            get { return minimumBitValue; }
            set { minimumBitValue = value; }
        }

        public SignalBase(string name, SignalType signalType, string signalIdentificationLabel, string deviceMapping, int numberOfBits, string category, string accessLevel, string safeLevel, Device assignedtoDevice)
        {
            this.Name = name;
            this.SignalType = signalType;
            this.SignalIdentificationLabel = signalIdentificationLabel;
            this.Category = category;
            this.AccessLevel = accessLevel;
            this.SafeLevel = safeLevel;
            this.AssignedtoDevice = assignedtoDevice;
            if (string.IsNullOrWhiteSpace(deviceMapping))
            {
                this.NumberOfBits = numberOfBits;
            }
            else
            {
                this.DeviceMapping = deviceMapping;
            }
        }


        public SignalBase(Instance instanceSignal, Device assignedtoDevice) 
        {
            this.Name = (string)instanceSignal.GetAttribute("Name");
            this.SignalType = (SignalType)Enum.Parse(typeof(SignalType),(string)instanceSignal.GetAttribute("SignalType"));
            this.SignalIdentificationLabel = (string)instanceSignal.GetAttribute("Label");
            this.Category = (string)instanceSignal.GetAttribute("Category");
            this.AccessLevel = (string)instanceSignal.GetAttribute("Access");
            this.SafeLevel = (string)instanceSignal.GetAttribute("SafeLevel");
            this.AssignedtoDevice = assignedtoDevice;
            string strDeviceMapping = (string)instanceSignal.GetAttribute("DeviceMap");
            int iSize = (int)instanceSignal.GetAttribute("Size");
            if (string.IsNullOrWhiteSpace(strDeviceMapping))
            {
                this.NumberOfBits = iSize;
            }
            else
            {
                this.DeviceMapping = strDeviceMapping;
            }
            this.DefaultValue = (float)instanceSignal.GetAttribute("Default");
            this.FilterTimePassive = (int)instanceSignal.GetAttribute("FiltPas");
            this.FilterTimeActive = (int)instanceSignal.GetAttribute("FiltAct");
            this.InvertPhysicalValue = (bool)instanceSignal.GetAttribute("Invert");
            this.AnalogEncodingType = (string)instanceSignal.GetAttribute("EncType");
            this.MaximumLogicalValue = (float)instanceSignal.GetAttribute("MaxLog");
            this.MaximumPhysicalValue = (float)instanceSignal.GetAttribute("MaxPhys");
            this.MaximumPhysicalValueLimit = (float)instanceSignal.GetAttribute("MaxPhysLimit");
            this.MaximumBitValue = (int)instanceSignal.GetAttribute("MaxBitVal");
            this.MinimumLogicalValue = (float)instanceSignal.GetAttribute("MinLog");
            this.MinimumPhysicalValue = (float)instanceSignal.GetAttribute("MinPhys");
            this.MinimumPhysicalValueLimit = (float)instanceSignal.GetAttribute("MinPhysLimit");
            this.MinimumBitValue = (int)instanceSignal.GetAttribute("MinBitVal");
        }
        public int CompareTo(SignalBase other)
        {
            if(this.AssignedtoDevice.Name != other.AssignedtoDevice.Name)
            {
                return this.AssignedtoDevice.Name.CompareTo(other.AssignedtoDevice.Name);
            }
            if ((this.SignalType == SignalType.DI || this.SignalType == SignalType.GI || this.SignalType == SignalType.AI ) && ( other.SignalType == SignalType.DO || other.SignalType == SignalType.GO || other.SignalType == SignalType.AO))
            {
                return -1;
            }
            else if ((this.SignalType == SignalType.DO || this.SignalType == SignalType.GO || this.SignalType == SignalType.AO) && (other.SignalType == SignalType.DI || other.SignalType == SignalType.GI || other.SignalType == SignalType.AI))
            {
                return 1;
            }
            //string thisDeviceMapping = this.DeviceMapping;
            //string otherDeviceMapping = other.DeviceMapping;
            //if (thisDeviceMapping == null)
            //{
            //    thisDeviceMapping = "";
            //}
            //if (otherDeviceMapping == null)
            //{
            //    otherDeviceMapping = "";
            //}
            //return thisDeviceMapping.CompareTo(otherDeviceMapping);
            return this.DeviceMappingFirst.CompareTo(other.DeviceMappingFirst);
        }

        static public List<int> GetDeviceMappings(string deviceMapping)
        {
            List<int> deviceMappings = new List<int>();
            foreach (string strDeviceMapping in deviceMapping.Replace(" ", "").Split(','))
            {
                int indexDash = strDeviceMapping.IndexOf('-');
                if (indexDash == -1)
                {
                    deviceMappings.Add(int.Parse(strDeviceMapping));
                }
                else
                {
                    int deviceMappingStart = int.Parse(strDeviceMapping.Substring(0, indexDash));
                    int deviceMappingEnd = int.Parse(strDeviceMapping.Substring(indexDash + 1));
                    while (deviceMappingStart <= deviceMappingEnd)
                    {
                        deviceMappings.Add(deviceMappingStart);
                        deviceMappingStart++;
                    }
                }
            }
            deviceMappings.Sort();
            return deviceMappings;
        }
    }

    public enum SignalType { DI=0, GI=1, AI=2, DO=3 , GO=4, AO=5 };

}
