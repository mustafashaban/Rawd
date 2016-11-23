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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MainWPF
{
    public partial class ItemQuantityControl : Window
    {
        public List<Order_Item> DeletedItems = new List<Order_Item>();
        List<Item> FilteredItems;
        object o;

        public ItemQuantityControl(Order o)
        {
            InitializeComponent();
            this.DataContext = o;
            if (!BaseDataBase.CurrentUser.PointAdmin)
            {
                lbItemsFiltered.IsEnabled = false;
                lbMainItems.IsEnabled = false;
                btnAddLister.IsEnabled = false;
                btnDelLister.IsEnabled = false;
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var o = this.DataContext as Order;
            if (o.Type != 1)
            {
                List<Item> lst = null;
                lst = await Item.GetAllItemsByInventory(o.InventoryID);

                if (lst == null || lst.Count == 0)
                {
                    MyMessageBox.Show("المستودع الذي تم اختياره لا يحوي أي مادة حاليّاً");
                    DialogResult = false;
                    return;
                }

                if (o.Id.HasValue)
                {
                    foreach (var item in o.OIs)
                    {
                        if (lst.Select(x => x.Id).Contains(item.Item.Id))
                            item.Item.CurrentQuantity = item.Quantity + lst.Where(x => x.Id == item.Item.Id).First().CurrentQuantity;
                        else item.Item.CurrentQuantity = item.Quantity;
                    }
                    dgSelectedItems.ItemsSource = o.OIs;
                    lbMainItems.ItemsSource = (from x in lst where !IsItemExist(x) select x).ToList<Item>();
                }
                else
                {
                    if (o.OIs.Count > 0)
                    {
                        foreach (var item in o.OIs)
                        {
                            if (lst.Select(x => x.Id).Contains(item.Item.Id))
                                item.Item.CurrentQuantity = item.Quantity + lst.Where(x => x.Id == item.Item.Id).First().CurrentQuantity;
                            else item.Item.CurrentQuantity = item.Quantity;
                        }
                        dgSelectedItems.ItemsSource = o.OIs;
                        lbMainItems.ItemsSource = (from x in lst where !IsItemExist(x) select x).ToList<Item>();
                    }
                    else
                    {
                        dgSelectedItems.ItemsSource = new List<Order_Item>();
                        lbMainItems.ItemsSource = lst;
                    }
                }

                if (lst != null && (o.Type == 3 || o.Type == 4))
                {
                    var FormedItems = o.Type == 3 ? FormedBasket.GetFormedBasketByFamilyID(o.FamilyID.Value).Where(x => x.IsUrgent == o.IsUrgentOrder).ToList() : FormedBasket.ActiveItems;
                    if (FormedItems != null && FormedItems.Count > 0)
                    {
                        FormedBasketSelectControl w = new FormedBasketSelectControl(FormedItems);
                        if (w.ShowDialog() == true)
                        {
                            var fb = w.lstFormedBakset.SelectedItem as FormedBasket;
                            foreach (var item in fb.FormedBasketItems)
                            {
                                if ((lbMainItems.ItemsSource as List<Item>).Where(x => x.Id == item.RelatedItem.Id).FirstOrDefault() == null)
                                {
                                    MyMessageBox.Show("المادة (" + item.RelatedItem.Name + ") غير موجودة في قائمة المواد الحالية\nلم يتم اضافة تشكيلة المواد");
                                    return;
                                }
                            }
                            foreach (var item in fb.FormedBasketItems)
                            {
                                Item i = (lbMainItems.ItemsSource as List<Item>).Where(x => x.Id == item.RelatedItem.Id).FirstOrDefault();
                                Order_Item s = new Order_Item();
                                s.Item = i;
                                s.Quantity = item.Quantity;
                                (dgSelectedItems.ItemsSource as List<Order_Item>).Add(s);
                                (lbMainItems.ItemsSource as List<Item>).Remove(i);
                            }
                        }
                    }
                }
            }
            else
            {
                if (o.Id.HasValue)
                {
                    dgSelectedItems.ItemsSource = o.OIs;
                    lbMainItems.ItemsSource = (from x in Item.AllItems where !IsItemExist(x) select x).ToList<Item>();
                }
                else
                {
                    dgSelectedItems.ItemsSource = o.OIs;
                    lbMainItems.ItemsSource = (from x in Item.AllItems where !IsItemExist(x) select x).ToList<Item>();
                }
            }


            //Criteria
            if (o != null && !o.Id.HasValue && o.FamilyID.HasValue)
            {
                var OIs = Order_Item.GetFamilyCriteria(o.FamilyID.Value);
                if (OIs != null && OIs.Count > 0)
                {
                    foreach (var s in OIs)
                    {
                        foreach (var i in lbMainItems.ItemsSource as List<Item>)
                        {
                            if (i.Id.HasValue && i.Id.Value == s.Item.Id.Value)
                            {
                                s.Item.Id = i.Id;
                                (dgSelectedItems.ItemsSource as List<Order_Item>).Add(s);
                                (lbMainItems.ItemsSource as List<Item>).Remove(i);
                                break;
                            }
                        }
                    }
                }
            }

            RefreshPanel();
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            Item i;
            if (lbItemsFiltered.Visibility == Visibility.Visible)
            {
                if (lbItemsFiltered.SelectedIndex == -1)
                    return;
                var iFiltered = lbItemsFiltered.SelectedItem as Item;
                i = (lbMainItems.ItemsSource as List<Item>).Where(x => x.Id == iFiltered.Id).FirstOrDefault();
                txtSearch.Text = string.Empty;
            }
            else
            {
                if (lbMainItems.SelectedIndex == -1)
                    return;
                i = lbMainItems.SelectedItem as Item;
            }
            Order_Item s = new Order_Item();
            s.Item = i;
            s.Quantity = 1;

            (dgSelectedItems.ItemsSource as List<Order_Item>).Add(s);
            (lbMainItems.ItemsSource as List<Item>).Remove(i);

            RefreshPanel();
        }

        private void btnDelItem_Click(object sender, RoutedEventArgs e)
        {
            if (dgSelectedItems.SelectedIndex != -1)
            {
                txtSearch.Text = string.Empty;
                Order_Item s = dgSelectedItems.SelectedItem as Order_Item;
                var i = s.Item;

                if (s.Order != null)
                {
                    double currentQuantity = 0;
                    double previousQuantity = 0;
                    if (s.Order.Id.HasValue)
                        double.TryParse(BaseDataBase._Scalar($"select IsNUll(sum(Quantity),0) from Order_Item where ItemId={s.Item.Id} and OrderID = {s.Order.Id}"), out previousQuantity);
                    else previousQuantity = 0;
                    double.TryParse(BaseDataBase._Scalar($"select Quantity from Batch_Item where InventoryID = {s.Order.InventoryID} and ItemID = {s.Item.Id}"), out currentQuantity);

                    if (s.Order.Type == 1)
                    {
                        if (currentQuantity - previousQuantity < (s.Item.MinimumQuantity.HasValue ? s.Item.MinimumQuantity : 0))
                        {
                            MyMessageBox.Show($"القيمة الحالية للمادة \"{s.Item.Name}\" هو {currentQuantity}\nبينما الحد الادنى الذي يجب ان يحويه المستودع هو {(s.Item.MinimumQuantity.HasValue ? s.Item.MinimumQuantity : 0)}\nلا يمكن حذف المادة");
                            return;
                        }
                    }
                    else
                    {
                        if (s.Item.MaximumQuantity.HasValue && currentQuantity + previousQuantity > s.Item.MaximumQuantity)
                        {
                            MyMessageBox.Show($"القيمة الحالية للمادة \"{s.Item.Name}\" هو {currentQuantity}\nبينما الحد الاعلى الذي يجب ان يحويه المستودع هو {s.Item.MaximumQuantity}\nلا يمكن حذف المادة");
                            return;
                        }
                    }
                }
                (lbMainItems.ItemsSource as List<Item>).Add(i);
                (dgSelectedItems.ItemsSource as List<Order_Item>).Remove(s);

                RefreshPanel();

                if (s != null)
                    DeletedItems.Add(s);
            }
        }
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearch.Text == string.Empty)
            {
                lbItemsFiltered.Visibility = Visibility.Collapsed;
                return;
            }
            lbItemsFiltered.Visibility = Visibility.Visible;
            FilteredItems = lbMainItems.ItemsSource as List<Item>;
            lbItemsFiltered.ItemsSource = FilteredItems.Where(x => x.Name.Contains(txtSearch.Text) || x.ItemType.Contains(txtSearch.Text));

            ICollectionView view = CollectionViewSource.GetDefaultView(lbItemsFiltered.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }

        private void lbMainItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnAddItem_Click(null, null);
        }

        private void RefreshPanel()
        {
            dgSelectedItems.Items.Refresh();
            lbMainItems.Items.Refresh();

            ICollectionView view = CollectionViewSource.GetDefaultView(lbMainItems.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }
        bool IsItemExist(Item i)
        {
            foreach (var item in dgSelectedItems.ItemsSource as List<Order_Item>)
            {
                if (item.Item.Id == i.Id)
                    return true;
            }
            return false;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void txtSearch_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                if (!BaseDataBase.CurrentUser.PointAdmin)
                    return;
                if (lbItemsFiltered.Items.Count == 1)
                {
                    lbItemsFiltered.SelectedIndex = 0;
                    btnAddItem_Click(null, null);
                }
            }
        }
        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var o = this.DataContext as Order;
                if (!IsDataGridItemsValidate())
                {
                    MyMessageBox.Show("يجب اختيار مواد بقيم غير معدومة");
                    return;
                }
                else
                {
                    if (ValidateAllItems())
                    {
                        o.OIs = dgSelectedItems.ItemsSource as List<Order_Item>;
                        DialogResult = true;
                    }
                }
            }
            catch { }
        }

        private bool ValidateAllItems()
        {
            var o = this.DataContext as Order;
            var lst = dgSelectedItems.ItemsSource as List<Order_Item>;
            if (o.Type >= 3)//FamilyOrder
            {
                foreach (var item in lst)
                {
                    if (item.Item.MaxQuantityPerOrder.HasValue && item.Quantity > item.Item.MaxQuantityPerOrder)
                    {
                        MyMessageBox.Show($"أعلى كمية ممكن أن تستلمها العائلة من المادة\" {item.Item.Name}\" في اليوم هي " + item.Item.MaxQuantityPerOrder);
                        return false;
                    }
                    if (item.Item.MaxQuantityPerFamily.HasValue)
                    {
                        double sum = 0;
                        double.TryParse(BaseDataBase._Scalar($"select sum(Quantity) from Order_item where OrderID in (select id from [Order] where IsNull(FamilyID,-1) = {o.FamilyID} and id <> {o.Id ?? -1}) and ItemID = {item.Item.Id}"), out sum);
                        if (sum + item.Quantity > item.Item.MaxQuantityPerFamily)
                        {
                            MyMessageBox.Show($"تم استلام {sum} من المادة \"{item.Item.Name}\"  لهذه العائلة سابقاً\nأعلى كمية ممكن ان تستلمها العائلة بشكل اجمالي هو {item.Item.MaxQuantityPerFamily}");
                            return false;
                        }
                    }
                    if (item.Item.MaxQuantityPerDay.HasValue)
                    {
                        double sum = 0;
                        double.TryParse(BaseDataBase._Scalar($"select sum(Quantity) from Order_item where OrderID in (select id from [Order] where Type >= 3 and id <> {o.Id ?? -1} and InventoryID = {o.InventoryID} and CONVERT(date, [Date]) = CONVERT(date, GETDATE())) and ItemID = {item.Item.Id}"), out sum);
                        if (sum + item.Quantity > item.Item.MaxQuantityPerDay)
                        {
                            MyMessageBox.Show($"تم تسليم {sum} من المادة \"{item.Item.Name}\" اليوم\nأعلى كمية ممكن تلسيها من هذه المادة في اليوم هو {item.Item.MaxQuantityPerDay }");
                            return false;
                        }
                    }
                }
            }
            double currentQuantity = 0;
            double previousQuantity = 0;
            foreach (var item in lst)
            {
                if (o.Id.HasValue)
                    double.TryParse(BaseDataBase._Scalar($"select IsNUll(sum(Quantity),0) from Order_Item where ItemId={item.Item.Id} and OrderID = {o.Id}"), out previousQuantity);
                else previousQuantity = 0;
                double.TryParse(BaseDataBase._Scalar($"select Quantity from Batch_Item where InventoryID = {o.InventoryID} and ItemID = {item.Item.Id}"), out currentQuantity);

                if (o.Type == 1)
                {
                    if (item.Item.MaximumQuantity.HasValue && item.Item.MaximumQuantity < currentQuantity - previousQuantity + item.Quantity)
                    {
                        MyMessageBox.Show($"القيمة الحالية للمادة \"{item.Item.Name}\" هو {currentQuantity}\nبينما الحد الاعلى الممكن ادخاله الى المستودع هو {item.Item.MaximumQuantity}\nلا يمكن الاضافة");
                        return false;
                    }
                }
                else
                {
                    if (currentQuantity + previousQuantity == 0 || item.Quantity > currentQuantity + previousQuantity)
                    {
                        MyMessageBox.Show($"القيمة الحالية للمادة \"{item.Item.Name}\" هو {currentQuantity}\nلا يمكن الاضافة");
                        return false;
                    }
                    if (item.Item.MinimumQuantity.HasValue && item.Item.MinimumQuantity > currentQuantity + previousQuantity - item.Quantity)
                    {
                        MyMessageBox.Show($"القيمة الحالية للمادة \"{item.Item.Name}\" هو {currentQuantity}\nبينما الحد الادنى الذي يجب ان يحويه المستودع هو {item.Item.MinimumQuantity}\nلا يمكن الاضافة");
                        return false;
                    }
                    else if (item.Item.WarningQuantity.HasValue && item.Item.WarningQuantity > currentQuantity + previousQuantity - item.Quantity)
                    {
                        if (!BaseDataBase.CurrentUser.IsAdmin)
                        {
                            MyMessageBox.Show($"القيمة الحالية للمادة \"{item.Item.Name}\" هو {currentQuantity}\nبينما حد التنبيه للمادة هو {item.Item.WarningQuantity}\nلا يمكن الاضافة");
                            return false;
                        }
                        else
                        {
                            if (MyMessageBox.Show($"القيمة الحالية للمادة \"{item.Item.Name}\" هو {currentQuantity}\nبينما حد التنبيه للمادة هو {item.Item.WarningQuantity}\nهل تريد الاضافة بجميع الاحوال؟", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                                return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool IsDataGridItemsValidate()
        {
            var lst = dgSelectedItems.ItemsSource as List<Order_Item>;
            if (lst == null)
                return false;
            foreach (var item in lst)
                if (item.Quantity == 0)
                    return false;
            return true;
        }
    }
}
