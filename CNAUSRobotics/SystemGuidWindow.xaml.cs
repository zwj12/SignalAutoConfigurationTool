using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CNAUSRobotics
{
    /// <summary>
    /// Interaction logic for SystemGuid.xaml
    /// </summary>
    public partial class SystemGuidWindow : Window
    {
        public string systemGuid;
        public string userName;
        public string password;
        public Controller myController;

        public SystemGuidWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.textBox_systemGuid.Text = systemGuid;
            this.textBox_userName.Text = userName;
            if (!string.IsNullOrEmpty(password))
            {
                this.passwordBox_password.Password = Class_Encrypt.DecryptDES(password);
            }
        }

        private void button_systemGuid_Click(object sender, RoutedEventArgs e)
        {
            Match m = Regex.Match(this.textBox_systemGuid.Text, @"^[0-9a-f]{8}(-[0-9a-f]{4}){3}-[0-9a-f]{12}$", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                NetworkScanner scanner = new NetworkScanner();
                scanner.Scan();
                ControllerInfoCollection controllers = scanner.Controllers;
                foreach (ControllerInfo controllerinfo in controllers)
                {
                    if (controllerinfo.SystemId.ToString().ToUpper() == this.textBox_systemGuid.Text.ToUpper())
                    {
                        try
                        {
                            if (this.myController != null)
                            {
                                this.myController.Logoff();
                                this.myController.Dispose();
                                this.myController = null;
                            }
                            this.myController = new Controller(new Guid(this.textBox_systemGuid.Text));
                            if (string.IsNullOrEmpty(this.textBox_userName.Text))
                            {
                                this.myController.Logon(UserInfo.DefaultUser);
                            }
                            else
                            {
                                UserInfo userInfo_login = new UserInfo(this.textBox_userName.Text);
                                userInfo_login.Password = this.passwordBox_password.Password;
                                this.myController.Logon(userInfo_login);
                            }

                            systemGuid = this.textBox_systemGuid.Text;
                            userName = this.textBox_userName.Text;
                            password = Class_Encrypt.EncryptDES(this.passwordBox_password.Password);

                            this.DialogResult = true;
                            return;
                        }catch(Exception exception)
                        {
                            MessageBox.Show(exception.Message);
                            return;
                        }
                    }
                }
                MessageBox.Show("This system guid is not valid or the controller is not connected, please check it angin.");
            }
            else
            {
                MessageBox.Show("This system guid's format is not valid, please check it angin.");
            }
        }
    }
}
