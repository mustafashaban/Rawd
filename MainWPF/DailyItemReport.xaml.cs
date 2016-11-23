using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for DailyItemReport.xaml
    /// </summary>
    public partial class DailyItemReport : UserControl
    {
        public DailyItemReport()
        {
            InitializeComponent();
            dp1.SelectedDate = dp2.SelectedDate = DateTime.Now;
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            if (!dp1.SelectedDate.HasValue || !dp2.SelectedDate.HasValue || cmboInventory.SelectedIndex == -1)
            {
                MyMessageBox.Show("يجب اختيار تاريخ البداية وتاريخ النهاية واختيار المستودع لاظهار التقرير");
                return;
            }

            Items.ItemsSource = BaseDataBase._Tabling($@"select name ItemName, Quantity, StandardUnit from Item 
inner join (select ItemID, SUM(Quantity) Quantity from Order_Item 
where OrderID in (select ID from [Order] o where (Type = 3 or Type = 4) and InventoryID = {cmboInventory.SelectedValue} and convert(date,date) between '{dp1.SelectedDate.Value.ToString("MM-dd-yyy")}' and '{dp2.SelectedDate.Value.ToString("MM-dd-yyy")}')
group by ItemID) g on g.ItemID = Item.Id order by ItemName").DefaultView;

            //Items.ItemsSource = BaseDataBase._Tabling($@"select Inventory.Name InventoryName, Item.Name ItemName, Quantity, StandardUnit from Item
            //inner join 
            //(select InventoryID, ItemID, SUM(Quantity) Quantity 
            //from [Order] o 
            //inner join Order_Item oi 
            //on o.Id = oi.OrderID 
            //and InventoryID = {cmboInventory.SelectedValue} and convert(date,date) between '{dp1.SelectedDate.Value.ToString("MM-dd-yyy")}' and '{dp2.SelectedDate.Value.ToString("MM-dd-yyy")}'
            //and (o.Type = 3 or o.Type = 4)
            //group by InventoryID, ItemID) g 
            //on g.ItemID = Item.Id 
            //inner join Inventory
            //on g.InventoryID = Inventory.ID
            //order by InventoryName, ItemName").DefaultView;

            grdToPrint.Visibility = Items.Items.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

            txtDate1.Text = dp1.SelectedDate.Value.ToString("dd/MM/yyyy");
            txtDate2.Text = dp2.SelectedDate.Value.ToString("dd/MM/yyyy");
            txtInventoryHeader.Text = (cmboInventory.SelectedItem as Inventory).Name;

            txtHeader.Text = Properties.Settings.Default.ReportHeaderDescription;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(Properties.Settings.Default.AssociationLogoPath, UriKind.RelativeOrAbsolute);
            bitmap.EndInit();
            img1.Source = bitmap;
            img2.Source = bitmap;
            txtNow.Text = BaseDataBase.DateNow.ToString("dd/MM/yyyy hh:mm");
            txtUser.Text = BaseDataBase.CurrentUser.Name;
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            if (grdToPrint.Visibility == Visibility.Collapsed)
            {
                MyMessageBox.Show("يجب عرض التقرير اولاً");
                return;
            }

            //var uc = LoadTemplate(Path.Combine(Environment.CurrentDirectory, "VoucherReportTemplate.xaml")) as UserControl;

            // Create the print dialog object and set options
            PrintDialog pDialog = new PrintDialog();
            pDialog.PageRangeSelection = PageRangeSelection.AllPages;
            pDialog.UserPageRangeEnabled = true;
            pDialog.PrintQueue = System.Printing.LocalPrintServer.GetDefaultPrintQueue();
            pDialog.PrintTicket = pDialog.PrintQueue.DefaultPrintTicket;
            pDialog.PrintTicket.PageScalingFactor = 1;

            System.Printing.PrintCapabilities capabilities = null;
            try
            { capabilities = pDialog.PrintQueue.GetPrintCapabilities(pDialog.PrintTicket); }
            catch
            { capabilities = null; }
            sv.Content = null;
            Viewbox vb = new Viewbox();
            vb.Child = grdToPrint;

            System.Windows.Size sz = new Size(760, grdToPrint.ActualHeight);
            vb.MinWidth = 1;
            vb.MinHeight = 1;
            vb.Measure(sz);
            vb.Arrange(new System.Windows.Rect(new System.Windows.Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));

            pDialog.PrintVisual(vb, "MyViewBox");
            vb.Child = null;
            sv.Content = grdToPrint;
        }
    }
}
