using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for ReportAllControl.xaml
    /// </summary>
    public partial class ReportAllControl : UserControl
    {
        public ReportAllControl()
        {
            InitializeComponent();
            ucCurrent = null;
        }
        List<TempFamily> tfs;
        List<Family> fs;
        string FileName;

        DataGrid dgmain;
        DataGrid DGMain
        {
            get { return dgmain; }
            set
            {
                dgmain = value;
                value.ClipboardCopyMode = DataGridClipboardCopyMode.None;
                if (value != null)
                {
                    value.Loaded += DGMain_Loaded;
                }
            }
        }

        void DGMain_Loaded(object sender, RoutedEventArgs e)
        {
            var dv = DGMain.ItemsSource as DataView;
            if (dv != null)
            {
                txtFieldCount.Text = "عدد السجلات " + dv.Count;
                dv.ListChanged += dv_ListChanged;
            }
        }
        void dv_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (DGMain != null)
            {
                txtFieldCount.Text = "عدد السجلات " + (sender as DataView).Count;
            }
        }

        UserControl uccurrent;
        UserControl ucCurrent
        {
            get { return uccurrent; }
            set
            {
                uccurrent = value;
                if (value != null)
                    DGMain = value.FindName("dgMain") as DataGrid;
            }
        }

        private void cmboMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmbo = sender as ComboBox;

            grdContainer.Children.Remove(ucCurrent);
            ucCurrent = null;

            switch (cmbo.SelectedIndex)
            {
                case 0:
                    ucCurrent = new ReportTempFamilyControlAll();
                    grdContainer.Children.Add(ucCurrent);
                    break;
                case 1:
                    ucCurrent = new ReportFixedFamilyControlAll();
                    grdContainer.Children.Add(ucCurrent);
                    break;
                default:
                    break;
            }
        }
        private void btnPrintSelected_Click(object sender, RoutedEventArgs e)
        {
            if (cmboPrintType.SelectedIndex == 0)
            {
                if (DGMain.SelectedItems.Count > 0)
                {
                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.DefaultExt = ".xps";
                    dlg.Filter = "XPS Print Documents (.xps)|*.xps";

                    if (dlg.ShowDialog() == true)
                    {
                        tfs = new List<TempFamily>();
                        FileName = dlg.FileName;
                        if (FileName.Substring(FileName.LastIndexOf('.'), FileName.Length - FileName.LastIndexOf('.')) != ".xps")
                        {
                            FileName += ".xps";
                        }
                        foreach (DataRowView item in dgmain.SelectedItems)
                        {
                            tfs.Add(TempFamily.GetTempFamilyByID((int)item[0]));
                        }
                        try
                        {
                            FixedDocument fd = new FixedDocument();
                            fd.DocumentPaginator.PageSize = new Size(1056, 768);
                            int index = 0;

                            foreach (var f in tfs)
                            {
                                index++;
                                var pc = LoadTemplate(Path.Combine(Environment.CurrentDirectory, "TempFamilyReportTemplate.xaml")) as PageContent;

                                if (pc != null)
                                {
                                    pc.Child.Margin = new Thickness(25);
                                    var img = pc.FindName("img") as Image;
                                    if (img != null)
                                    {
                                        img.Stretch = Stretch.Fill;
                                        BitmapImage bi = new BitmapImage();
                                        bi.BeginInit();
                                        bi.UriSource = new Uri(Properties.Settings.Default.AssociationLogoPath, UriKind.RelativeOrAbsolute);
                                        bi.EndInit();
                                        img.Source = bi;
                                    }
                                    var txtUser = pc.FindName("txtUser") as TextBlock;
                                    if (txtUser != null)
                                        txtUser.Text = BaseDataBase.CurrentUser.Name;

                                    var txt = pc.FindName("txtMainDescription") as TextBlock;
                                    if (txt != null)
                                        txt.Text = Properties.Settings.Default.ReportHeaderDescription;

                                    var gridChild = pc.Child.FindName("grdChilds") as Grid;
                                    if (gridChild != null)
                                    {
                                        int i = 1;
                                        foreach (var tc in f.TempChilds)
                                        {
                                            TextBlock tb1 = new TextBlock();
                                            tb1.Text = tc.Name;
                                            tb1.SetValue(Grid.ColumnProperty, 1);
                                            tb1.SetValue(Grid.RowProperty, i);

                                            TextBlock tb2 = new TextBlock();
                                            tb2.Text = tc.Gender;
                                            tb2.SetValue(Grid.ColumnProperty, 2);
                                            tb2.SetValue(Grid.RowProperty, i);

                                            TextBlock tb3 = new TextBlock();
                                            tb3.Text = tc.DOB.Value.ToString("dd/MM/yyyy");
                                            tb3.SetValue(Grid.ColumnProperty, 4);
                                            tb3.SetValue(Grid.RowProperty, i);


                                            gridChild.Children.Add(tb1);
                                            gridChild.Children.Add(tb2);
                                            gridChild.Children.Add(tb3);
                                            i++;
                                        }
                                    }

                                    pc.Child.DataContext = null;
                                    pc.Child.DataContext = f;
                                    fd.Pages.Add(pc);
                                }
                            }
                            CreateXPSDocument(fd.DocumentPaginator, FileName);

                            foreach (var item in tfs)
                            {
                                item.Printer = BaseDataBase.CurrentUser.Name;
                                item.IsPrinted = true;
                                TempFamily.UpadteData(item);
                            }
                            if (cmboPrintType.SelectedIndex == 0)
                                DGMain.ItemsSource = BaseDataBase._TablingStoredProcedure("GetTempFamilyAllTableForPrint").DefaultView;
                            MyMessage.CustomMessage("تمت الطباعة بنجاح");
                        }
                        catch (Exception ex)
                        {
                            MyMessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            else
            {
                if (DGMain.SelectedItems.Count > 0)
                {
                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.DefaultExt = ".xps";
                    dlg.Filter = "XPS Print Documents (.xps)|*.xps";

                    if (dlg.ShowDialog() == true)
                    {
                        fs = new List<Family>();
                        FileName = dlg.FileName;
                        if (FileName.Substring(FileName.LastIndexOf('.'), FileName.Length - FileName.LastIndexOf('.')) != ".xps")
                        {
                            FileName += ".xps";
                        }
                        foreach (DataRowView item in dgmain.SelectedItems)
                        {
                            fs.Add(Family.GetFamilyByID((int)item[0]));
                        }
                        try
                        {
                            FixedDocument fd = new FixedDocument();
                            fd.DocumentPaginator.PageSize = new Size(1056, 768);
                            int index = 0;

                            foreach (var f in fs)
                            {
                                index++;
                                var pc = LoadTemplate(Path.Combine(Environment.CurrentDirectory, "FamilyReportTemplate.xaml")) as PageContent;

                                if (pc != null)
                                {
                                    pc.Child.Margin = new Thickness(25);
                                    var img = pc.FindName("img") as Image;
                                    if (img != null)
                                    {
                                        img.Stretch = Stretch.Fill;
                                        BitmapImage bi = new BitmapImage();
                                        bi.BeginInit();
                                        bi.UriSource = new Uri(Properties.Settings.Default.AssociationLogoPath, UriKind.RelativeOrAbsolute);
                                        bi.EndInit();
                                        img.Source = bi;
                                    }
                                    var txt = pc.FindName("txtMainDescription") as TextBlock;
                                    if (txt != null)
                                        txt.Text = Properties.Settings.Default.ReportHeaderDescription;

                                    var gridHouse = pc.Child.FindName("grdHouse") as Grid;
                                    if (gridHouse != null)
                                    {
                                        var hs = House.GetHouseAllByFamilyID(f.FamilyID);
                                        if (hs != null && hs.Count > 0)
                                            gridHouse.DataContext = hs.Last();
                                    }
                                    var gridChild = pc.Child.FindName("grdChilds") as Grid;
                                    if (gridChild != null)
                                    {
                                        int i = 1;
                                        foreach (var tc in FamilyPerson.GetFamilyPersonByFamilyID(f.FamilyID.Value))
                                        {
                                            TextBlock tb1 = new TextBlock();
                                            tb1.Text = tc.FirstName;
                                            tb1.SetValue(Grid.ColumnProperty, 1);
                                            tb1.SetValue(Grid.RowProperty, i);

                                            TextBlock tb2 = new TextBlock();
                                            tb2.Text = tc.Gender;
                                            tb2.SetValue(Grid.ColumnProperty, 2);
                                            tb2.SetValue(Grid.RowProperty, i);

                                            TextBlock tb3 = new TextBlock();
                                            tb3.Text = tc.RelationShip;
                                            tb3.SetValue(Grid.ColumnProperty, 3);
                                            tb3.SetValue(Grid.RowProperty, i);

                                            TextBlock tb4 = new TextBlock();
                                            tb4.Text = tc.DOB.Value.ToString("dd/MM/yyyy");
                                            tb4.SetValue(Grid.ColumnProperty, 4);
                                            tb4.SetValue(Grid.RowProperty, i);

                                            TextBlock tb5 = new TextBlock();
                                            tb5.Text = tc.StudyStatus;
                                            tb5.SetValue(Grid.ColumnProperty, 5);
                                            tb5.SetValue(Grid.RowProperty, i);

                                            TextBlock tb6 = new TextBlock();
                                            tb6.Text = tc.HealthStatus;
                                            tb6.SetValue(Grid.ColumnProperty, 6);
                                            tb6.SetValue(Grid.RowProperty, i);

                                            TextBlock tb7 = new TextBlock();
                                            tb7.Text = tc.MaritalStatus;
                                            tb7.SetValue(Grid.ColumnProperty, 7);
                                            tb7.SetValue(Grid.RowProperty, i);

                                            TextBlock tb8 = new TextBlock();
                                            tb8.Text = tc.Job;
                                            tb8.SetValue(Grid.ColumnProperty, 8);
                                            tb8.SetValue(Grid.RowProperty, i);

                                            gridChild.Children.Add(tb1);
                                            gridChild.Children.Add(tb2);
                                            gridChild.Children.Add(tb3);
                                            gridChild.Children.Add(tb4);
                                            gridChild.Children.Add(tb5);
                                            gridChild.Children.Add(tb6);
                                            gridChild.Children.Add(tb7);
                                            gridChild.Children.Add(tb8);
                                            i++;
                                        }
                                    }

                                    pc.Child.DataContext = null;
                                    pc.Child.DataContext = f;
                                    fd.Pages.Add(pc);
                                }
                            }
                            CreateXPSDocument(fd.DocumentPaginator, FileName);
                            
                            MyMessage.CustomMessage("تمت الطباعة بنجاح");
                        }
                        catch (Exception ex)
                        {
                            MyMessageBox.Show(ex.Message);
                        }
                    }
                }
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
        public static void CreateXPSDocument(DocumentPaginator paginator, string FileNamePath)
        {
            using (Package container = Package.Open(FileNamePath, FileMode.Create))
            {
                using (XpsDocument xpsDoc = new XpsDocument(container, CompressionOption.Maximum))
                {
                    XpsSerializationManager xpsSM = new XpsSerializationManager(new XpsPackagingPolicy(xpsDoc), false);
                    xpsSM.SaveAsXaml(paginator);
                }
            }
        }
        public static void ConvertToXps(FixedDocument fixedDoc, Stream outputStream)
        {
            var package = Package.Open(outputStream, FileMode.Create);
            var xpsDoc = new XpsDocument(package, CompressionOption.Normal);
            XpsDocumentWriter xpsWriter = XpsDocument.CreateXpsDocumentWriter(xpsDoc);

            // xps documents are built using fixed document sequences
            var fixedDocSeq = new FixedDocumentSequence();
            var docRef = new DocumentReference();
            docRef.BeginInit();
            docRef.SetDocument(fixedDoc);
            docRef.EndInit();
            ((IAddChild)fixedDocSeq).AddChild(docRef);

            // write out our fixed document to xps
            xpsWriter.Write(fixedDocSeq.DocumentPaginator);

            xpsDoc.Close();
            package.Close();
        }

        private void btnPrintAll_Click(object sender, RoutedEventArgs e)
        {
            if (cmboPrintType.SelectedIndex == 0)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.DefaultExt = ".xps";
                dlg.Filter = "XPS Print Documents (.xps)|*.xps";

                if (dlg.ShowDialog() == true)
                {
                    tfs = new List<TempFamily>();
                    FileName = dlg.FileName;
                    if (FileName.Substring(FileName.LastIndexOf('.'), FileName.Length - FileName.LastIndexOf('.')) != ".xps")
                    {
                        FileName += ".xps";
                    }
                    foreach (DataRowView item in dgmain.ItemsSource as DataView)
                    {
                        tfs.Add(TempFamily.GetTempFamilyByID((int)item[0]));
                    }
                    try
                    {
                        FixedDocument fd = new FixedDocument();
                        fd.DocumentPaginator.PageSize = new Size(1056, 768);
                        int index = 0;

                        foreach (var f in tfs)
                        {
                            index++;
                            var pc = LoadTemplate(Path.Combine(Environment.CurrentDirectory, "TempFamilyReportTemplate.xaml")) as PageContent;

                            if (pc != null)
                            {
                                pc.Child.Margin = new Thickness(25);
                                var img = pc.FindName("img") as Image;
                                if (img != null)
                                {
                                    img.Stretch = Stretch.Fill;
                                    BitmapImage bi = new BitmapImage();
                                    bi.BeginInit();
                                    bi.UriSource = new Uri(Properties.Settings.Default.AssociationLogoPath, UriKind.RelativeOrAbsolute);
                                    bi.EndInit();
                                    img.Source = bi;
                                }
                                var txtUser = pc.FindName("txtUser") as TextBlock;
                                if (txtUser != null)
                                    txtUser.Text = BaseDataBase.CurrentUser.Name;

                                var txt = pc.FindName("txtMainDescription") as TextBlock;
                                if (txt != null)
                                    txt.Text = Properties.Settings.Default.ReportHeaderDescription;

                                var gridChild = pc.Child.FindName("grdChilds") as Grid;
                                if (gridChild != null)
                                {
                                    int i = 1;
                                    foreach (var tc in f.TempChilds)
                                    {
                                        TextBlock tb1 = new TextBlock();
                                        tb1.Text = tc.Name;
                                        tb1.SetValue(Grid.ColumnProperty, 1);
                                        tb1.SetValue(Grid.RowProperty, i);

                                        TextBlock tb2 = new TextBlock();
                                        tb2.Text = tc.Gender;
                                        tb2.SetValue(Grid.ColumnProperty, 2);
                                        tb2.SetValue(Grid.RowProperty, i);

                                        TextBlock tb3 = new TextBlock();
                                        tb3.Text = tc.DOB.Value.ToString("dd/MM/yyyy");
                                        tb3.SetValue(Grid.ColumnProperty, 4);
                                        tb3.SetValue(Grid.RowProperty, i);


                                        gridChild.Children.Add(tb1);
                                        gridChild.Children.Add(tb2);
                                        gridChild.Children.Add(tb3);
                                        i++;
                                    }
                                }

                                pc.Child.DataContext = null;
                                pc.Child.DataContext = f;
                                fd.Pages.Add(pc);
                            }
                        }
                        CreateXPSDocument(fd.DocumentPaginator, FileName);

                        foreach (var item in tfs)
                        {
                            item.Printer = BaseDataBase.CurrentUser.Name;
                            item.IsPrinted = true;
                            TempFamily.UpadteData(item);
                        }
                        MyMessageBox.Show("تمت الطباعة بنجاح");
                    }
                    catch (Exception ex)
                    {
                        MyMessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.DefaultExt = ".xps";
                dlg.Filter = "XPS Print Documents (.xps)|*.xps";

                if (dlg.ShowDialog() == true)
                {
                    fs = new List<Family>();
                    FileName = dlg.FileName;
                    if (FileName.Substring(FileName.LastIndexOf('.'), FileName.Length - FileName.LastIndexOf('.')) != ".xps")
                    {
                        FileName += ".xps";
                    }
                    foreach (DataRowView item in dgmain.ItemsSource as DataView)
                    {
                        fs.Add(Family.GetFamilyByID((int)item[0]));
                    }
                    try
                    {
                        FixedDocument fd = new FixedDocument();
                        fd.DocumentPaginator.PageSize = new Size(1056, 768);
                        int index = 0;

                        foreach (var f in fs)
                        {
                            index++;
                            var pc = LoadTemplate(Path.Combine(Environment.CurrentDirectory, "FamilyReportTemplate.xaml")) as PageContent;

                            if (pc != null)
                            {
                                pc.Child.Margin = new Thickness(25);
                                var img = pc.FindName("img") as Image;
                                if (img != null)
                                {
                                    img.Stretch = Stretch.Fill;
                                    BitmapImage bi = new BitmapImage();
                                    bi.BeginInit();
                                    bi.UriSource = new Uri(Properties.Settings.Default.AssociationLogoPath, UriKind.RelativeOrAbsolute);
                                    bi.EndInit();
                                    img.Source = bi;
                                }
                                var txt = pc.FindName("txtMainDescription") as TextBlock;
                                if (txt != null)
                                    txt.Text = Properties.Settings.Default.ReportHeaderDescription;

                                var gridHouse = pc.Child.FindName("grdHouse") as Grid;
                                if (gridHouse != null)
                                {
                                    var hs = House.GetHouseAllByFamilyID(f.FamilyID);
                                    if (hs != null && hs.Count > 0)
                                        gridHouse.DataContext = hs.Last();
                                }
                                var gridChild = pc.Child.FindName("grdChilds") as Grid;
                                if (gridChild != null)
                                {
                                    int i = 1;
                                    foreach (var tc in FamilyPerson.GetFamilyPersonByFamilyID(f.FamilyID.Value))
                                    {
                                        TextBlock tb1 = new TextBlock();
                                        tb1.Text = tc.FirstName;
                                        tb1.SetValue(Grid.ColumnProperty, 1);
                                        tb1.SetValue(Grid.RowProperty, i);

                                        TextBlock tb2 = new TextBlock();
                                        tb2.Text = tc.Gender;
                                        tb2.SetValue(Grid.ColumnProperty, 2);
                                        tb2.SetValue(Grid.RowProperty, i);

                                        TextBlock tb3 = new TextBlock();
                                        tb3.Text = tc.RelationShip;
                                        tb3.SetValue(Grid.ColumnProperty, 3);
                                        tb3.SetValue(Grid.RowProperty, i);

                                        TextBlock tb4 = new TextBlock();
                                        tb4.Text = tc.DOB.Value.ToString("dd/MM/yyyy");
                                        tb4.SetValue(Grid.ColumnProperty, 4);
                                        tb4.SetValue(Grid.RowProperty, i);

                                        TextBlock tb5 = new TextBlock();
                                        tb5.Text = tc.StudyStatus;
                                        tb5.SetValue(Grid.ColumnProperty, 5);
                                        tb5.SetValue(Grid.RowProperty, i);

                                        TextBlock tb6 = new TextBlock();
                                        tb6.Text = tc.HealthStatus;
                                        tb6.SetValue(Grid.ColumnProperty, 6);
                                        tb6.SetValue(Grid.RowProperty, i);

                                        TextBlock tb7 = new TextBlock();
                                        tb7.Text = tc.MaritalStatus;
                                        tb7.SetValue(Grid.ColumnProperty, 7);
                                        tb7.SetValue(Grid.RowProperty, i);

                                        TextBlock tb8 = new TextBlock();
                                        tb8.Text = tc.Job;
                                        tb8.SetValue(Grid.ColumnProperty, 8);
                                        tb8.SetValue(Grid.RowProperty, i);

                                        gridChild.Children.Add(tb1);
                                        gridChild.Children.Add(tb2);
                                        gridChild.Children.Add(tb3);
                                        gridChild.Children.Add(tb4);
                                        gridChild.Children.Add(tb5);
                                        gridChild.Children.Add(tb6);
                                        gridChild.Children.Add(tb7);
                                        gridChild.Children.Add(tb8);
                                        i++;
                                    }
                                }

                                pc.Child.DataContext = null;
                                pc.Child.DataContext = f;
                                fd.Pages.Add(pc);
                            }
                        }
                        CreateXPSDocument(fd.DocumentPaginator, FileName);

                        MyMessageBox.Show("تمت الطباعة بنجاح");
                    }
                    catch (Exception ex)
                    {
                        MyMessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void btn_ReportHeader_Click(object sender, RoutedEventArgs e)
        {
            ReportHeaderWindow w = new ReportHeaderWindow();
            w.ShowDialog();
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            switch (cmboPrintType.SelectedIndex)
            {
                case 0:
                    DGMain.ItemsSource = BaseDataBase._TablingStoredProcedure("GetTempFamilyAllTableForPrint").DefaultView;
                    var uc1 = ucCurrent as ReportTempFamilyControlAll;
                    if (uc1 != null)
                        uc1.Changing();
                    DGMain_Loaded(DGMain, null);
                    break;
                case 1:
                    DGMain.ItemsSource = BaseDataBase._TablingStoredProcedure("GetFixedFamilyAllTableForPrint").DefaultView;
                    var uc2 = ucCurrent as ReportFixedFamilyControlAll;
                    if (uc2 != null)
                        uc2.Changing();
                    DGMain_Loaded(DGMain, null);
                    break;
            }
        }
    }
}
