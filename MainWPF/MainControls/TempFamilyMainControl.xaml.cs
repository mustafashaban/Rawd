using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for TempFamilyMainControl.xaml
    /// </summary>
    public partial class TempFamilyMainControl : UserControl
    {
        DataTable dt;
        DataTable dt2;
        int currentIndex = 1;
        public TempFamilyMainControl()
        {
            InitializeComponent();
        }

        private void SetItemsSource(int index)
        {
            if (dt2 == null)
            {
                dgTempFamilys.ItemsSource = null;
                txtDataIndex.Text = 0 + " من " + 0;
                return;
            }
            if (dt2.Rows.Count == 0)
            {
                dgTempFamilys.ItemsSource = dt2.DefaultView;
                txtDataIndex.Text = 0 + " من " + 0;
                return;
            }
            int total = dt2.Rows.Count / (int)100 + (dt2.Rows.Count / (double)100 - dt2.Rows.Count / 100 > 0 ? 1 : 0);
            dgTempFamilys.ItemsSource = dt2.Select().Skip((index - 1) * 100).Take(100).CopyToDataTable().DefaultView;
            txtDataIndex.Text = index + " من " + total;
            currentIndex = index;
        }

        private void btnAddNewTempFamily_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.CanEnterTempFamily)
            {
                MyMessageBox.Show("ليس لديك صلاحية اضافة عائلة مؤقتة");
                return;
            }

            string Header = "إضافة عائلة مؤقتة";
            MainWindow m = App.Current.MainWindow as MainWindow;
            if (m.CheckTabControl(Header))
            {
                TabItem ti = new TabItem();
                ti.Header = Header;
                var tfc = new TempFamilyControl(null);

                ti.Content = tfc;
                m.SendTabItem(ti);
            }
        }
        private void btnFixTempFamily_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.CanEnterFamily)
            {
                MyMessageBox.Show("ليس لديك صلاحية تثبيت عائلة");
                return;
            }
            if (dgTempFamilys.SelectedItem != null)
            {
                e.Handled = true;
                var id = (int)(dgTempFamilys.SelectedItem as DataRowView)["ID"];
                TempFamily tf = TempFamily.GetTempFamilyByID(id);
                if (tf.FamilyID.HasValue)
                {
                    MyMessageBox.Show("تم تثبيت هذه العائلة");
                    return;
                }
                if (tf.IsCancelled)
                {
                    MyMessageBox.Show("لا يمكن تثبيت العائلة لأنه تم إلغاؤها\n\nالسبب : \n" +
                                tf.CancelReason);
                    return;
                }
                if (!tf.FamilyID.HasValue)
                {
                    string Header = "تثبيت : " + tf.FamilyCode + " " + tf.FamilyName;
                    MainWindow m = App.Current.MainWindow as MainWindow;
                    if (m.CheckTabControl(Header))
                        m.SendTabItem(new TabItem() { Header = Header, Content = new AddFamilyControlHilal(tf) });
                }
                else
                {
                    if (MyMessageBox.Show("تم تثبيت هذه العائلة \nهل تريد الانتقال الى البيانات التفصيليلة لها", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        var f = Family.GetFamilyByID(tf.FamilyID.Value);
                        string Header = f.FamilyCode + " " + f.FamilyName;
                        MainWindow m = App.Current.MainWindow as MainWindow;
                        if (m.CheckTabControl(Header))
                        {
                            TabItem ti = new TabItem();
                            ti.Header = Header;
                            ti.Content = new AddFamilyControlHilal(f.FamilyID.Value);
                            m.SendTabItem(ti);
                        }
                    }
                }
            }
        }

        private void Control_Changed(object sender, TextChangedEventArgs e)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                dgTempFamilys.ItemsSource = null;
                txtDataIndex.Text = 0 + " من " + 0;
                return;
            }
            var dv = dt.DefaultView;
            dv.RowFilter = string.Format("FamilyCode Like '{0}%'", txtFamilyCode.Text);
            string s3 = txtFamilyName.Text;
            if (!string.IsNullOrEmpty(s3))
                dv.RowFilter += string.Format(" and (FamilyName like '*{0}*' or Father like '*{0}*' or Mother Like '*{0}*' or Father like '*{1}*' or Mother Like '*{1}*' or Father like '*{2}*' or Mother Like '*{2}*' or Father like '*{3}*' or Mother Like '*{3}*' or Father like '*{4}*' or Mother Like '*{4}*' or Father like '*{5}*' or Mother Like '*{5}*' or Father like '*{6}*' or Mother Like '*{6}*' or Father like '*{7}*' or Mother Like '*{7}*' or Father like '*{8}*' or Mother Like '*{8}*' )", s3, s3.Replace('أ', 'ا'), s3.Replace('ا', 'أ'), s3.Replace('ى', 'ا'), s3.Replace('ا', 'ى'), s3.Replace('آ', 'ا'), s3.Replace('ا', 'آ'), s3.Replace('ة', 'ه'), s3.Replace('ه', 'ة'));
            dt2 = dv.ToTable();
            SetItemsSource(1);
        }

        private void btnDelTempFamily_Click(object sender, RoutedEventArgs e)
        {
            if (dgTempFamilys.SelectedIndex != -1)
            {
                if (!BaseDataBase.CurrentUser.CanDeleteTempFamily)
                {
                    MyMessageBox.Show("ليس لديك صلاحيات للحذف");
                }
                else
                {
                    if (MyMessageBox.Show("هل تريد تأكيد حذف العائلة المؤقتة", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        var id = (int)(dgTempFamilys.SelectedItem as DataRowView)["ID"];
                        TempFamily tf = TempFamily.GetTempFamilyByID(id);
                        if (TempFamily.DeleteData(tf))
                        {
                            btnSearch_Click(null, null);
                            Control_Changed(null, null);
                            MyMessage.DeleteMessage();
                        }
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dt = dt2 = TempFamily.GetTempFamilyTable();
            SetItemsSource(1);
            Control_Changed(null, null);
        }

        private void dgTempFamilys_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgTempFamilys.SelectedItem != null)
            {
                e.Handled = true;
                var id = (int)(dgTempFamilys.SelectedItem as DataRowView)["ID"];
                TempFamily tf = TempFamily.GetTempFamilyByID(id);
                if (tf.IsCancelled)
                {
                    MyMessageBox.Show("لا يمكن تعديل بيانات العائلة لأنه تم إلغاؤها\n\nالسبب : \n" +
                                tf.CancelReason);
                    return;
                }
                if (!tf.FamilyID.HasValue)
                {
                    if (!BaseDataBase.CurrentUser.CanEnterTempFamily)
                    {
                        MyMessageBox.Show("ليس لديك صلاحية تعديل بيانات عائلة مؤقتة");
                        return;
                    }

                    string Header = "مؤقت : " + tf.FamilyCode + " " + tf.FamilyName;
                    MainWindow m = App.Current.MainWindow as MainWindow;
                    if (m.CheckTabControl(Header))
                    {
                        TabItem ti = new TabItem();
                        ti.Header = Header;
                        var tfc = new TempFamilyControl(tf);
                        ti.Content = tfc;

                        m.SendTabItem(ti);
                    }
                }
                else
                {
                    if (MyMessageBox.Show("تم تثبيت هذه العائلة \nهل تريد الانتقال الى البيانات التفصيليلة لها", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        if (!BaseDataBase.CurrentUser.CanUpdateFamily)
                        {
                            MyMessageBox.Show("ليس لديك صلاحية الانتقال للبيانات التفصيلية");
                            return;
                        }

                        var f = Family.GetFamilyByID(tf.FamilyID.Value);
                        string Header = f.FamilyCode + " " + f.FamilyName;
                        MainWindow m = App.Current.MainWindow as MainWindow;
                        if (m.CheckTabControl(Header))
                        {
                            TabItem ti = new TabItem();
                            ti.Header = Header;
                            ti.Content = new AddFamilyControlHilal(f.FamilyID.Value);
                            m.SendTabItem(ti);
                        }
                    }
                }
            }
        }

        private void btnCancelFamily_Click(object sender, RoutedEventArgs e)
        {
            if (dgTempFamilys.SelectedItem != null)
            {
                if (!BaseDataBase.CurrentUser.CanCancelTempFamily)
                {
                    MyMessageBox.Show("ليس لديك صلاحيات للإلغاء");
                }
                else
                {
                    var id = (int)(dgTempFamilys.SelectedItem as DataRowView)["ID"];
                    TempFamily tf = TempFamily.GetTempFamilyByID(id);
                    CancelTempFamilyWindow w = new CancelTempFamilyWindow(tf);
                    if (w.ShowDialog() == true)
                    {
                        (dgTempFamilys.Items[dgTempFamilys.SelectedIndex] as DataRowView).Row["IsCancelled"] = tf.IsCancelled;
                        dgTempFamilys.Items.Refresh();
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
        public static void CreateXPSDocument(DocumentPaginator paginator)
        {
            using (Package container = Package.Open("test" + ".xps", FileMode.Create))
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