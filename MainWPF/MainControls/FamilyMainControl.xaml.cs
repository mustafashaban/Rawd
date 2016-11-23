using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Packaging;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for FamilyMainControl.xaml
    /// </summary>
    public partial class FamilyMainControl : UserControl
    {
        DataTable dt;
        DataTable dt2;
        BackgroundWorker bw = new BackgroundWorker();
        bool IsFenished = true, IsFirstLoad = true;
        int currentIndex = 1;

        string s0, s1, s2, s3, s4;
        bool isWorking = false;
        public FamilyMainControl()
        {
            InitializeComponent();
            bw.WorkerSupportsCancellation = true;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.DoWork += bw_DoWork;
            cmboSector.ItemsSource = BaseDataBase.GetAllStringsWithAll("select Name from Sector");

            var ilst = Inventory.AllInventories;
            ilst.Insert(0, new Inventory() { ID = 0, Name = "الكل" });
            cmboInventory.ItemsSource = ilst;
        }

        private void SetItemsSource(int index)
        {
            if (dt2 == null)
            {
                dgFamily.ItemsSource = null;
                txtDataIndex.Text = 0 + " من " + 0;
                return;
            }
            if (dt2.Rows.Count == 0)
            {
                dgFamily.ItemsSource = dt2.DefaultView;
                txtDataIndex.Text = 0 + " من " + 0;
                return;
            }
            int total = dt2.Rows.Count / (int)100 + (dt2.Rows.Count / (double)100 - dt2.Rows.Count / 100 > 0 ? 1 : 0);
            dgFamily.ItemsSource = dt2.Select().Skip((index - 1) * 100).Take(100).CopyToDataTable().DefaultView;
            txtDataIndex.Text = index + " من " + total;
            currentIndex = index;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
                Control_Changed(null, null);
        }

        private void Control_Changed(object sender, TextChangedEventArgs e)
        {
            if (IsFirstLoad)
                IsFirstLoad = false;
            else
            {
                s0 = BaseDataBase.IsStringNumber(txtSpecialCardCode.Text) ? txtSpecialCardCode.Text : "";
                s1 = txtFamilyCode.Text;
                s2 = txtFamilyName.Text;
                s3 = txtPID.Text;
                s4 = cmboSector.SelectedIndex > 0 ? cmboSector.SelectedItem?.ToString() : "";
                btnSelectFile_Click(null, null);
            }
        }
        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            if (dgFamily.ItemsSource != null)
            {
                bw.CancelAsync();
                if (!bw.IsBusy)
                {
                    IsFenished = true;
                    bw.RunWorkerAsync(dt);
                }
                else
                    IsFenished = false;
            }
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!isWorking)
            {
                if (cmboInventory.SelectedItem == null)
                {
                    MyMessageBox.Show("يجب اختيار المستودع");
                    return;
                }
                isWorking = true;
                Storyboard sb = (App.Current.Resources["sbRotateButton"] as Storyboard).Clone();
                sb.SetValue(Storyboard.TargetProperty, sender);
                sb.Begin();
                dt = await BaseDataBase._TablingStoredProcedureAsync("sp_GetFamilyAllTable", new SqlParameter("@InventoryID", (cmboInventory.SelectedItem as Inventory).ID));
                await Task.Run(() => dt2 = dt.Copy());
                sb.Pause();
                SetItemsSource(1);
                Control_Changed(null, null);
                isWorking = false;
            }
        }
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var dt = e.Argument as DataTable;
                dt2 = dt.Copy();
                var dv = dt2.DefaultView;

                string s = string.Format("(FamilyCode like '{0}*'", s1);
                s += BaseDataBase.IsStringNumber(s1) ? string.Format(" or FamilyID = {0})", s1) : ")";

                dv.RowFilter = s;
                if (!string.IsNullOrEmpty(s0))
                    dv.RowFilter += string.Format(" and SpecialCardCode like '{0}*' ", s0);
                if (!string.IsNullOrEmpty(s2))
                    dv.RowFilter += string.Format(" and (FamilyName like '*{0}*' or Father like '*{0}*' or Mother Like '*{0}*' or Father like '*{1}*' or Mother Like '*{1}*' or Father like '*{2}*' or Mother Like '*{2}*' or Father like '*{3}*' or Mother Like '*{3}*' or Father like '*{4}*' or Mother Like '*{4}*' or Father like '*{5}*' or Mother Like '*{5}*' or Father like '*{6}*' or Mother Like '*{6}*' or Father like '*{7}*' or Mother Like '*{7}*' or Father like '*{8}*' or Mother Like '*{8}*' )", s2, s2.Replace('أ', 'ا'), s2.Replace('ا', 'أ'), s2.Replace('ى', 'ا'), s2.Replace('ا', 'ى'), s2.Replace('آ', 'ا'), s2.Replace('ا', 'آ'), s2.Replace('ة', 'ه'), s2.Replace('ه', 'ة'));
                if (!string.IsNullOrEmpty(s3))
                    dv.RowFilter += string.Format(" and (FatherNa like '{0}*' or MotherNa Like '{0}*')", s3);
                //if (s4 != "0")
                //    dv.RowFilter += string.Format(" and IsCanceled = {0}", s4 == "1" ? 0 : 1);
                if (!string.IsNullOrEmpty(s4))
                    dv.RowFilter += string.Format($" and SectorName like '{s4}'");
                dt2 = dt2.DefaultView.ToTable();
            }
            catch { bw.CancelAsync(); }
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (IsFenished)
            {
                SetItemsSource(1);
            }
            else
            {
                btnSelectFile_Click(null, null);
            }
        }

        private void btnAddFamilyData_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.CanEnterFamily)
            {
                MyMessageBox.Show("ليس لديك صلاحيات للدخول");
            }
            else
            {
                string Header = "إضافة عائلة";
                MainWindow m = App.Current.MainWindow as MainWindow;
                if (m.CheckTabControl(Header))
                {
                    TabItem ti = new TabItem();
                    ti.Header = Header;
                    if (BaseDataBase.IsHilal)
                    {
                        var x = new AddFamilyControlHilal();
                        x.Margin = new Thickness(-25);
                        ti.Content = x;
                    }
                    else
                        ti.Content = new AddFamilyControl();
                    m.SendTabItem(ti);
                }
            }
        }
        private void btnUpdateFamilyData_Click(object sender, RoutedEventArgs e)
        {
            if (dgFamily.SelectedIndex != -1)
            {
                if (!BaseDataBase.CurrentUser.CanUpdate)
                {
                    MyMessageBox.Show("ليس لديك صلاحيات للدخول");
                }
                else
                {
                    Family f = Family.GetFamilyCancelDataByID((int)(dgFamily.SelectedItem as DataRowView)[0]);
                    if (f.IsCanceled == true)
                    {
                        if (BaseDataBase.CurrentUser.IsAdmin)
                            MyMessageBox.Show("العائلة التي تم اختيارها تم الغاؤها بسبب : \n" + f.CancelReason);
                        else
                        {
                            MyMessageBox.Show("لا يمكن تعديل بيانات العائلة لأنه تم إلغاؤها\n\nالسبب : \n" + f.CancelReason);
                            return;
                        }
                    }
                    if (!(bool)(dgFamily.SelectedItem as DataRowView)["IsActiveSector"])
                    {
                        if (BaseDataBase.CurrentUser.IsAdmin)
                            MyMessageBox.Show("القطاع التابع للعائلة التي تم اختيارها غير مفعل");
                        else
                        {
                            MyMessageBox.Show("لا يمكن تعديل بيانات العائلة بسبب الغاء تفعيل فطاع " + (dgFamily.SelectedItem as DataRowView)["SectorName"]);
                            return;
                        }
                    }
                    string Header = (dgFamily.SelectedItem as DataRowView)[1].ToString() + " " + (dgFamily.SelectedItem as DataRowView)[2].ToString();
                    TabItem ti = new TabItem();
                    ti.Header = Header;
                    var x = new AddFamilyControlHilal((int)(dgFamily.SelectedItem as DataRowView)[0]);
                    x.Margin = new Thickness(-25);
                    ti.Content = x;
                    MainWindow m = App.Current.MainWindow as MainWindow;
                    m.SendTabItem(ti);
                }
            }
        }
        private void dgFamily_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            btnUpdateFamilyData_Click(null, null);
        }

        private void btnTempFamilyData_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.CanEnterTempFamily)
            {
                MyMessageBox.Show("ليس لديك صلاحيات للدخول");
            }
            else
            {
                string Header = "عوائل مؤقتة";
                MainWindow m = App.Current.MainWindow as MainWindow;
                if (m.CheckTabControl(Header))
                {
                    TabItem ti = new TabItem();
                    ti.Header = Header;
                    ti.Content = new TempFamilyMainControl();
                    m.SendTabItem(ti);
                }
            }
        }

        private void btnDeleteFamilyData_Click(object sender, RoutedEventArgs e)
        {
            if (dgFamily.SelectedIndex != -1)
            {
                if (!BaseDataBase.CurrentUser.PointAdmin)
                {
                    MyMessageBox.Show("ليس لديك صلاحيات للحذف");
                }
                else
                {
                    Family f = Family.GetFamilyByID((int)(dgFamily.Items[dgFamily.SelectedIndex] as DataRowView)[0]);
                    if (MyMessageBox.Show("هل تريد تأكيد حذف بيانات العائلة", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        if (DBMain.DeleteData(f))
                        {
                            dt = BaseDataBase._TablingStoredProcedure("sp_GetFamilyAllTable");
                            SetItemsSource(1);
                            Control_Changed(null, null);
                            MyMessage.DeleteMessage();
                        }
                    }
                }
            }
        }
        private void btnCancelFamilyData_Click(object sender, RoutedEventArgs e)
        {
            if (dgFamily.SelectedIndex != -1)
            {
                if (!BaseDataBase.CurrentUser.CanCancelFamily)
                {
                    MyMessageBox.Show("ليس لديك صلاحيات للإلغاء");
                }
                else
                {
                    CancelFamilyWindow w = new CancelFamilyWindow((int)(dgFamily.Items[dgFamily.SelectedIndex] as DataRowView)[0]);
                    if (w.ShowDialog() == true)
                    {
                        (dgFamily.Items[dgFamily.SelectedIndex] as DataRowView).Row["IsCanceled"] = ((w.DataContext as Family).IsCanceled);
                        // (dgFamily.Items[dgFamily.SelectedIndex] as DataRowView).Row["IsAcquittance"] = ((w.DataContext as Family).IsAcquittance);
                        dgFamily.Items.Refresh();
                    }
                }
            }
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    {
                        if (dgFamily.ItemsSource == null)
                        {
                            btnSearch_Click(null, null);
                        }
                        else
                        {
                            btnUpdateFamilyData_Click(null, null);
                        }
                        break;
                    }
            }
        }

        private void CommandBindingNew_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnAddFamilyData_Click(null, null);
        }
        private void CommandBindingSelectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnSearch_Click(null, null);
        }
        private void CommandBindingFind_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Keyboard.FocusedElement == txtFamilyCode)
                Keyboard.Focus(txtFamilyName);
            else
                Keyboard.Focus(txtFamilyCode);
        }
        private void CommandBindingOpen_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnUpdateFamilyData_Click(null, null);
        }

        private void btnPrintSelected_Click(object sender, RoutedEventArgs e)
        {
            if (dgFamily.SelectedIndex != -1)
            {
                if (!BaseDataBase.CurrentUser.CanPrintReports)
                {
                    MyMessageBox.Show("ليس لديك صلاحية طباعة");
                    return;
                }
                try
                {
                    Family f = Family.GetFamilyByID((int)(dgFamily.Items[dgFamily.SelectedIndex] as DataRowView)[0]);

                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.FileName = "استمارة " + f.FamilyCode;
                    dlg.DefaultExt = ".xps";
                    dlg.Filter = "XPS Print Documents (.xps)|*.xps";

                    if (dlg.ShowDialog() == true)
                    {
                        string filename = dlg.FileName;
                        if (filename.Substring(filename.LastIndexOf('.'), filename.Length - filename.LastIndexOf('.')) != ".xps")
                        {
                            filename += ".xps";
                        }
                        FixedDocument fd = new FixedDocument();
                        fd.DocumentPaginator.PageSize = new Size(1056, 768);
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
                            var txtUser = pc.FindName("txtUser") as TextBlock;
                            if (txtUser != null)
                                txtUser.Text = BaseDataBase.CurrentUser.Name;

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
                        CreateXPSDocument(fd.DocumentPaginator, filename);
                        MyMessage.CustomMessage("تمت الطباعة بنجاح");
                    }
                }
                catch (Exception ex)
                {
                    MyMessageBox.Show(ex.Message);
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

        private void btnPrintAll_Click(object sender, RoutedEventArgs e)
        {
            if (BaseDataBase.CurrentUser.CanPrintReports)
            {
                string Header = "استمارات الطباعة";
                MainWindow m = App.Current.MainWindow as MainWindow;
                if (m.CheckTabControl(Header))
                {
                    TabItem ti = new TabItem();
                    ti.Header = Header;
                    ti.Content = new ReportAllControl();

                    m.SendTabItem(ti);
                }
            }
            else MyMessageBox.Show("ليس لديك صلاحية طباعة");
        }

        private void btnNextData_Click(object sender, RoutedEventArgs e)
        {
            if (dt != null)
            {
                int total = dt2.Rows.Count / (int)100 + (dt2.Rows.Count / (double)100 - dt2.Rows.Count / 100 > 0 ? 1 : 0);
                if (currentIndex + 1 <= total)
                {
                    SetItemsSource(currentIndex + 1);
                }
            }
        }

        private void btnSpecialFamily_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.PointAdmin)
            {
                MyMessageBox.Show("ليس لديك صلاحية دخول");
                return;
            }
            var main = App.Current.MainWindow as MainWindow;
            main.SendTabItem(new TabItem() { Header = "عوائل خاصة", Content = new SpecialFamilyControl() });
        }

        private void btnSpecialCard_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.PointAdmin)
            {
                MyMessageBox.Show("ليس لديك صلاحية للدخول");
                return;
            }
            var main = App.Current.MainWindow as MainWindow;
            main.SendTabItem(new TabItem() { Header = "بطاقات خاصة", Content = new SpecialCardMainControl() });
        }

        private void btnFirstData_Click(object sender, RoutedEventArgs e)
        {
            SetItemsSource(1);
        }

        private void btnPreviousData_Click(object sender, RoutedEventArgs e)
        {
            if ((currentIndex - 1) > 0)
            {
                SetItemsSource(currentIndex - 1);
            }
        }

        private void btnLastData_Click(object sender, RoutedEventArgs e)
        {
            if (dt != null)
            {
                int lastIndex = dt2.Rows.Count / (int)100 + (dt2.Rows.Count / (double)100 - dt2.Rows.Count / 100 > 0 ? 1 : 0);
                SetItemsSource(lastIndex == 0 ? 1 : lastIndex);
            }
        }
    }
}
