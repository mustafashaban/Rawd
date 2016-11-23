using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for InventoryItemSta.xaml
    /// </summary>
    public partial class InventoryItemSta : UserControl
    {
        TextBox txtItem;
        public InventoryItemSta()
        {
            InitializeComponent();
            cmboItem.ItemsSource = BaseDataBase.GetAllStrings("select Name from Item order by Name");
        }

        private void txtItem_TextChanged(object sender, TextChangedEventArgs e)
        {
            cmbo_SelectionChanged(null, null);
        }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (cmboInventory.SelectedItem == null)
            {
                MyMessageBox.Show("يجب اختيار مستودع");
                return;
            }
            Storyboard sb = (App.Current.Resources["sbRotateButton"] as Storyboard).Clone();
            sb.SetValue(Storyboard.TargetProperty, sender);
            sb.Begin();
            DataTable dt = await BaseDataBase._TablingStoredProcedureAsync("GetStatistictsInventoryItems"
                    , new SqlParameter("@InventoryID", cmboInventory.SelectedValue)
                    , new SqlParameter("@date1", date1.SelectedDate ?? new DateTime(2000, 1, 1))
                    , new SqlParameter("@date2", date2.SelectedDate ?? new DateTime(2050, 1, 1)));
            sb.Pause();
            dg.ItemsSource = dt.DefaultView;
            cmbo_SelectionChanged(null, null);
        }

        private async void dg_RowDetailsVisibilityChanged(object sender, DataGridRowDetailsEventArgs e)
        {
            if (e.Row.DetailsVisibility == Visibility.Visible)
            {
                DataGrid innerDataGrid = e.DetailsElement.FindName("dg2") as DataGrid;
                DataTable dt = await BaseDataBase._TablingStoredProcedureAsync("GetInventoryItemStaDetails",
                    new SqlParameter("@ItemID", (e.Row.Item as DataRowView)["ItemID"]),
                    new SqlParameter("@InventoryID", (e.Row.Item as DataRowView)["InventoryID"]),
                    new SqlParameter("@date1", date1.SelectedDate ?? new DateTime(2000, 1, 1)),
                    new SqlParameter("@date2", date2.SelectedDate ?? new DateTime(2050, 1, 1)));
                innerDataGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date1.SelectedDate.HasValue && date2.SelectedDate.HasValue && date1.SelectedDate > date2.SelectedDate)
            {
                MyMessageBox.Show("تاريخ البداية بجب ان يكون اصغر او يساوي تاريخ النهاية");
                var dp = sender as DatePicker;
                dp.SelectedDate = null;
            }
        }

        private void cmbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dv = dg.ItemsSource as DataView;

            if (dv != null)
            {
                try
                {
                    //dv.RowFilter = $"InventoryName like '{(cmboInventory.SelectedIndex > 0 ? cmboInventory.SelectedItem.ToString() : "%")}' and ItemName Like '{(txtItem.Text == "" ? "%" : cmboItem.Text)}%'";
                    dv.RowFilter = $"ItemName Like '{(txtItem.Text == "" ? "%" : cmboItem.SelectedItem)}%'";
                }
                catch
                {
                    dv.RowFilter = "";
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtItem = (TextBox)cmboItem.Template.FindName("PART_EditableTextBox", cmboItem);
            txtItem.TextChanged += txtItem_TextChanged;
        }

        private void btnExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.CanExport)
            {
                MyMessageBox.Show("لا يوجد لديك صلاحيات للتصدير");
                return;
            }
            var dg = ((sender as Button).Parent as Grid).FindName("dg2") as DataGrid;
            if (dg.Items.Count > 0)
            {
                var dt = (dg.ItemsSource as DataView).Table.Copy();
                List<int> indexesToRemove = new List<int>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    int j = 0;
                    for (; j < dg.Columns.Count; j++)
                    {
                        if (dg.Columns[j].SortMemberPath == dt.Columns[i].ColumnName)
                        {
                            dt.Columns[i].ColumnName = dg.Columns[j].Header.ToString();
                            break;
                        }
                    }
                    if (j == dg.Columns.Count)
                        indexesToRemove.Add(i);
                }
                for (int i = indexesToRemove.Count - 1; i >= 0; i--)
                    dt.Columns.RemoveAt(i);

                ExportToExcel.ExportDataTableToExcel x = new MainWPF.ExportToExcel.ExportDataTableToExcel(dt);
                x.GenerateReport();
            }
        }

        private void btnExportToExcelMain_Click(object sender, RoutedEventArgs e)
        {
            if (dg.Items.Count > 0)
            {
                if (BaseDataBase.CurrentUser.CanExport)
                {
                    var dt = (dg.ItemsSource as DataView).Table.Clone();
                    for (int i = 0; i < dg.Items.Count; i++)
                    {
                        var dr = dt.NewRow();
                        object[] os = new object[dt.Columns.Count];
                        for (int j = 0; j < dt.Columns.Count; j++)
                            os[j] = (dg.Items[i] as DataRowView).Row.ItemArray[j];
                        dr.ItemArray = os;
                        dt.Rows.Add(dr);
                    }

                    List<int> indexesToRemove = new List<int>();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        int j = 0;
                        for (; j < dg.Columns.Count; j++)
                        {
                            if (dg.Columns[j].SortMemberPath == dt.Columns[i].ColumnName)
                            {
                                dt.Columns[i].ColumnName = dg.Columns[j].Header.ToString();
                                break;
                            }
                        }
                        if (j == dg.Columns.Count)
                            indexesToRemove.Add(i);
                    }
                    for (int i = indexesToRemove.Count - 1; i >= 0; i--)
                        dt.Columns.RemoveAt(i);
                    ExportToExcel.ExportDataTableToExcel x = new ExportToExcel.ExportDataTableToExcel(dt);
                    x.GenerateReport();
                }
            }
            else
            {
                MyMessageBox.Show("لا يوجد لديك صلاحيات للتصدير");
            }
        }

        private void btnAllInventorySta_Click(object sender, RoutedEventArgs e)
        {
            var main = App.Current.MainWindow as MainWindow;
            main.SendTabItem(new TabItem() { Header = "احصائيات كامل المستودعات", Content = new AllInventoryStaControl() });
        }
    }
}
