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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for OrphanMainControl.xaml
    /// </summary>
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
                    if (!string.IsNullOrEmpty(txtLastName.Text))
                        view.CustomFilter += string.Format(" and LastName like '%{0}%'", txtLastName.Text);
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

        private void btnOrphanDetails_Click(object sender, RoutedEventArgs e)
        {
            if (dgOrphans.SelectedIndex != -1)
            {
                if (Orphan.GetOrphanByID((int)(dgOrphans.SelectedItem as DataRowView)[0]).DeathDate != null)
                    if (MyMessageBox.Show("هذا اليتيم تم إلغاؤه.\n هل تريد المتابعة على أي حال", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                        return;
                string Header = (dgOrphans.SelectedItem as DataRowView)[1].ToString() + " " + (dgOrphans.SelectedItem as DataRowView)[2].ToString();
                TabItem ti = new TabItem();
                ti.Header = Header;
                ti.Content = new OrphanDetailsControl((int)(dgOrphans.SelectedItem as DataRowView)[0]);
                MainWindow m = App.Current.MainWindow as MainWindow;
                m.SendTabItem(ti);
            }
        }

        private void dgOrphans_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            btnOrphanDetails_Click(null, null);
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

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dgOrphans.ItemsSource = BaseDataBase._TablingStoredProcedure("sp_GetOrphansAll").DefaultView;
            Control_Changed(null, null);
        }
    }
}
