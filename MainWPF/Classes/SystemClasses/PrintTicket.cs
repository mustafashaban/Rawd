using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;

namespace MainWPF
{
    public class PrintTicket
    {

        private static Rect GetPrintableArea(PrintDialog printDialog)
        {
            System.Printing.PrintCapabilities cap = null;
            try
            {
                cap = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);
            }
            catch
            {
                return Rect.Empty;
            }

            if (cap.PageImageableArea == null)
                return Rect.Empty;

            var leftMargin = cap.OrientedPageMediaWidth.HasValue ? (cap.OrientedPageMediaWidth.Value - cap.PageImageableArea.ExtentWidth) / 2 : 0;
            var topMargin = cap.OrientedPageMediaHeight.HasValue ? (cap.OrientedPageMediaHeight.Value - cap.PageImageableArea.ExtentHeight) / 2 : 0;
            var width = cap.PageImageableArea.ExtentWidth;
            var height = cap.PageImageableArea.ExtentHeight;
            return new Rect(leftMargin, topMargin, width, height);
        }


        public static void printReport(Order order, BitmapImage bi)
        {
            try
            {
                Family f = Family.GetFamilyByID(order.FamilyID.Value);
                Paragraph paragraph3 = new Paragraph();
                paragraph3.FlowDirection = FlowDirection.RightToLeft;
                paragraph3.Inlines.Add(new Bold(new Run(Properties.Settings.Default.VoucherHeaderText)));
                paragraph3.Inlines.Add(new LineBreak());
                paragraph3.Inlines.Add(new LineBreak());
                paragraph3.Inlines.Add(new Run("التاريخ " + order.Date.Value.ToString("dd/MM/yyyy hh:mm")));
                paragraph3.Inlines.Add(new LineBreak());
                paragraph3.Inlines.Add(new Run("المسلم " + User.GetUserNameByID(order.LastUserID)));
                paragraph3.Inlines.Add(new LineBreak());
                paragraph3.Inlines.Add(new Run("رمز العائلة " + f.FamilyCode));
                paragraph3.Inlines.Add(new LineBreak());
                paragraph3.Inlines.Add(new Run("اسم العائلة " + f.FamilyName));
                paragraph3.Inlines.Add(new LineBreak());
                if (order.NextOrderDate.HasValue)
                {
                    paragraph3.Inlines.Add(new Run("الاستلام القادم " + order.NextOrderDate.Value.ToString("dd-MM-yyyy")));
                    paragraph3.Inlines.Add(new LineBreak());
                }
                paragraph3.Inlines.Add(new Underline(new Run("المواد المسلّمة :")));
                paragraph3.Inlines.Add(new LineBreak());

                for (int i = 0; i < order.OIs.Count; i++)
                {
                    var x = order.OIs[i].Item;
                    paragraph3.Inlines.Add(new Run(x.Source + " " + x.Name + "\t (" + order.OIs[i].Quantity + ")"));
                    paragraph3.Inlines.Add(new LineBreak());
                }

                paragraph3.FontFamily = new FontFamily("Arial");
                paragraph3.FontSize = 16;

                FlowDocumentScrollViewer temp = new FlowDocumentScrollViewer();
                temp.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                FlowDocument myFlowDocument = new FlowDocument();
                temp.FlowDirection = FlowDirection.RightToLeft;
                myFlowDocument.FlowDirection = FlowDirection.RightToLeft;

                paragraph3.FlowDirection = FlowDirection.RightToLeft;
                paragraph3.Inlines.Add(new Underline(new Run("بصمة المستلم")));
                myFlowDocument.Blocks.Add(paragraph3);
                // myFlowDocument.Blocks.Add(paragraph4);
                myFlowDocument.Blocks.Add(new BlockUIContainer(new Image() { Source = bi, Height = 100, Stretch = Stretch.Uniform }));

                FlowDocumentReader myFlowDocumentReader = new FlowDocumentReader();
                myFlowDocumentReader.Document = myFlowDocument;

                myFlowDocumentReader.FlowDirection = FlowDirection.RightToLeft;
                temp.Document = myFlowDocument;

                temp.FlowDirection = FlowDirection.RightToLeft;
                temp.HorizontalAlignment = HorizontalAlignment.Center;
                temp.Width = 250;
                temp.Height = 285 + (order.OIs.Count * 21);

                PrintDialog pDialog = new PrintDialog();
                pDialog.PageRangeSelection = PageRangeSelection.AllPages;
                pDialog.UserPageRangeEnabled = true;
                pDialog.PrintQueue = System.Printing.LocalPrintServer.GetDefaultPrintQueue();
                pDialog.PrintTicket = pDialog.PrintQueue.DefaultPrintTicket;
                pDialog.PrintTicket.PageScalingFactor = 1;

                Rect printableArea = GetPrintableArea(pDialog);

                Viewbox viewBox = new Viewbox { Child = temp };
                printableArea.Height = 285 + (order.OIs.Count * 21);
                printableArea.Width = 250;

                viewBox.Measure(printableArea.Size);
                viewBox.Arrange(printableArea);
                pDialog.PrintVisual(viewBox, "Letter Canvas");
            }
            catch (Exception ex) { MyMessageBox.Show(ex.Message); }
        }


        public static void printReportNew(Order order)
        {
            Family f = Family.GetFamilyByID(order.FamilyID.Value);
            TextBlock tb = new TextBlock();
            tb.FontFamily = new System.Windows.Media.FontFamily("Arial");
            tb.Inlines.Add(new Bold(new Run("جمعية الإحسان الخيرية التنموية بحلب")));
            tb.Inlines.Add(new LineBreak());
            tb.Inlines.Add(new LineBreak());
            tb.Inlines.Add(new Run("التاريخ " + order.Date.Value.ToString("dd/MM/yyyy hh:mm")));
            tb.Inlines.Add(new LineBreak());
            tb.Inlines.Add(new Run("المسلم " + User.GetUserNameByID(order.LastUserID)));
            tb.Inlines.Add(new LineBreak());
            tb.Inlines.Add(new Run("رمز العائلة " + f.FamilyCode));
            tb.Inlines.Add(new LineBreak());
            tb.Inlines.Add(new Run("اسم العائلة " + f.FamilyName));
            tb.Inlines.Add(new LineBreak());
            if (order.NextOrderDate.HasValue)
            {
                tb.Inlines.Add(new Run("الاستلام القادم " + order.NextOrderDate.Value.ToString("dd-MM-yyyy")));
                tb.Inlines.Add(new LineBreak());
            }
            tb.Inlines.Add(new Underline(new Run("المواد المسلّمة :")));
            tb.Inlines.Add(new LineBreak());

            // Add some Bold text to the paragraph
            for (int i = 0; i < order.OIs.Count; i++)
            {
                var x = order.OIs[i].Item;
                tb.Inlines.Add(new Run(x.Source + " " + x.Name + "\t (" + order.OIs[i].Quantity + ")"));
                tb.Inlines.Add(new LineBreak());
            }
            tb.Margin = new Thickness(5, 2, 5, 2);
            tb.FontSize = 12;

            Grid g = new Grid();
            g.FlowDirection = FlowDirection.RightToLeft;
            g.Children.Add(tb);

            // Create the print dialog object and set options
            PrintDialog pDialog = new PrintDialog();
            pDialog.PageRangeSelection = PageRangeSelection.AllPages;
            pDialog.UserPageRangeEnabled = true;
            pDialog.PrintQueue = System.Printing.LocalPrintServer.GetDefaultPrintQueue();
            pDialog.PrintTicket = pDialog.PrintQueue.DefaultPrintTicket;
            pDialog.PrintTicket.PageScalingFactor = 1;

            System.Printing.PrintCapabilities capabilities = null;
            try
            {
                capabilities = pDialog.PrintQueue.GetPrintCapabilities(pDialog.PrintTicket);
            }
            catch
            {
                capabilities = null;
            }

            Viewbox vb = new Viewbox();
            vb.Child = g;

            System.Windows.Size sz = new System.Windows.Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);
            vb.MinWidth = 1;
            vb.MinHeight = 1;
            vb.Measure(sz);
            vb.Arrange(new System.Windows.Rect(new System.Windows.Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));

            double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / vb.ActualWidth, capabilities.PageImageableArea.ExtentHeight / vb.ActualHeight);
            vb.LayoutTransform = new ScaleTransform(scale, scale);

            pDialog.PrintVisual(vb, "MyViewBox");
        }

        internal static void printReportA6(Order o)
        {
            try
            {
                if (o.Type < 3)
                {
                    var uc = LoadTemplate(Path.Combine(Environment.CurrentDirectory, "VoucherReportTemplate.xaml")) as UserControl;
                    uc.DataContext = o;
                    (uc.FindName("Items") as ItemsControl).ItemsSource = from x in o.OIs select new { ItemName = x.Item.Name, Quantity = x.Quantity, StandardUnit = x.Item.StandardUnit };
                    (uc.FindName("txtTag") as TextBlock).Text = "1/1";
                    (uc.FindName("txtHeader") as TextBlock).Text = Properties.Settings.Default.VoucherHeaderText;
                    (uc.FindName("grdFamily") as Grid).Visibility = Visibility.Collapsed;
                    (uc.FindName("grdOrder") as Grid).Visibility = Visibility.Visible;

                    var img = uc.FindName("img") as Image;
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(Properties.Settings.Default.AssociationLogoPath, UriKind.RelativeOrAbsolute);
                    bitmap.EndInit();
                    img.Source = bitmap;

                    // Create the print dialog object and set options
                    PrintDialog pDialog = new PrintDialog();
                    pDialog.PageRangeSelection = PageRangeSelection.AllPages;
                    pDialog.UserPageRangeEnabled = true;
                    pDialog.PrintQueue = System.Printing.LocalPrintServer.GetDefaultPrintQueue();
                    pDialog.PrintTicket = pDialog.PrintQueue.DefaultPrintTicket;
                    pDialog.PrintTicket.PageScalingFactor = 1;

                    System.Printing.PrintCapabilities capabilities = null;
                    try
                    {
                        capabilities = pDialog.PrintQueue.GetPrintCapabilities(pDialog.PrintTicket);
                    }
                    catch
                    {
                        capabilities = null;
                    }
                    Viewbox vb = new Viewbox();
                    vb.Child = uc;

                    System.Windows.Size sz = new Size(520, 380);
                    vb.MinWidth = 1;
                    vb.MinHeight = 1;
                    vb.Measure(sz);
                    vb.Arrange(new System.Windows.Rect(new System.Windows.Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));

                    double scale = 1;
                    vb.LayoutTransform = new ScaleTransform(scale, scale);

                    pDialog.PrintVisual(vb, "MyViewBox");
                }
                else if (o.Type == 3)
                {
                    var dtVoucherCriteria = BaseDataBase._Tabling($@"select IsNull(GroupID,-1) GroupID,Item.Name ItemName, Quantity, StandardUnit from
                                    (select ItemID, Quantity from Order_Item where OrderID = {o.Id}) t1 
                                    inner join Item on t1.ItemID = item.Id
                                    left outer join VoucherCriteria t2
                                    on t1.ItemID = t2.ItemID");
                    if (dtVoucherCriteria != null && dtVoucherCriteria.Rows.Count > 0)
                    {
                        List<DataTable> subTables = dtVoucherCriteria.AsEnumerable().GroupBy(row => row.Field<int>("GroupID")).Select(g => g.CopyToDataTable()).ToList();
                        if (subTables.Count > 1)
                        { MyMessageBox.Show("ستم طباعة " + subTables.Count + " وصل"); }

                        var dt = BaseDataBase._Tabling($@"select OrderCode Id,[Order].Barcode, dbo.GetInventory(InventoryID) InventoryName,dbo.fn_getSectorByOrderID([Order].Id) Sector, 
	                            Family.FamilyID FamilyCode, 
	                            case when FatherName is not null then FatherName else (case when MotherName is not null then MotherName else FamilyName end) end as FatherName, 
	                            case when FatherPID is not null then FatherPID else (case when MotherPID is not null then MotherPID else 'لايوجد رقم وطني' end) end as PID, 
	                            NextOrderDate,
                                Date, Users.Name Presenter from [Order]
                                inner join Family on [Order].Id = {o.Id} and Family.FamilyID = [Order].FamilyID
                                inner join Users on Users.Id = [Order].LastUserID 
                                left outer join
                                (select FirstName + ' ' + IsNUll(LastName,'') FatherName ,PID FatherPID, FamilyID from Parent where Gender like N'ذكر') x 
                                on x.FamilyID = [Order].FamilyID
	                            left outer join
	                            (select FirstName + ' ' + IsNUll(LastName,'') MotherName ,PID MotherPID, FamilyID from Parent where Gender like N'أنثى') y 
                                on y.FamilyID = [Order].FamilyID");

                        for (int i = 0; i < subTables.Count; i++)
                        {
                            var uc = LoadTemplate(Path.Combine(Environment.CurrentDirectory, "VoucherReportTemplate.xaml")) as UserControl;
                            uc.DataContext = null;
                            uc.DataContext = dt.DefaultView;
                            (uc.FindName("Items") as ItemsControl).ItemsSource = subTables[i].DefaultView;
                            (uc.FindName("txtTag") as TextBlock).Text = (i + 1) + "/" + subTables.Count;
                            (uc.FindName("txtHeader") as TextBlock).Text = Properties.Settings.Default.VoucherHeaderText;

                            var img = uc.FindName("img") as Image;
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(Properties.Settings.Default.AssociationLogoPath, UriKind.RelativeOrAbsolute);
                            bitmap.EndInit();
                            img.Source = bitmap;

                            // Create the print dialog object and set options
                            PrintDialog pDialog = new PrintDialog();
                            pDialog.PageRangeSelection = PageRangeSelection.AllPages;
                            pDialog.UserPageRangeEnabled = true;
                            pDialog.PrintQueue = System.Printing.LocalPrintServer.GetDefaultPrintQueue();
                            pDialog.PrintTicket = pDialog.PrintQueue.DefaultPrintTicket;
                            pDialog.PrintTicket.PageScalingFactor = 1;

                            System.Printing.PrintCapabilities capabilities = null;
                            try
                            {
                                capabilities = pDialog.PrintQueue.GetPrintCapabilities(pDialog.PrintTicket);
                            }
                            catch
                            {
                                capabilities = null;
                            }
                            Viewbox vb = new Viewbox();
                            vb.Child = uc;

                            System.Windows.Size sz = new Size(520, 380);
                            vb.MinWidth = 1;
                            vb.MinHeight = 1;
                            vb.Measure(sz);
                            vb.Arrange(new System.Windows.Rect(new System.Windows.Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));

                            double scale = 1;
                            vb.LayoutTransform = new ScaleTransform(scale, scale);

                            pDialog.PrintVisual(vb, "MyViewBox");
                        }
                    }
                }
                else
                {
                    var dtVoucherCriteria = BaseDataBase._Tabling($@"select IsNull(GroupID,-1) GroupID,Item.Name ItemName, Quantity, StandardUnit from
                                    (select ItemID, Quantity from Order_Item where OrderID = {o.Id}) t1 
                                    inner join Item on t1.ItemID = item.Id
                                    left outer join VoucherCriteria t2
                                    on t1.ItemID = t2.ItemID");
                    if (dtVoucherCriteria != null && dtVoucherCriteria.Rows.Count > 0)
                    {
                        List<DataTable> subTables = dtVoucherCriteria.AsEnumerable().GroupBy(row => row.Field<int>("GroupID")).Select(g => g.CopyToDataTable()).ToList();
                        if (subTables.Count > 1)
                        { MyMessageBox.Show("ستم طباعة " + subTables.Count + " وصل"); }

                        var dt = BaseDataBase._Tabling($@"select OrderCode Id,[Order].Barcode, dbo.GetInventory(InventoryID) InventoryName,'عائلة خاصة' Sector, SpecialFamily.Id FamilyCode, SpecialFamily.Name FatherName, PID,
                                            Date, Users.Name Presenter from [Order]
                                            inner join SpecialFamily on [Order].Id = {o.Id} and SpecialFamily.Id = [Order].SpecialFamilyID
                                            inner join Users on Users.Id = [Order].LastUserID");


                        for (int i = 0; i < subTables.Count; i++)
                        {
                            var uc = LoadTemplate(Path.Combine(Environment.CurrentDirectory, "VoucherReportTemplate.xaml")) as UserControl;
                            uc.DataContext = null;
                            uc.DataContext = dt;
                            (uc.FindName("Items") as ItemsControl).ItemsSource = subTables[i].DefaultView;
                            (uc.FindName("txtTag") as TextBlock).Text = (i + 1) + "/" + subTables.Count;
                            (uc.FindName("txtHeader") as TextBlock).Text = Properties.Settings.Default.VoucherHeaderText;
                            //(uc.FindName("Items") as ItemsControl).ItemsSource = from x in o.OIs select new { ItemName = x.Item.Name, Quantity = x.Quantity, StandardUnit = x.Item.StandardUnit };
                            //(uc.FindName("txtTag") as TextBlock).Text = "1/1";

                            var img = uc.FindName("img") as Image;
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(Properties.Settings.Default.AssociationLogoPath, UriKind.RelativeOrAbsolute);
                            bitmap.EndInit();
                            img.Source = bitmap;

                            // Create the print dialog object and set options
                            PrintDialog pDialog = new PrintDialog();
                            pDialog.PageRangeSelection = PageRangeSelection.AllPages;
                            pDialog.UserPageRangeEnabled = true;
                            pDialog.PrintQueue = System.Printing.LocalPrintServer.GetDefaultPrintQueue();
                            pDialog.PrintTicket = pDialog.PrintQueue.DefaultPrintTicket;
                            pDialog.PrintTicket.PageScalingFactor = 1;

                            System.Printing.PrintCapabilities capabilities = null;
                            try
                            {
                                capabilities = pDialog.PrintQueue.GetPrintCapabilities(pDialog.PrintTicket);
                            }
                            catch
                            {
                                capabilities = null;
                            }
                            Viewbox vb = new Viewbox();
                            vb.Child = uc;

                            System.Windows.Size sz = new Size(520, 380);
                            vb.MinWidth = 1;
                            vb.MinHeight = 1;
                            vb.Measure(sz);
                            vb.Arrange(new System.Windows.Rect(new System.Windows.Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));

                            double scale = 1;
                            vb.LayoutTransform = new ScaleTransform(scale, scale);

                            pDialog.PrintVisual(vb, "MyViewBox");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(ex.Message);
            }
        }
        public static object LoadTemplate(string templatePath)
        {
            object template;

            // get the needed template paths
            string absolutePath = Path.GetFullPath(templatePath);
            string directoryPath = Path.GetDirectoryName(absolutePath);

            using (FileStream inputStream = File.OpenRead(absolutePath))
            {
                var pc = new ParserContext
                {
                    // It is critical to have the trailing backslash here
                    // will not work without it!
                    BaseUri = new Uri(directoryPath + "\\")
                };
                template = XamlReader.Load(inputStream, pc);
            }
            return template;
        }
    }
}
