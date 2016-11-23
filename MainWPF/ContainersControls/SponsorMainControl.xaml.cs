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
    /// Interaction logic for SponsorMainControl.xaml
    /// </summary>
    public partial class SponsorMainControl : UserControl
    {
        public SponsorMainControl()
        {
            InitializeComponent();
        }

        private void Control_Changed(object sender, TextChangedEventArgs e)
        {
            try
            {
                BindingListCollectionView view =
                    CollectionViewSource.GetDefaultView(dgSponsor.ItemsSource)
                    as BindingListCollectionView;
                if (view != null)
                {
                    view.CustomFilter = string.Format("FirstName like '%{0}%'", txtFirstName.Text);
                    if (!string.IsNullOrEmpty(txtLastName.Text))
                        view.CustomFilter += string.Format(" and LastName like '%{0}%'", txtLastName.Text);
                    if (cmboGender.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and Gender like '{0}'", (cmboGender.Items[cmboGender.SelectedIndex] as ComboBoxItem).Content);
                }
            }
            catch { }
        }

        private void cmboGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Control_Changed(null, null);
        }

        private void btnAddNewSponsor_Click(object sender, RoutedEventArgs e)
        {
            SponsorControl w = new SponsorControl();
            if (w.ShowDialog() == true)
            {
                dgSponsor.ItemsSource = Sponsor.GetAllSponsorTable;
            }
        }

        private void dgSponsor_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnUpdateSponsor_Click(null, null);
        }

        private void btnUpdateSponsor_Click(object sender, RoutedEventArgs e)
        {
            if (dgSponsor.SelectedIndex >= 0)
            {
                SponsorControl w = new SponsorControl(Sponsor.GetSponsorByID((int)(dgSponsor.Items[dgSponsor.SelectedIndex] as DataRowView)[0]));
                if (w.ShowDialog() == true)
                {
                    dgSponsor.ItemsSource = Sponsor.GetAllSponsorTable;
                }
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dgSponsor.ItemsSource = Sponsor.GetAllSponsorTable;
            Control_Changed(null, null);
        }

        private void btnDelSupervisor_Click(object sender, RoutedEventArgs e)
        {
            if (dgSponsor.SelectedIndex != -1)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("ليس لديك صلاحيات للحذف");
                }
                else
                {
                    Sponsor s = Sponsor.GetSponsorByID((int)(dgSponsor.Items[dgSponsor.SelectedIndex] as DataRowView)[0]);
                    if (int.Parse(BaseDataBase._Scalar("select dbo.fn_SponsorOrphansCount(" + s.SponsorID + ")")) > 0)
                    {
                        MyMessageBox.Show("لا يمكن حذف الكفيل \nبسبب وجود بيانات كفالة سابقة له");
                    }
                    else
                    {
                        if (s.DeleteSponsorData())
                        {
                            dgSponsor.ItemsSource = Sponsor.GetAllSponsorTable;
                            MyMessage.DeleteMessage();
                        }
                    }
                }
            }
        }
    }
}
