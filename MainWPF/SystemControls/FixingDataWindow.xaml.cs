using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for FixingDataWindow.xaml
    /// </summary>
    public partial class FixingDataWindow : Window
    {
        public FixingDataWindow()
        {
            InitializeComponent();
        }
        string column = "";

        private void Rad1_Checked(object sender, RoutedEventArgs e)
        {
            if (radBasePlace.IsChecked == true)
                column = "OldAddress";
            else if (radAddress.IsChecked == true)
                column = "HouseSection";
            else if (radStreet.IsChecked == true)
                column = "HouseStreet";
            else if (radPoint.IsChecked == true)
                column = "Address";
            else if (radJob.IsChecked == true)
                column = "Job";
            else if (radHealth.IsChecked == true)
                column = "HealthStatus";
            else
                column = "";
            lstData.ItemsSource = column == "" ? null : BaseDataBase.GetAllStrings("Select Distinct " + column + " from " + (column == "Job" || column == "HealthStatus" ? "Parent" : "House") + " order by " + column);
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (lstData.SelectedItems.Count <= 0)
            {
                MyMessageBox.Show("لم يتم اختيار أي قيمة للتعديل");
                return;
            }
            if (txtUpdatedValue.Text.Trim() == "")
            {
                MyMessageBox.Show("يجب ادخال القيمة الجديدة");
                return;
            }
            if (MyMessageBox.Show("هل تريد تأكيد تعديل البيانات", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                string query = "Update " + (column == "Job" || column == "HealthStatus" ? "Parent" : "House") + " set " + column + " = '" + txtUpdatedValue.Text.Replace("\'","").Trim() + "' where " + column + " in (";
                foreach (string item in lstData.SelectedItems)
                {
                    query += "'" + item.Replace("\'","") + "',";
                }
                query = query.Remove(query.Length - 1) + ")";
                
                MyMessageBox.Show("تم تعديل " + BaseDataBase._NonQuery(query) + " قيمة في قاعدة البيانات");
                

                lstData.ItemsSource = column == "" ? null : BaseDataBase.GetAllStrings("Select Distinct " + column + " from " + (column == "Job" || column == "HealthStatus" ? "Parent" : "House") + " order by " + column);
            }
        }
    }
}
