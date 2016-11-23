using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    public partial class InventoryControl : UserControl
    {
        public InventoryControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dg.ItemsSource = Inventory.GetAllInventory();
        }

        private async void dg_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var i = e.Row.DataContext as Inventory;
                if (i != null && i.IsValidate())
                {
                    if (!i.ID.HasValue)
                    {
                        if (Inventory.InsertData(i))
                            MyMessage.InsertMessage();
                        else
                            e.Cancel = true;
                    }
                    else
                    {
                        if (await Inventory.UpdateData(i))
                            MyMessage.UpdateMessage();
                        else
                            e.Cancel = true;
                    }
                }
            }
        }
        bool isWorking = false;
        private async void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var i = dg.SelectedItem as Inventory;
            if (i != null && i.ID.HasValue)
            {
                if (!isWorking)
                {
                    List<Item> lst = null;
                    dgRelatedItems.ItemsSource = null;
                    while (!isWorking || (dg.SelectedItem as Inventory).ID != i.ID)
                    {
                        isWorking = true;
                        i = dg.SelectedItem as Inventory;
                        await Task.Run(async () =>
                        {
                            lst = await Item.GetAllItemsByInventory(i.ID.Value);
                        });
                    }
                    dgRelatedItems.ItemsSource = lst;
                    isWorking = false;
                }
            }
        }

        private async void btnDeleteInventory_Click(object sender, RoutedEventArgs e)
        {
            var i = dg.SelectedItem as Inventory;
            if (i != null)
            {
                if (i.CanRemove())
                {
                    if (await Inventory.DeleteData(i))
                    {
                        dg.ItemsSource = null;
                        dg.ItemsSource = Inventory.GetAllInventory();
                        MyMessage.DeleteMessage();
                    }
                }
                else MyMessageBox.Show("لايمكن حذف المستودع بسبب وجود أوامر ادخال او اخراج مرتبطة به");
            }
        }
    }
}
