using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABB.Robotics.Controllers.ConfigurationDomain;
using log4net;

namespace SignalAutoConfigurationTool.EIO
{
    /// <summary>
    /// Cfgname:EIO_CROSS
    /// </summary>
    public class CrossConnection
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CrossConnection));

        private string name;
        /// <summary>
        /// Cfgname:Name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string resultant;
        /// <summary>
        /// Cfgname:Res
        /// </summary>
        public string Resultant
        {
            get { return resultant; }
            set { resultant = value; }
        }
        
        private string actor1;
        /// <summary>
        /// Cfgname:Act1
        /// </summary>
        public string Actor1
        {
            get { return actor1; }
            set { actor1 = value; }
        }
        
        private bool invertActor1;
        /// <summary>
        /// Cfgname:Act1_invert
        /// </summary>
        public bool InvertActor1
        {
            get { return invertActor1; }
            set { invertActor1 = value; }
        }
        
        private string operator1;
        /// <summary>
        /// Cfgname:Oper1 (AND, OR)
        /// </summary>
        public string Operator1
        {
            get { return operator1; }
            set { operator1 = value; }
        }
        
        private string actor2;
        /// <summary>
        /// Cfgname:Act2
        /// </summary>
        public string Actor2
        {
            get { return actor2; }
            set { actor2 = value; }
        }

        private bool invertActor2;
        /// <summary>
        /// Cfgname:Act2_invert
        /// </summary>
        public bool InvertActor2
        {
            get { return invertActor2; }
            set { invertActor2 = value; }
        }

        private string operator2;
        /// <summary>
        /// Cfgname:Oper2 (AND, OR)
        /// </summary>
        public string Operator2
        {
            get { return operator2; }
            set { operator2 = value; }
        }

        private string actor3;
        /// <summary>
        /// Cfgname:Act3
        /// </summary>
        public string Actor3
        {
            get { return actor3; }
            set { actor3 = value; }
        }

        private bool invertActor3;
        /// <summary>
        /// Cfgname:Act3_invert
        /// </summary>
        public bool InvertActor3
        {
            get { return invertActor3; }
            set { invertActor3 = value; }
        }

        private string operator3;
        /// <summary>
        /// Cfgname:Oper3 (AND, OR)
        /// </summary>
        public string Operator3
        {
            get { return operator3; }
            set { operator3 = value; }
        }
        
        private string actor4;
        /// <summary>
        /// Cfgname:Act4
        /// </summary>
        public string Actor4
        {
            get { return actor4; }
            set { actor4 = value; }
        }

        private bool invertActor4;
        /// <summary>
        /// Cfgname:Act4_invert
        /// </summary>
        public bool InvertActor4
        {
            get { return invertActor4; }
            set { invertActor4 = value; }
        }

        private string operator4;
        /// <summary>
        /// Cfgname:Oper4 (AND, OR)
        /// </summary>
        public string Operator4
        {
            get { return operator4; }
            set { operator4 = value; }
        }

        private string actor5;
        /// <summary>
        /// Cfgname:Act5
        /// </summary>
        public string Actor5
        {
            get { return actor5; }
            set { actor5 = value; }
        }

        private bool invertActor5;
        /// <summary>
        /// Cfgname:Act5_invert
        /// </summary>
        public bool InvertActor5
        {
            get { return invertActor5; }
            set { invertActor5 = value; }
        }

        private FieldBus fieldBus;

        public FieldBus FieldBus
        {
            get { return fieldBus; }
        }

        public CrossConnection(Instance instanceSystemOutput, FieldBus fieldBus)
        {
            this.fieldBus = fieldBus;
            this.Name = (string)instanceSystemOutput.GetAttribute("Name");
            this.Resultant = (string)instanceSystemOutput.GetAttribute("Res");
            this.Actor1 = instanceSystemOutput.GetAttribute("Act1").ToString();
            this.InvertActor1 =(bool)instanceSystemOutput.GetAttribute("Act1_invert");
            this.Operator1 = instanceSystemOutput.GetAttribute("Oper1").ToString();
            if (!string.IsNullOrWhiteSpace(this.Operator1))
            {
                this.Actor2 = instanceSystemOutput.GetAttribute("Act2").ToString();
                this.InvertActor2 = (bool)instanceSystemOutput.GetAttribute("Act2_invert");
                this.Operator2 = instanceSystemOutput.GetAttribute("Oper2").ToString();
                if (!string.IsNullOrWhiteSpace(this.Operator2))
                {
                    this.Actor3 = instanceSystemOutput.GetAttribute("Act3").ToString();
                    this.InvertActor3 = (bool)instanceSystemOutput.GetAttribute("Act3_invert");
                    this.Operator3 = instanceSystemOutput.GetAttribute("Oper3").ToString();
                }
                if (!string.IsNullOrWhiteSpace(this.Operator3))
                {
                    this.Actor4 = instanceSystemOutput.GetAttribute("Act4").ToString();
                    this.InvertActor4 = (bool)instanceSystemOutput.GetAttribute("Act4_invert");
                    this.Operator4 = instanceSystemOutput.GetAttribute("Oper4").ToString();
                    if (!string.IsNullOrWhiteSpace(this.Operator4))
                    {
                        this.Actor5 = instanceSystemOutput.GetAttribute("Act5").ToString();
                        this.InvertActor5 = (bool)instanceSystemOutput.GetAttribute("Act5_invert");
                    }
                }
            }
            //log.Debug(this.Name);
        }

        public string GetCrossConnectionCFG()
        {
            List<string> strPreLines = new List<string>
            {
                string.Format("      -Name \"{0}\"", this.Name),
                string.Format(" -Res \"{0}\"", this.Resultant)
            };

            strPreLines.Add(string.Format(" -Act1 \"{0}\"", this.Actor1));
            if (this.invertActor1)
            {
                strPreLines.Add(" -Act1_invert");
            }
            if (!string.IsNullOrWhiteSpace(this.Operator1))
            {
                strPreLines.Add("\\\n");
                strPreLines.Add(string.Format("      -Oper1 \"{0}\"", this.Operator1));
                strPreLines.Add(string.Format(" -Act2 \"{0}\"", this.Actor2));
                if (this.invertActor2)
                {
                    strPreLines.Add(" -Act2_invert");
                }
                if (!string.IsNullOrWhiteSpace(this.Operator2))
                {
                    strPreLines.Add("\\\n");
                    strPreLines.Add(string.Format("      -Oper2 \"{0}\"", this.Operator2));
                    strPreLines.Add(string.Format(" -Act3 \"{0}\"", this.Actor3));
                    if (this.invertActor3)
                    {
                        strPreLines.Add(" -Act3_invert");
                    }
                }
                if (!string.IsNullOrWhiteSpace(this.Operator3))
                {
                    strPreLines.Add("\\\n");
                    strPreLines.Add(string.Format("      -Oper3 \"{0}\"", this.Operator3));
                    strPreLines.Add(string.Format(" -Act4 \"{0}\"", this.Actor4));
                    if (this.invertActor4)
                    {
                        strPreLines.Add(" -Act4_invert");
                    }
                }
                if (!string.IsNullOrWhiteSpace(this.Operator4))
                {
                    strPreLines.Add("\\\n");
                    strPreLines.Add(string.Format("      -Oper4 \"{0}\"", this.Operator4));
                    strPreLines.Add(string.Format(" -Act5 \"{0}\"", this.Actor5));
                    if (this.invertActor5)
                    {
                        strPreLines.Add(" -Act5_invert");
                    }
                }
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
