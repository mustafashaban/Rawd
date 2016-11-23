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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for ReportFixedFamilyControlAll.xaml
    /// </summary>
    public partial class ReportFixedFamilyControlAll : UserControl
    {
        public ReportFixedFamilyControlAll()
        {
            InitializeComponent();
        }
        private void Control_TextChanged(object sender, TextChangedEventArgs e)
        {
            Changing();
        }
        private void Control_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Changing();
        }
        private void Control_SelectedDateChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Changing();
        }
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Changing();
        }

        int fieldCount;
        public int FieldCount
        {
            get { return fieldCount; }
            set { fieldCount = value; }
        }
       
        public void Changing()
        {
            try
            {
                BindingListCollectionView view = CollectionViewSource.GetDefaultView(dgMain.ItemsSource) as BindingListCollectionView;
                if (view != null)
                {
                    view.CustomFilter = "";
                    view.CustomFilter = string.Format("FamilyCode like '{0}%'", txtFamilyCode.Text);
                    if (cmboHouseSection.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and HouseSection like '{0}'", cmboHouseSection.SelectedItem);
                    if (cmboHouseStreet.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and HouseStreet like '{0}'", cmboHouseStreet.SelectedItem);
                    if (cmboHouseHouseBuiling.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and HouseBuildingNumber like '{0}'", cmboHouseHouseBuiling.SelectedItem);
                    if (cmboHouseAddress.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and HouseAddress like '{0}'", cmboHouseAddress.SelectedItem);

                    if (dtp1.SelectedDate != null)
                        view.CustomFilter += string.Format(" and ApplyDate >= #{0}#", dtp1.SelectedDate.Value.ToString("MM/dd/yyyy"));
                    if (dtp2.SelectedDate != null)
                        view.CustomFilter += string.Format(" and ApplyDate <= #{0}#", dtp2.SelectedDate.Value.ToString("MM/dd/yyyy"));

                    FieldCount = view.Count;

                    cmboHouseStreet.ItemsSource = SetViewToStringList((dgMain.ItemsSource as DataView).ToTable(true, "HouseStreet").DefaultView);
                    cmboHouseSection.ItemsSource = SetViewToStringList((dgMain.ItemsSource as DataView).ToTable(true, "HouseSection").DefaultView);
                    cmboHouseAddress.ItemsSource = SetViewToStringList((dgMain.ItemsSource as DataView).ToTable(true, "HouseAddress").DefaultView);
                    cmboHouseHouseBuiling.ItemsSource = SetViewToStringList((dgMain.ItemsSource as DataView).ToTable(true, "HouseBuildingNumber").DefaultView);
                }
            }
            catch { }
        }

        private List<string> SetViewToStringList(DataView dv)
        {
            List<string> strings = new List<string>();

            strings.Add("الكل");
            foreach (var item in dv)
            {
                strings.Add((item as DataRowView)[0].ToString());
            }
            return strings;
        }

       
    }
}
