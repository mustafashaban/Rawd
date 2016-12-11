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
using System.Windows.Media.Animation;

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
                    view.CustomFilter = string.Format("Name like '%{0}%'", txtName.Text);
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
            MainWindow m = App.Current.MainWindow as MainWindow;
            m.SendTabItem(new TabItem() { Header = "اضافة كفيل جديد", Content = new SponsorControl(new MainWPF.Sponsor()) });
        }

        private void dgSponsor_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            btnUpdateSponsor_Click(null, null);
        }

        private void btnUpdateSponsor_Click(object sender, RoutedEventArgs e)
        {
            if (dgSponsor.SelectedIndex >= 0)
            {
                var s = Sponsor.GetSponsorByID((int)(dgSponsor.Items[dgSponsor.SelectedIndex] as DataRowView)[0]);
                MainWindow m = App.Current.MainWindow as MainWindow;
                m.SendTabItem(new TabItem() { Header = s.Name, Content = new SponsorControl(s) });
            }
        }
        bool isWorking = false;
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!isWorking)
            {
                isWorking = true;
                Storyboard sb = (App.Current.Resources["sbRotateButton"] as Storyboard).Clone();
                sb.SetValue(Storyboard.TargetProperty, sender);
                sb.Begin();
                dgSponsor.ItemsSource = Sponsor.GetAllSponsorTable;
                Control_Changed(null, null);
                sb.Pause();
                isWorking = false;
            }
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
                        if (Sponsor.DeleteData(s))
                        {
                            dgSponsor.ItemsSource = Sponsor.GetAllSponsorTable;
                            MyMessage.DeleteMessage();
                        }
                    }
                }
            }
        }

        private void btnDeleteSponsor_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
