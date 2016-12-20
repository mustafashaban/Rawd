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
    /// Interaction logic for AccountMainControl.xaml
    /// </summary>
    public partial class AccountMainControl : UserControl
    {
        public AccountMainControl()
        {
            InitializeComponent();
            var lst = new List<string>(Account.AccountTypes.Select(x => x.Value));
            lst.Insert(0, "الكل");
            cmboType.ItemsSource = lst;
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
                dgAccounts.ItemsSource = Account.GetAllAccount();
                filter();
                sb.Pause();
                isWorking = false;
            }
        }

        private void dgAccounts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgAccounts.SelectedIndex != -1)
            {
                var a = dgAccounts.SelectedItem as Account;
                var w = App.Current.MainWindow as MainWindow;
                w.SendTabItem(new TabItem() { Header = "حساب " + a.Name, Content = new AccountControl() { Account = a } });
            }
        }

        private void txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            filter();
        }

        private void Control_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filter();
        }

        void filter()
        {
            ICollectionView lst = CollectionViewSource.GetDefaultView(dgAccounts.ItemsSource);
            if (lst != null)
                lst.Filter = CustomerFilter;
        }


        private bool CustomerFilter(object item)
        {
            var a = item as Account;
            if (!string.IsNullOrWhiteSpace(txtName.Text) && !a.Name.Contains(txtName.Text))
                return false;
            if (BaseDataBase.IsStringNumber(txtCode.Text) && a.Code != (int.Parse(txtCode.Text)))
                return false;
            if (cmboType.SelectedIndex > 0 && a.Type != cmboType.SelectedIndex - 1)
                return false;
            return true;
        }
    }
}
