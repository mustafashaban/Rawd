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
    /// Interaction logic for StaOrphan.xaml
    /// </summary>
    public partial class StaOrphan : UserControl
    {
        public StaOrphan()
        {
            InitializeComponent();

            cmboImpeding.ItemsSource = BaseDataBase.GetAllStringsWithAll("select distinct impedingType from impeding");
            cmboHealth.ItemsSource = BaseDataBase.GetAllStringsWithAll("select Distinct HealthSituation from OrphanHealth");
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
                    view.CustomFilter = string.Format("[رمز العائلة] like '%{0}%'", txtFamilyCodeOrphan.Text);
                    if (!string.IsNullOrEmpty(txtOrphanName.Text))
                        view.CustomFilter += string.Format(" and [اسم اليتيم] like '%{0}%'", txtOrphanName.Text);
                    if (cmboGender.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and [الجنس] like '{0}'", (cmboGender.Items[cmboGender.SelectedIndex] as ComboBoxItem).Content);
                    if (dtpDOB1.SelectedDate != null)
                        view.CustomFilter += string.Format(" and [تاريخ الولادة] >= #{0}#", ((DateTime)dtpDOB1.SelectedDate).ToString("MM/dd/yyyy"));
                    if (dtpDOB2.SelectedDate != null)
                        view.CustomFilter += string.Format(" and [تاريخ الولادة] <= #{0}#", ((DateTime)dtpDOB2.SelectedDate).ToString("MM/dd/yyyy"));
                    if (dtpCancelDate1.SelectedDate != null)
                        view.CustomFilter += string.Format(" and [تاريخ الإلغاء] >= #{0}#", ((DateTime)dtpCancelDate1.SelectedDate).ToString("MM/dd/yyyy"));
                    if (dtpCancelDate2.SelectedDate != null)
                        view.CustomFilter += string.Format(" and [تاريخ الإلغاء] <= #{0}#", ((DateTime)dtpCancelDate2.SelectedDate).ToString("MM/dd/yyyy"));
                    if (nudAge.Value != null)
                        view.CustomFilter += string.Format(" and [العمر] = {0}", ((int)nudAge.Value));
                    if (nudFeetSize.Value != null)
                        view.CustomFilter += string.Format(" and [نمرة الرجل] = {0}", ((int)nudFeetSize.Value));
                    if (nudTall.Value != null)
                        view.CustomFilter += string.Format(" and [الطول] = {0}", ((int)nudTall.Value));
                    if (nudWasteSize.Value != null)
                        view.CustomFilter += string.Format(" and [قياس الخصر] = {0}", ((int)nudWasteSize.Value));
                    if (cmboTeachingStage.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and [آخر مرحلة تعليمية] like '%{0}%'", (cmboTeachingStage.Items[cmboTeachingStage.SelectedIndex] as ComboBoxItem).Content);
                    if (cmboImpeding.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and [الإعاقات] like '%{0}%'", cmboImpeding.SelectedItem);
                    if (cmboHealth.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and [الحالات الصحية] like '%{0}%'", cmboHealth.SelectedItem);
                    FieldCount = view.Count;
                }
            }
            catch { }
        }
    }
}
