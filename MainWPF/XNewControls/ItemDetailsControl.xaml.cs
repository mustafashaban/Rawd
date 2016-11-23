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
    public partial class ItemDetailsControl : UserControl
    {
        List<Basket_Item> DeletedItems = new List<Basket_Item>();
        public ItemDetailsControl(Item i)
        {
            InitializeComponent();
            this.DataContext = i;
            dgRelatedItems.ItemsSource = i.BasketItems;
            if (!i.IsBasket)
            {
                btnSelectItems.Visibility = Visibility.Collapsed;
                dgRelatedItems.Visibility = Visibility.Collapsed;
                this.MaxWidth = 800;
            }
        }
        private void btnSelectItems_Click(object sender, RoutedEventArgs e)
        {
            var i = this.DataContext as Item;
            BasketItemsQuantityControl w = new BasketItemsQuantityControl(i);
            if (w.ShowDialog() == true)
            {
                this.DeletedItems = w.DeletedItems;
                dgRelatedItems.ItemsSource = null;
                dgRelatedItems.ItemsSource = i.BasketItems;
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var i = this.DataContext as Item;
            if (i.IsValidate())
            {
                if (!i.Id.HasValue)
                {
                    if (Item.InsertData(i))
                        MyMessage.InsertMessage();
                }
                else
                {
                    if (Item.UpdateData(i))
                        MyMessage.UpdateMessage();
                }

                foreach (var item in i.BasketItems)
                {
                    if (item.Basket == null)
                    {
                        item.Basket = i;
                        Basket_Item.InsertData(item);
                    }
                    else
                        Basket_Item.UpdateData(item);
                }
                foreach (var item in DeletedItems)
                {
                    Basket_Item.UpdateData(item);
                    Basket_Item.DeleteData(item);
                }
            }
        }
    }
}
