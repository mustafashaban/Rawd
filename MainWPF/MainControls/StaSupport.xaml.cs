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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for StaSupport.xaml
    /// </summary>
    public partial class StaSupport : UserControl
    {
        public StaSupport()
        {
            InitializeComponent();
        }

        public async void GetData()
        {
            List<string> lst1 = null, lst2 = null, lst3 = null, lst4 = null, lst5 = null, lst6 = null, lst7 = null;
            await Task.Run(() =>
            {
                lst1 = BaseDataBase.GetAllStringsWithAll("select distinct Name Presenter from [Order] inner join Users on LastUserID = Users.ID");
                lst2 = BaseDataBase.GetAllStringsWithAll("select distinct [Source] from Item");
                lst3 = BaseDataBase.GetAllStringsWithAll("select distinct [ItemType] from Item");
                lst4 = BaseDataBase.GetAllStringsWithAll("select [Name] from Item");
                lst5 = BaseDataBase.GetAllStringsWithAll("select Name from Sector");
                lst4.Add("عائلة خاصة");
                lst6 = BaseDataBase.GetAllStringsWithAll("select Name from Inventory");
                lst7 = House.GetHouseSectionsWithAll;
            });
            cmboPresenter.ItemsSource = lst1;
            cmboSource.ItemsSource = lst2;
            cmboSupportType.ItemsSource = lst3;
            cmboSupportName.ItemsSource = lst4;
            cmboSector.ItemsSource = lst5;
            cmboInventory.ItemsSource = lst6;
            cmboHouseSection.ItemsSource = lst7;
        }

        int fieldCount;
        public int FieldCount
        {
            get { return fieldCount; }
            set { fieldCount = value; }
        }

        private void Control_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Changing();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void Control_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Changing();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void Control_SelectedDateChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                Changing();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Changing();
        }
        private void nud_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
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
                    view.CustomFilter = string.Format("FamilyCode like '{0}%'", txtFamilyCodeSupports.Text);
                    if (nudOrderID.Value.HasValue)
                        view.CustomFilter += string.Format(" and OrderID = {0}", nudOrderID.Value);
                    //view.CustomFilter += string.Format(" and PresentDate <= #{0}#", dtpPresentDateSupport2.SelectedDate.Value.ToString("MM/dd/yyyy 23:59"));
                    if (cmboSource.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and Source like '{0}'", cmboSource.SelectedItem.ToString());
                    if (cmboSupportType.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and ItemType like '{0}'", cmboSupportType.SelectedItem.ToString());
                    if (cmboSupportName.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and Name like '{0}'", cmboSupportName.SelectedItem.ToString());
                    if (cmboInventory.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and InventoryName like '{0}'", cmboInventory.SelectedItem.ToString());
                    if (cmboSector.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and Sector like '%{0}%'", cmboSector.SelectedItem.ToString());
                    if (cmboPresenter.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and Presenter like '{0}'", cmboPresenter.SelectedItem.ToString().Trim());
                    if (cmboHouseSection.SelectedIndex > 0)
                        view.CustomFilter += string.Format(" and HouseSection like '{0}'", cmboHouseSection.SelectedItem.ToString().Trim());
                    FieldCount = view.Count;
                }
            }
            catch { }
        }

        private void cmboGroup3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(dgMain.ItemsSource);
            view.GroupDescriptions.Clear();

            if (cmboGroup31.SelectedIndex > 0)
            {
                switch (cmboGroup31.SelectedIndex)
                {
                    case 1:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("FamilyCode"));
                        break;
                    case 2:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("Source"));
                        break;
                    case 3:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("ItemType"));
                        break;
                    case 4:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("Name"));
                        break;
                    case 5:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("Presenter"));
                        break;
                    case 6:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("Sector"));
                        break;
                    case 7:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("HouseSection"));
                        break;
                    case 8:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("InventoryName"));
                        break;
                }
            }

            if (cmboGroup32.SelectedIndex == cmboGroup31.SelectedIndex)
            {
                cmboGroup32.SelectedIndex = 0;
                e.Handled = true;
                return;
            }
            if (cmboGroup32.SelectedIndex > 0)
            {
                switch (cmboGroup32.SelectedIndex)
                {
                    case 1:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("FamilyCode"));
                        break;
                    case 2:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("Source"));
                        break;
                    case 3:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("ItemType"));
                        break;
                    case 4:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("Name"));
                        break;
                    case 5:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("Presenter"));
                        break;
                    case 6:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("Sector"));
                        break;
                    case 7:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("HouseSection"));
                        break;
                    case 8:
                        view.GroupDescriptions.Add(new PropertyGroupDescription("InventoryName"));
                        break;
                }
            }
        }

        bool isWorking = false;
        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!isWorking)
            {
                if (dtpPresentDateSupport1.SelectedDate.HasValue && dtpPresentDateSupport2.SelectedDate.HasValue)
                {
                    isWorking = true;
                    Storyboard sb = (App.Current.Resources["sbRotateButton"] as Storyboard).Clone();
                    sb.SetValue(Storyboard.TargetProperty, sender);
                    sb.Begin();
                    dgMain.ItemsSource = (await BaseDataBase._TablingStoredProcedureAsync("GetStatistictsItems",
                        new System.Data.SqlClient.SqlParameter("@StartDate", dtpPresentDateSupport1.SelectedDate),
                        new System.Data.SqlClient.SqlParameter("@EndDate", dtpPresentDateSupport2.SelectedDate))).DefaultView;
                    GetData();
                    Changing();
                    sb.Pause();
                    isWorking = false;
                }
                else MyMessageBox.Show("يجب اختيار مجال التاريخ");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsLoaded && dgMain.Items.Count == 0)
            {
                dtpPresentDateSupport1.SelectedDate = DateTime.Now;
                dtpPresentDateSupport2.SelectedDate = DateTime.Now;
                btnSearch_Click(btnRefresh, null);
            }
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            var mw = App.Current.MainWindow as MainWindow;
            mw.SendTabItem(new TabItem() { Header = "تقرير مواد", Content = new DailyItemReport() });
        }
    }
}
