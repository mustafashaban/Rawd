using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for ServerWindow.xaml
    /// </summary>
    public partial class ServerWindow : Window
    {
        string tempConnectionString = "";
        
        public ServerWindow()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string err;
            if (string.IsNullOrEmpty(tempConnectionString) || !BaseDataBase.CheckConnection(tempConnectionString, out err))
            {
                MyMessageBox.Show("خطأ في الاتصال يرجى فحص الاتصال أولاً");
                return;
            }

            Properties.Settings.Default.ConnectionString = tempConnectionString;
            Properties.Settings.Default.Save();
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnCheckConnection_Click(object sender, RoutedEventArgs e)
        {
            if (!chkIsLocalServer.IsChecked.Value)
            {
                if (txtPassword.Password == "" || txtPort.Text == "" || txtServerName.Text == "" || txtUserName.Text == "")
                {
                    MyMessageBox.Show("يجب ادخال كامل الحقول");
                    return;
                }
                tempConnectionString = string.Format("server=tcp:{0}\\SQLEXPRESS,{1};User ID={2};Password={3};", txtServerName.Text, txtPort.Text, txtUserName.Text, txtPassword.Password);
            }
            else
                tempConnectionString = @"Data Source=(local)\SQLEXPRESS;Integrated Security=True";

            string err;
            if (!BaseDataBase.CheckConnection(tempConnectionString, out err))
            {
                MyMessageBox.Show("خطأ في الاتصال\n" + err);
            }
            else MyMessageBox.Show("تم الاتصال بالمخدم بنجاح");
        }
    }
}
