using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for GuardianMainControl.xaml
    /// </summary>
    public partial class GuardianMainControl : UserControl
    {
        public GuardianMainControl()
        {
            InitializeComponent();
        }

        private void Control_Changed(object sender, TextChangedEventArgs e)
        {
            try
            {
                BindingListCollectionView view =
                    CollectionViewSource.GetDefaultView(dgGuardians.ItemsSource)
                    as BindingListCollectionView;
                if (view != null)
                {
                    view.CustomFilter = string.Format("FirstName like '%{0}%'", txtFirstName.Text);
                    if (!string.IsNullOrEmpty(txtLastName.Text))
                        view.CustomFilter += string.Format(" and LastName like '%{0}%'", txtLastName.Text);
                }
            }
            catch { }
        }


        private void btnAddNewGuardian_Click(object sender, RoutedEventArgs e)
        {
            GuardianControl w = new GuardianControl("ذكر");
            if (w.ShowDialog() == true)
            {
                dgGuardians.ItemsSource = Guardian.GetAllGuardianTable;
            }
        }

        private void dgGuardian_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnUpdateGuardian_Click(null, null);
        }

        private void btnUpdateGuardian_Click(object sender, RoutedEventArgs e)
        {
            if (dgGuardians.SelectedIndex >= 0)
            {
                GuardianControl w = new GuardianControl(Guardian.GetGuardianByID((int)(dgGuardians.Items[dgGuardians.SelectedIndex] as DataRowView)[0]));
                if (w.ShowDialog() == true)
                {
                    dgGuardians.ItemsSource = Guardian.GetAllGuardianTable;
                }
            }
        }

        private void dgGuardians_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnUpdateGuardian_Click(null, null);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dgGuardians.ItemsSource = Guardian.GetAllGuardianTable;
            Control_Changed(null, null);
        }

        private void btnDelGuardian_Click(object sender, RoutedEventArgs e)
        {
            if (dgGuardians.SelectedIndex != -1)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("ليس لديك صلاحيات للحذف");
                }
                else
                {
                    Guardian g = Guardian.GetGuardianByID((int)(dgGuardians.Items[dgGuardians.SelectedIndex] as DataRowView)[0]);
                    if (int.Parse(BaseDataBase._Scalar("select dbo.GetOrphanCountByGuardianID(" + g.GuardianID + ")")) > 0 || int.Parse(BaseDataBase._Scalar("select dbo.GetFamilyCountByGuardianID(" + g.GuardianID + ")")) > 0)
                    {
                        MyMessageBox.Show("لا يمكن حذف الوصي \nبسبب وجود بيانات وصاية سابقة له");
                    }
                    else
                    {
                        if (g.DeleteGuardianData())
                        {
                            dgGuardians.ItemsSource = Guardian.GetAllGuardianTable;
                            MyMessage.DeleteMessage();
                        }
                    }
                }
            }
        }
    }
}
