using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABB.Robotics.Controllers.ConfigurationDomain;

namespace SignalAutoConfigurationTool.EIO
{
    /// <summary>
    /// Cfgname:SYSSIG_IN
    /// </summary>
    public class SystemInput
    {
        //private readonly string id;
        /// <summary>
        /// Cfgname:Status & Signal
        /// </summary>
        public string ID
        {
            get { return this.action + this.signalName; }
        }

        private string action;
        /// <summary>
        /// Cfgname:Action
        /// </summary>
        public string Action
        {
            get { return action; }
            set { action = value; }
        }

        private string signalName;
        /// <summary>
        /// Cfgname:Signal
        /// </summary>
        public string SignalName
        {
            get { return signalName; }
            set { signalName = value; }
        }

        private string arg1;
        /// <summary>
        /// Cfgname:Arg1
        /// </summary>
        public string Arg1
        {
            get { return arg1; }
            set { arg1 = value; }
        }

        private string arg2;
        /// <summary>
        /// Cfgname:Arg2
        /// </summary>
        public string Arg2
        {
            get { return arg2; }
            set { arg2 = value; }
        }

        private string arg3;
        /// <summary>
        /// Cfgname:Arg3
        /// </summary>
        public string Arg3
        {
            get { return arg3; }
            set { arg3 = value; }
        }

        private string arg4;
        /// <summary>
        /// Cfgname:Arg4
        /// </summary>
        public string Arg4
        {
            get { return arg4; }
            set { arg4 = value; }
        }

        private string arg5;
        /// <summary>
        /// Cfgname:Arg5
        /// </summary>
        public string Arg5
        {
            get { return arg5; }
            set { arg5 = value; }
        }

        private string arg6;
        /// <summary>
        /// Cfgname:Arg6
        /// </summary>
        public string Arg6
        {
            get { return arg6; }
            set { arg6 = value; }
        }

        private string taskName;
        /// <summary>
        /// Cfgname:Arg7
        /// </summary>
        public string TaskName
        {
            get { return taskName; }
            set { taskName = value; }
        }

        private string arg8;
        /// <summary>
        /// Cfgname:Arg7
        /// </summary>
        public string Arg8
        {
            get { return arg8; }
            set { arg8 = value; }
        }

        private FieldBus fieldBus;

        public FieldBus FieldBus
        {
            get { return fieldBus; }
        }

        public SystemInput(Instance instanceSystemOutput, FieldBus fieldBus)
        {
            this.fieldBus = fieldBus;
            this.Action = (string)instanceSystemOutput.GetAttribute("Action");
            this.SignalName = (string)instanceSystemOutput.GetAttribute("Signal");
            this.Arg1 = instanceSystemOutput.GetAttribute("Arg1").ToString();
            this.Arg2 = instanceSystemOutput.GetAttribute("Arg2").ToString();
            this.Arg3 = instanceSystemOutput.GetAttribute("Arg3").ToString();
            this.Arg4 = instanceSystemOutput.GetAttribute("Arg4").ToString();
            this.Arg5 = instanceSystemOutput.GetAttribute("Arg5").ToString();
            this.Arg6 = instanceSystemOutput.GetAttribute("Arg6").ToString();
            this.TaskName = instanceSystemOutput.GetAttribute("Arg7").ToString();
            //this.Arg8 = instanceSystemOutput.GetAttribute("Arg8").ToString();
        }

        public string GetSystemInputCFG()
        {
            List<string> strPreLines = new List<string>
            {
                string.Format("      -Action \"{0}\"", this.Action),
                string.Format(" -Signal \"{0}\"", this.SignalName)
            };
            switch (this.Action)
            {
                case "Start":
                    strPreLines.Add(string.Format(" -Arg1 \"{0}\"", this.Arg1));
                    break;
                case "MotOnStart":
                    strPreLines.Add(string.Format(" -Arg1 \"{0}\"", this.Arg1));
                    break;
            }

            StringBuilder strBuilder = new StringBuilder();
            foreach (string str in strPreLines)
            {
                strBuilder.Append(str);
            }
            return strBuilder.ToString();
        }
    }
}
