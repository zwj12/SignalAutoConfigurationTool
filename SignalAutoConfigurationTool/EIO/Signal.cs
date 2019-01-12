using ABB.Robotics.Controllers.ConfigurationDomain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SignalAutoConfigurationTool.EIO
{
    public class Signal: IComparable<Signal>, INotifyPropertyChanged
    {
        private static string strRegexIdentifier = "^[a-zA-Z_][a-zA-Z0-9_]*$";

        private string name;
        /// <summary>
        /// Cfgname:Name
        /// </summary>
        public string Name
        {
            get { return name; }
            set {
                if (Regex.IsMatch(value, strRegexIdentifier))
                {
                    name = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    }
                }
                else
                {
                    throw new Exception("Name must be a valid identifier");
                }
            }
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
            set {
                signalType = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SignalType"));
                }
            }
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
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("DeviceMapping"));
                }
                if (!string.IsNullOrWhiteSpace(value))
                {
                    List<int> deviceMappings = Signal.GetDeviceMappings(value);
                    this.numberOfBits= deviceMappings.Count;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("NumberOfBits"));
                    }
                    this.deviceMappingFirst = deviceMappings[0];
                    this.deviceMappingLast = deviceMappings[deviceMappings.Count-1];
                }
                else
                {
                    this.deviceMappingFirst = -1;
                    this.deviceMappingLast = -1;
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

        private int deviceMappingLast;
        public int DeviceMappingLast
        {
            get { return deviceMappingLast; }
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
                if(numberOfBits != value && value>=1 && value <= 32)
                {
                    numberOfBits = value;
                    this.deviceMapping = null;
                    this.deviceMappingFirst = -1;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DeviceMapping"));
                        PropertyChanged(this, new PropertyChangedEventArgs("NumberOfBits"));
                    }
                }
            }
        }
        public int DefaultNumberOfBits
        {
            get
            {
                if (this.SignalType == SignalType.DI || this.SignalType == SignalType.DO)
                {
                    return 1;
                }
                else
                {
                    return 23;
                }
                //if (this.assignedtoDevice.Name == "Virtual1")
                //{
                //    return 23;
                //}
                //else
                //{
                //    if (this.SignalType == SignalType.DI || this.SignalType == SignalType.DO)
                //    {
                //        return 1;
                //    }
                //    else
                //    {
                //        return 16;
                //    }
                //}
            }
        }

        private int index;

        public int Index
        {
            get { return index; }
            set {
                index = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Index"));
                }
            }
        }

        private string category;
        /// <summary>
        /// Cfgname:Category
        /// </summary>
        public string Category
        {
            get { return category; }
            set {
                if (string.IsNullOrWhiteSpace(value))
                {
                    category = value.Trim();
                }
                else
                {
                    if (Regex.IsMatch(value, strRegexIdentifier))
                    {
                        category = value;
                    }
                    else
                    {
                        throw new Exception("Category must be a valid identifier");
                    }
                }       
            }
        }
        public string DefaultCategory
        {
            get { return ""; }
        }

        private AccessLevel accessLevel;
        /// <summary>
        /// Cfgname:Access
        /// </summary>
        public AccessLevel AccessLevel
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
        public int DefaultFilterTimePassive
        {
            get { return 0; }
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
        public int DefaultFilterTimeActive
        {
            get { return 0; }
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
        public bool DefaultInvertPhysicalValue
        {
            get { return false; }
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
        public float DefaultDefaultValue
        {
            get { return 0; }
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
        public string DefaultAnalogEncodingType
        {
            get { return "TWO_COMP"; }
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
        public float DefaultMaximumLogicalValue
        {
            get { return 0; }
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
        public float DefaultMaximumPhysicalValue
        {
            get { return 0; }
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
        public float DefaultMaximumPhysicalValueLimit
        {
            get { return 0; }
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
        public int DefaultMaximumBitValue
        {
            get { return 0; }
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
        public float DefaultMinimumLogicalValue
        {
            get { return 0; }
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
        public float DefaultMinimumPhysicalValue
        {
            get { return 0; }
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
        public float DefaultMinimumPhysicalValueLimit
        {
            get { return 0; }
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
        public int DefaultMinimumBitValue
        {
            get { return 0; }
        }

        private bool reservedDeviceMapping;

        public bool ReservedDeviceMapping
        {
            get { return reservedDeviceMapping; }
            set { reservedDeviceMapping = value; }
        }

        private bool alignmentByte;

        public bool AlignmentByte
        {
            get { return alignmentByte; }
            set { alignmentByte = value; }
        }

        private double signalValue;

        public double SignalValue
        {
            get { return signalValue; }
            set { signalValue = value; }
        }

        private bool littleEndian = false;

        public bool LittleEndian
        {
            get { return littleEndian; }
            set { littleEndian = value; }
        }

        private bool inputAsPhysical;

        public bool InputAsPhysical
        {
            get { return inputAsPhysical; }
            set { inputAsPhysical = value; }
        }

        private bool simulated;

        public bool Simulated
        {
            get { return simulated; }
            set { simulated = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Signal(string name, SignalType signalType, string signalIdentificationLabel, string deviceMapping, int numberOfBits, string category, AccessLevel accessLevel, string safeLevel, Device assignedtoDevice)
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
        
        public Signal(Instance instanceSignal, Device assignedtoDevice) 
        {
            this.Name = (string)instanceSignal.GetAttribute("Name");
            this.SignalType = (SignalType)Enum.Parse(typeof(SignalType),(string)instanceSignal.GetAttribute("SignalType"));
            this.SignalIdentificationLabel = (string)instanceSignal.GetAttribute("Label");
            this.Category = (string)instanceSignal.GetAttribute("Category");
            if (assignedtoDevice.ConnectedtoIndustrialNetwork.FieldBus.AccessLevels.ContainsKey((string)instanceSignal.GetAttribute("Access")))
            {
                this.AccessLevel = assignedtoDevice.ConnectedtoIndustrialNetwork.FieldBus.AccessLevels[(string)instanceSignal.GetAttribute("Access")];
            }
            else
            {
                foreach(AccessLevel accessLevel in assignedtoDevice.ConnectedtoIndustrialNetwork.FieldBus.AccessLevels.Values)
                {
                    if(accessLevel.Name.ToLower()== ((string)instanceSignal.GetAttribute("Access")).ToLower())
                    {
                        this.AccessLevel = accessLevel;
                        break;
                    }
                }
            }
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

        public int CompareTo(Signal other)
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

        public void InitAnalogEncoding()
        {
            if (this.SignalType == SignalType.AI || this.SignalType == SignalType.AO) 
            {
                if (this.analogEncodingType == "TWO_COMP")
                {
                    this.minimumLogicalValue = 0;
                    this.minimumPhysicalValue = 0;
                    this.minimumPhysicalValueLimit = 0;
                    this.minimumBitValue = -32768;

                    this.maximumLogicalValue = 10;
                    this.maximumPhysicalValue = 10;
                    this.maximumPhysicalValueLimit = 10;
                    this.maximumBitValue = 32767;
                }
                else
                {
                    this.minimumLogicalValue = 0;
                    this.minimumPhysicalValue = 0;
                    this.minimumPhysicalValueLimit = 0;
                    this.minimumBitValue = 0;

                    this.maximumLogicalValue = 10;
                    this.maximumPhysicalValue = 10;
                    this.maximumPhysicalValueLimit = 10;
                    this.maximumBitValue = 65535;
                }
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MinimumLogicalValue"));
                    PropertyChanged(this, new PropertyChangedEventArgs("MinimumPhysicalValue"));
                    PropertyChanged(this, new PropertyChangedEventArgs("MinimumPhysicalValueLimit"));
                    PropertyChanged(this, new PropertyChangedEventArgs("MinimumBitValue"));
                    PropertyChanged(this, new PropertyChangedEventArgs("MaximumLogicalValue"));
                    PropertyChanged(this, new PropertyChangedEventArgs("MaximumPhysicalValue"));
                    PropertyChanged(this, new PropertyChangedEventArgs("MaximumPhysicalValueLimit"));
                    PropertyChanged(this, new PropertyChangedEventArgs("MaximumBitValue"));
                }

            }
        }

        public bool GetSignalValue()
        {
            ABB.Robotics.Controllers.IOSystemDomain.Signal signal = this.AssignedtoDevice.ConnectedtoIndustrialNetwork.FieldBus.Controller.IOSystem.GetSignal(this.Name);
            if (signal != null)
            {
                this.SignalValue=signal.Value;
                this.InputAsPhysical=signal.InputAsPhysical;
                this.Simulated = signal.State.Simulated;
                return true;
            }
            return false;
        }
    }

    public enum SignalType { DI=0, GI=1, AI=2, DO=3 , GO=4, AO=5 };

}
