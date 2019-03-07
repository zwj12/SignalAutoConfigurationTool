using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.RapidDomain;

namespace CNAUSRobotics
{
    public class Class_Controller
    {
        private NetworkScanner networkScanner = new NetworkScanner();
        public Controller controller = null;

        public Controller ConnectbyGuid(string strGuid)
        {
            try
            {
                if (controller != null)
                {
                    controller.Logoff();
                    controller.Dispose();
                    controller = null;
                }
                ControllerInfo myControllerInfo = null;
                Guid myGuid = Guid.Empty;
                if (Guid.TryParse(strGuid, out myGuid))
                {
                    networkScanner.TryFind(myGuid, out myControllerInfo);
                }
                else
                {
                    throw new Exception("Invalid guid string");
                }
                if (myControllerInfo == null || myControllerInfo.Availability != Availability.Available)
                {
                    throw new Exception("Wrong guid string");
                }
                else
                {
                    controller = ControllerFactory.CreateFrom(myControllerInfo);
                    return controller;
                }
            }
            finally { }

        }

        public void Login( string userName, string password)
        {
            try
            {
                UserInfo userInfo_login = UserInfo.DefaultUser;
                if (!string.IsNullOrEmpty(userName))
                {
                    userInfo_login = new UserInfo(userName);
                    if (!string.IsNullOrEmpty(password))
                    {
                        userInfo_login.Password = Class_Encrypt.DecryptDES(password);
                    }
                }
                controller.Logon(userInfo_login);
            }
            finally
            {

            }
        }
    }
}
