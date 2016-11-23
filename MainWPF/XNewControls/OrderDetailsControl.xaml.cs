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
    public partial class OrderDetailsControl : Window
    {
        List<Order_Item> DeletedItems = new List<Order_Item>();
        public OrderDetailsControl(Order order)
        {
            InitializeComponent();
            cmboInventory.ItemsSource = Inventory.GetAllInventory().Where(x => x.IsActive); ;
            if (order.Type == 5)
                cmboInventory2.ItemsSource = Inventory.GetAllInventory().Where(x => x.IsActive);
            SetDataContext(order);
            if (!order.Id.HasValue)
                spNavigator.Visibility = Visibility.Collapsed;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnSelectItems_Click(object sender, RoutedEventArgs e)
        {
            var order = this.DataContext as Order;
            if (order.IsValidateMainOrderData())
            {
                ItemQuantityControl w = new ItemQuantityControl(order);
                if (w.ShowDialog() == true)
                {
                    this.DeletedItems.AddRange(w.DeletedItems);
                    dgOrderItems.ItemsSource = null;
                    dgOrderItems.ItemsSource = order.OIs;
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var order = this.DataContext as Order;
            if (order.IsValidateMainOrderData() && order.IsValidateOrderItemsData())
            {
                try
                {
                    if (order.Type == 5)
                    {
                        if (cmboInventory2.SelectedValue == null || int.Parse(cmboInventory2.SelectedValue.ToString()) == order.InventoryID)
                        {
                            MyMessageBox.Show("يجب اختيار المستودع المراد نقل المواد اليه ويجب ان يكون مختلفا عن المستودع الاساسي");
                            return;
                        }
                    }
                    if (order.Id == null)
                    {
                        if (Order.InsertData(order))
                        {
                            if (MyMessageBox.Show("هل تريد طباعة وصل", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                PrintTicket.printReportA6(order);
                            MyMessage.InsertMessage();
                        }
                    }
                    else
                    {
                        if (Order.UpdateData(order))
                            MyMessage.UpdateMessage();
                    }
                    foreach (var item in order.OIs)
                    {
                        if (item.Order == null)
                        {
                            item.Order = order;
                            Order_Item.InsertData(item);
                        }
                        else
                            Order_Item.UpdateData(item);
                    }
                    foreach (var item in DeletedItems)
                    {
                        if (item.Order != null)
                            Order_Item.DeleteData(item);
                    }
                    DialogResult = true;
                }
                catch (Exception ex) { MyMessageBox.Show(ex.Message); }
            }
        }

        private void cmboInventory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                var order = this.DataContext as Order;
                if (order != null && !isWorking && order.OIs.Count > 0)
                {
                    MyMessageBox.Show("سيتم تفريغ المواد ");
                    this.DeletedItems.AddRange(order.OIs.Where(x => x.Order != null));
                    order.OIs = null;
                    dgOrderItems.ItemsSource = null;
                }
            }
        }

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            var o = DataContext as Order;
            var Id = BaseDataBase._Scalar("select Max(Id) from [order] o where Type in (1,2,5)");
            if (!string.IsNullOrEmpty(Id))
                SetDataContext(Order.GetOrderByID(int.Parse(Id)));
            else
                MyMessageBox.Show("تم الوصول لأول امر");
        }
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            var o = DataContext as Order;
            var Id = BaseDataBase._Scalar("select Max(Id) from [order] o where Type in (1,2,5) and Id < " + o.Id);
            if (!string.IsNullOrEmpty(Id))
                SetDataContext(Order.GetOrderByID(int.Parse(Id)));
            else
                MyMessageBox.Show("تم الوصول لأول امر");
        }


        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            var o = DataContext as Order;
            var Id = BaseDataBase._Scalar("select Min(Id) from [order] o where Type in (1,2,5)");
            if (!string.IsNullOrEmpty(Id))
                SetDataContext(Order.GetOrderByID(int.Parse(Id)));
            else
                MyMessageBox.Show("تم الوصول لاخر امر");
        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            var o = DataContext as Order;
            var Id = BaseDataBase._Scalar("select Min(Id) from [order] o where Type in (1,2,5) and Id > " + o.Id);
            if (!string.IsNullOrEmpty(Id))
                SetDataContext(Order.GetOrderByID(int.Parse(Id)));
            else
                MyMessageBox.Show("تم الوصول لاخر امر");
        }
        bool isWorking = false;
        void SetDataContext(Order order)
        {
            isWorking = true;
            if (order.Type != 5)
                cmboInventory2.ItemsSource = null;
            cmboSources.Visibility = order.Type == 5 ? Visibility.Collapsed : Visibility.Visible;
            cmboInventory2.Visibility = order.Type == 5 ? Visibility.Visible : Visibility.Collapsed;
            txtSource.Text = order.Type == 2 ? "المستلم" : order.Type == 5 ? "الى مستودع" : "مصدر الامر";
            txtInventory.Text = order.Type == 5 ? "من مستودع" : "المستودع";
            this.DataContext = null;
            this.DataContext = order;
            dgOrderItems.ItemsSource = null;
            dgOrderItems.ItemsSource = order.OIs;
            if (order.Id.HasValue)
                cmboInventory.IsEnabled = cmboInventory2.IsEnabled = false;
            if (order.Type == 5)
                cmboInventory2.ItemsSource = Inventory.GetAllInventory().Where(x => x.IsActive);

            else order.Date = BaseDataBase.DateNow;
            isWorking = false;
        }
    }
}
