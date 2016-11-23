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
    public partial class UsersControl : Window
    {
        public UsersControl()
        {
            InitializeComponent();
            dgUsers.ItemsSource = User.GetUserAll();
            this.DataContext = new User();
        }

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as User;
            if (radAdd.IsChecked == true)
            {
                if (string.IsNullOrEmpty(x.Name) || string.IsNullOrEmpty(x.Password))
                {
                    MyMessageBox.Show("يجب إدخال كل الحقول");
                }
                else
                {
                    foreach (var item in User.GetUserAll())
                    {
                        if (item.Name.ToLower() == x.Name.ToLower())
                        {
                            MyMessageBox.Show("لايمكن اضافة المستخدم بسبب وجود مستخدم آخر بنفس الاسم");
                            return;
                        }
                    }
                    if (User.InsertData(x))
                    {
                        MyMessage.InsertMessage();
                        (dgUsers.ItemsSource as List<User>).Add(x);
                        dgUsers.Items.Refresh();
                    }
                }
            }
            else
                if (dgUsers.SelectedIndex != -1)
                {
                    if (radUpd.IsChecked == true)
                    {
                        if (string.IsNullOrEmpty(x.Name) || string.IsNullOrEmpty(x.Password))
                        {
                            MyMessageBox.Show("يجب إدخال كل الحقول");
                        }
                        else
                        {
                            foreach (var item in User.GetUserAll())
                            {
                                if (item.Name.ToLower() == x.Name.ToLower() && item.ID != x.ID)
                                {
                                    MyMessageBox.Show("لايمكن اضافة المستخدم بسبب وجود مستخدم آخر بنفس الاسم");
                                    return;
                                }
                            }
                            if (User.UpdateData(x))
                            {
                                MyMessage.UpdateMessage();
                                dgUsers.Items.Refresh();
                            }
                        }
                    }
                    else
                    {
                        if (x.ID == BaseDataBase.CurrentUser.ID)
                        {
                            MyMessageBox.Show("لا يمكن حذف المستخدم الحالي");
                        }
                        else if (User.DeleteData(x))
                        {
                            MyMessage.UpdateMessage();
                            (dgUsers.ItemsSource as List<User>).Remove(x);
                            dgUsers.Items.Refresh();
                        }
                    }
                }
        }

        private void radAdd_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                btnExecute.Content = radAdd.Content;
                this.SetBinding(Window.DataContextProperty, new Binding());
                this.DataContext = new User();
            }
        }

        private void radUpd_Checked(object sender, RoutedEventArgs e)
        {
            btnExecute.Content = (sender as RadioButton).Content;
            if (this.IsLoaded)
            {
                Binding b = new Binding();
                b.ElementName = "dgUsers";
                b.Path = new PropertyPath(DataGrid.SelectedItemProperty);
                this.SetBinding(Window.DataContextProperty, b);
            }
        }
    }
}
