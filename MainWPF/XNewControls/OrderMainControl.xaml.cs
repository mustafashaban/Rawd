using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class OrderMainControl : UserControl
    {
        public OrderMainControl()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (dg.ItemsSource == null)
                dg.ItemsSource = (await Order.GetAllOrderTable()).DefaultView;
        }

        private void btnEnterOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderDetailsControl w = new OrderDetailsControl(new Order() { Type = 1 });
            w.ShowDialog();
        }

        private void btnExitOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderDetailsControl w = new OrderDetailsControl(new Order() { Type = 2 });
            w.ShowDialog();
        }

        private void btnInventoryOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderDetailsControl w = new OrderDetailsControl(new Order() { Type = 5 });
            w.ShowDialog();
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedItem != null)
            {
                Order order = Order.GetOrderByID((int)(dg.SelectedItem as DataRowView)[0]);
                OrderDetailsControl w = new OrderDetailsControl(order);
                if (w.ShowDialog() == true)
                    btnRefresh_Click(btnRefresh, null);
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedItem != null && MyMessageBox.Show("هل تريد تأكيد حذف الامر؟", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (!BaseDataBase.CurrentUser.IsAdmin)
                {
                    MyMessageBox.Show("لا يمكن حذف الامر الا من قبل مدير النظام");
                    return;
                }
                Order order = Order.GetOrderByID((int)(dg.SelectedItem as DataRowView)[0]);
                if (CanRemoveOrder(order))
                    if (Order.DeleteData(order))
                        MyMessage.DeleteMessage();
                btnRefresh_Click(btnRefresh, null);
            }
        }

        bool CanRemoveOrder(Order o)
        {
            if (o != null)
            {
                foreach (var item in o.OIs)
                {
                    double currentQuantity = 0;
                    double previousQuantity = 0;
                    if (item.Order.Id.HasValue)
                        double.TryParse(BaseDataBase._Scalar($"select IsNUll(sum(Quantity),0) from Order_Item where ItemId={item.Item.Id} and OrderID = {item.Order.Id}"), out previousQuantity);
                    else previousQuantity = 0;
                    double.TryParse(BaseDataBase._Scalar($"select Quantity from Batch_Item where InventoryID = {item.Order.InventoryID} and ItemID = {item.Item.Id}"), out currentQuantity);

                    if (o.Type == 1)
                    {
                        if (currentQuantity - previousQuantity < (item.Item.MinimumQuantity.HasValue ? item.Item.MinimumQuantity : 0))
                        {
                            MyMessageBox.Show($"القيمة الحالية للمادة \"{item.Item.Name}\" هو {currentQuantity}\nبينما الحد الادنى الذي يجب ان يحويه المستودع هو {(item.Item.MinimumQuantity.HasValue ? item.Item.MinimumQuantity : 0)}\nلا يمكن حذف المادة");
                            return false;
                        }
                    }
                    else
                    {
                        if (item.Item.MaximumQuantity.HasValue && currentQuantity + previousQuantity > item.Item.MaximumQuantity)
                        {
                            MyMessageBox.Show($"القيمة الحالية للمادة \"{item.Item.Name}\" هو {currentQuantity}\nبينما الحد الاعلى الذي يجب ان يحويه المستودع هو {item.Item.MaximumQuantity}\nلا يمكن حذف المادة");
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        void Filter()
        {
            var dv = dg.ItemsSource as DataView;
            if (dv != null)
            {
                try
                {
                    {
                        if (txtCode.Text.Trim() != "")
                        {
                            dv.RowFilter = "OrderCode = " + txtCode.Text;
                            if (cmboType.SelectedIndex > 0)
                                dv.RowFilter += $" and TypeText like '{(cmboType.SelectedItem as ComboBoxItem).Content.ToString().Trim()}' ";
                        }
                        else if (cmboType.SelectedIndex > 0)
                            dv.RowFilter = $" TypeText like '{(cmboType.SelectedItem as ComboBoxItem).Content.ToString().Trim()}' ";
                        else
                            dv.RowFilter = "";
                    }
                }
                catch { dv.RowFilter = ""; }
            }
        }
        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = (App.Current.Resources["sbRotateButton"] as Storyboard).Clone();
            sb.SetValue(Storyboard.TargetProperty, sender);
            sb.Begin();
            DataTable dt = await Order.GetAllOrderTable();
            sb.Pause();
            dg.ItemsSource = dt.DefaultView;
            Filter();
        }
    }
}
