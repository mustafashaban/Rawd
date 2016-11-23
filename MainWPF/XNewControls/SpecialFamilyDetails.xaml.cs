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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for SpecialFamilyDetails.xaml
    /// </summary>
    public partial class SpecialFamilyDetails : UserControl, INotifyPropertyChanged
    {
        public SpecialFamilyDetails(SpecialFamily x)
        {
            InitializeComponent();
            this.DataContext = x;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private void AddSupport_Click(object sender, RoutedEventArgs e)
        {
            var sf = this.DataContext as SpecialFamily;
            if (!sf.Id.HasValue)
            {
                MyMessageBox.Show("يجب حفظ بيانات العائلة اولاً");
                return;
            }
            if (!BaseDataBase.CurrentUser.CanPresent)
            {
                MyMessageBox.Show("ليس لديك صلاحية لاضافة اعانة");
            }
            else
            {
                int NumberOfDays = 0;
                try { NumberOfDays = int.Parse(BaseDataBase._Scalar("select NumberOfDays from NextOrderSpecialFamily")); }
                catch { }

                if (NumberOfDays != 0)
                {
                    DateTime? MaxOrderDate;
                    try { MaxOrderDate = (from x in (lvOrders.ItemsSource as List<Order>) where x.Date.HasValue select x.Date).Max(); }
                    catch { MaxOrderDate = null; }

                    if (MaxOrderDate.HasValue && (BaseDataBase.DateNow - MaxOrderDate.Value).Days < NumberOfDays)
                    {
                        //if (MyMessageBox.Show("متبقي " + (MaxOrderDate.Value - BaseDataBase.DateNow).Days + " ايام للاستلام القادم\nهل تريد المتابعة", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                        MyMessageBox.Show("متبقي " + (MaxOrderDate.Value - BaseDataBase.DateNow).Days + " ايام للاستلام القادم");
                        return;
                    }
                }

                Order o = new Order();
                o.Type = 4;
                o.SpecialFamilyID = sf.Id;
                FamilyOrderControl w = new FamilyOrderControl(o);
                w.cmboInventory.IsEnabled = true;
                if (w.ShowDialog() == true)
                {
                    sf.Orders.Add(w.DataContext as Order);
                    this.DataContext = null;
                    this.DataContext = sf;
                }
            }
        }

        private void UpdateSupport_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrders.SelectedIndex != -1)
            {
                if (!BaseDataBase.CurrentUser.CanUpdateSupport)
                {
                    MyMessageBox.Show("ليس لديك صلاحية لتعديل اعانة");
                }
                else
                {
                    Order o = lvOrders.SelectedItem as Order;
                    if (!BaseDataBase.CurrentUser.IsAdmin && o.LastUserID != BaseDataBase.CurrentUser.ID.Value)
                    {
                        MyMessageBox.Show("لايمكن تعديل التسليمات الا مسلم الاعانة فقط");
                        return;
                    }
                    if (!BaseDataBase.CurrentUser.IsAdmin && o.Date.Value.ToShortDateString() != BaseDataBase.DateNow.ToShortDateString())
                    {
                        MyMessageBox.Show("لايمكن تعديل التسليمات الا في يوم التسليم فقط");
                        return;
                    }
                    FamilyOrderControl w = new FamilyOrderControl(o);
                    if (w.ShowDialog() == true)
                    {
                        var sf = this.DataContext as SpecialFamily;
                        this.DataContext = null;
                        this.DataContext = sf;
                    }
                }
            }
        }

        private void btnDelSupport_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrders.SelectedIndex != -1)
            {
                if (!BaseDataBase.CurrentUser.CanDeleteSupport)
                {
                    MyMessageBox.Show("ليس لديك صلاحية لحذف الاعانة");
                    return;
                }
                var o = lvOrders.SelectedItem as Order;
                if (!BaseDataBase.CurrentUser.IsAdmin && o.LastUserID != BaseDataBase.CurrentUser.ID.Value)
                {
                    MyMessageBox.Show("لايمكن حذف التسليمات الا مسلم الحصة فقط");
                    return;
                }
                var diff = (BaseDataBase.DateNow - o.Date.Value).Days;
                if (!BaseDataBase.CurrentUser.IsAdmin && diff > 1)
                {
                    MyMessageBox.Show("ليس لديك صلاحية لحذف الاعانة");
                    return;
                }
                if (MyMessageBox.Show("هل تريد تأكيد حذف الإعانة", MessageBoxButton.YesNo) == MessageBoxResult.Yes && Order.DeleteData(o))
                {
                    MyMessage.DeleteMessage();
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var sf = this.DataContext as SpecialFamily;
            if (sf.IsValidate())
            {
                if (sf.Id.HasValue)
                {
                    if (SpecialFamily.UpdateData(sf))
                        MyMessage.UpdateMessage();
                }
                else if (SpecialFamily.InsertData(sf))
                    MyMessage.InsertMessage();
            }
        }

        private void lvOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateSupport_Click(null, null);
        }
    }
}
