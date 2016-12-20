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

        bool isWorking = false;
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!isWorking)
            {
                isWorking = true;
                Storyboard sb = (App.Current.Resources["sbRotateButton"] as Storyboard).Clone();
                sb.SetValue(Storyboard.TargetProperty, sender);
                sb.Begin();
                dgOrphans.ItemsSource = BaseDataBase._TablingStoredProcedure("sp_GetOrphansTable").DefaultView;
                Control_Changed(null, null);
                sb.Pause();
                isWorking = false;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var drv = dgOrphans.SelectedItem as DataRowView;
            MainWindow m = App.Current.MainWindow as MainWindow;
            m.SendTabItem(new TabItem() { Header = "إضافة عائلة أيتام", Content = new OrphanDetailsControl(new Family() { FamilyType = "أيتام" }) });
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (radOrphans.IsChecked == true && dgOrphans.SelectedIndex != -1)
            {
                var drv = dgOrphans.SelectedItem as DataRowView;
                MainWindow m = App.Current.MainWindow as MainWindow;
                m.SendTabItem(new TabItem() { Header = drv[1].ToString(), Content = new OrphanDetailsControl(Orphan.GetOrphanByID((int)drv[0])) });
            }
            else if (radFamilies.IsChecked == true && dgFamily.SelectedIndex != -1)
            {
                var drv = dgFamily.SelectedItem as DataRowView;
                MainWindow m = App.Current.MainWindow as MainWindow;
                m.SendTabItem(new TabItem() { Header = drv["FamilyName"].ToString(), Content = new OrphanDetailsControl(Family.GetFamilyByID((int)drv[0])) });
            }
        }

        private void dgFamily_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            btnUpdate_Click(null, null);
        }
        bool isWorking2 = false;
        private async void btnSearch2_Click(object sender, RoutedEventArgs e)
        {
            if (!isWorking2)
            {
                isWorking2 = true;
                Storyboard sb = (App.Current.Resources["sbRotateButton"] as Storyboard).Clone();
                sb.SetValue(Storyboard.TargetProperty, sender);
                sb.Begin();
                var dt = await BaseDataBase._TablingStoredProcedureAsync("sp_GetOrphanFamilyAllTable");
                dgFamily.ItemsSource = dt.DefaultView;
                sb.Pause();
                Control_Changed(null, null);
                isWorking2 = false;
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

        private void Control1_Changed(object sender, TextChangedEventArgs e)
        {
            var dv = dgFamily.ItemsSource as DataView;
            try
            {
                dv.RowFilter = string.Format("FamilyCode like '{0}*'", txtFamilyCode.Text);
                if (!string.IsNullOrEmpty(txtFamilyName.Text))
                    dv.RowFilter += string.Format(" and (FamilyName like '*{0}*' or Father like '*{0}*' or Mother Like '*{0}*' or Father like '*{1}*' or Mother Like '*{1}*' or Father like '*{2}*' or Mother Like '*{2}*' or Father like '*{3}*' or Mother Like '*{3}*' or Father like '*{4}*' or Mother Like '*{4}*' or Father like '*{5}*' or Mother Like '*{5}*' or Father like '*{6}*' or Mother Like '*{6}*' or Father like '*{7}*' or Mother Like '*{7}*' or Father like '*{8}*' or Mother Like '*{8}*' )", txtFamilyName.Text, txtFamilyName.Text.Replace('أ', 'ا'), txtFamilyName.Text.Replace('ا', 'أ'), txtFamilyName.Text.Replace('ى', 'ا'), txtFamilyName.Text.Replace('ا', 'ى'), txtFamilyName.Text.Replace('آ', 'ا'), txtFamilyName.Text.Replace('ا', 'آ'), txtFamilyName.Text.Replace('ة', 'ه'), txtFamilyName.Text.Replace('ه', 'ة'));
                if (!string.IsNullOrEmpty(txtPID.Text))
                    dv.RowFilter += string.Format(" and (FatherNa like '{0}*' or MotherNa Like '{0}*')", txtPID.Text);
            }
            catch { dv.RowFilter = ""; }
        }
    }
}
