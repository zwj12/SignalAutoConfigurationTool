using ABB.Robotics.Controllers.ConfigurationDomain;
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

        private string vendorName;
        /// <summary>
        /// Cfgname:VendorName
        /// </summary>
        public string VendorName
        {
            get { return vendorName; }
            set { vendorName = value; }
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

        private Dictionary<string, SignalBase> signals = new Dictionary<string, EIO.SignalBase>();

        public Dictionary<string, SignalBase> Signals
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
            List<SignalBase> signals = this.signals.Values.ToList();
            signals.Sort();
            int indexInput = 0;
            int indexOutput = 0;
            foreach (SignalBase signalbase in signals)
            {
                if(signalbase.SignalType== SignalType.DI || signalbase.SignalType == SignalType.GI ||signalbase.SignalType == SignalType.AI)
                {
                    signalbase.Index = indexInput++;
                }
                else
                {
                    signalbase.Index = indexOutput++;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReArrangeSignalDeviceMappingbyIndex()
        {
            List<SignalBase> signals = this.signals.Values.ToList();
            signals.Sort((x, y) => x.Index - y.Index);
            int iDeviceMappingNextBit = 0;
            int oDeviceMappingNextBit = 0;
            foreach (SignalBase signalbase in signals)
            {
                switch (signalbase.SignalType)
                {
                    case SignalType.DI:
                        signalbase.DeviceMapping = iDeviceMappingNextBit.ToString();
                        iDeviceMappingNextBit++;
                        break;
                    case SignalType.GI:
                        if(iDeviceMappingNextBit % 8 != 0)
                        {
                            iDeviceMappingNextBit += 8 - iDeviceMappingNextBit % 8;
                        }
                        signalbase.DeviceMapping =string.Format("{0}-{1}", iDeviceMappingNextBit, iDeviceMappingNextBit+ signalbase.NumberOfBits-1);
                        iDeviceMappingNextBit+= signalbase.NumberOfBits;
                        break;
                    case SignalType.AI:
                        if (iDeviceMappingNextBit % 8 != 0)
                        {
                            iDeviceMappingNextBit += 8 - iDeviceMappingNextBit % 8;
                        }
                        signalbase.DeviceMapping = string.Format("{0}-{1}", iDeviceMappingNextBit, iDeviceMappingNextBit + signalbase.NumberOfBits - 1);
                        iDeviceMappingNextBit += signalbase.NumberOfBits;
                        break;
                    case SignalType.DO:
                        signalbase.DeviceMapping = oDeviceMappingNextBit.ToString();
                        oDeviceMappingNextBit++;
                        break;
                    case SignalType.GO:
                        if (oDeviceMappingNextBit % 8 != 0)
                        {
                            oDeviceMappingNextBit += 8 - oDeviceMappingNextBit % 8;
                        }
                        signalbase.DeviceMapping = string.Format("{0}-{1}", oDeviceMappingNextBit, oDeviceMappingNextBit + signalbase.NumberOfBits - 1);
                        oDeviceMappingNextBit  += signalbase.NumberOfBits;
                        break;
                    case SignalType.AO:
                        if (oDeviceMappingNextBit % 8 != 0)
                        {
                            oDeviceMappingNextBit += 8 - oDeviceMappingNextBit % 8;
                        }
                        signalbase.DeviceMapping = string.Format("{0}-{1}", oDeviceMappingNextBit, oDeviceMappingNextBit + signalbase.NumberOfBits - 1);
                        oDeviceMappingNextBit += signalbase.NumberOfBits;
                        break;
                }
            }
        }

        public void SaveSignalstoCFG()
        {
            SaveFileDialog mySaveFileDialog = new SaveFileDialog();
            mySaveFileDialog.FileName = this.Name + ".cfg";
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

            myStreamWriter.Write("#\nEIO_SIGNAL:\n");

            string strSignalLine= "     ";
            foreach (SignalBase signalbase in this.Signals.Values)
            {
                myStreamWriter.Write("\n");

                strSignalLine= WriteCfgLine(myStreamWriter, strSignalLine, "Name", signalbase.Name,signalbase.DefaultName);
                strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "SignalType", signalbase.SignalType.ToString(), signalbase.DefaultSignalType);
                strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "Device", signalbase.AssignedtoDevice.Name, signalbase.AssignedtoDevice.DefaultName);
                strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "DeviceMap", signalbase.DeviceMapping, signalbase.DefaultDeviceMapping);
                strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "Category", signalbase.Category, signalbase.DefaultCategory);
                strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "Label", signalbase.SignalIdentificationLabel, signalbase.DefaultSignalIdentificationLabel);
                strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "Access", signalbase.AccessLevel, signalbase.DefaultAccessLevel);
                strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "SafeLevel", signalbase.SafeLevel, signalbase.DefaultSafeLevel);
                switch (signalbase.SignalType)
                {
                    case SignalType.DI:
                        SignalDigitalInput signalDigitalInput = (SignalDigitalInput)signalbase;
                        strSignalLine = WriteCfgLine(myStreamWriter, strSignalLine, "Default", signalDigitalInput.DefaultValue,0);

                        break;
                }

                if (!string.IsNullOrEmpty(strSignalLine))
                {
                    myStreamWriter.WriteLine(strSignalLine);
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
    }
}
