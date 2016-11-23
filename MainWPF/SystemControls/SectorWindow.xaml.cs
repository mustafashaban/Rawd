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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for PlaceCodeWindow.xaml
    /// </summary>
    public partial class SectorWindow : Window
    {
        List<Inventory> inventoryAll;
        public List<Inventory> InventoryAll
        {
            get
            {
                if (inventoryAll == null)
                    inventoryAll = Inventory.GetAllInventory();
                return inventoryAll;
            }
        }
        public SectorWindow()
        {
            InitializeComponent();
            dgMain.ItemsSource = Sector.GetAllSector();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var s = dgMain.SelectedItem as Sector;
            if (s != null)
            {
                if (s.CanRemove())
                {
                    if (Sector.DeleteData(s))
                    {
                        dgMain.ItemsSource = null;
                        dgMain.ItemsSource = Sector.GetAllSector();
                        MyMessage.DeleteMessage();
                    }
                }
                else MyMessageBox.Show("لايمكن حذف القطاع بسبب وجود عوائل او سلل مواد مرتبطة به");
            }
        }

        private void dg_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var s = e.Row.DataContext as Sector;
                if (s != null && s.IsValidate())
                {
                    if (!s.ID.HasValue)
                    {
                        if (Sector.InsertData(s))
                            MyMessage.InsertMessage();
                        else
                            e.Cancel = true;
                    }
                    else
                    {
                        if (Sector.UpdateData(s))
                            MyMessage.UpdateMessage();
                        else
                            e.Cancel = true;
                    }
                }
            }
        }
    }
}
