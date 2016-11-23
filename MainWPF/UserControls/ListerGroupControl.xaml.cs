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
    /// Interaction logic for ListerGroupControl.xaml
    /// </summary>
    public partial class ListerGroupControl : UserControl, INotifyPropertyChanged
    {
        public ListerGroupControl()
        {
            InitializeComponent();
            if (BaseDataBase.CurrentUser.IsAdmin)
                txtNotes.IsReadOnly = false;
        }

        int? familyID;
        public int? FamilyID
        {
            get { return familyID; }
            set
            {
                familyID = value;
                ListerGroups = ListerGroup.GetListerGroupsByFamilyID(value);
            }
        }

        int? orphanID;
        public int? OrphanID
        {
            get { return orphanID; }
            set
            {
                orphanID = value;
                ListerGroups = ListerGroup.GetListerGroupsByOrphanID(value);
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
                        this.DataContext = ListerGroups[Place - 1];
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

        List<ListerGroup> listerGroups;
        public List<ListerGroup> ListerGroups
        {
            get { return listerGroups; }
            set
            {
                listerGroups = value;
                TotalCount = value.Count;
                if (TotalCount == 0)
                {
                    Place = 0;
                    ListerGroup lg = new ListerGroup();
                    if (FamilyID == null)
                        lg.OrphanID = OrphanID;
                    else
                        lg.FamilyID = FamilyID;
                    this.DataContext = lg;
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
            if (!BaseDataBase.CurrentUser.CanEnterListerGroup)
            {
                MyMessageBox.Show("ليس لديك صلاحية ادخال بيانات فريق تقييم");
                return;
            }
            var x = this.DataContext as ListerGroup;
            if (lbListerGroup.Items.Count <= 0)
            {
                MyMessageBox.Show("يجب  إدخال مقيّمين");
                return;
            }
            if (x.IsValidate())
            {
                if (x.FamilyID == null && x.OrphanID == null)
                {
                    MyMessageBox.Show("لم يتم إدخال بيانات العائلة او اليتيم");
                    return;
                }
                if (x.GroupID != null)
                {
                    if (!BaseDataBase.CurrentUser.IsAdmin && BaseDataBase.DateNow.ToShortDateString() != x.CreateDate.ToShortDateString())
                    {
                        MyMessageBox.Show("لايمكن تعديل بيانات فريق التقييم الا في يوم الادخال فقط");
                        return;
                    }
                    if (DBMain.UpdateData(x))
                        MyMessage.UpdateMessage();
                }
                else
                {
                    if (DBMain.InsertData(x))
                    {
                        MyMessage.InsertMessage();
                        ListerGroups.Add(x);
                        TotalCount = ListerGroups.Count;
                        Place = TotalCount;
                    }
                }
            }
        }
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as ListerGroup;
            if (x.GroupID != null)
            {
                var lg = new ListerGroup();
                if (FamilyID == null)
                    lg.OrphanID = OrphanID;
                else
                    lg.FamilyID = FamilyID;
                this.DataContext = lg;
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var x = this.DataContext as ListerGroup;
            if (x.GroupID != null)
            {
                if (!BaseDataBase.CurrentUser.CanDelete)
                {
                    MyMessageBox.Show("لا يوجد لديك صلاحيات للحذف");
                }
                else if (MyMessageBox.Show("هل تريد تأكيد حذف البيانات", MessageBoxButton.YesNo) == MessageBoxResult.Yes && DBMain.DeleteData(x))
                {
                    MyMessage.DeleteMessage();
                    if (FamilyID != null)
                        FamilyID = FamilyID;
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
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            Lister_OrphanContainer w = new Lister_OrphanContainer();
            if (w.ShowDialog() == true)
            {
                (this.DataContext as ListerGroup).Listers = w.lbListerGroup.ItemsSource as List<Lister>;
            }
        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            var lg = this.DataContext as ListerGroup;
            if (lg != null)
            {
                FamilyNeed_ListerGroupControl w = new FamilyNeed_ListerGroupControl(lg.FamilyNeeds);
                if (w.ShowDialog() == true)
                {
                    var xs = w.dgFamilyNeed.ItemsSource as List<FamilyNeedByLister>;
                    if (xs != null)
                    {
                        foreach (var item in xs)
                        {
                            lg.FamilyNeeds.Add(new FamilyNeed_ListerGroup() { Count = item.Count, FamilyNeedByLister = item });
                        }
                        dgFamilyNeeds.Items.Refresh();
                    }
                }
            }
        }
        private void btnDeleteFamilyNeed_Click(object sender, RoutedEventArgs e)
        {
            if (MyMessageBox.Show("سيتم حذف الاحتياج هل تريد التأكيد", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var x = this.DataContext as ListerGroup;
                x.FamilyNeeds.Remove(dgFamilyNeeds.SelectedItem as FamilyNeed_ListerGroup);

                dgFamilyNeeds.Items.Refresh();
            }
        }

        private void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            btnSave_Click(null, null);
        }
        private void NewCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            btnNew_Click(null, null);
        }

        private void btnAddNotes_Click(object sender, RoutedEventArgs e)
        {
            NotesWindow w = new MainWPF.NotesWindow();
            if (w.ShowDialog() == true)
            {
                var lg = this.DataContext as ListerGroup;
                if (!string.IsNullOrWhiteSpace(lg.Notes))
                    lg.Notes += "\n";
                lg.Notes += w.txt.Text + $" ({BaseDataBase.CurrentUser.Name} - {BaseDataBase.DateNow.ToString("dd/MM/yyy")})";
                lg.NotifyPropertyChanged("Notes");
                btnSave_Click(null, null);
            }
        }
    }
}
