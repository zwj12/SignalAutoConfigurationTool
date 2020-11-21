using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.RapidDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNAUSRobotics.Symbol
{
    class SpeedData : IRapidData, IDisposable
    {
        #region Fields      

        /// <summary>
        /// UserDefined (struct type) is a value type
        /// </summary>
        private UserDefined data;

        private RapidData rapidData;

        public double Tcp
        {
            get
            {
                double res = Convert.ToDouble(data.Components[0].ToString());
                return res;
            }
            set
            {
                data.Components[0] = new Num(value);
            }
        }

        public double Ori
        {
            get
            {
                double res = Convert.ToDouble(data.Components[1].ToString());
                return res;
            }
            set
            {
                data.Components[1] = new Num(value);
            }
        }

        public double Leax
        {
            get
            {
                double res = Convert.ToDouble(data.Components[2].ToString());
                return res;
            }
            set
            {
                data.Components[2] = new Num(value);
            }
        }

        public double Reax
        {
            get
            {
                double res = Convert.ToDouble(data.Components[3].ToString());
                return res;
            }
            set
            {
                data.Components[3] = new Num(value);
            }
        }

        #endregion

        public SpeedData(RapidDataType rdt)
        {
            this.data = new UserDefined(rdt);
            this.data.FillFromString2("[0,0,0,0]");
        }

        public SpeedData(RapidDataType rdt, DataNode root)
        {
            this.data = new UserDefined(rdt);
            this.data.Fill2(root);
        }
        
        public SpeedData(RapidDataType rdt, string newValue)
        {
            this.data = new UserDefined(rdt);
            this.data.FillFromString2(newValue);
        }

        public SpeedData(Controller controller, string taskName, string dataModuleName, string dataName)
        {
            this.rapidData = controller.Rapid.GetRapidData(taskName, dataModuleName, dataName);
            this.data = (UserDefined)rapidData.Value;
        }

        #region Dispose

        private bool _disposed;

        ~SpeedData()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
            }
            _disposed = true;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        public void Fill(DataNode root)
        {
            this.data.Fill2(root);
        }

        [Obsolete("Use FillFromString() instead.")]
        public void Fill(string value)
        {
            throw new NotImplementedException();
        }

        public void FillFromString(string newValue)
        {
            this.data.FillFromString2(newValue);
        }

        public DataNode ToStructure()
        {
            return data.ToStructure();
        }

        public override string ToString()
        {
            return data.ToString();
        }

        public void Update()
        {
            if (this.rapidData != null)
            {
                this.rapidData.Value = this.data;
            }            
        }
    }
}
