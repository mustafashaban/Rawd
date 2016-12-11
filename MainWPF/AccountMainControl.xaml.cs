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

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgAccounts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void txt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Control_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
