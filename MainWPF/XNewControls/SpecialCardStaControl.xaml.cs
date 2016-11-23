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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for SpecialCardStaControl.xaml
    /// </summary>
    public partial class SpecialCardStaControl : UserControl
    {
        bool isWorking = false;
        public SpecialCardStaControl()
        {
            InitializeComponent();
            btnSearch_Click(btnRefresh, null);
        }
        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!isWorking)
            {
                isWorking = true;
                Storyboard sb = (App.Current.Resources["sbRotateButton"] as Storyboard).Clone();
                sb.SetValue(Storyboard.TargetProperty, sender);
                sb.Begin();
                var dt = await BaseDataBase._TablingStoredProcedureAsync("sp_GetSpeicalCardSourceTable");
                sb.Pause();
                dg.ItemsSource = dt.DefaultView;

                FillData(dt);
                Control_Changed(null, null);
                isWorking = false;
            }
        }

        private void FillData(DataTable dt)
        {
            txtFamilyCode.Text = "";
            txtCardCode.Text = "";
            cmboCardName.SelectedIndex = 0;
            cmboCardType.SelectedIndex = 0;
            cmboCenterName.SelectedIndex = 0;
            cmboSector.SelectedIndex = 0;
            dp1.SelectedDate = null;
            dp2.SelectedDate = null;

            List<string> lst = (from r in dt.AsEnumerable() select r.Field<string>("CardName")).Distinct().ToList();
            lst.Insert(0, "الكل");
            cmboCardName.ItemsSource = lst;

            lst = (from r in dt.AsEnumerable() select r.Field<string>("CenterName")).Distinct().ToList();
            lst.Insert(0, "الكل");
            cmboCenterName.ItemsSource = lst;

            lst = (from r in dt.AsEnumerable() select r.Field<string>("CardType")).Distinct().ToList();
            lst.Insert(0, "الكل");
            cmboCardType.ItemsSource = lst;

            lst = (from r in dt.AsEnumerable() select r.Field<string>("Sector")).Distinct().ToList();
            lst.Insert(0, "الكل");
            cmboSector.ItemsSource = lst;
        }

        private void btnExportToExcelMain_Click(object sender, RoutedEventArgs e)
        {
            if (dg.Items.Count == 0) return;
            if (!BaseDataBase.CurrentUser.CanExport)
            {
                MyMessageBox.Show("لا يوجد لديك صلاحيات للتصدير");
                return;
            }
            var dt = (dg.ItemsSource as DataView).Table.Copy();
            for (int i = 0; i < dt.Columns.Count; i++)
                for (int j = 0; j < dg.Columns.Count; j++)
                    if (dg.Columns[j].SortMemberPath == dt.Columns[i].ColumnName)
                    {
                        dt.Columns[i].ColumnName = dg.Columns[j].Header.ToString();
                        break;
                    }

            ExportToExcel.ExportDataTableToExcel x = new MainWPF.ExportToExcel.ExportDataTableToExcel(dt);
            x.GenerateReport();
        }

        private void Control_Changed(object sender, TextChangedEventArgs e)
        {
            var dv = dg.ItemsSource as DataView;
            if (dv != null)
            {
                dv.RowFilter = string.Format("Code like '{0}*'", txtCardCode.Text);
                if (cmboCardName.SelectedIndex > 0)
                    dv.RowFilter += string.Format(" and CardName like '{0}' ", cmboCardName.SelectedItem);
                if (cmboCenterName.SelectedIndex > 0)
                    dv.RowFilter += string.Format(" and CenterName like '{0}' ", cmboCenterName.SelectedItem);
                if (cmboCardType.SelectedIndex > 0)
                    dv.RowFilter += string.Format(" and CardType like '{0}' ", cmboCardType.SelectedItem);
                if (cmboSector.SelectedIndex > 0)
                    dv.RowFilter += string.Format(" and Sector like '{0}' ", cmboSector.SelectedItem);
                if (!string.IsNullOrWhiteSpace(txtFamilyCode.Text))
                    dv.RowFilter = string.Format("FamilyCode like '{0}*'", txtFamilyCode.Text);
                if (dp1.SelectedDate != null)
                    dv.RowFilter += string.Format(" and StartDate >= #{0}#", (dp1.SelectedDate.Value).ToString("MM/dd/yyyy"));
                if (dp2.SelectedDate != null)
                    dv.RowFilter += string.Format(" and EndDate <= #{0}#", (dp2.SelectedDate.Value).ToString("MM/dd/yyyy"));
            }
        }

        private void cmbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Control_Changed(null, null);
        }
    }
}
