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
    /// Interaction logic for HouseControl.xaml
    /// </summary>
    public partial class HouseControl : UserControl, INotifyPropertyChanged
    {
        public HouseControl()
        {
            InitializeComponent();
            cmboOldAddress.ItemsSource = House.GetOldAddresses;
            cmboHouseSection.ItemsSource = House.GetHouseSections;
            cmboHouseStreet.ItemsSource = House.GetHouseStreets;
        }

        int? familyID;
        public int? FamilyID
        {
            get { return familyID; }
            set
            {
                familyID = value;
                Houses = House.GetHouseAllByFamilyID(value);
            }
        }

        int? orphanID;
        public int? OrphanID
        {
            get { return orphanID; }
            set
            {
                orphanID = value;
                Houses = House.GetHouseAllByOrphanID(value);
            }
        }

        int place = 0;
        public int Place
        {
            get { return place; }
            set
            {
                if (value >= 0 && value <= TotalCount)
                {
                    place = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Place"));
                    if (value == 0)
                    {
                        btnBack.IsEnabled = btnNext.IsEnabled = false;
                    }
                    else
                    {
                        btnBack.IsEnabled = (value == 1) ? false : true;
                        btnNext.IsEnabled = (value == TotalCount) ? false : true;
                        this.DataContext = Houses[Place - 1];
                    }
                }
            }
        }

        int totalCount = 0;
        public int TotalCount
        {
            get { return totalCount; }
            set
            {
                totalCount = value;
                if (value == 0)
                    BaseDataBase.MakeTabItemRed(this.Parent as TabItem);
                else
                    BaseDataBase.MakeTabItemGreen(this.Parent as TabItem);
                OnPropertyChanged(new PropertyChangedEventArgs("TotalCount"));
            }
        }

        List<House> houses;
        public List<House> Houses
        {
            get { return houses; }
            set
            {
                houses = value;
                TotalCount = value.Count;
                if (TotalCount == 0)
                {
                    Place = 0;
                    House fn = new House();
                    if (FamilyID == null)
                        fn.OrphanID = OrphanID;
                    else
                        fn.FamilyID = FamilyID;
                    this.DataContext = fn;
                }
                else
                {
                    Place = 1;
                }
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Place++;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Place--;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as House;
            if (x.IsValidate())
            {
                if (x.FamilyID == null && x.OrphanID == null)
                {
                    MyMessageBox.Show("لم يتم إدخال بيانات العائلة او اليتيم");
                    return;
                }
                if (x.HouseID != null)
                {
                    if (!BaseDataBase.CurrentUser.CanUpdate)
                        MyMessageBox.Show("لا يوجد لديك صلاحيات للتعديل");
                    else if (DBMain.UpdateData(x))
                        MyMessage.UpdateMessage();
                }
                else
                {
                    if (DBMain.InsertData(x))
                    {
                        MyMessage.InsertMessage();
                        Houses.Add(x);
                        TotalCount = Houses.Count;
                        Place = TotalCount;
                    }
                }
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            svMain.ScrollToHome();
            var x = this.DataContext as House;
            if (x.HouseID != null)
            {
                var h = new House();
                if (FamilyID == null)
                    h.OrphanID = OrphanID;
                else
                    h.FamilyID = FamilyID;
                this.DataContext = h;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as House;
            if (x.HouseID != null)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("لا يوجد لديك صلاحيات للحذف");
                }
                else if (MyMessageBox.Show("هل تريد تأكيد حذف البيانات", MessageBoxButton.YesNo) == MessageBoxResult.Yes && DBMain.DeleteData(x))
                {
                    MyMessage.DeleteMessage();
                    if (FamilyID != null) 
                        FamilyID = FamilyID;
                    else OrphanID = OrphanID;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private void SectorNum_Click(object sender, RoutedEventArgs e)
        {
            HouseSector w = new HouseSector();
            w.DataContext = this.DataContext;
            w.ShowDialog();
        }
    }
}
