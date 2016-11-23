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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainWPF
{
    public partial class VoucherCriteriaControl : Window
    {
        Dictionary<int, List<Item>> VoucherGruopItems;
        public VoucherCriteriaControl()
        {
            InitializeComponent();

            VoucherGruopItems = new Dictionary<int, List<Item>>();
            var dt = BaseDataBase._Tabling("Select GroupID, ItemID from VoucherCriteria order by GroupID");
            int GroupID = 0;
            List<Item> items = null;
            foreach (DataRow item in dt.Rows)
            {
                int x = (int)item[0];
                if (x != GroupID)
                {
                    if (GroupID != 0)
                        VoucherGruopItems.Add(GroupID, items);
                    GroupID = x;
                    items = new List<Item>();
                }
                items.Add(Item.GetItemByID((int)item[1]));
            }
            if (GroupID != 0)
                VoucherGruopItems.Add(GroupID, items);
            lstMain.ItemsSource = VoucherGruopItems;
        }
        private void btnItemSelect_Click(object sender, RoutedEventArgs e)
        {
            var lst = lstItem.ItemsSource as List<Item>;
            if (lst.Count() == 0)
            {
                MyMessageBox.Show("لم يتم اختيار اي مادة");
                return;
            }
            if (VoucherGruopItems == null)
                VoucherGruopItems = new Dictionary<int, List<Item>>();
            VoucherGruopItems.Add(VoucherGruopItems.Count + 1, (lstItem.ItemsSource as List<Item>).Where(x => x.IsSelected).ToList());
            brdr.Visibility = Visibility.Collapsed;
            NumberGroup();
            lstMain.ItemsSource = null;
            lstMain.ItemsSource = VoucherGruopItems;
        }

        private void btnItemSelectCancel_Click(object sender, RoutedEventArgs e)
        {
            brdr.Visibility = Visibility.Collapsed;
        }

        private void btnAddGroup_Click(object sender, RoutedEventArgs e)
        {
            lstMain.SelectedIndex = -1;
            lstItem.ItemsSource = Item.AllItems;
            brdr.Visibility = Visibility.Visible;
        }

        private void btnUpdateGroup_Click(object sender, RoutedEventArgs e)
        {
            //if (lstMain.SelectedItem != null)
            //{
            //    var lst = Item.AllItems.Where(x => !x.IsFormed).ToList();
            //    var y = (KeyValuePair<int, List<Item>>)lstMain.SelectedItem;
            //    foreach (var i in lst)
            //    {
            //        foreach (var item in y.Value)
            //        {
            //            if (item.Id == i.Id)
            //            {
            //                i.IsFormed = true;
            //                break;
            //            }
            //        }
            //    }
            //    lstItem.ItemsSource = lst;
            //    brdr.Visibility = Visibility.Visible;
            //}
        }

        private void btnDeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            if (lstMain.SelectedItem != null)
            {
                var x = (KeyValuePair<int, List<Item>>)lstMain.SelectedItem;
                VoucherGruopItems.Remove(x.Key);
                NumberGroup();
                lstMain.ItemsSource = null;
                lstMain.ItemsSource = VoucherGruopItems;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string query = "delete from VoucherCriteria; \n";
            foreach (var item in VoucherGruopItems)
                foreach (var i in item.Value)
                    query += string.Format("insert into VoucherCriteria Values ({0},{1},{2}); \n", i.Id, item.Key, BaseDataBase.CurrentUser.ID);
            if (BaseDataBase._NonQuery(query) > 0)
            {
                MyMessage.InsertMessage();
                DialogResult = true;
            }
        }

        void NumberGroup()
        {
            int count = 1;
            var newValues = VoucherGruopItems.ToDictionary(x => count++, x => x.Value);
            VoucherGruopItems.Clear();
            foreach (var item in newValues) VoucherGruopItems.Add(item.Key, item.Value);
        }
    }
}
