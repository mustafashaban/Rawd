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
    /// Interaction logic for ListerGroupSta.xaml
    /// </summary>
    public partial class ListerGroupSta : UserControl
    {
        public ListerGroupSta()
        {
            InitializeComponent();
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
        private void chkIsEnsured_Checked(object sender, RoutedEventArgs e)
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
                    view.CustomFilter = string.Format("FamilyCode like '%{0}%'", txtFamilyCode.Text);
                    if (!string.IsNullOrEmpty(txtFamilyName.Text))
                        view.CustomFilter += string.Format(" and FamilyName like '%{0}%'", txtFamilyName.Text);
                    if (cmboFamilyType.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and FamilyType like '{0}'", (cmboFamilyType.Items[cmboFamilyType.SelectedIndex] as ComboBoxItem).Content);
                    if (!string.IsNullOrEmpty(txtListerName.Text))
                        view.CustomFilter += string.Format(" and ListerName like '%{0}%'", txtListerName.Text);
                    if (cmboGender.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and Gender like '{0}'", cmboGender.SelectedItem);
                    if (cmboEvaluation.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and Evaluation like '{0}'", cmboEvaluation.SelectedItem);
                    if (dtpDate1.SelectedDate != null)
                        view.CustomFilter += string.Format(" and Date >= #{0}#", ((DateTime)dtpDate1.SelectedDate).ToString("MM/dd/yyyy"));
                    if (dtpDate2.SelectedDate != null)
                        view.CustomFilter += string.Format(" and Date <= #{0}#", ((DateTime)dtpDate2.SelectedDate).ToString("MM/dd/yyyy"));
                    if (dtpApplyDate1.SelectedDate != null)
                        view.CustomFilter += string.Format(" and ApplyDate >= #{0}#", ((DateTime)dtpApplyDate1.SelectedDate).ToString("MM/dd/yyyy"));
                    if (dtpApplyDate2.SelectedDate != null)
                        view.CustomFilter += string.Format(" and ApplyDate <= #{0}#", ((DateTime)dtpApplyDate2.SelectedDate).ToString("MM/dd/yyyy"));
                    if (dtpCreateDate1.SelectedDate != null)
                        view.CustomFilter += string.Format(" and CreateDate >= #{0}#", ((DateTime)dtpCreateDate1.SelectedDate).ToString("MM/dd/yyyy"));
                    if (dtpCreateDate2.SelectedDate != null)
                        view.CustomFilter += string.Format(" and CreateDate <= #{0}#", ((DateTime)dtpCreateDate2.SelectedDate).ToString("MM/dd/yyyy"));

                    FieldCount = view.Count;
                }
            }
            catch { }
        }

        private void cmboGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(dgMain.ItemsSource);
            view.GroupDescriptions.Clear();

            if (cmboGroup1.SelectedIndex > 0)
            {
                switch (cmboGroup1.SelectedIndex)
                {
                    case 1:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("GroupID"));
                        break;
                    case 2:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("ListerName"));
                        break;
                    case 3:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("Gender"));
                        break;
                    case 4:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("Date"));
                        break;
                    case 5:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("CreateDate"));
                        break;
                    case 6:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("Evaluation"));
                        break;
                    case 7:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("FamilyType"));
                        break;
                }
            }

            if (cmboGroup2.SelectedIndex == cmboGroup1.SelectedIndex)
            {
                cmboGroup2.SelectedIndex = 0;
                e.Handled = true;
                return;
            }
            if (cmboGroup2.SelectedIndex > 0)
            {
                switch (cmboGroup2.SelectedIndex)
                {
                    case 1:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("GroupID"));
                        break;
                    case 2:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("ListerName"));
                        break;
                    case 3:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("Gender"));
                        break;
                    case 4:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("Date"));
                        break;
                    case 5:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("CreateDate"));
                        break;
                    case 6:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("Evaluation"));
                        break;
                    case 7:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("FamilyType"));
                        break;
                }
            }
        }


    }
}
