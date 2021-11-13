using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.FileSystemDomain;
using ABB.Robotics.Controllers.Hosting;
using ABB.Robotics.Controllers.MotionDomain;
using ABB.Robotics.Controllers.RapidDomain;
using CNAUSRobotics.Symbol;

namespace CNAUSRobotics
{
    public class Class_Controller:IDisposable
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private NetworkScanner networkScanner = new NetworkScanner();
        public Controller controller = null;
        public Mastership mastership = null;


        #region Dispose

        private bool _disposed;

        ~Class_Controller()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            logger.Debug("Dispose");
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                logger.Debug("disposing");
                if (this.mastership != null)
                {
                    this.mastership.Release();
                    this.mastership = null;
                }
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
                    controller = Controller.Connect(myControllerInfo, ConnectionType.Standalone);
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

        public void RequestWriteAccess()
        {
            if(this.controller is null)
            {
                return;
            }
            try
            {
                this.mastership = Mastership.Request(controller);
                MessageBox.Show("Wait for the operator to grant your request on the FlexPendant, or press OK to cancel the request operation.", "SignalAutoConfigurationTool", MessageBoxButton.OK);
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBox.Show("Mastership is held by another client." + ex.Message);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Unexpected error occurred: " + ex.Message);
            }

        }

        public void ReleaseWriteAccess()
        {
            if (this.mastership != null)
            {
                this.mastership.Release();
            }
        }

        public void TestSpeedData()
        {
            try
            {
                SpeedData speedData = new SpeedData(this.controller, "T_ROB1", "CalibDataModule", "speedAir");
                speedData.Tcp += 1;
                speedData.Ori += 2;
                speedData.Leax += 3;
                speedData.Reax += 4;
                speedData.Update();
                throw new Exception();
            }
            catch
            {
            }
            finally
            {
                if (this.mastership != null)
                {
                    this.mastership.Release();
                }
            }

        }

        public void TestSpeedDataArray()
        {
            RapidData rapidData = this.controller.Rapid.GetRapidData("T_ROB1", "MainModule", "speed1");
            RapidDataType rapidDataType = controller.Rapid.GetRapidDataType("T_ROB1", "MainModule", "speed1");
            if (rapidData.IsArray)
            { 
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                ArrayData arrayData = (ArrayData)rapidData.Value;
                arrayData.Mode = ArrayModes.Snapshot;
                for (int i = 0; i < arrayData.Length; i++)
                {
                    SpeedData speedData = new SpeedData(rapidDataType, arrayData[i].ToStructure());
                    //SpeedData speedData =new SpeedData(rapidDataType,rapidData.ReadItem(i).ToStructure());
                    speedData.Tcp += i;
                    rapidData.WriteItem(speedData, i);
                }

                stopwatch.Stop();
                logger.Debug(stopwatch.ElapsedMilliseconds.ToString());
            }
        }

        public void TestFileSystem()
        {
            FileSystem fileSystem = this.controller.FileSystem;
            string remoteDir = fileSystem.RemoteDirectory;
            string localDir = fileSystem.LocalDirectory;
            logger.Debug("remoteDir=" + remoteDir); 
            logger.Debug("localDir=" + localDir);
            //fileSystem.GetFile("wg.csv", "sd.csv");
            //fileSystem.PutFile("sd.csv", "sd.csv");
            //fileSystem.CopyFile("sd.csv", "sd1.csv", true);
            //fileSystem.CopyDirectory("Logging", "Logging1", true);
            ControllerFileSystemInfo[] anArray;
            ControllerFileSystemInfo info;
            anArray = fileSystem.GetFilesAndDirectories("*");
            for (int i = 0; i < anArray.Length; i++)
            {
                info = anArray[i];
                logger.Debug(info.FullName);
            }

            string yamlPath = fileSystem.RemoteDirectory + "\\" + "yaml";
            if (fileSystem.FileExists(yamlPath))
            {
                // file exists
            }
        }

        public void TestGetPosition()
        {
            MechanicalUnit mechanicalUnit= this.controller.MotionSystem.ActiveMechanicalUnit;
            logger.Debug(mechanicalUnit.GetPosition());
            logger.Debug(mechanicalUnit.GetPosition(CoordinateSystemType.WorkObject));
            foreach (MechanicalUnit item in this.controller.MotionSystem.MechanicalUnits)
            {
                logger.Debug(item.Task.Name + ":" + item.GetPosition());
                logger.Debug(item.Task.Name + ":" + item.GetPosition(CoordinateSystemType.WorkObject));
            }

        }
    }
}
