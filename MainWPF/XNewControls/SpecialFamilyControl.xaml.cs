using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for SpecialFamilyControl.xaml
    /// </summary>
    public partial class SpecialFamilyControl : UserControl
    {
        public SpecialFamilyControl()
        {
            InitializeComponent();
        }

        private void btnAddFamilyData_Click(object sender, RoutedEventArgs e)
        {
            var main = App.Current.MainWindow as MainWindow;
            main.SendTabItem(new TabItem() { Header = "عائلة خاصة", Content = new SpecialFamilyDetails(new SpecialFamily()) });
        }

        private void btnUpdateFamilyData_Click(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedItem != null)
            {
                var sf = dg.SelectedItem as SpecialFamily;
                sf.GetOrders();
                var main = App.Current.MainWindow as MainWindow;
                main.SendTabItem(new TabItem() { Header = "عائلة خاصة : " + sf.Name, Content = new SpecialFamilyDetails(sf) });
            }
        }

        private void btnDeleteFamilyData_Click(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedItem != null)
            {
                var sf = dg.SelectedItem as SpecialFamily;
                if (BaseDataBase.CurrentUser.IsAdmin)
                {
                    if (MyMessageBox.Show("هل تريد تأكيد حذف بيانات العائلة الخاصة؟", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        if (SpecialFamily.DeleteData(sf))
                            MyMessage.DeleteMessage();
                    }
                }
                else MyMessageBox.Show("لبس لديك صلاحية حذف");
            }
        }
        bool isWorking = false;
        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!isWorking)
            {
                isWorking = true;
                Storyboard sb = (App.Current.Resources["sbRotateButton"] as Storyboard).Clone();
                sb.SetValue(Storyboard.TargetProperty, sender);
                sb.Begin();
                List<SpecialFamily> sfList = null;
                await Task.Run(() => sfList = SpecialFamily.GetAllSpecialFamily());
                sb.Pause();
                dg.ItemsSource = sfList;
                Control_Changed(null, null);
                isWorking = false;
            }
        }

        private void dgFamily_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            btnUpdateFamilyData_Click(null, null);
        }

        private void Control_Changed(object sender, TextChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(dg.ItemsSource);
            if (view != null)
                view.Filter = FilterPredicate;
        }

        private bool FilterPredicate(object obj)
        {
            var s = obj as SpecialFamily;
            if (!string.IsNullOrEmpty(txtCode.Text) && BaseDataBase.IsStringNumber(txtCode.Text) && s.Id != int.Parse(txtCode.Text))
                return false;
            if (!string.IsNullOrEmpty(txtName.Text) && !s.Name.Contains(txtName.Text))
                return false;
            if (!string.IsNullOrEmpty(txtPID.Text) && !s.PID.StartsWith(txtPID.Text))
                return false;
            return true;
        }
    }
}
