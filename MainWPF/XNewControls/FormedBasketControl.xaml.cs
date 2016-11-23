using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainWPF
{
    public partial class FormedBasketControl : UserControl
    {
        List<Item> FilteredItems;
        object o;
        public FormedBasketControl()
        {
            InitializeComponent();
            cmboFormedBasket.ItemsSource = BaseDataBase._Tabling("select Id,Name from FormedBasket").DefaultView;
            this.DataContext = new FormedBasket();
            dgSelectedItems.ItemsSource = new List<FormedBasket_Item>();
            lbMainItems.ItemsSource = Item.AllItems;
            lbSectors.ItemsSource = Sector.GetAllSector();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var fb = this.DataContext as FormedBasket;
            if (dgSelectedItems.Items.Count > 0)
            {
                fb.FormedBasketItems.Clear();
                fb.FormedBasketItems.AddRange(dgSelectedItems.ItemsSource as List<FormedBasket_Item>);
            }
            fb.RelatedSectors = (lbSectors.ItemsSource as List<Sector>).Where(x => x.IsSelected).ToList();

            if (fb.IsValidate())
            {
                if (!fb.Id.HasValue)
                {
                    if (FormedBasket.InsertData(fb))
                        MyMessage.InsertMessage();
                }
                else
                {
                    if (FormedBasket.UpdateData(fb))
                        MyMessage.UpdateMessage();
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            var fb = this.DataContext as FormedBasket;
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
            FormedBasket_Item s = new FormedBasket_Item();
            s.RelatedItem = i;
            s.Quantity = 1;

            (dgSelectedItems.ItemsSource as List<FormedBasket_Item>).Add(s);
            (lbMainItems.ItemsSource as List<Item>).Remove(i);

            RefreshPanel();
        }

        private void btnDelItem_Click(object sender, RoutedEventArgs e)
        {
            if (dgSelectedItems.SelectedIndex != -1)
            {
                txtSearch.Text = string.Empty;
                FormedBasket_Item s = dgSelectedItems.SelectedItem as FormedBasket_Item;
                var i = s.RelatedItem;

                (lbMainItems.ItemsSource as List<Item>).Add(i);
                (dgSelectedItems.ItemsSource as List<FormedBasket_Item>).Remove(s);

                RefreshPanel();
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
            foreach (var item in dgSelectedItems.ItemsSource as List<FormedBasket_Item>)
            {
                if (item.RelatedItem.Id == i.Id)
                    return true;
            }
            return false;
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

        private void cmboFormedBasket_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded && cmboFormedBasket.SelectedItem != null)
            {
                brdrBack.Visibility = Visibility.Collapsed;
                var id = (int)(cmboFormedBasket.SelectedItem as DataRowView)["Id"];
                var fb = FormedBasket.GetItemByID(id);
                this.DataContext = null;
                lbMainItems.ItemsSource = null;
                dgSelectedItems.ItemsSource = null;
                this.DataContext = fb;
                dgSelectedItems.ItemsSource = new List<FormedBasket_Item>();
                (dgSelectedItems.ItemsSource as List<FormedBasket_Item>).AddRange(fb.FormedBasketItems);
                lbMainItems.ItemsSource = (from x in Item.AllItems where !IsItemExist(x) select x).ToList<Item>();
                RefreshPanel();
                lbSectors.ItemsSource = null;
                lbSectors.ItemsSource = Sector.GetAllSectorByFormedBasketID(fb.Id.Value);
            }
        }


        private void rad_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                if (radUpdate.IsChecked == true)
                {
                    cmboFormedBasket.SelectedIndex = -1;
                    brdrBack.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.DataContext = new FormedBasket();
                    var fb = this.DataContext as FormedBasket;
                    dgSelectedItems.ItemsSource = new List<FormedBasket_Item>();
                    lbMainItems.ItemsSource = Item.AllItems;
                lbSectors.ItemsSource = null;
                    lbSectors.ItemsSource = Sector.GetAllSector();
                    RefreshPanel();
                }
            }
        }

        private void btnSelectAllSectors_Click(object sender, RoutedEventArgs e)
        {
            if (lbSectors.Items.Count > 0)
            {
                var lst = lbSectors.ItemsSource as List<Sector>;
                bool b = lst.Where(x => !x.IsSelected).Count() == lst.Count();
                foreach (var item in lst)
                    item.IsSelected = b;
            }
        }
    }
}