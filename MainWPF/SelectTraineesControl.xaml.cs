using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    public partial class SelectTraineesControl : Window
    {
        public SelectTraineesControl(Training t)
        {
            InitializeComponent();
            this.DataContext = t;
            dgTrained.ItemsSource = t.Trainees.ToList();
        }

        private void cmboGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Control_Changed(null, null);
        }
        private void Control_Changed(object sender, TextChangedEventArgs e)
        {
            if (dgTemp.ItemsSource != null)
            {
                var dv = dgTemp.ItemsSource as DataView;
                try
                {
                    dv.RowFilter = $"Name like '*{txtName.Text}*'";
                    if (cmboGender.SelectedIndex > 0)
                        dv.RowFilter += $" and Gender = '{(cmboGender.SelectedItem as ComboBoxItem).Content.ToString()}'";
                }
                catch
                { dv.RowFilter = ""; }
            }
        }

        private void dgTemp_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnAddTrained_Click(null, null);
        }
        private void btnAddTrained_Click(object sender, RoutedEventArgs e)
        {
            var drv = dgTemp.SelectedItem as DataRowView;
            if (drv != null)
            {
                var tr = this.DataContext as Training;
                Trained t = new Trained();

                var lst = dgTrained.ItemsSource as List<Trained>;

                t.TrainedID = (int)drv["ID"];
                t.TrainedName = drv["Name"].ToString();
                t.TrainedType = (Trained.TrainedEnumType)drv["Type"];
                t.Training = tr;

                foreach (var item in lst)
                {
                    if (item.TrainedID == t.TrainedID && t.TrainedType == item.TrainedType)
                    {
                        MyMessageBox.Show("تم اختيار هذا المتدرب مسبقاً");
                        return;
                    }
                }

                lst.Add(t);
                dgTrained.Items.Refresh();
            }
        }
        private void btnDelTrained_Click(object sender, RoutedEventArgs e)
        {
            if (dgTrained.SelectedIndex != -1)
            {
                var t = dgTrained.SelectedItem as Trained;
                (dgTrained.ItemsSource as List<Trained>).Remove(t);
                dgTrained.Items.Refresh();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }


        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                var r = sender as RadioButton;
                if (r == null || r.Content == null)
                    return;

                switch (r.Content.ToString())
                {
                    case "أيتام":
                        dgTemp.ItemsSource = BaseDataBase._TablingStoredProcedure("sp_Get_All_Trainees_ByType", (new SqlParameter("@Type", 1))).DefaultView;
                        Control_Changed(null, null);
                        break;
                    case "طلاب علم":
                        dgTemp.ItemsSource = BaseDataBase._TablingStoredProcedure("sp_Get_All_Trainees_ByType", new SqlParameter("@Type", 2)).DefaultView;
                        Control_Changed(null, null);
                        break;
                    case "أمهات":
                        dgTemp.ItemsSource = BaseDataBase._TablingStoredProcedure("sp_Get_All_Trainees_ByType", new SqlParameter("@Type", 3)).DefaultView;
                        Control_Changed(null, null);
                        break;
                    case "حاضنات":
                        dgTemp.ItemsSource = BaseDataBase._TablingStoredProcedure("sp_Get_All_Trainees_ByType", new SqlParameter("@Type", 4)).DefaultView;
                        Control_Changed(null, null);
                        break;
                    case "أوصياء":
                        dgTemp.ItemsSource = BaseDataBase._TablingStoredProcedure("sp_Get_All_Trainees_ByType", new SqlParameter("@Type", 5)).DefaultView;
                        Control_Changed(null, null);
                        break;
                }
            }
        }

    }
}
