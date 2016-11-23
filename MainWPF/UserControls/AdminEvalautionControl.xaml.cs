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
    /// Interaction logic for AdminEvalautionControl.xaml
    /// </summary>
    public partial class AdminEvalautionControl : UserControl, INotifyPropertyChanged
    {
        public AdminEvalautionControl()
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
                AdminEvaluations = AdminEvaluation.GetAdminEvaluationsByFamilyID(value);
            }
        }

        int? orphanID;
        public int? OrphanID
        {
            get { return orphanID; }
            set
            {
                orphanID = value;
                AdminEvaluations = AdminEvaluation.GetAdminEvaluationByOrphanID(value);
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
                        this.DataContext = AdminEvaluations[Place - 1];
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

        List<AdminEvaluation> adminEvaluations;
        public List<AdminEvaluation> AdminEvaluations
        {
            get { return adminEvaluations; }
            set
            {
                adminEvaluations = value;
                TotalCount = value.Count;
                if (TotalCount == 0)
                {
                    Place = 0;
                    AdminEvaluation ae = new AdminEvaluation();
                    if (FamilyID == null)
                        ae.OrphanID = OrphanID;
                    else
                        ae.FamilyID = FamilyID;
                    this.DataContext = ae;
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
            var x = this.DataContext as AdminEvaluation;
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
                    {
                        MyMessageBox.Show("لا يوجد لديك صلاحيات للتعديل");
                    }
                    else if (DBMain.UpdateData(x))
                        MyMessage.UpdateMessage();
                }
                else
                {
                    if (DBMain.InsertData(x))
                    {
                        MyMessage.InsertMessage();
                        AdminEvaluations.Add(x);
                        TotalCount = AdminEvaluations.Count;
                        Place = TotalCount;
                    }
                }
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as AdminEvaluation;
            if (x.ID != null)
            {
                var ae = new AdminEvaluation();
                if (FamilyID == null)
                    ae.OrphanID = OrphanID;
                else
                    ae.FamilyID = FamilyID;
                this.DataContext = ae;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            var x = this.DataContext as AdminEvaluation;
            if (x.FamilyID == null && x.OrphanID == null)
            {
                MyMessageBox.Show("لم يتم إدخال بيانات العائلة او اليتيم");
                return;
            }
            if (x.ID != null)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("لا يوجد لديك صلاحيات للحذف");
                }
                else if (DBMain.DeleteData(x))
                {
                    MyMessage.UpdateMessage();
                    if (FamilyID != null)
                        FamilyID = FamilyID;
                    else OrphanID = OrphanID;
                }
            }
        }
    }
}
