using Microsoft.Win32;
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
    /// Interaction logic for UsersControl.xaml
    /// </summary>
    public partial class SignInUser : Window
    {
        int tryCount = 1;
        bool ApplicationFirstRun = true;
        bool CanClose = false;
        public bool IsAccepted { get; set; }
        public static bool IsActivated { get; set; }
        public SignInUser(bool ApplicationFirstRun)
        {
            InitializeComponent();
            SignInUser.IsActivated = true;
            IsAccepted = false;
            this.ApplicationFirstRun = ApplicationFirstRun;
            txtName.Focus();
            if (!ApplicationFirstRun)
            {
                txtName.IsEnabled = false;
                txtName.Text = BaseDataBase.CurrentUser.Name;
            }
        }

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtName.Text) && !string.IsNullOrEmpty(txtPassword.Password))
            {
                var u = User.GetUser(txtName.Text, txtPassword.Password);
                if (u != null)
                {
                    BaseDataBase.CurrentUser = u;
                    CanClose = true;
                    Accepted();
                    return;
                }
            }
            else MyMessageBox.Show("يجب ادخال كلمة المرور واسمة المستخدم");
            if (tryCount++ == 3)
            {
                CanClose = true;
                Environment.Exit(Environment.ExitCode);
            }
        }

        private void Accepted()
        {
            if (ApplicationFirstRun)
            {
                MainWindow w = new MainWindow();
                App.Current.MainWindow = w;
                w.Show();
            }
            IsAccepted = true;
            SignInUser.IsActivated = false;
            this.Close();
        }

        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnExecute_Click(null, null);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            CanClose = true;
            Environment.Exit(Environment.ExitCode);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Alt && e.SystemKey == Key.Space)
            {
                e.Handled = true;
            }
            else
            {
                base.OnKeyDown(e);
            }
        }

        private void Window_Closed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!CanClose)
                e.Cancel = true;
        }
    }
}
