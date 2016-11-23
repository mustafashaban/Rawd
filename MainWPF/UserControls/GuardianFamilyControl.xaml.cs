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
    /// Interaction logic for GuardianFamilyControl.xaml
    /// </summary>
    public partial class GuardianFamilyControl : UserControl, INotifyPropertyChanged
    {
        public GuardianFamilyControl()
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
                GuardianOrphans = Guardian_Orphan.GetGuardianAllByFamilyID(value);
            }
        }

        int? orphanID;
        public int? OrphanID
        {
            get { return orphanID; }
            set
            {
                orphanID = value;
                GuardianOrphans = Guardian_Orphan.GetGuardianAllByOrphanID(value);
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
                        this.DataContext = GuardianOrphans[Place - 1];
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

        List<Guardian_Orphan> guardianOrphans;
        public List<Guardian_Orphan> GuardianOrphans
        {
            get { return guardianOrphans; }
            set
            {
                guardianOrphans = value;
                TotalCount = value.Count;
                if (TotalCount == 0)
                {
                    Place = 0;
                    Guardian_Orphan go = new Guardian_Orphan();
                    if (FamilyID == null)
                        go.OrphanID = OrphanID;
                    else
                        go.FamilyID = FamilyID;
                    this.DataContext = go;
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
            var x = this.DataContext as Guardian_Orphan;
            if (x.IsValidate())
            {
                if (x.FamilyID == null && x.OrphanID == null)
                {
                    MyMessageBox.Show("لم يتم إدخال بيانات العائلة او اليتيم");
                    return;
                }
                if (x.ID != null)
                {
                    if (!BaseDataBase.CurrentUser.CanUpdate)
                        MyMessageBox.Show("لا يوجد لديك صلاحيات للتعديل");
                    else if (x.UpdateOrphan_GuardianData())
                        MyMessage.UpdateMessage();
                }
                else
                {
                    if (x.InsertGuardianOrphanData())
                    {
                        MyMessage.InsertMessage();
                        GuardianOrphans.Add(x);
                        TotalCount = GuardianOrphans.Count;
                        Place = TotalCount;
                    }
                }
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as Guardian_Orphan;
            if (x.ID != null)
            {
                var go = new Guardian_Orphan();
                if (FamilyID == null)
                    go.OrphanID = OrphanID;
                else
                    go.FamilyID = FamilyID;
                this.DataContext = go;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as Guardian_Orphan;
            if (x.ID != null)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("لا يوجد لديك صلاحيات للحذف");
                }
                else if (MyMessageBox.Show("هل تريد تأكيد حذف البيانات", MessageBoxButton.YesNo) == MessageBoxResult.Yes && x.DeleteOrphan_GuardianData())
                {
                    MyMessage.DeleteMessage();
                    if(FamilyID != null) FamilyID = FamilyID;
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GuardianMaleControl w = new GuardianMaleControl();
            if (w.ShowDialog() == true)
            {
                (this.DataContext as Guardian_Orphan).GuardianID = (w.dgGuardian.SelectedItem as Guardian).GuardianID;
            }
        }

    }
}
