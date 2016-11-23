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
    public partial class SpecialCardControl : UserControl, INotifyPropertyChanged
    {
        public SpecialCardControl()
        {
            InitializeComponent();
            cmboType.ItemsSource = SpecialCardSource.Types;
            cmboSpecialCard.ItemsSource = SpecialCard.GetAllSpecialCard().Where(x => x.IsActive).ToList();
            cmboCenter.ItemsSource = SpecialCardCenter.GetAllSpecialCardCenter().Where(x => x.IsActive).ToList();
        }
        private void MyControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        int? familyID;
        public int? FamilyID
        {
            get { return familyID; }
            set
            {
                familyID = value;
                if (value != null)
                    SpecialCardSources = SpecialCardSource.GetAllSpecialCardSourcByFamilyID(FamilyID.Value);
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
                        this.DataContext = SpecialCardSources[Place - 1];
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
                if (value == 0)
                    BaseDataBase.MakeTabItemRed(this.Parent as TabItem);
                else
                    BaseDataBase.MakeTabItemGreen(this.Parent as TabItem);
                totalCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalCount"));
            }
        }

        List<SpecialCardSource> specialCardSources;
        public List<SpecialCardSource> SpecialCardSources
        {
            get { return specialCardSources; }
            set
            {
                specialCardSources = value;
                TotalCount = value.Count;
                if (TotalCount == 0)
                {
                    Place = 0;
                    btnNew_Click(null, null);
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
            if (!(BaseDataBase.CurrentUser.CanUpdateFamily || BaseDataBase.CurrentUser.PointAdmin))
            {
                MessageBox.Show("ليس لديك صلاحية اضافة بطافة خاصة");
                return;
            }
            var x = this.DataContext as SpecialCardSource;
            if (x.IsValidate())
            {
                if (x.Id.HasValue)
                {
                    if (!BaseDataBase.CurrentUser.PointAdmin)
                    {
                        MessageBox.Show("ليس لديك صلاحية تعديل بطافة خاصة");
                        return;
                    }
                    if (BaseDataBase.CurrentUser.CanUpdate && SpecialCardSource.UpdateData(x))
                        MyMessage.UpdateMessage();
                }
                else
                {
                    if (SpecialCardSource.InsertData(x))
                    {
                        MyMessage.InsertMessage();
                        SpecialCardSources.Add(x);
                        TotalCount = SpecialCardSources.Count;
                        Place = TotalCount;
                    }
                }
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as SpecialCardSource;
            if (x == null || x.Id.HasValue)
            {
                try
                {
                    var MaxEndDate = SpecialCardSources.Select(s => s.EndDate).Max().Value;
                    if (BaseDataBase.DateNow < MaxEndDate)
                    {
                        MyMessageBox.Show("تاريخ نهاية البطاقة السابقة هو " + MaxEndDate.ToShortDateString());
                        return;
                    }
                }
                catch { }
                var scs = new SpecialCardSource();
                scs.StartDate = BaseDataBase.DateNow;
                scs.EndDate = scs.StartDate.Value.AddDays(120);
                this.DataContext = scs;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as SpecialCardSource;
            if (x.Id.HasValue)
            {
                if (!BaseDataBase.CurrentUser.PointAdmin)
                    MyMessageBox.Show("لا يوجد لديك صلاحيات للحذف");
                else if (MyMessageBox.Show("هل تريد تأكيد حذف البيانات", MessageBoxButton.YesNo) == MessageBoxResult.Yes && SpecialCardSource.DeleteData(x))
                {
                    MyMessage.DeleteMessage();
                    FamilyID = FamilyID;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            btnSave_Click(null, null);
        }
        private void NewCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            btnNew_Click(null, null);
        }

        private void cmboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmboType.SelectedItem != null)
                SetCmboNameItemsSource(((KeyValuePair<int, string>)cmboType.SelectedItem).Key);
        }

        void SetCmboNameItemsSource(int TypeID)
        {
            cmboName.ItemsSource = null;
            switch (TypeID)
            {
                case 1:
                    cmboName.ItemsSource = BaseDataBase._Tabling("select FamilyID ID, FamilyName Name from Family where FamilyID = " + FamilyID).DefaultView;
                    break;
                case 2:
                    cmboName.ItemsSource = BaseDataBase._Tabling("select ParentrID ID, FirstName Name from Parent where FamilyID = " + FamilyID).DefaultView;
                    break;
                case 3:
                    cmboName.ItemsSource = BaseDataBase._Tabling("select FamilyPersonID ID, FirstName Name from FamilyPerson where FamilyID = " + FamilyID).DefaultView;
                    break;
                default:
                    break;
            }
        }
    }
}
