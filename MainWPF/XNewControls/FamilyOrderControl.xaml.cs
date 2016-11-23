using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
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
    public partial class FamilyOrderControl : Window
    {
        List<Order_Item> DeletedItems = new List<Order_Item>();
        object o;
        int CurrentMaxOrderID;
        ZKFPEngXControl.ZKFPEngX ZFP { get { return BaseDataBase.ZFP; } }
        public FamilyOrderControl(Order order)
        {
            InitializeComponent();
            if (order.FamilyID.HasValue)
                int.TryParse(BaseDataBase._Scalar($"select Max(Id) from [order] where FamilyID = " + order.FamilyID), out CurrentMaxOrderID);
            else
                int.TryParse(BaseDataBase._Scalar($"select Max(Id) from [order] where SpecialFamilyID = " + order.SpecialFamilyID), out CurrentMaxOrderID);
            this.DataContext = order;
            dgOrderItems.ItemsSource = order.OIs;
            cmboInventory.ItemsSource = Inventory.GetAllInventory().Where(x => x.IsActive);
            if (!order.Id.HasValue)
            {
                order.Date = BaseDataBase.DateNow;
                if (order.Type == 3)
                {
                    // if the order is urgent no need to select NextOrderDate
                    if (!order.IsUrgentOrder)
                    {
                        order.NextOrderDate = order.Date.Value.AddDays(int.Parse(BaseDataBase._Scalar($"select dbo.GetNextOrderDaysByFamilyID({order.FamilyID})")));
                        //if the NextOrderDate is Friday or Thursday .. change it to Satruday
                        order.NextOrderDate = order.NextOrderDate.Value.DayOfWeek == DayOfWeek.Thursday || order.NextOrderDate.Value.DayOfWeek == DayOfWeek.Friday ? order.NextOrderDate.Value.AddDays(2) : order.NextOrderDate.Value;
                    }
                    btnSelectItems_Click(null, null);
                }
            }
            //if(!order.Id.HasValue)
            //        i.FamilyNeeds = FamilyNeed_ListerGroup.GetFamilyNeed_ListerGroupBySupportGroupID(-1, i.FamilyID.Value);
            //  else  i.FamilyNeeds = FamilyNeed_ListerGroup.GetFamilyNeed_ListerGroupBySupportGroupID(i.SupportGroupID, i.FamilyID.Value);
        }

        private void ZFP_OnCapture(bool ActionResult, object ATemplate)
        {
            o = new object();
            ZFP.GetFingerImage(ref o);
            MemoryStream ms = new MemoryStream(o as byte[]);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
            img.Source = bi;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (ZFP != null)
                ZFP.OnCapture -= ZFP_OnCapture;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
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
                    //                foreach (var item in x.FamilyNeeds)
                    //                    if (!item.IsEnsured)
                    //                        if (MyMessageBox.Show("لم يتم تأمين كامل احتياجات العائلة .. هل تريد المتابعة", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                    //                            return;
                    //                        break;
                    if (!order.Id.HasValue)
                    {
                        //check if there is any other order from other device
                        int CurrentMaxOrderID_temp = 0;
                        if (order.FamilyID.HasValue)
                            int.TryParse(BaseDataBase._Scalar("select Max(Id) from [order] where FamilyID=" + order.FamilyID), out CurrentMaxOrderID_temp);
                        else
                            int.TryParse(BaseDataBase._Scalar("select Max(Id) from [order] where SpecialFamilyID=" + order.SpecialFamilyID), out CurrentMaxOrderID_temp);
                        if (CurrentMaxOrderID_temp != 0 && CurrentMaxOrderID_temp != CurrentMaxOrderID)
                        {
                            MyMessageBox.Show("لقد تمت اضافة اعانة جديدة من جهاز اخر\nبجب اغلاق النافذة وتحديث البيانات للتحقق\nلايمكن الاضافة");
                            return;
                        }
                        if (Properties.Settings.Default.ActiveFingerPrint && o == null && MyMessageBox.Show("لم يتم اضافة البصمة الالكترونية هل تريد المتابعة", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                            return;
                        if (Order.InsertData(order))
                            SendFingerPrint(order);
                    }
                    else if (Order.UpdateData(order))
                        MyMessage.UpdateMessage();

                    Task.Run(() => InsertOrderItems(order));
                    //PrintOrder(order);

                    DialogResult = true;
                }
                catch (Exception ex) { MyMessageBox.Show(ex.Message); }
            }
        }

        async void InsertOrderItems(Order order)
        {
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
            await Dispatcher.BeginInvoke(new Action(() =>
            {
                PrintOrder(order);
            }), System.Windows.Threading.DispatcherPriority.Normal);
        }
        async void PrintOrder(Order order)
        {
            try
            {
                //if (MyMessageBox.Show("هل تريد طباعة وصل استلام؟", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (Properties.Settings.Default.VoucherType == 0)
                        PrintTicket.printReport(order, img.Source as BitmapImage);
                    else PrintTicket.printReportA6(order);
                }
            }
            catch (Exception ex) { MyMessageBox.Show("خطأ في الطباعة\n" + ex.Message); }
            await Task.Delay(10);
        }
        async void SendFingerPrint(Order order)
        {

            if (Properties.Settings.Default.ActiveFingerPrint)
            {
                try
                {
                    BinaryFormatter br = new BinaryFormatter();
                    TcpClient myclient = new TcpClient();
                    myclient.Connect("AL-IHSAN-ICT2", 7000);
                    NetworkStream myns = myclient.GetStream();
                    br.Serialize(myns, string.Format("{0}", order.Id.Value));
                    BinaryWriter mysw = new BinaryWriter(myns);
                    mysw.Write(o as byte[]);
                    mysw.Close();
                    myns.Close();
                    myclient.Close();
                }
                catch { MyMessageBox.Show("لايمكن الاتصال بمخدم البصمة الالكترونية\n\nتمت اضافة الاعانة ولم يتم اضافة البصمة الالكترونية"); }
                await Task.Delay(10);
            }
        }


        private void cmboInventory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                var order = this.DataContext as Order;
                if (order.Type != 1 && order.OIs.Count != 0)
                {
                    MyMessageBox.Show("سيتم تفريغ المواد ");
                    this.DeletedItems.AddRange(order.OIs.Where(x => x.Order != null));
                    order.OIs = null;
                    dgOrderItems.ItemsSource = null;
                }
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.ActiveFingerPrint)
            {
                try
                {
                    if (ZFP == null && !(await BaseDataBase.InitializeFingerPrintDevice()))
                        MyMessageBox.Show("جهاز البصامة الالكترونية غير جاهز");
                    if (!ZFP.Active)
                        MyMessageBox.Show("جهاز البصامة الالكترونية غير جاهز");
                    else
                        ZFP.OnCapture += ZFP_OnCapture;
                }
                catch { MyMessageBox.Show("حدث خطأ أثناء تشغيل البصامة الالكترونية"); }
            }
        }
    }
}

