using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MainWPF
{
    public partial class OrphanMainControl : UserControl
    {
        public OrphanMainControl()
        {
            InitializeComponent();
        }

        private void Control_Changed(object sender, TextChangedEventArgs e)
        {
            try
            {
                BindingListCollectionView view =
                    CollectionViewSource.GetDefaultView(dgOrphans.ItemsSource)
                    as BindingListCollectionView;
                if (view != null)
                {
                    view.CustomFilter = string.Format("FirstName like '%{0}%'", txtFirstName.Text);
                    if (cmboGender.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and Gender like '{0}'", (cmboGender.Items[cmboGender.SelectedIndex] as ComboBoxItem).Content);
                    if (cmboType.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and FamilyType like '{0}'", (cmboType.Items[cmboType.SelectedIndex] as ComboBoxItem).Content);
                }
            }
            catch { }
        }

        private void cmboGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Control_Changed(null, null);
        }

        private void dgOrphans_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            btnUpdate_Click(null, null);
        }

        private void btnOrphanFamilyData_Click(object sender, RoutedEventArgs e)
        {
            if (dgOrphans.SelectedIndex != -1)
            {
                string Header = "تعديل عائلة";
                MainWindow m = App.Current.MainWindow as MainWindow;
                if (m.CheckTabControl(Header))
                {
                    TabItem ti = new TabItem();
                    ti.Header = Header;
                    ti.Content = new AddFamilyControl((Orphan.GetOrphanByID((int)((dgOrphans.SelectedItem as DataRowView)[0])).FamilyID.Value));
                    m.SendTabItem(ti);
                }
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
                dgOrphans.ItemsSource = BaseDataBase._TablingStoredProcedure("sp_GetOrphansAll").DefaultView;
                Control_Changed(null, null);
                sb.Pause();
                isWorking = false;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgOrphans.SelectedIndex != -1)
            {
                if (Orphan.GetOrphanByID((int)(dgOrphans.SelectedItem as DataRowView)[0]).DeathDate != null)
                    if (MyMessageBox.Show("هذا اليتيم تم إلغاؤه.\n هل تريد المتابعة على أي حال", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                        return;
                var drv = dgOrphans.SelectedItem as DataRowView;
                MainWindow m = App.Current.MainWindow as MainWindow;
                m.SendTabItem(new TabItem() { Header = drv[1].ToString() + " " + drv[2].ToString(), Content = new OrphanDetailsControl((int)drv[0]) });
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPrintSelected_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnFirstData_Click(object sender, RoutedEventArgs e)
        {
            //SetItemsSource(1);
        }

        private void btnPreviousData_Click(object sender, RoutedEventArgs e)
        {
            //if ((currentIndex - 1) > 0)
            //{
            //    SetItemsSource(currentIndex - 1);
            //}
        }

        private void btnLastData_Click(object sender, RoutedEventArgs e)
        {
            //if (dt != null)
            //{
            //    int lastIndex = dt2.Rows.Count / (int)100 + (dt2.Rows.Count / (double)100 - dt2.Rows.Count / 100 > 0 ? 1 : 0);
            //    SetItemsSource(lastIndex == 0 ? 1 : lastIndex);
            //}
        }

        private void btnNextData_Click(object sender, RoutedEventArgs e)
        {
            //if (dt != null)
            //{
            //    int total = dt2.Rows.Count / (int)100 + (dt2.Rows.Count / (double)100 - dt2.Rows.Count / 100 > 0 ? 1 : 0);
            //    if (currentIndex + 1 <= total)
            //    {
            //        SetItemsSource(currentIndex + 1);
            //    }
            //}
        }

    }
}
