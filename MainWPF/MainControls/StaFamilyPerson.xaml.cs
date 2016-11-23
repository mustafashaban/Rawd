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
    /// Interaction logic for StaFamilyPerson.xaml
    /// </summary>
    public partial class StaFamilyPerson : UserControl
    {
        public StaFamilyPerson()
        {
            InitializeComponent();
        }

        public async void GetData()
        {
            await DBMain.UpdateAllPregnant();
            List<string> lst1, lst2, lst3, lst4, lst5, lst6, lst7, lst8;
            lst1 = lst2 = lst3 = lst4 = lst5 = lst6 = lst7 = lst8 = null;
            await Task.Run(() =>
            {
                lst1 = BaseDataBase.GetAllStringsWithAll("select distinct job from (select Job from Parent Union select Job from FamilyPerson) x where ISNULL(Job,'') <> '' order by Job");
                lst2 = BaseDataBase.GetAllStringsWithAll("select distinct HealthStatus from (select HealthStatus from Parent Union select HealthStatus from FamilyPerson) x WHERE ISNULL(HealthStatus,'') <> '' order by HealthStatus");
                lst3 = BaseDataBase.GetAllStringsWithAll("select distinct StudyStatus from (select StudyStatus from Parent Union select StudyStatus from FamilyPerson) x where Isnull(StudyStatus,'') <> '' order by StudyStatus");
                lst4 = BaseDataBase.GetAllStringsWithAll("select distinct HouseSection from House where ISNULL(HouseSection,'') <> '' order by HouseSection");
                lst5 = BaseDataBase.GetAllStringsWithAll("select distinct HouseStreet from House where isnull(HouseStreet,'') <> '' order by HouseStreet");
                lst6 = BaseDataBase.GetAllStringsWithAll("select distinct OldAddress from House x where ISNULL(OldAddress,'') <> '' order by OldAddress");
                lst7 = BaseDataBase.GetAllStringsWithAll("select distinct impededType from (select impededType from Parent Union select impededType from FamilyPerson) x where ISNULL(impededType,'') <> '' order by impededType");
                lst8 = BaseDataBase.GetAllStringsWithAll("select distinct RelationShip from (select 'أب' RelationShip Union select 'أم' Union (select RelationShip from FamilyPerson)) x where ISNULL(RelationShip,'') <> '' order by RelationShip");

            });
            cmboJob.ItemsSource = lst1;
            cmboHealthStatus.ItemsSource = lst2;
            cmboStudyStatus.ItemsSource = lst3;
            cmboHouseSection.ItemsSource = lst4;
            cmboHouseStreet.ItemsSource = lst5;
            cmboOldAddress.ItemsSource = lst6;
            cmboImpedeType.ItemsSource = lst7;
            cmboRelationShip.ItemsSource = lst8;
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
                    if (!string.IsNullOrEmpty(txtPersonName.Text))
                        view.CustomFilter += string.Format(" and Name like '%{0}%'", txtPersonName.Text);
                    if (cmboGender.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and Gender like '{0}'", cmboGender.SelectedItem);
                    if (cmboRelationShip.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and RelationShip like '{0}'", cmboRelationShip.SelectedItem);
                    if (dtpDOB1.SelectedDate != null)
                        view.CustomFilter += string.Format(" and DOB >= #{0}#", (dtpDOB1.SelectedDate.Value).ToString("MM/dd/yyyy"));
                    if (dtpDOB2.SelectedDate != null)
                        view.CustomFilter += string.Format(" and DOB <= #{0}#", (dtpDOB2.SelectedDate.Value).ToString("MM/dd/yyyy"));
                    if (cmboJob.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and Job like '{0}'", cmboJob.SelectedItem);
                    if (cmboIsImpeded.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and IsImpeded like '{0}'", cmboIsImpeded.SelectedItem);
                    if (cmboIsPregnant.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and IsPregnant like '{0}'", cmboIsPregnant.SelectedItem);
                    if (cmboImpedeType.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and impededType like '{0}'", cmboImpedeType.SelectedItem);
                    if (cmboIsDead.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and IsDead like '{0}'", cmboIsDead.SelectedItem);
                    if (cmboMaritalStatus.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and MaritalStatus like '{0}'", cmboMaritalStatus.SelectedItem);
                    if (cmboHealthStatus.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and HealthStatus like '{0}'", cmboHealthStatus.SelectedItem);
                    if (cmboStudyStatus.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and StudyStatus like '{0}'", cmboStudyStatus.SelectedItem);
                    if (cmboHealthStatus.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and HealthStatus like '{0}'", cmboHealthStatus.SelectedItem);
                    if (cmboIsCancelled.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and IsCanceled like '{0}'", cmboIsCancelled.SelectedItem);
                    if (cmboEvaluation.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and Evaluation like '{0}'", cmboEvaluation.SelectedItem);
                    if (cmboHouseSection.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and HouseSection like '{0}'", cmboHouseSection.SelectedItem);
                    if (cmboHouseStreet.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and HouseStreet like '{0}'", cmboHouseStreet.SelectedItem);
                    if (cmboOldAddress.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and OldAddress like '{0}'", cmboOldAddress.SelectedItem);

                    FieldCount = view.Count;
                }
            }
            catch { }
        }
    }
}
