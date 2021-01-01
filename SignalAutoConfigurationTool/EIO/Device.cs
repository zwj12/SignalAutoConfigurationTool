using ABB.Robotics.Controllers.ConfigurationDomain;
using log4net;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalAutoConfigurationTool.EIO
{
    public class Device
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Device));

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
            get { return "Virtual1"; }
        }

        private IndustrialNetwork connectedtoIndustrialNetwork;
        /// <summary>
        /// Cfgname:Network
        /// </summary>
        public IndustrialNetwork ConnectedtoIndustrialNetwork
        {
            get { return connectedtoIndustrialNetwork; }
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
        public string DefaultIdentificationLabel
        {
            get { return ""; }
        }

        private string vendorName;
        /// <summary>
        /// Cfgname:VendorName
        /// </summary>
        public string VendorName
        {
            get { return vendorName; }
            set { vendorName = value; }
        }
        public string DefaultVendorName
        {
            get { return ""; }
        }

        private string productName;
        /// <summary>
        /// Cfgname:ProductName
        /// </summary>
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }
        public string DefaultProductName
        {
            get { return ""; }
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
        public bool DefaultSimulated
        {
            get { return false; }
        }


        private Dictionary<string, Signal> signals = new Dictionary<string, EIO.Signal>();

        public Dictionary<string, Signal> Signals
        {
            get { return signals; }
            set { signals = value; }
        }

        public Device(string name,string IdentificationLabel,string VendorName, string ProductName, IndustrialNetwork connectedtoIndustrialNetwork)
        {
            this.Name = name;
            this.IdentificationLabel = IdentificationLabel;
            this.VendorName = VendorName;
            this.ProductName = ProductName;
            this.connectedtoIndustrialNetwork = connectedtoIndustrialNetwork;
        }

        public Device(Instance instanceDevice, string name, IndustrialNetwork connectedtoIndustrialNetwork)
        {
            if (instanceDevice != null)
            {
                this.Name = (string)instanceDevice.GetAttribute("Name");
                this.IdentificationLabel = (string)instanceDevice.GetAttribute("Label");
                this.VendorName = (string)instanceDevice.GetAttribute("VendorName");
                this.ProductName = (string)instanceDevice.GetAttribute("ProductName");
                this.Simulated = (bool)instanceDevice.GetAttribute("Simulated");
            }
            else
            {
                this.Name = name;
                this.IdentificationLabel = "";
                this.VendorName = "";
                this.ProductName = "";
            }
            this.connectedtoIndustrialNetwork = connectedtoIndustrialNetwork;
        }

        public void RefreshSignalIndex()
        {
            List<Signal> signals = this.signals.Values.ToList();
            //List<string> categories = new List<string>();
            signals.Sort();
            int indexInput = 0;
            int indexOutput = 0;
            int iDeviceMappingNextBit = 0;
            int oDeviceMappingNextBit = 0;
            foreach (Signal signalbase in signals)
            {
                //if(!string.IsNullOrWhiteSpace(signalbase.Category))
                //{
                //    if (!categories.Contains(signalbase.Category))
                //    {
                //        categories.Add(signalbase.Category);
                //        signalbase.ReservedDeviceMapping = true;
                //    }
                //}
                if (signalbase.SignalType== SignalType.DI || signalbase.SignalType == SignalType.GI ||signalbase.SignalType == SignalType.AI)
                {
                    signalbase.Index = indexInput++;
                    if(signalbase.DeviceMappingFirst!= iDeviceMappingNextBit)
                    {
                        signalbase.ReservedDeviceMapping = true;
                    }
                    iDeviceMappingNextBit = signalbase.DeviceMappingLast + 1;
                }
                else
                {
                    signalbase.Index = indexOutput++;
                    if (signalbase.DeviceMappingFirst != oDeviceMappingNextBit)
                    {
                        signalbase.ReservedDeviceMapping = true;
                    }
                    oDeviceMappingNextBit = signalbase.DeviceMappingLast + 1;
                }
            }
        }

        public void ResetSignalTypeByName()
        {
            foreach (Signal signalbase in signals.Values)
            {
                string strPrefix = signalbase.Name.Substring(0, 2).ToUpper();
                SignalType signalType;
                if (Enum.TryParse(strPrefix, out signalType))
                {
                    if(signalType!= signalbase.SignalType)
                    {
                        signalbase.SignalType = signalType;
                        switch (signalType)
                        {
                            case SignalType.DI:
                            case SignalType.DO:
                                signalbase.NumberOfBits = 1;
                                break;
                            case SignalType.GI:
                            case SignalType.GO:
                                signalbase.NumberOfBits = 16;
                                break;
                            case SignalType.AI:
                            case SignalType.AO:
                                signalbase.NumberOfBits = 16;
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReArrangeSignalDeviceMappingbyIndex()
        {
            List<Signal> signals = this.signals.Values.ToList();
            signals.Sort((x, y) => x.Index - y.Index);
            int iDeviceMappingNextBit = 0;
            int oDeviceMappingNextBit = 0;
            foreach (Signal signalbase in signals)
            {
                if (signalbase.ReservedDeviceMapping)
                {
                    if (signalbase.SignalType == SignalType.DI || signalbase.SignalType == SignalType.GI || signalbase.SignalType == SignalType.AI)
                    {
                        if(signalbase.DeviceMappingFirst> iDeviceMappingNextBit)
                        {
                            iDeviceMappingNextBit = signalbase.DeviceMappingFirst;
                        }
                    }
                    else
                    {
                        if (signalbase.DeviceMappingFirst > oDeviceMappingNextBit)
                        {
                            oDeviceMappingNextBit = signalbase.DeviceMappingFirst;
                        }
                    }
                }
                switch (signalbase.SignalType)
                {
                    case SignalType.DI:
                        signalbase.DeviceMapping = iDeviceMappingNextBit.ToString();
                        iDeviceMappingNextBit++;
                        break;
                    case SignalType.GI:
                        if (signalbase.AlignmentByte)
                        {
                            if(iDeviceMappingNextBit % 8 != 0)
                            {
                                iDeviceMappingNextBit += 8 - iDeviceMappingNextBit % 8;
                            }
                        }
                        if (signalbase.LittleEndian)
                        {
                            string strDeviceMapping = "";
                            int iLittleEndian = iDeviceMappingNextBit + signalbase.NumberOfBits - 1;
                            while (true)
                            {
                                int iLittleEndianBase = iLittleEndian - iLittleEndian % 8;
                                if(iLittleEndian< iDeviceMappingNextBit)
                                {
                                    break;
                                }
                                if (!string.IsNullOrWhiteSpace(strDeviceMapping))
                                {
                                    strDeviceMapping += ",";
                                }
                                if (iLittleEndianBase == iLittleEndian)
                                {
                                    strDeviceMapping += string.Format("{0}", iLittleEndian);
                                }
                                else
                                {
                                    strDeviceMapping += string.Format("{0}-{1}", iLittleEndianBase>= iDeviceMappingNextBit? iLittleEndianBase: iDeviceMappingNextBit, iLittleEndian);
                                }
                                iLittleEndian = iLittleEndianBase - 1;
                            }
                            signalbase.DeviceMapping = strDeviceMapping;
                        }
                        else
                        {
                            signalbase.DeviceMapping = string.Format("{0}-{1}", iDeviceMappingNextBit, iDeviceMappingNextBit + signalbase.NumberOfBits - 1);
                        }
                        iDeviceMappingNextBit += signalbase.NumberOfBits;
                        break;
                    case SignalType.AI:
                        if (signalbase.AlignmentByte)
                        {
                            if (iDeviceMappingNextBit % 8 != 0)
                            {
                                iDeviceMappingNextBit += 8 - iDeviceMappingNextBit % 8;
                            }
                        }
                        signalbase.DeviceMapping = string.Format("{0}-{1}", iDeviceMappingNextBit, iDeviceMappingNextBit + signalbase.NumberOfBits - 1);
                        iDeviceMappingNextBit += signalbase.NumberOfBits;
                        break;
                    case SignalType.DO:
                        signalbase.DeviceMapping = oDeviceMappingNextBit.ToString();
                        oDeviceMappingNextBit++;
                        break;
                    case SignalType.GO:
                        if (signalbase.AlignmentByte)
                        {
                            if (oDeviceMappingNextBit % 8 != 0)
                            {
                                oDeviceMappingNextBit += 8 - oDeviceMappingNextBit % 8;
                            }
                        }
                        if (signalbase.LittleEndian)
                        {
                            string strDeviceMapping = "";
                            int oLittleEndian = oDeviceMappingNextBit + signalbase.NumberOfBits - 1;
                            while (true)
                            {
                                int oLittleEndianBase = oLittleEndian - oLittleEndian % 8;
                                if (oLittleEndian < oDeviceMappingNextBit)
                                {
                                    break;
                                }
                                if (!string.IsNullOrWhiteSpace(strDeviceMapping))
                                {
                                    strDeviceMapping += ",";
                                }
                                if (oLittleEndianBase == oLittleEndian)
                                {
                                    strDeviceMapping += string.Format("{0}", oLittleEndian);
                                }
                                else
                                {
                                    strDeviceMapping += string.Format("{0}-{1}", oLittleEndianBase >= oDeviceMappingNextBit ? oLittleEndianBase : oDeviceMappingNextBit, oLittleEndian);
                                }
                                oLittleEndian = oLittleEndianBase - 1;
                            }
                            signalbase.DeviceMapping = strDeviceMapping;
                        }
                        else
                        {
                            signalbase.DeviceMapping = string.Format("{0}-{1}", oDeviceMappingNextBit, oDeviceMappingNextBit + signalbase.NumberOfBits - 1);
                        }
                        oDeviceMappingNextBit  += signalbase.NumberOfBits;
                        break;
                    case SignalType.AO:
                        if (signalbase.AlignmentByte)
                        {
                            if (oDeviceMappingNextBit % 8 != 0)
                            {
                                oDeviceMappingNextBit += 8 - oDeviceMappingNextBit % 8;
                            }
                        }
                        signalbase.DeviceMapping = string.Format("{0}-{1}", oDeviceMappingNextBit, oDeviceMappingNextBit + signalbase.NumberOfBits - 1);
                        oDeviceMappingNextBit += signalbase.NumberOfBits;
                        break;
                }
            }
        }

        public virtual string GetDeviceCFG()
        {
            return null;
        }

        public void SaveSignalstoCFG()
        {
            bool hasSystemInput = false;
            bool hasSystemOutput = false;
            bool hasCrossConnection = false;
            Dictionary<string, SystemOutput> systemOutputsInThisDevice = new Dictionary<string, SystemOutput>();
            Dictionary<string, SystemInput> systemInputsInThisDevice = new Dictionary<string, SystemInput>();
            Dictionary<string, CrossConnection> crossConnectionsInThisDevice = new Dictionary<string, CrossConnection>();

            SaveFileDialog mySaveFileDialog = new SaveFileDialog();
            mySaveFileDialog.FileName ="EIO_" + this.Name + ".cfg";
            mySaveFileDialog.Filter = "EIO files (*.cfg)|*.cfg";
            mySaveFileDialog.RestoreDirectory = true;
            Nullable<bool> result = mySaveFileDialog.ShowDialog();
            if (result == false)
            {
                return;
            }
            FileStream fs = new FileStream(mySaveFileDialog.FileName, FileMode.Create);
            StreamWriter myStreamWriter = new StreamWriter(fs);
            myStreamWriter.Write("EIO:CFG_1.0::\n");

            if(this is DeviceNetDevice)
            {
                myStreamWriter.Write("#\nDEVICENET_DEVICE:\n");
            }
            else if(this is DeviceNetInternalDevice)
            {
                myStreamWriter.Write("#\nDEVICENET_INTERNAL_DEVICE:\n");
            }
            else if (this is ProfinetDevice)
            {
                myStreamWriter.Write("#\nPROFINET_DEVICE:\n");
            }
            else if (this is ProfinetInternalDevice)
            {
                myStreamWriter.Write("#\nPROFINET_INTERNAL_DEVICE:\n");
            }
            else if (this is EtherNetIPDevice)
            {
                myStreamWriter.Write("#\nETHERNETIP_DEVICE:\n");
            }
            else if (this is EtherNetIPInternalDevice)
            {
                myStreamWriter.Write("#\nPROFINET_INTERNAL_DEVICE:\n");
            }
            else if (this is LocalDevice)
            {
                myStreamWriter.Write("#\nLOCAL_DEVICE:\n");
            }
            myStreamWriter.WriteLine("");
            myStreamWriter.WriteLine(this.GetDeviceCFG());
                       
            myStreamWriter.Write("#\nEIO_SIGNAL:\n");

            List<Signal> signals = this.signals.Values.ToList();
            signals.Sort();

            foreach (Signal signalbase in signals)
            {                
                if (this.connectedtoIndustrialNetwork.FieldBus.SystemOutputs.Count(p => p.Value.SignalName == signalbase.Name)> 0)
                {
                    hasSystemOutput=true;
                    SystemOutput systemOutput = this.connectedtoIndustrialNetwork.FieldBus.SystemOutputs.First(p => p.Value.SignalName == signalbase.Name).Value;
                    systemOutputsInThisDevice.Add(systemOutput.ID, systemOutput);
                    //log.Debug("SystemOutput: " + systemOutput.SignalName);
                }
                if (this.connectedtoIndustrialNetwork.FieldBus.SystemInputs.Count(p => p.Value.SignalName == signalbase.Name) > 0)
                {
                    hasSystemInput = true;
                    SystemInput systemInput = this.connectedtoIndustrialNetwork.FieldBus.SystemInputs.First(p => p.Value.SignalName == signalbase.Name).Value;
                    systemInputsInThisDevice.Add(systemInput.ID, systemInput);
                    //log.Debug("SystemInput: " + systemInput.SignalName);
                }
                if (this.connectedtoIndustrialNetwork.FieldBus.CrossConnections.Count(p =>( p.Value.Resultant == signalbase.Name) || (p.Value.Actor1 == signalbase.Name) || (p.Value.Actor2 == signalbase.Name) || (p.Value.Actor3 == signalbase.Name) || (p.Value.Actor4 == signalbase.Name) || (p.Value.Actor5 == signalbase.Name)) > 0)
                {
                    hasCrossConnection = true;
                    CrossConnection crossConnection = this.connectedtoIndustrialNetwork.FieldBus.CrossConnections.First(p => (p.Value.Resultant == signalbase.Name) || (p.Value.Actor1 == signalbase.Name) || (p.Value.Actor2 == signalbase.Name) || (p.Value.Actor3 == signalbase.Name) || (p.Value.Actor4 == signalbase.Name) || (p.Value.Actor5 == signalbase.Name)).Value;
                    if (!crossConnectionsInThisDevice.ContainsKey(crossConnection.Name))
                    {
                        crossConnectionsInThisDevice.Add(crossConnection.Name, crossConnection);
                        log.Debug("CrossConnection: " + crossConnection.Resultant);
                    }
                }
                string strSignalLine= "     ";
                myStreamWriter.Write("\n");

                strSignalLine= WriteCfgLine(myStreamWriter, strSignalLine, "Name", signalbase.Name,signalbase.DefaultName);
                strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "SignalType", signalbase.SignalType.ToString(), signalbase.DefaultSignalType);
                strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "Device", signalbase.AssignedtoDevice.Name, signalbase.AssignedtoDevice.DefaultName);
                strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "DeviceMap", signalbase.DeviceMapping, signalbase.DefaultDeviceMapping);
                strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "Category", signalbase.Category, signalbase.DefaultCategory);
                strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "Label", signalbase.SignalIdentificationLabel, signalbase.DefaultSignalIdentificationLabel);
                strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "Access", signalbase.AccessLevel.Name, signalbase.DefaultAccessLevel);
                strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "SafeLevel", signalbase.SafeLevel, signalbase.DefaultSafeLevel);
                strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "Default", signalbase.DefaultValue, signalbase.DefaultDefaultValue);
                if(signalbase.AssignedtoDevice.name== "Virtual1" || signalbase.AssignedtoDevice.Simulated)
                {
                    strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "Size", signalbase.NumberOfBits, signalbase.DefaultNumberOfBits);
                }
                switch (signalbase.SignalType)
                {
                    case SignalType.DI:
                    case SignalType.DO:
                    case SignalType.GI:
                    case SignalType.GO:
                        strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "FiltPas", signalbase.FilterTimePassive, signalbase.DefaultFilterTimePassive);
                        strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "FiltAct", signalbase.FilterTimeActive, signalbase.DefaultFilterTimeActive);
                        strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "Invert", signalbase.InvertPhysicalValue, signalbase.DefaultInvertPhysicalValue);
                        break;
                    case SignalType.AI:
                    case SignalType.AO:
                        strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "EncType", signalbase.AnalogEncodingType, signalbase.DefaultAnalogEncodingType);
                        strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "MaxLog", signalbase.MaximumLogicalValue, signalbase.DefaultMaximumLogicalValue);
                        strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "MaxPhys", signalbase.MaximumPhysicalValue, signalbase.DefaultMaximumPhysicalValue);
                        strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "MaxPhysLimit", signalbase.MaximumPhysicalValueLimit, signalbase.DefaultMaximumPhysicalValueLimit);
                        strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "MaxBitVal", signalbase.MaximumBitValue, signalbase.DefaultMaximumBitValue);
                        strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "MinLog", signalbase.MinimumLogicalValue, signalbase.DefaultMinimumLogicalValue);
                        strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "MinPhys", signalbase.MinimumPhysicalValue, signalbase.DefaultMinimumPhysicalValue);
                        strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "MinPhysLimit", signalbase.MinimumPhysicalValueLimit, signalbase.DefaultMinimumPhysicalValueLimit);
                        strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "MinBitVal", signalbase.MinimumBitValue, signalbase.DefaultMinimumBitValue);
                        break;
                }

                if (!string.IsNullOrEmpty(strSignalLine))
                {
                    myStreamWriter.WriteLine(strSignalLine);
                }

            }

            //Write section SYSSIG_IN:
            if (hasSystemInput)
            {
                myStreamWriter.Write("#\nSYSSIG_IN:\n");
                foreach (SystemInput systemInput in systemInputsInThisDevice.Values)
                {
                    myStreamWriter.WriteLine("");
                    myStreamWriter.WriteLine(systemInput.GetSystemInputCFG());
                }
            }

            //Write section SYSSIG_OUT:
            if (hasSystemOutput)
            {
                myStreamWriter.Write("#\nSYSSIG_OUT:\n");
                foreach(SystemOutput systemOutput in systemOutputsInThisDevice.Values)
                {
                    myStreamWriter.WriteLine("");
                    myStreamWriter.WriteLine(systemOutput.GetSystemOutputCFG());
                }
            }

            //Write section EIO_CROSS:
            if (hasCrossConnection)
            {
                myStreamWriter.Write("#\nEIO_CROSS:\n");
                foreach (CrossConnection crossConnection in crossConnectionsInThisDevice.Values)
                {
                    myStreamWriter.WriteLine("");
                    myStreamWriter.WriteLine(crossConnection.GetCrossConnectionCFG());
                }
            }

            myStreamWriter.Close();
            fs.Close();
        }

        //static public string WriteCfgLine(StreamWriter myStreamWriter, string strSignalLine, string strSignalParameter)
        //{
        //    string strIndentation = "      ";
        //    if (strSignalLine.Length + strSignalParameter.Length < 80)
        //    {
        //        return strSignalLine + strSignalParameter;
        //    }
        //    else
        //    {
        //        myStreamWriter.WriteLine(strSignalLine + "\\");
        //        return strIndentation + strSignalParameter;
        //    }
        //}

        static public string WriteCfgLine(StreamWriter myStreamWriter, string strSignalLine, string strSignalParameter, string strSignalParameterValue, string strDefaultSignalParameterValue)
        {
            if (string.IsNullOrWhiteSpace(strSignalParameterValue) || strDefaultSignalParameterValue== strSignalParameterValue)
            {
                return strSignalLine;
            }
            string strIndentation = "     ";
            strSignalParameter = string.Format(" -{0} \"{1}\"", strSignalParameter,strSignalParameterValue);
            if (strSignalLine.Length + strSignalParameter.Length < 80)
            {
                return strSignalLine + strSignalParameter;
            }
            else
            {
                myStreamWriter.WriteLine(strSignalLine + "\\");
                return strIndentation + strSignalParameter;
            }
        }
        static public string WriteCfgLine(StreamWriter myStreamWriter, string strSignalLine, string strSignalParameter, bool boolSignalParameterValue, bool boolDefaultSignalParameterValue)
        {
            if (boolSignalParameterValue== boolDefaultSignalParameterValue)
            {
                return strSignalLine;
            }
            string strIndentation = "     ";
            strSignalParameter = string.Format(" -{0}", strSignalParameter);
            if (strSignalLine.Length + strSignalParameter.Length < 80)
            {
                return strSignalLine + strSignalParameter;
            }
            else
            {
                myStreamWriter.WriteLine(strSignalLine + "\\");
                return strIndentation + strSignalParameter;
            }
        }
        static public string WriteCfgLine(StreamWriter myStreamWriter, string strSignalLine, string strSignalParameter, float floatSignalParameterValue, float floatDefaultSignalParameterValue)
        {
            if ( floatSignalParameterValue == floatDefaultSignalParameterValue)
            {
                return strSignalLine;
            }
            string strIndentation = "     ";
            strSignalParameter = string.Format(" -{0} {1}", strSignalParameter, floatSignalParameterValue);
            if (strSignalLine.Length + strSignalParameter.Length < 80)
            {
                return strSignalLine + strSignalParameter;
            }
            else
            {
                myStreamWriter.WriteLine(strSignalLine + "\\");
                return strIndentation + strSignalParameter;
            }
        }
        static public string WriteCfgLine(StreamWriter myStreamWriter, string strSignalLine, string strSignalParameter, int intSignalParameterValue, int intDefaultSignalParameterValue)
        {
            if (intSignalParameterValue == intDefaultSignalParameterValue)
            {
                return strSignalLine;
            }
            string strIndentation = "     ";
            strSignalParameter = string.Format(" -{0} {1}", strSignalParameter, intSignalParameterValue);
            if (strSignalLine.Length + strSignalParameter.Length < 80)
            {
                return strSignalLine + strSignalParameter;
            }
            else
            {
                myStreamWriter.WriteLine(strSignalLine + "\\");
                return strIndentation + strSignalParameter;
            }
        }

        static public void FillCfgLines(List<string> strPreLines, string strParameter, int intParameterValue, int intDefaultParameterValue)
        {
            if (intParameterValue == intDefaultParameterValue)
            {
                return;
            }
            string strIndentation = "     ";
            strParameter = string.Format(" -{0} {1}", strParameter, intParameterValue);
            if (strPreLines[strPreLines.Count - 1].Length + strParameter.Length < 80)
            {
                strPreLines[strPreLines.Count - 1] = strPreLines[strPreLines.Count - 1] + strParameter;
            }
            else
            {
                strPreLines[strPreLines.Count - 1] = strPreLines[strPreLines.Count - 1] + "\\\n";
                strPreLines.Add(strIndentation + strParameter);
            }
        }
        static public void FillCfgLines(List<string> strPreLines, string strParameter, float floatParameterValue, float floatDefaultParameterValue)
        {
            if (floatParameterValue == floatDefaultParameterValue)
            {
                return;
            }
            string strIndentation = "     ";
            strParameter = string.Format(" -{0} {1}", strParameter, floatParameterValue);
            if (strPreLines[strPreLines.Count - 1].Length + strParameter.Length < 80)
            {
                strPreLines[strPreLines.Count - 1] = strPreLines[strPreLines.Count - 1] + strParameter;
            }
            else
            {
                strPreLines[strPreLines.Count - 1] = strPreLines[strPreLines.Count - 1] + "\\\n";
                strPreLines.Add(strIndentation + strParameter);
            }
        }

        static public void FillCfgLines(List<string> strPreLines, string strParameter, string strParameterValue, string strDefaultParameterValue)
        {
            if (strParameterValue == strDefaultParameterValue)
            {
                return;
            }
            string strIndentation = "     ";
            strParameter = string.Format(" -{0} \"{1}\"", strParameter, strParameterValue);
            if (strPreLines[strPreLines.Count - 1].Length + strParameter.Length < 80)
            {
                strPreLines[strPreLines.Count - 1] = strPreLines[strPreLines.Count - 1] + strParameter;
            }
            else
            {
                strPreLines[strPreLines.Count - 1] = strPreLines[strPreLines.Count - 1] + "\\\n";
                strPreLines.Add(strIndentation + strParameter);
            }
        }

        static public void FillCfgLines(List<string> strPreLines, string strParameter, bool boolParameterValue, bool boolDefaultParameterValue)
        {
            if (boolParameterValue == boolDefaultParameterValue)
            {
                return;
            }
            string strIndentation = "     ";
            strParameter = string.Format(" -{0}", strParameter);
            if (strPreLines[strPreLines.Count - 1].Length + strParameter.Length < 80)
            {
                strPreLines[strPreLines.Count - 1] = strPreLines[strPreLines.Count - 1] + strParameter;
            }
            else
            {
                strPreLines[strPreLines.Count - 1] = strPreLines[strPreLines.Count - 1] + "\\\n";
                strPreLines.Add(strIndentation + strParameter);
            }
        }
    }
}
