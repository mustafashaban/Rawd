using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Xsl;


namespace MainWPF
{
    /// <summary>
    /// Interaction logic for StatisticsControl.xaml
    /// </summary>
    public partial class StatisticsControl : UserControl
    {
        public StatisticsControl()
        {
            InitializeComponent();

            GetData();
            // ((LineSeries)mcChart.Series[0]).ItemsSource =
            //new KeyValuePair<DateTime, int>[]{
            // new KeyValuePair<DateTime, int>(DateTime.Now, 100),
            // new KeyValuePair<DateTime, int>(DateTime.Now.AddMonths(1), 130),
            // new KeyValuePair<DateTime, int>(DateTime.Now.AddMonths(2), 150),
            // new KeyValuePair<DateTime, int>(DateTime.Now.AddMonths(3), 125),
            // new KeyValuePair<DateTime, int>(DateTime.Now.AddMonths(4),155) };
        }

        async void GetData()
        {
            DataTable dt = await BaseDataBase._TablingStoredProcedureAsync("GetStatisticsItemsCount", new SqlParameter("@Date1", BaseDataBase.DateNow.ToString("MM-dd-yyyy 00:00")), new SqlParameter("@Date2", BaseDataBase.DateNow.ToString("MM-dd-yyyy 23:59")));
            //DataTable dt2 = await BaseDataBase._TablingAsync(@"select * from
            //    (select 'العوائل الكلية' Name, COUNT(*) Number,1 ID from Family
            //    Union
            //    select 'العوائل الغير ملغاة', COUNT(*), 2 from Family where IsCanceled <> 1
            //    Union
            //    select 'العوائل الملغاة', COUNT(*), 3 from Family where IsCanceled = 1
            //    ) a
            //    order by ID");
            DataTable dt2 = await BaseDataBase._TablingAsync(@"
                select Name, Number from Sector inner join
                (select SectorID,COUNT(*) Number from Family group by SectorID) g
                on g.SectorID = Sector.ID and IsActive = 1 order by Number DESC");
            grdStaMain.DataContext = new StatisticsClass(dt);
            grdStaFamily.DataContext = new StatisticsClass(dt2);
        }
        private void btnColumnSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var control = tcMain.SelectedItem as TabItem;
                var DGMain = (control.Content as FrameworkElement).FindName("dgMain") as DataGrid;
                if (DGMain.ItemsSource != null)
                {
                    ColumnsSelectWindow w = new ColumnsSelectWindow();
                    foreach (var item in DGMain.Columns)
                    {
                        item.IsReadOnly = (item.Visibility == System.Windows.Visibility.Visible) ? true : false;
                    }
                    w.lst.ItemsSource = DGMain.Columns;
                    if (w.ShowDialog() == true)
                    {
                        foreach (var item in DGMain.Columns)
                        {
                            item.Visibility = (item.IsReadOnly) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.CanExport)
            {
                MyMessageBox.Show("ليس لديك صلاحية تصدير");
                return;
            }
            var control = tcMain.SelectedItem as TabItem;
            var DGMain = (control.Content as FrameworkElement).FindName("dgMain") as DataGrid;
            if (DGMain.Items.Count > 0)
            {
                if (BaseDataBase.CurrentUser.IsAdmin)
                {
                    var dt = (DGMain.ItemsSource as DataView).Table;
                    ExportToExcel.ExportDataTableToExcel x = new ExportToExcel.ExportDataTableToExcel(dt);
                    x.GenerateReport();
                }
                else
                {
                    MyMessageBox.Show("لا يوجد لديك صلاحيات للتصدير");
                }
            }
        }
        private void btnExportSelected_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.CanExport)
            {
                MyMessageBox.Show("ليس لديك صلاحية تصدير");
                return;
            }
            var control = tcMain.SelectedItem as TabItem;
            var DGMain = (control.Content as FrameworkElement).FindName("dgMain") as DataGrid;
            if (DGMain.SelectedItems.Count > 0)
            {
                if (BaseDataBase.CurrentUser.IsAdmin)
                {
                    var dtx = (DGMain.ItemsSource as DataView).Table;
                    if (dtx.Rows.Count > 0)
                    {
                        var dt = dtx.Clone();
                        for (int i = 0; i < DGMain.SelectedItems.Count; i++)
                        {
                            var dr = dt.NewRow();
                            object[] os = new object[dt.Columns.Count];
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                os[j] = (DGMain.SelectedItems[i] as DataRowView).Row.ItemArray[j];
                            }
                            dr.ItemArray = os;
                            dt.Rows.Add(dr);
                        }

                        List<int> indexes = new List<int>();
                        for (int i = 0; i < DGMain.Columns.Count; i++)
                        {
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                if (DGMain.Columns[i].SortMemberPath == dt.Columns[j].ColumnName)
                                {
                                    if (!DGMain.Columns[i].IsReadOnly)
                                    {
                                        dt.Columns.Remove(dt.Columns[j]);
                                        break;
                                    }
                                    else
                                    {
                                        dt.Columns[j].ColumnName = DGMain.Columns[i].Header.ToString();
                                    }
                                }
                            }
                        }
                        ExportToExcel.ExportDataTableToExcel x = new ExportToExcel.ExportDataTableToExcel(dt);
                        x.GenerateReport();
                    }
                }
                else
                {
                    MyMessageBox.Show("لا يوجد لديك صلاحيات للتصدير");
                }
            }
        }
        private async void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tcMain.SelectedIndex == 0)
                grdBottom.Visibility = System.Windows.Visibility.Collapsed;
            if (tcMain.SelectedIndex > 0)
            {
                grdBottom.Visibility = System.Windows.Visibility.Visible;
                var control = tcMain.SelectedItem as TabItem;
                var DGMain = (control.Content as FrameworkElement).FindName("dgMain") as DataGrid;
                grdBottom.DataContext = DGMain;
                if (DGMain.ItemsSource == null)
                {
                    switch (tcMain.SelectedIndex)
                    {
                        case 1:
                            DGMain.ItemsSource = (await BaseDataBase._TablingStoredProcedureAsync("GetStatistictsFamily")).DefaultView;
                            ucFamily.GetData();
                            break;
                        case 2:
                            //DGMain.ItemsSource = (await BaseDataBase._TablingStoredProcedureAsync("GetStatistictsItems")).DefaultView;
                            break;
                        case 3:
                            DGMain.ItemsSource = (await BaseDataBase._TablingStoredProcedureAsync("GetStatistictsFamilyPerson")).DefaultView;
                            ucFamilyPerson.GetData();
                            break;
                        case 4:
                            DGMain.ItemsSource = (await BaseDataBase._TablingStoredProcedureAsync("GetStatisticsFamilyNeed")).DefaultView;
                            ucFamilyNeed.GetData();
                            break;
                        case 5:
                            DGMain.ItemsSource = (await BaseDataBase._TablingAsync(@"
                            select FirstName + ' ' + LastName ListerName,FamilyCode,FamilyName,FamilyType,ApplyDate,FamilyPersonCount,IsCanceled,x.*,Family.Evaluation FamilyEvaluation from (select a.*, FamilyID,OrphanID,Date,ListerGroup.Notes ListerGroupNotes,Evaluation,CreatePerson,CreateDate,LastModifiedPerson FROM ListerGroup inner join 
                            (select Lister.*,GroupID from Lister_ListerGroup
                            inner join Lister ON Lister_ListerGroup.ListerID = Lister.ListerID) a
                            on ListerGroup.GroupID  = a.GroupID) x
                            Left outer join Family on x.FamilyID = Family.FamilyID")).DefaultView;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        private void btnToday_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = BaseDataBase._TablingStoredProcedure("GetStatisticsItemsCount", new SqlParameter("@Date1", BaseDataBase.DateNow.ToString("MM-dd-yyyy 00:00")), new SqlParameter("@Date2", BaseDataBase.DateNow.ToString("MM-dd-yyyy 23:59")));
            grdStaMain.DataContext = new StatisticsClass(dt);
        }
        private void btnWeek_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = BaseDataBase._TablingStoredProcedure("GetStatisticsItemsCount", new SqlParameter("@Date1", BaseDataBase.DateNow.AddDays(-7).ToString("MM-dd-yyyy 00:00")), new SqlParameter("@Date2", BaseDataBase.DateNow.ToString("MM-dd-yyyy 23:59")));
            grdStaMain.DataContext = new StatisticsClass(dt);
        }
        private void btnYear_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = BaseDataBase._TablingStoredProcedure("GetStatisticsItemsCount", new SqlParameter("@Date1", BaseDataBase.DateNow.AddMonths(-1).ToString("MM-dd-yyyy 00:00")), new SqlParameter("@Date2", BaseDataBase.DateNow.ToString("MM-dd-yyyy 23:59")));
            grdStaMain.DataContext = new StatisticsClass(dt);
        }
        private void btnMonth_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = BaseDataBase._TablingStoredProcedure("GetStatisticsItemsCount", new SqlParameter("@Date1", BaseDataBase.DateNow.AddYears(-1).ToString("MM-dd-yyyy 00:00")), new SqlParameter("@Date2", BaseDataBase.DateNow.ToString("MM-dd-yyyy 23:59")));
            grdStaMain.DataContext = new StatisticsClass(dt);
        }
        private void btnCustom_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = BaseDataBase._TablingStoredProcedure("GetStatisticsItemsCount", new SqlParameter("@Date1", dtp1.SelectedDate.HasValue ? dtp1.SelectedDate.Value.ToString("MM-dd-yyyy 00:00") : "01-01-2000 00:00"), new SqlParameter("@Date2", dtp2.SelectedDate.HasValue ? dtp2.SelectedDate.Value.ToString("MM-dd-yyyy 23:59") : "01-01-2050 00:00"));
            grdStaMain.DataContext = new StatisticsClass(dt);
        }
    }
}