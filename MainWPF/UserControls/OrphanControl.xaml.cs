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
    /// Interaction logic for OrphanControl.xaml
    /// </summary>
    public partial class OrphanControl : UserControl, INotifyPropertyChanged
    {
        public OrphanControl()
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
                Orphans = Orphan.GetOrphanAllByFamilyID(value);
            }
        }

        int? orphanID;
        public int? OrphanID
        {
            get { return orphanID; }
            set
            {
                orphanID = value;
                grdOrphansAll.Visibility = System.Windows.Visibility.Hidden;
                grdOrphanSingle.Visibility = System.Windows.Visibility.Visible;
                this.DataContext = Orphan.GetOrphanByID(value);
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
                        this.DataContext = Orphans[Place - 1];
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

        List<Orphan> orphans;
        public List<Orphan> Orphans
        {
            get { return orphans; }
            set
            {
                orphans = value;
                TotalCount = value.Count;
                if (TotalCount == 0)
                {
                    Place = 0;
                    Orphan o = new Orphan();
                    o.FamilyID = FamilyID;
                    this.DataContext = o;
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
            var x = this.DataContext as Orphan;
            if (x.IsValidate())
            {
                if (x.FamilyID == null)
                {
                    MyMessageBox.Show("لم يتم إدخال بيانات العائلة");
                    return;
                }

                if (Family.GetFamilyByID(x.FamilyID.Value).FamilyType == "أيتام")
                {
                    if (x.Gender == "ذكر" && (((BaseDataBase.DateNow - x.DOB.Value)).Days / 30 / 12) >= 18)
                    {
                        if (MyMessageBox.Show("عمر اليتيم أكبر من 18\nهل تريد المتابعة", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                            return;
                    }
                    else if (x.Gender == "أنثى" && x.MaritalStatus != "أعزب")
                    {
                        if (MyMessageBox.Show("اليتيمة ليست عازبة\nهل تريد المتابعة", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                            return;
                    }
                }
                
                if (x.OrphanID != null)
                {
                    if (!BaseDataBase.CurrentUser.CanUpdate)
                        MyMessageBox.Show("لا يوجد لديك صلاحيات للتعديل");
                    else if (x.UpdateOrphanData())
                        MyMessage.UpdateMessage();
                }
                else
                {
                    if (x.InsertOrphanData())
                    {
                        MyMessage.InsertMessage();
                        Orphans.Add(x);
                        TotalCount = Orphans.Count;
                        Place = TotalCount;
                    }
                }
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            svMain.ScrollToHome();
            var x = this.DataContext as Orphan;
            if (x.OrphanID != null)
            {
                var o = new Orphan();
                o.FamilyID = FamilyID;
                this.DataContext = o;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as Orphan;
            if (x.OrphanID != null)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("لا يوجد لديك صلاحيات للحذف");
                }
                else if (int.Parse(BaseDataBase._Scalar("select COUNT(ID) from [Orphan-Sponsor] where OrphanID = " + x.OrphanID)) > 0)
                    MyMessageBox.Show("لا يمكن حذف بيانات اليتيم بسبب وجود بيانات كفالة له\nيجب حذف بيانات الكفالة أولاً");
                else if (int.Parse(BaseDataBase._Scalar("select COUNT(ID) from [Orphan-Supervisor] where OrphanID = " + x.OrphanID)) > 0)
                    MyMessageBox.Show("لا يمكن حذف بيانات اليتيم بسبب وجود بيانات إشراف له\nيجب حذف بيانات الإشراف أولاً");
                else if (MyMessageBox.Show("هل تريد تأكيد حذف بيانات اليتيم\nيمكنك إلغاء اليتيم عبر اختيار تاريخ الإلغاء مع السبب", MessageBoxButton.YesNo) == MessageBoxResult.Yes && MyMessageBox.Show("هل تريد تأكيد حذف بيانات اليتيم\n\nملاحظة:\nسيتم حذف البيانات التالية لليتيم\n\tالحالة الصحية، وضع الإعاقة، المؤهلات، الدورات التدريبية، بيانات المستوى التعليمي", MessageBoxButton.YesNo) == MessageBoxResult.Yes &&
                            x.DeleteOrphanData())
                {
                    MyMessage.DeleteMessage();
                    FamilyID = FamilyID;
                    svMain.ScrollToHome();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }


        private void btnOrphanHealth_Click(object sender, RoutedEventArgs e)
        {
            Orphan o = this.DataContext as Orphan;
            if (o != null && o.OrphanID != null)
            {
                OrphanHealthControl w = new OrphanHealthControl();
                w.OrphanID = o.OrphanID;
                w.ShowDialog();
            }
            else MyMessageBox.Show("لم يتم إدخال بيانات اليتيم");
        }

        private void btnOrphanImpeding_Click(object sender, RoutedEventArgs e)
        {
            Orphan o = this.DataContext as Orphan;
            if (o != null && o.OrphanID != null)
            {
                OrphanImpedingControl w = new OrphanImpedingControl();
                w.OrphanID = o.OrphanID;
                w.ShowDialog();
            }
            else MyMessageBox.Show("لم يتم إدخال بيانات اليتيم");
        }

        private void btnOrphanTeachingStage_Click(object sender, RoutedEventArgs e)
        {
            Orphan o = this.DataContext as Orphan;
            if (o != null && o.OrphanID != null)
            {
                TeachingStageControl w = new TeachingStageControl();
                w.OrphanID = (this.DataContext as Orphan).OrphanID;
                w.ShowDialog();
            }
            else MyMessageBox.Show("لم يتم إدخال بيانات اليتيم");
        }

        private void btnOrphanQualification_Click(object sender, RoutedEventArgs e)
        {
            var o = (Orphan)this.DataContext;
            if (o != null && o.OrphanID != null)
            {
                QualificationContainerWindow w = new QualificationContainerWindow(Qualification.QualificationPersonType.Orphan);
                w.PersonID = o.OrphanID;
                w.ShowDialog();
            }
            else MyMessageBox.Show("لم يتم إدخال بيانات اليتيم");
        }
    }
}
