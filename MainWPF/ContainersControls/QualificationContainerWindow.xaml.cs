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
using System.Windows.Shapes;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for QualificationContainerWindow.xaml
    /// </summary>
    public partial class QualificationContainerWindow : Window, INotifyPropertyChanged
    {
        Qualification.QualificationPersonType PersonType;
        public QualificationContainerWindow(Qualification.QualificationPersonType PersonType)
        {
            InitializeComponent();
            this.PersonType = PersonType;
            cmbo.ItemsSource = BaseDataBase.GetAllStrings("select Distinct QualificationName from PersonQualification");
        }

        int? personID;
        public int? PersonID
        {
            get { return personID; }
            set
            {
                personID = value;
                Qualifications = Qualification.GetQualificationAllByPersonID(PersonType, PersonID);
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
                        this.DataContext = Qualifications[Place - 1];
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
                OnPropertyChanged(new PropertyChangedEventArgs("TotalCount"));
            }
        }

        List<Qualification> qualifications;
        public List<Qualification> Qualifications
        {
            get { return qualifications; }
            set
            {
                qualifications = value;
                TotalCount = value.Count;
                if (TotalCount == 0)
                {
                    Place = 0;
                    Qualification c = new Qualification();
                    c.PersonType = PersonType;
                    c.PersonID = PersonID;
                    this.DataContext = c;
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
            if ((this.DataContext as Qualification).IsValidate())
            {
                var x = this.DataContext as Qualification;
                if (x.PersonID == null)
                {
                    MyMessageBox.Show("لم يتم إدخال بيانات الوالد");
                    return;
                }
                if (x.PersonQualificationID != null)
                {
                    if (!BaseDataBase.CurrentUser.CanUpdate)
                        MyMessageBox.Show("لا يوجد لديك صلاحيات للتعديل");
                    else if (x.UpdatePersonQualificationData())
                        MyMessage.UpdateMessage();
                }
                else
                {
                    if (x.InsertPersonQualificationData())
                    {
                        MyMessage.InsertMessage();
                        Qualifications.Add(x);
                        TotalCount = Qualifications.Count;
                        Place = TotalCount;
                    }
                }
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as Qualification;
            if (x.PersonQualificationID != null)
            {
                var b = new Qualification();
                b.PersonType = PersonType;
                b.PersonID = personID;
                this.DataContext = b;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as Qualification;
            if (x.PersonQualificationID != null)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("لا يوجد لديك صلاحيات للحذف");
                }
                else if (MyMessageBox.Show("هل تريد تأكيد حذف البيانات", MessageBoxButton.YesNo) == MessageBoxResult.Yes && x.DeletePersonQualificationData())
                {
                    MyMessage.DeleteMessage();
                    x.PersonType = PersonType;
                    x.PersonID = personID;
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
