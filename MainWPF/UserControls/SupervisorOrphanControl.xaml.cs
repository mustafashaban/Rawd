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
    /// Interaction logic for SupervisorOrphanControl.xaml
    /// </summary>
    public partial class SupervisorOrphanControl : UserControl, INotifyPropertyChanged
    {
        public SupervisorOrphanControl()
        {
            InitializeComponent();
            cmboSupervisorType.ItemsSource = Orphan_Supervisor.GetAllSupervisorTypes;
        }

        int? orphanID;
        public int? OrphanID
        {
            get { return orphanID; }
            set
            {
                orphanID = value;
                OrphanSupervisors = Orphan_Supervisor.GetOrphanSupervisorAllByOrphanID(value);
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
                        this.DataContext = OrphanSupervisors[Place - 1];
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

        List<Orphan_Supervisor> orphanSupervisors;
        public List<Orphan_Supervisor> OrphanSupervisors
        {
            get { return orphanSupervisors; }
            set
            {
                orphanSupervisors = value;
                TotalCount = value.Count;
                if (TotalCount == 0)
                {
                    Place = 0;
                    Orphan_Supervisor os = new Orphan_Supervisor();
                    os.OrphanID = OrphanID;
                    this.DataContext = os;
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
            var x = this.DataContext as Orphan_Supervisor;
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
                    else if (x.UpdateData())
                        MyMessage.UpdateMessage();
                }
                else
                {
                    if (x.InsertData())
                    {
                        MyMessage.InsertMessage();
                        OrphanSupervisors.Add(x);
                        TotalCount = OrphanSupervisors.Count;
                        Place = TotalCount;
                    }
                }
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as Orphan_Supervisor;
            if (x.ID != null)
            {
                var os = new Orphan_Supervisor();
                os.OrphanID = OrphanID;
                this.DataContext = os;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as Orphan_Supervisor;
            if (x.ID != null)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("لا يوجد لديك صلاحيات للحذف");
                }
                else if (MyMessageBox.Show("هل تريد تأكيد حذف البيانات", MessageBoxButton.YesNo) == MessageBoxResult.Yes && x.DeleteData())
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SupervisorSelectControl w = new SupervisorSelectControl();
            if (w.ShowDialog() == true)
            {
                (this.DataContext as Orphan_Supervisor).SupervisorID = (w.dgSupervisor.SelectedItem as Supervisor).SupervisorID;
            }
        }

    }
}
