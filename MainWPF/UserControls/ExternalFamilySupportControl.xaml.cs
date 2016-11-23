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
    /// Interaction logic for ExternalFamilySupportControl.xaml
    /// </summary>
    public partial class ExternalFamilySupportControl : UserControl, INotifyPropertyChanged
    {
        public ExternalFamilySupportControl()
        {
            InitializeComponent();
        }

        private void MyControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCombo();
        }
        void UpdateCombo()
        {
            cmbo.ItemsSource = BaseDataBase.GetAllStrings("select Distinct SupportType from ExternalFamilySupport order by SupportType");
        }

        List<ExternalFamilySupport> externalFamilySupports;
        public List<ExternalFamilySupport> ExternalFamilySupports
        {
            get { return externalFamilySupports; }
            set
            {
                externalFamilySupports = value;
                TotalCount = value.Count;
                if (TotalCount == 0)
                {
                    Place = 0;
                    ExternalFamilySupport efs = new ExternalFamilySupport();
                    efs.FamilyID = FamilyID;
                    this.DataContext = efs;
                }
                else
                {
                    Place = 1;
                }
            }
        }

        int? familyID;
        public int? FamilyID
        {
            get { return familyID; }
            set
            {
                familyID = value;
                ExternalFamilySupports = ExternalFamilySupport.GetExternalFamilySupportAllByFamilyID(FamilyID);
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
                        this.DataContext = ExternalFamilySupports[Place - 1];
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
            var x = this.DataContext as ExternalFamilySupport;
            if (x.IsValidate())
            {
                if (x.FamilyID == null)
                {
                    MyMessageBox.Show("لم يتم إدخال بيانات العائلة");
                    return;
                }
                if (x.ExternalFamilySupportID != null)
                {
                    if (x.UpdateExternalFamilyFata())
                    {
                        MyMessage.UpdateMessage();
                        UpdateCombo();                        
                    }
                }
                else
                {
                    if (x.InsertExternalFamilyData())
                    {
                        MyMessage.InsertMessage();
                        ExternalFamilySupports.Add(x);
                        TotalCount = ExternalFamilySupports.Count;
                        Place = TotalCount;
                        UpdateCombo();
                    }
                }
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as ExternalFamilySupport;
            if (x.ExternalFamilySupportID != null)
            {
                var efs = new ExternalFamilySupport();
                efs.FamilyID = FamilyID;
                this.DataContext = efs;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as ExternalFamilySupport;
            if (x.ExternalFamilySupportID != null)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("لا يوجد لديك صلاحيات للحذف");
                }
                else if (MyMessageBox.Show("هل تريد تأكيد حذف البيانات", MessageBoxButton.YesNo) == MessageBoxResult.Yes && x.DeleteExternalFamilyData())
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

    }
}
