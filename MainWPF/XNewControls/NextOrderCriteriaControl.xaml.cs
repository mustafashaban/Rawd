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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MainWPF
{
    public partial class NextOrderCriteriaControl : Window
    {
        List<NextOrderCriteria> lst;
        public NextOrderCriteriaControl()
        {
            InitializeComponent();

            try { nudDaysSpecialFamily.Value = int.Parse(SystemProperties.GetSystemPropertyValue(SystemProperties.Property.NextOrderSpecialFamily)); }
            catch { }
            try { nudNextOrderGap.Value = int.Parse(SystemProperties.GetSystemPropertyValue(SystemProperties.Property.NextOrderGap)); }
            catch { }

            var dt = BaseDataBase._Tabling("select FromMember,ToMember,NumberOfDays from NextOrderCriteria");
            if (dt != null && dt.Rows.Count > 0)
            {
                var lst = new List<NextOrderCriteria>();
                foreach (DataRow item in dt.Rows)
                    lst.Add(new NextOrderCriteria() { FromMember = (int)item[0], ToMember = (int)item[1], NumberOfDays = (int)item[2] });
                dg.ItemsSource = lst;
            }
            else dg.ItemsSource = new List<NextOrderCriteria>();

            dgSector.ItemsSource = Sector.GetAllSector();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (radGeneral.IsChecked == true)
            {
                var lst = dg.ItemsSource as List<NextOrderCriteria>;
                foreach (var item in lst)
                    if (!item.IsValidate() || !AllValidated(lst))
                        return;
                string query = "delete from NextOrderCriteria;";
                foreach (var item in lst)
                    query += $"insert into NextOrderCriteria (FromMember,ToMember,NumberOfDays,LastUserID) Values ({item.FromMember},{item.ToMember},{item.NumberOfDays},{BaseDataBase.CurrentUser.ID});";
                if (BaseDataBase._NonQuery(query) > 0)
                    MyMessage.CustomMessage("تم الحفظ بنجاح");

                SystemProperties.UpdateData(SystemProperties.Property.NextOrderSpecialFamily, "");
                if (nudDaysSpecialFamily.Value.HasValue)
                    SystemProperties.UpdateData(SystemProperties.Property.NextOrderSpecialFamily, nudDaysSpecialFamily.Value.ToString());

                SystemProperties.UpdateData(SystemProperties.Property.NextOrderGap, "");
                if (nudDaysSpecialFamily.Value.HasValue)
                    SystemProperties.UpdateData(SystemProperties.Property.NextOrderGap, nudNextOrderGap.Value.ToString());

                DialogResult = true;
            }
            else
            {
                if (dgSector.Items.Count > 0)
                {
                    var lst = dgSector.ItemsSource as List<Sector>;
                    foreach (var item in lst)
                        Sector.UpdateData(item);
                    DialogResult = true;
                    MyMessage.UpdateMessage();
                }
            }
        }

        private bool AllValidated(List<NextOrderCriteria> lst)
        {
            List<int> AllNumbers = new List<int>();
            foreach (var item in lst)
            {
                AllNumbers.AddRange(Enumerable.Range(item.FromMember.Value, item.ToMember.Value + 1 - item.FromMember.Value));
                if (AllNumbers.GroupBy(n => n).Any(c => c.Count() > 1))
                {
                    MyMessageBox.Show("يوجد تداخل في المجالات التي تم اختيارها. يرجى التأكد");
                    return false;
                }
            }
            return true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                grdGeneral.Visibility = radGeneral.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                grdSector.Visibility = radSector.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
