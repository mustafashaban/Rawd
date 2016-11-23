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
    /// Interaction logic for MyContinuingOrphanControl.xaml
    /// </summary>
    public partial class MyContinuingOrphanControl : UserControl, INotifyPropertyChanged
    {
        public MyContinuingOrphanControl()
        {
            InitializeComponent();
        }

        int? specialtyID;
        public int? SpecialtyID
        {
            get { return specialtyID; }
            set
            {
                specialtyID = value;
            }
        }

        int? orphanID;
        public int? OrphanID
        {
            get { return orphanID; }
            set
            {
                orphanID = value;
                Specialties = Specialty.GetAllSpecialtyByOrphanID(value, SpecialtyID);
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
                    if (value == 0)
                    {
                        btnBack.IsEnabled = btnNext.IsEnabled = false;
                    }
                    else
                    {
                        btnBack.IsEnabled = (value == 1) ? false : true;
                        btnNext.IsEnabled = (value == TotalCount) ? false : true;
                        this.DataContext = Specialties[Place - 1];
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("Place"));
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

        List<Specialty> specialties;
        public List<Specialty> Specialties
        {
            get { return specialties; }
            set
            {
                specialties = value;
                TotalCount = value.Count;
                if (TotalCount == 0)
                {
                    Place = 0;
                    Specialty s = new Specialty();
                    s.OrphanID = OrphanID;
                    s.SpecialtyID = SpecialtyID;
                    this.DataContext = s;
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
            var x = this.DataContext as Specialty;
            if (string.IsNullOrEmpty(x.SpecialtyValue) || x.Date == null)
            {
                MyMessageBox.Show("يجب إدخال التقييم والتاريخ");
                return;
            }
            if (x.OrphanID == null)
            {
                MyMessageBox.Show("لم يتم إدخال بيانات العائلة");
                return;
            }
            if (x.ID != null)
            {
                x.UpdateData();
                MyMessage.UpdateMessage();
            }
            else
            {
                x.InsertData();
                MyMessage.InsertMessage();
                Specialties.Add(x);
                TotalCount = Specialties.Count;
                Place = TotalCount;
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as Specialty;
            if (x.ID != null)
            {
                var s = new Specialty();
                s.OrphanID = OrphanID;
                s.SpecialtyID = SpecialtyID;
                this.DataContext = s;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as Specialty;
            if (x.ID != null)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("لا يوجد لديك صلاحيات للحذف");
                }
                else if (MyMessageBox.Show("هل تريد تأكيد حذف البيانات", MessageBoxButton.YesNo) == MessageBoxResult.Yes && x.DeleteData())
                {
                    OrphanID = OrphanID;
                    MyMessage.DeleteMessage();
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
