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
    public partial class FamilyOrderContainer : UserControl
    {
        public FamilyOrderContainer()
        {
            InitializeComponent();
        }

        private int? orphanID;
        public int? OrphanID
        {
            get { return orphanID; }
            set
            {
                orphanID = value;
                lvOrders.ItemsSource = Order.GetAllOrderByOrphanID(value.Value);
                if (value != null)
                {
                    if (lvOrders.Items.Count == 0)
                        BaseDataBase.MakeTabItemRed(this.Parent as TabItem);
                    else
                        BaseDataBase.MakeTabItemGreen(this.Parent as TabItem);
                }
            }
        }
        int? familyID;
        public int? FamilyID
        {
            get { return familyID; }
            set
            {
                familyID = value;
                lvOrders.ItemsSource = Order.GetAllOrderByFamilyID(value.Value).OrderByDescending(x => x.Date);
                GetTotal();
                if (value != null)
                {
                    if (lvOrders.Items.Count == 0)
                        BaseDataBase.MakeTabItemRed(this.Parent as TabItem);
                    else
                        BaseDataBase.MakeTabItemGreen(this.Parent as TabItem);
                }
            }
        }


        async void GetTotal()
        {
            dgTotalQuantities.ItemsSource = null;
            var dt = await BaseDataBase._TablingAsync($@"select Name Item, Total from item inner join 
                    (select ItemID, SUM(Quantity) Total from Order_Item where OrderID in
                    (select id from[Order] where FamilyID = {FamilyID})
                    group by ItemID) g
                    on g.ItemId = item.id");
            dgTotalQuantities.ItemsSource = dt.DefaultView;
        }


        private void AddSupport_Click(object sender, RoutedEventArgs e)
        {
            if (!CanPresent())
            {
                MyMessageBox.Show("لا يمكن التسليم حاليا لهذا القطاع");
                return;
            }
            if (!(BaseDataBase.CurrentUser.CanPresent || BaseDataBase.CurrentUser.PointAdmin))
                MyMessageBox.Show("ليس لديك صلاحية لاضافة اعانة");
            else
            {
                DateTime? MaxNextOrderDate;
                try
                {
                    MaxNextOrderDate = (from x in (lvOrders.ItemsSource as List<Order>) where x.NextOrderDate.HasValue select x.NextOrderDate).Max();
                }
                catch { MaxNextOrderDate = null; }
                if (MaxNextOrderDate.HasValue && BaseDataBase.DateNow < MaxNextOrderDate.Value)
                {
                    string s = "متبقي " + (MaxNextOrderDate.Value - BaseDataBase.DateNow).Days + " ايام للاستلام القادم\nتاريخ الاستلام القادم " + MaxNextOrderDate.Value.ToShortDateString();

                    int gapDays = 0;
                    int.TryParse(SystemProperties.GetSystemPropertyValue(SystemProperties.Property.NextOrderGap), out gapDays);

                    if (gapDays >= (MaxNextOrderDate.Value - BaseDataBase.DateNow).Days)
                    {
                        s += "\nويوجد " + gapDays + " ايام مسموح فيها التسليم. هل تريد المتابعة";
                        if (MyMessageBox.Show(s, MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                            return;
                    }
                    else if (!BaseDataBase.CurrentUser.PointAdmin)
                    {
                        s += "\nلا يمكن اضافة اعانة حالياً";
                        MyMessageBox.Show(s);
                        return;
                    }
                    else MyMessageBox.Show(s);
                }

                Order o = new Order();
                o.Type = 3;
                o.OrphanID = this.OrphanID;
                o.FamilyID = this.FamilyID;
                o.InventoryID = Sector.GetInventoryByFamilyID(o.FamilyID);
                FamilyOrderControl w = new FamilyOrderControl(o);
                if (w.ShowDialog() == true)
                {
                    MyMessage.InsertMessage();
                    var main = App.Current.MainWindow as MainWindow;
                    main.tcMain.Items.Remove(main.tcMain.SelectedItem);
                    if (main.tcMain.Items.Count == 0)
                        main.ShowMainControls();
                    //var orders = lvOrders.ItemsSource as List<Order>;
                    //orders.Insert(0, o);
                    //GetTotal();
                }
            }
        }


        private void UpdateSupport_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrders.SelectedIndex != -1)
            {
                if (!CanPresent())
                {
                    MyMessageBox.Show("لا يمكن التسليم حاليا لهذا القطاع");
                    return;
                }
                if (!(BaseDataBase.CurrentUser.CanUpdateSupport || BaseDataBase.CurrentUser.PointAdmin))
                    MyMessageBox.Show("ليس لديك صلاحية لتعديل اعانة");
                else
                {
                    Order o = lvOrders.SelectedItem as Order;
                    if (o.Date.Value.ToShortDateString() != BaseDataBase.DateNow.ToShortDateString())
                    {
                        MyMessageBox.Show("لايمكن تعديل التسليمات الا في يوم التسليم فقط");
                        return;
                    }
                    FamilyOrderControl w = new FamilyOrderControl(o);
                    if (w.ShowDialog() == true)
                    {
                        if (FamilyID != null) FamilyID = FamilyID;
                        else OrphanID = OrphanID;
                    }
                }
            }
        }
        private void btnDelSupport_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrders.SelectedIndex != -1)
            {
                if (!(BaseDataBase.CurrentUser.CanDeleteSupport || BaseDataBase.CurrentUser.PointAdmin))
                {
                    MyMessageBox.Show("ليس لديك صلاحية لحذف الاعانة");
                    return;
                }
                if (!CanPresent())
                {
                    MyMessageBox.Show("لا يمكن التسليم حاليا لهذا القطاع");
                    return;
                }
                var o = lvOrders.SelectedItem as Order;
                string s = "";
                var diff = (BaseDataBase.DateNow - o.Date.Value).Days;
                if (diff > 1)
                    s = "مضى على تسليم الاعانة اكثر من يوم، ";

                s += "هل تريد تأكيد حذف الإعانة";
                if (MyMessageBox.Show(s, MessageBoxButton.YesNo) == MessageBoxResult.Yes && Order.DeleteData(o))
                {
                    MyMessage.DeleteMessage();
                    //if (FamilyID != null) FamilyID = FamilyID;
                    //else OrphanID = OrphanID;

                    var orders = lvOrders.ItemsSource as List<Order>;
                    orders.Remove(o);
                    //lvOrders.Items.Remove(o);
                    lvOrders.Items.Refresh();
                    GetTotal();
                }
            }
        }

        private void AddUgentSupport_Click(object sender, RoutedEventArgs e)
        {
            if (!(BaseDataBase.CurrentUser.CanPresent || BaseDataBase.CurrentUser.PointAdmin))
                MyMessageBox.Show("ليس لديك صلاحية لاضافة اعانة");
            else if (!CanPresent())
                MyMessageBox.Show("لا يمكن التسليم حاليا لهذا القطاع");
            else
            {
                Order o = new Order();
                o.Type = 3;
                o.OrphanID = this.OrphanID;
                o.IsUrgentOrder = true;
                o.FamilyID = this.FamilyID;
                o.InventoryID = Sector.GetInventoryByFamilyID(o.FamilyID);
                FamilyOrderControl w = new FamilyOrderControl(o);
                if (w.ShowDialog() == true)
                {
                    var main = App.Current.MainWindow as MainWindow;
                    main.tcMain.Items.Remove(main.tcMain.SelectedItem);
                    if (main.tcMain.Items.Count == 0)
                        main.ShowMainControls();
                }
            }
        }

        bool CanPresent()
        {
            var s = BaseDataBase._Scalar($"select isnull(IsPresentable,0) from Sector where id = (select SectorID from Family where FamilyID = {FamilyID})");
            if (!string.IsNullOrEmpty(s))
                return bool.Parse(s);
            return true;
        }

        private void lvOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateSupport_Click(null, null);
        }
    }
}
