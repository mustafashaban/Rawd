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
    /// Interaction logic for FamilyNeedControl.xaml
    /// </summary>
    public partial class FamilyNeedControl : UserControl, INotifyPropertyChanged
    {
        public FamilyNeedControl()
        {
            InitializeComponent();
        }

        private void MyControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCombo();
        }
        void UpdateCombo()
        {
            cmbo.ItemsSource = BaseDataBase.GetAllStrings("select Distinct NeedType from FamilyNeed order by NeedType");
        }

        int? familyID;
        public int? FamilyID
        {
            get { return familyID; }
            set
            {
                familyID = value;
                FamilyNeeds = FamilyNeed.GetFamilyNeedAllByFamilyID(FamilyID);
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
                        this.DataContext = FamilyNeeds[Place - 1];
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

        List<FamilyNeed> familyNeeds;
        public List<FamilyNeed> FamilyNeeds
        {
            get { return familyNeeds; }
            set
            {
                familyNeeds = value;
                TotalCount = value.Count;
                if (TotalCount == 0)
                {
                    Place = 0;
                    FamilyNeed fn = new FamilyNeed();
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
            if (!BaseDataBase.CurrentUser.CanEnterFamilyNeed)
            {
                MyMessageBox.Show("ليس لديك صلاحية اضافة احتياجات عائلة");
                return;
            }
            var x = this.DataContext as FamilyNeed;
            if (x.IsValidate())
            {
                if (x.FamilyID == null)
                {
                    MyMessageBox.Show("لم يتم إدخال بيانات العائلة");
                    return;
                }
                if (x.FamilyNeedID != null)
                {
                    if (BaseDataBase.CurrentUser.CanUpdate && DBMain.UpdateData(x))
                    {
                        MyMessage.UpdateMessage();
                        UpdateCombo();
                    }
                }
                else
                {
                    if (DBMain.InsertData(x))
                    {
                        MyMessage.InsertMessage();
                        FamilyNeeds.Add(x);
                        TotalCount = FamilyNeeds.Count;
                        Place = TotalCount;
                        UpdateCombo();
                    }
                }
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as FamilyNeed;
            if (x.FamilyNeedID != null)
            {
                var fn = new FamilyNeed();
                fn.FamilyID = FamilyID;
                this.DataContext = fn;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as FamilyNeed;
            if (x.FamilyNeedID != null)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("لا يوجد لديك صلاحيات للحذف");
                }
                else if (MyMessageBox.Show("هل تريد تأكيد حذف البيانات", MessageBoxButton.YesNo) == MessageBoxResult.Yes && DBMain.DeleteData(x))
                {
                    MyMessage.DeleteMessage();
                    FamilyID = FamilyID;
                    UpdateCombo();
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

    }
}
