using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for StaFamily.xaml
    /// </summary>
    public partial class StaFamily : UserControl
    {
        public StaFamily()
        {
            InitializeComponent();
        }

        public async void GetData()
        {
            List<string> lst1 = null, lst2 = null, lst3 = null, lst4 = null;
            await Task.Run(() =>
            {
                lst1 = House.GetHouseSectionsWithAll;
                lst2 = House.GetHouseStreetsWithAll;
                lst3 = House.GetOldAddressesWithAll;
                lst4 = SystemData.GetFamilyTypes;
                lst4.Insert(0, "الكل");
            });
            cmboHouseSection.ItemsSource = lst1;
            cmboHouseStreet.ItemsSource = lst2;
            cmboOldAddress.ItemsSource = lst3;
            cmboSector.ItemsSource = BaseDataBase.GetAllStringsWithAll("Select Name from Sector");
            cmboFamilyType.ItemsSource = lst4;
        }

        int fieldCount;
        public int FieldCount
        {
            get { return fieldCount; }
            set { fieldCount = value; }
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
        private void nud_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Changing();
        }
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Changing();
        }

        void Changing()
        {
            try
            {
                BindingListCollectionView view = CollectionViewSource.GetDefaultView(dgMain.ItemsSource) as BindingListCollectionView;
                if (view != null)
                {
                    view.CustomFilter = "";
                    view.CustomFilter = string.Format("FamilyCode like '{0}%'", txtFamilyCode.Text);
                    if (!string.IsNullOrEmpty(txtFamilyName.Text))
                        view.CustomFilter += string.Format(" and (FamilyName like '%{0}%' or Father like '%{0}%' or Mother like '%{0}%')", txtFamilyName.Text);
                    if (!string.IsNullOrEmpty(txtPID.Text))
                        view.CustomFilter += string.Format(" and (FatherPID like '%{0}%' or MotherPID like '%{0}%')", txtPID.Text);
                    if (!string.IsNullOrEmpty(txtFIDentityID.Text))
                        view.CustomFilter += string.Format(" and (FamilyIdentityID like '%{0}%')", txtFIDentityID.Text);
                    if (cmboSector.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and Sector like '{0}'", cmboSector.SelectedItem);
                    if (cmboSectorType.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and IsActiveSector = {0}", cmboSectorType.SelectedIndex - 1);
                    if (cmboFamilyType.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and FamilyType like '{0}'", cmboFamilyType.SelectedItem.ToString());
                    if (cmboIsCancelled.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and IsCanceled = ({0})", (cmboIsCancelled.SelectedIndex - 1).ToString());
                    if (dtpApplyDate1.SelectedDate != null)
                        view.CustomFilter += string.Format(" and ApplyDate >= #{0}#", ((DateTime)dtpApplyDate1.SelectedDate).ToString("MM/dd/yyyy"));
                    if (dtpApplyDate2.SelectedDate != null)
                        view.CustomFilter += string.Format(" and ApplyDate <= #{0}#", ((DateTime)dtpApplyDate2.SelectedDate).ToString("MM/dd/yyyy"));
                    if (dtpListDate1.SelectedDate != null)
                        view.CustomFilter += string.Format(" and LastListDate >= #{0}#", ((DateTime)dtpListDate1.SelectedDate).ToString("MM/dd/yyyy"));
                    if (dtpListDate1.SelectedDate != null)
                        view.CustomFilter += string.Format(" and LastListDate <= #{0}#", ((DateTime)dtpListDate2.SelectedDate).ToString("MM/dd/yyyy"));
                    if (dtpPresentDate1.SelectedDate != null)
                        view.CustomFilter += string.Format(" and LastPresentDate >= #{0}#", ((DateTime)dtpPresentDate1.SelectedDate).ToString("MM/dd/yyyy"));
                    if (dtpPresentDate2.SelectedDate != null)
                        view.CustomFilter += string.Format(" and LastPresentDate <= #{0}#", ((DateTime)dtpPresentDate2.SelectedDate).ToString("MM/dd/yyyy"));
                    if (dtpNextOrderDate1.SelectedDate != null)
                        view.CustomFilter += string.Format(" and NextOrderDate >= #{0}#", ((DateTime)dtpNextOrderDate1.SelectedDate).ToString("MM/dd/yyyy"));
                    if (dtpNextOrderDate2.SelectedDate != null)
                        view.CustomFilter += string.Format(" and NextOrderDate <= #{0}#", ((DateTime)dtpNextOrderDate2.SelectedDate).ToString("MM/dd/yyyy"));
                    if (nudChidCount.Value != null)
                        view.CustomFilter += string.Format(" and FamilyPersonCount like '{0}'", (nudChidCount.Value.Value));
                    if (cmboEvaluation.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and Evaluation like '{0}'", (cmboEvaluation.SelectedItem as ComboBoxItem).Content.ToString());
                    if (cmboHouseSection.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and HouseSection like '{0}'", cmboHouseSection.SelectedItem.ToString());
                    if (cmboHouseStreet.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and HouseStreet like '{0}'", cmboHouseStreet.SelectedItem.ToString());
                    if (cmboOldAddress.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and OldAddress like '{0}'", cmboOldAddress.SelectedItem.ToString());

                    FieldCount = view.Count;
                }
            }
            catch { }
        }

        private void cmboGroup1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(dgMain.ItemsSource);
            view.GroupDescriptions.Clear();

            if (cmboGroup1.SelectedIndex > 0)
            {
                switch (cmboGroup1.SelectedIndex)
                {
                    case 1:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("Sector"));
                        break;
                    case 2:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("HouseSection"));
                        break;
                }

                if (cmboGroup2.SelectedIndex == cmboGroup1.SelectedIndex)
                {
                    cmboGroup2.SelectedIndex = 0;
                    e.Handled = true;
                    return;
                }
                switch (cmboGroup2.SelectedIndex)
                {
                    case 1:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("Sector"));
                        break;
                    case 2:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("HouseSection"));
                        break;
                }
            }
        }
    }
}
