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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for StaFamilyNeed.xaml
    /// </summary>
    public partial class StaFamilyNeed : UserControl
    {
        public StaFamilyNeed()
        {
            InitializeComponent();
        }

        public async void GetData()
        {
            List<string> lst1 = null;
            await Task.Run(() => lst1 = BaseDataBase.GetAllStringsWithAll("select distinct NeedType from FamilyNeed"));
            cmboNeedType.ItemsSource = lst1;
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
                    view.CustomFilter = string.Format("[رمز العائلة] like '%{0}%'", txtFamilyCode.Text);
                    if (!string.IsNullOrEmpty(txtFamilyName.Text))
                        view.CustomFilter += string.Format(" and [اسم العائلة] like '%{0}%'", txtFamilyName.Text);
                    if (cmboFamilyType.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and [نوع العائلة] like '{0}'", (cmboFamilyType.Items[cmboFamilyType.SelectedIndex] as ComboBoxItem).Content);
                    if (cmboNeedType.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and [نوع الاحتياج] like '{0}%'", (cmboNeedType.Items[cmboNeedType.SelectedIndex]));
                    if (!string.IsNullOrEmpty(txtNeedName.Text))
                        view.CustomFilter += string.Format(" and [شرح الاحتياج] like '%{0}%'", txtNeedName.Text);
                    if (!string.IsNullOrEmpty(txtValue.Text))
                        view.CustomFilter += string.Format(" and [قيمة الاحتياج] like '%{0}%'", txtValue.Text);
                    if (chkIsEnsured.IsChecked != null)
                        view.CustomFilter += string.Format(" and [تم تأمين الاحتياج؟] = {0}", (chkIsEnsured.IsChecked == true) ? 1 : 0);
                    if (!string.IsNullOrEmpty(txtEnsuredSupport.Text))
                        view.CustomFilter += string.Format(" and [الجهة المؤمنة] like '%{0}%'", txtEnsuredSupport.Text);
                    if (dtpAskDate1.SelectedDate != null)
                        view.CustomFilter += string.Format(" and [تاريخ الطلب] >= #{0}#", ((DateTime)dtpAskDate1.SelectedDate).ToString("MM/dd/yyyy"));
                    if (dtpAskDate2.SelectedDate != null)
                        view.CustomFilter += string.Format(" and [تاريخ الطلب] <= #{0}#", ((DateTime)dtpAskDate2.SelectedDate).ToString("MM/dd/yyyy"));

                    FieldCount = view.Count;
                }
            }
            catch { }
        }


    }
}
