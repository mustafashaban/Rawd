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
    /// Interaction logic for ChildControl.xaml
    /// </summary>
    public partial class FamilyPersonControl : UserControl, INotifyPropertyChanged
    {
        public FamilyPersonControl()
        {
            InitializeComponent();
        }

        int? familyID;
        public int? FamilyID
        {
            get { return familyID; }
            set
            {
                familyID = value;
                if (familyID.HasValue)
                    FamilyPersons = (from fp in FamilyPerson.GetFamilyPersonByFamilyID(FamilyID.Value) orderby fp.DOB descending select fp).ToList();
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
                        this.DataContext = FamilyPersons[Place - 1];
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
                if(value == 0)
                    BaseDataBase.MakeTabItemRed(this.Parent as TabItem);
                else
                    BaseDataBase.MakeTabItemGreen(this.Parent as TabItem);
                totalCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalCount"));
            }
        }

        List<FamilyPerson> familyPersons;
        public List<FamilyPerson> FamilyPersons
        {
            get { return familyPersons; }
            set
            {
                familyPersons = value;
                TotalCount = value.Count;
                if (TotalCount == 0)
                {
                    Place = 0;
                    FamilyPerson fp = new FamilyPerson();
                    fp.FamilyID = FamilyID;
                    this.DataContext = fp;
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
            var x = this.DataContext as FamilyPerson;
            if (x.IsValidate())
            {
                if (x.FamilyID == null)
                {
                    MyMessageBox.Show("لم يتم إدخال بيانات العائلة");
                    return;
                }
                if (x.FamilyPersonID.HasValue)
                {
                    if (BaseDataBase.CurrentUser.CanUpdate && DBMain.UpdateData(x))
                        MyMessage.UpdateMessage();
                }
                else
                {
                    if (DBMain.InsertData(x))
                    {
                        MyMessage.InsertMessage();
                        FamilyPersons.Add(x);
                        TotalCount = FamilyPersons.Count;
                        Place = TotalCount;
                    }
                }
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            svMain.ScrollToHome();
            Keyboard.Focus(txtFirstName);
            var x = this.DataContext as FamilyPerson;
            if (x.FamilyPersonID.HasValue)
            {
                var fp = new FamilyPerson();
                fp.FamilyID = FamilyID;
                this.DataContext = fp;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as FamilyPerson;
            if (x.FamilyPersonID.HasValue)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("لا يوجد لديك صلاحيات للحذف");
                }
                else if (MyMessageBox.Show("هل تريد تأكيد حذف البيانات", MessageBoxButton.YesNo) == MessageBoxResult.Yes && DBMain.DeleteData(x))
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



        private void btnQualification_Click(object sender, RoutedEventArgs e)
        {
            var fp = (FamilyPerson)this.DataContext;
            if (fp.FamilyPersonID.HasValue)
            {
                QualificationContainerWindow w = new QualificationContainerWindow(Qualification.QualificationPersonType.Child);
                w.PersonID = fp.FamilyPersonID;
                w.ShowDialog();
            }
        }
    }
}
