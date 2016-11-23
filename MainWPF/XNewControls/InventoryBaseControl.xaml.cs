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
    public partial class InventoryBaseControl : UserControl
    {
        public InventoryBaseControl()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dgNotifications.ItemsSource = (await BaseDataBase._TablingStoredProcedureAsync("sp_Get_Notifications")).DefaultView;
        }

        private void btnInventory_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.ReportCreator)
            {
                MyMessageBox.Show("ليس لديك صلاحية دخول");
                return;
            }
            var main = App.Current.MainWindow as MainWindow;
            main.SendTabItem(new TabItem() { Header = "بيانات المستودعات", Content = new InventoryControl() });
        }

        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.ReportCreator)
            {
                MyMessageBox.Show("ليس لديك صلاحية دخول");
                return;
            }
            var main = App.Current.MainWindow as MainWindow;
            main.SendTabItem(new TabItem() { Header = "حركة المستودعات", Content = new OrderMainControl() });
        }

        private void btnCriteria_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.ReportCreator)
            {
                MyMessageBox.Show("ليس لديك صلاحية دخول");
                return;
            }
            ItemCriteriaControl w = new ItemCriteriaControl();
            w.ShowDialog();
        }

        private void btnItems_Click(object sender, RoutedEventArgs e)
        {
            if (!(BaseDataBase.CurrentUser.ReportCreator|| BaseDataBase.CurrentUser.PointAdmin))
            {
                MyMessageBox.Show("ليس لديك صلاحية دخول");
                return;
            }
            var main = App.Current.MainWindow as MainWindow;
            main.SendTabItem(new TabItem() { Header = "بيانات المواد", Content = new ItemControl() });
        }

        private void btnVoucherCriteria_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.PointAdmin)
            {
                MyMessageBox.Show("ليس لديك صلاحية دخول");
                return;
            }
            VoucherCriteriaControl w = new VoucherCriteriaControl();
            w.ShowDialog();
        }

        private void btnNextOrderCriteria_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.PointAdmin)
            {
                MyMessageBox.Show("ليس لديك صلاحية دخول");
                return;
            }
            NextOrderCriteriaControl w = new NextOrderCriteriaControl();
            w.ShowDialog();
        }

        private void btnSta_Click(object sender, RoutedEventArgs e)
        {
            if (!(BaseDataBase.CurrentUser.ReportCreator|| BaseDataBase.CurrentUser.PointAdmin))
            {
                MyMessageBox.Show("ليس لديك صلاحية دخول");
                return;
            }
            var main = App.Current.MainWindow as MainWindow;
            main.SendTabItem(new TabItem() { Header = "احصائيات المواد", Content = new InventoryItemSta() });
        }
    }
}
