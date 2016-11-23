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
    /// Interaction logic for TrainerControl.xaml
    /// </summary>
    public partial class TrainerControl : Window, INotifyPropertyChanged
    {
        public TrainerControl()
        {
            InitializeComponent();
        }

        int? trainingid;
        public int? TraininigID
        {
            get { return trainingid; }
            set
            {
                trainingid = value;
                Trainers = Trainer.GetTrainerAllByTraining(TraininigID);
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
                        this.DataContext = Trainers[Place - 1];
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

        List<Trainer> trainers;
        public List<Trainer> Trainers
        {
            get { return trainers; }
            set
            {
                trainers = value;
                TotalCount = value.Count;
                if (TotalCount == 0)
                {
                    Place = 0;
                    Trainer t = new Trainer();
                    t.TrainingID = TraininigID;
                    this.DataContext = t;
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
            var x = this.DataContext as Trainer;
            if (string.IsNullOrEmpty(x.TrainerName))
            {
                MyMessageBox.Show("يجب إدخال اسم المدرب");
                return;
            }
            if (x.ID != null)
            {
                if (!BaseDataBase.CurrentUser.CanUpdate)
                    MyMessageBox.Show("لا يوجد لديك صلاحيات للتعديل");
                else if (x.UpdateTrainerData())
                    MyMessage.UpdateMessage();
            }
            else
            {
                if (x.InsertTrainerData())
                {
                    MyMessage.InsertMessage();
                    Trainers.Add(x);
                    TotalCount = Trainers.Count;
                    Place = TotalCount;
                }
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as Trainer;
            if (x.ID != null)
            {
                var fn = new Trainer();
                fn.TrainingID = TraininigID;
                this.DataContext = fn;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as Trainer;
            if (x.ID != null)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("لا يوجد لديك صلاحيات للحذف");
                }
                else if (MyMessageBox.Show("هل تريد تأكيد حذف البيانات", MessageBoxButton.YesNo) == MessageBoxResult.Yes && x.DeleteTrainerData())
                {
                    MyMessage.DeleteMessage();
                    TraininigID = TraininigID;
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
