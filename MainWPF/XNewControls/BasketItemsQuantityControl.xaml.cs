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
    public partial class BasketItemsQuantityControl : Window
    {
        public List<Basket_Item> DeletedItems = new List<Basket_Item>();
        List<Item> FilteredItems;
        object o;
        public BasketItemsQuantityControl(Item i)
        {
            InitializeComponent();
            this.DataContext = i;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var i = this.DataContext as Item;
            if (i.Id.HasValue)
            {
                dgSelectedItems.ItemsSource = i.BasketItems;
                lbMainItems.ItemsSource = (from x in Item.ActiveItems where !IsItemExist(x) select x).ToList<Item>();
            }
            else
            {
                if (i.BasketItems.Count > 0)
                {
                    dgSelectedItems.ItemsSource = i.BasketItems;
                    lbMainItems.ItemsSource = (from x in Item.ActiveItems where !IsItemExist(x) select x).ToList<Item>();
                }
                else
                {
                    dgSelectedItems.ItemsSource = new List<Basket_Item>();
                    lbMainItems.ItemsSource = Item.ActiveItems;
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
            Basket_Item bi = new Basket_Item();
            bi.RelatedItem = i;
            bi.Quantity = 1;

            (dgSelectedItems.ItemsSource as List<Basket_Item>).Add(bi);
            (lbMainItems.ItemsSource as List<Item>).Remove(i);

            RefreshPanel();
        }

        private void btnDelItem_Click(object sender, RoutedEventArgs e)
        {
            if (dgSelectedItems.SelectedIndex != -1)
            {
                txtSearch.Text = string.Empty;
                Basket_Item bi = dgSelectedItems.SelectedItem as Basket_Item;
                var i = bi.RelatedItem;

                (lbMainItems.ItemsSource as List<Item>).Add(i);
                (dgSelectedItems.ItemsSource as List<Basket_Item>).Remove(bi);

                RefreshPanel();

                if (bi != null)
                    DeletedItems.Add(bi);
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
            foreach (var item in dgSelectedItems.ItemsSource as List<Basket_Item>)
            {
                if (item.RelatedItem.Id == i.Id)
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
                var i = this.DataContext as Item;
                if (!IsDataGridItemsValidate())
                {
                    MyMessageBox.Show("يجب اختيار مواد بقيم غير معدومة");
                    return;
                }
                else
                {
                    i.BasketItems = dgSelectedItems.ItemsSource as List<Basket_Item>;
                    DialogResult = true;
                    this.Close();
                }
            }
            catch { }
        }

        private bool IsDataGridItemsValidate()
        {
            var lst = dgSelectedItems.ItemsSource as List<Basket_Item>;
            if (lst == null)
                return false;
            foreach (var item in lst)
                if (item.Quantity == 0)
                    return false;
            return true;
        }
    }
}