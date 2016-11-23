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
    /// Interaction logic for OrphanHealthControl.xaml
    /// </summary>
    public partial class OrphanHealthControl : Window, INotifyPropertyChanged
    {
        public OrphanHealthControl()
        {
            InitializeComponent();
            cmbo.ItemsSource = BaseDataBase.GetAllStrings("select Distinct HealthSituation from OrphanHealth");
        }

        int? orphanID;
        public int? OrphanID
        {
            get { return orphanID; }
            set
            {
                orphanID = value;
                OrphanHealthes = Orphan_Health.GetOrphanHealthAllByOrphanID(OrphanID);
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
                        this.DataContext = OrphanHealthes[Place - 1];
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

        List<Orphan_Health> orphanHealthes;
        public List<Orphan_Health> OrphanHealthes
        {
            get { return orphanHealthes; }
            set
            {
                orphanHealthes = value;
                TotalCount = value.Count;
                if (TotalCount == 0)
                {
                    Place = 0;
                    Orphan_Health oh = new Orphan_Health();
                    oh.OrphanID = OrphanID;
                    this.DataContext = oh;
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
            var x = this.DataContext as Orphan_Health;
            if (x.IsValidate())
            {
                if (x.OrphanID == null)
                {
                    MyMessageBox.Show("لم يتم إدخال بيانات اليتيم");
                    return;
                }
                if (x.ID != null)
                {
                    if (!BaseDataBase.CurrentUser.CanUpdate)
                        MyMessageBox.Show("لا يوجد لديك صلاحيات للتعديل");
                    else if (x.UpdateOrphanHealthData())
                        MyMessage.UpdateMessage();
                }
                else
                {
                    if (x.InsertOrphanHealthData())
                    {
                        MyMessage.InsertMessage();
                        OrphanHealthes.Add(x);
                        TotalCount = OrphanHealthes.Count;
                        Place = TotalCount;
                    }
                }
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as Orphan_Health;
            if (x.ID != null)
            {
                var oh = new Orphan_Health();
                oh.OrphanID = OrphanID;
                this.DataContext = oh;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as Orphan_Health;
            if (x.ID != null)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("لا يوجد لديك صلاحيات للحذف");
                }
                else if (MyMessageBox.Show("هل تريد تأكيد حذف البيانات", MessageBoxButton.YesNo) == MessageBoxResult.Yes && x.DeleteOrphanHealthData())
                {
                    MyMessage.DeleteMessage();
                    OrphanID = OrphanID;
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
