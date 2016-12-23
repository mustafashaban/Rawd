using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for OrphanFamilyDetails.xaml
    /// </summary>
    public partial class OrphanFamilyDetails : UserControl
    {

        private List<Orphan> orphans;
        public List<Orphan> Orphans
        {
            get { return orphans; }
            set
            {
                orphans = value;
                lvOrphans.ItemsSource = null;
                lvOrphans.ItemsSource = Orphans;
            }
        }
        public OrphanFamilyDetails()
        {
            InitializeComponent();
            if (BaseDataBase.CurrentUser.IsAdmin)
                txtNotes.IsReadOnly = false;
            GetData();
        }
        private async void GetData()
        {
            List<string> lst1 = null, lst2 = null, lst3 = null, lst4 = null, lst5 = null;
            await Task.Run(() =>
            {
                lst1 = House.GetOldAddresses;
                lst2 = House.GetHouseSections;
                lst3 = House.GetHouseStreets;
                lst4 = SystemData.GetHouseStatuses;
                lst5 = SystemData.GetHouseTypes;
            });
            cmboOldAddress.ItemsSource = lst1;
            cmboHouseSection.ItemsSource = lst2;
            cmboHouseStreet.ItemsSource = lst3;
            cmboHouseStatus.ItemsSource = lst4;
            cmboHouseType.ItemsSource = lst5;
        }


        private void btnAddNotes_Click(object sender, RoutedEventArgs e)
        {
            NotesWindow w = new MainWPF.NotesWindow();
            if (w.ShowDialog() == true)
            {
                var f = this.DataContext as Family;
                if (!string.IsNullOrWhiteSpace(f.Notes))
                    f.Notes += "\n";
                f.Notes += w.txt.Text + $" ({BaseDataBase.CurrentUser.Name} - {BaseDataBase.DateNow.ToString("dd/MM/yyy")})";
                f.NotifyPropertyChanged("Notes");
            }
        }

        private void btnDeleteFamilyPerson_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmboPlace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (IsLoaded)
            //{
            //    try
            //    {
            //        var f = DataContext as Family;
            //        //if (!f.FamilyID.HasValue || BaseDataBase.CurrentUser.IsAdmin)
            //        {
            //            if (cmboSector.SelectedIndex == -1)
            //            {
            //                f.FamilyCode = "";
            //                return;
            //            }
            //            string a = (cmboSector.SelectedItem as Sector).Code;
            //            string s = a + BaseDataBase._Scalar_StoredProcedure("sp_GetMaxFamilyCodeByChar", new SqlParameter("@char", a));
            //            f.FamilyCode = s;
            //        }
            //    }
            //    catch { }
            //}
        }

        private void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;
            if (txt != null && txt.IsFocused)
                txt.SelectAll();
        }

        private void txtDigitalNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            var f = this.DataContext as Family;
            if (f != null)
            {
                if (!string.IsNullOrEmpty(txtDigitalNumber.Text))
                {
                    var dt = BaseDataBase._Tabling($@"select FamilyCode from Family where FamilyID in
                    (select FamilyID from house where 
                    DigitalAddress like '{txtDigitalNumber.Text}'" +
                    (f.FamilyID.HasValue ? $" and familyID <> {f.FamilyID})" : ")"));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string s = "العنوان الرقمي المدخل موجود في:";
                        foreach (DataRow r in dt.Rows)
                            s += "\n" + r.ItemArray[0].ToString();
                        txtDigitalNumberFamilies.Text = s;
                        pop.IsOpen = true;
                    }
                    else pop.IsOpen = false;
                }
            }
        }

        private void DeleteFamilyPerson_CommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (!BaseDataBase.CurrentUser.CanUpdateFamily)
            {
                MyMessageBox.Show("ليس لديك صلاحية حذف");
                return;
            }
            FamilyPerson fp = dgChild.SelectedCells[0].Item as FamilyPerson;
            if (fp != null)
            {
                if (MyMessageBox.Show("هل تريد حذف السجل؟", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var fps = dgChild.ItemsSource as List<FamilyPerson>;
                    if (fp != null)
                    {
                        dgChild.ItemsSource = null;
                        if (fp.FamilyPersonID.HasValue)
                            DBMain.DeleteData(fp);
                        fps.Remove(fp);
                        dgChild.ItemsSource = fps;
                    }
                }
            }
        }

        private void HandlePreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is DataGrid && !e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }
        static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
        private void dgChild_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            var fp = e.Row.DataContext as FamilyPerson;
            if (fp != null && !fp.FamilyPersonID.HasValue && string.IsNullOrEmpty(fp.LastName))
            {
                var f = grdFather1.DataContext as Parent;
                if (f != null)
                    fp.LastName = f.LastName;
            }
        }

        private void dgChild_RowEditEnding_1(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Cancel)
            {
                e.Cancel = true;
                return;
            }
            var f = DataContext as Family;
            var x = e.Row.DataContext as FamilyPerson;
            if (x != null && f != null)
            {
                if (!x.IsValidate())
                    e.Cancel = true;
                else
                {
                    dgChild.SelectedCells.Clear();
                    var dgc = new DataGridCellInfo(dgChild.Items[dgChild.Items.Count - 1], dgChild.Columns[1]);
                    dgChild.SelectedCells.Add(dgc);
                    dgChild.CurrentCell = dgc;
                }
            }
        }

        private void btnDeleteOrphan_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrphans.SelectedItem != null)
            {
                var o = lvOrphans.SelectedItem as Orphan;
                var f = DataContext as Family;
                if (MyMessageBox.Show("هل تريد تأكيد حذف اليتيم/طالب العلم؟", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                //if CanRemove
                {
                    if (o.OrphanID.HasValue)
                        Orphan.DeleteData(o);
                    Orphans.Remove(o);
                    lvOrphans.Items.Refresh();
                }
            }
        }

        private void btnAddNewOrphan_Click(object sender, RoutedEventArgs e)
        {
            var f = DataContext as Family;
            Orphan o = new Orphan() { OrphanFamily = f };
            OrphanWindow w = new MainWPF.OrphanWindow(o);
            if (w.ShowDialog() == true)
            {
                lvOrphans.ItemsSource = null;
                if (Orphans == null)
                    Orphans = new List<Orphan>();
                Orphans.Add(o);
                if (f.FamilyID.HasValue)
                {
                    if (Orphan.InsertData(o))
                    {
                        o.Account = new Account();
                        o.Account.Name = o.FirstName + o.LastName;
                        o.Account.Type = o.Type == "يتيم" ? Account.AccountType.Orphan : o.Type == "يتيم طالب علم" ? Account.AccountType.OrphanStudent : Account.AccountType.Student;
                        o.Account.CurrentBalance = 0;
                        o.Account.CreateDate = BaseDataBase.DateNow;
                        o.Account.OwnerID = o.OrphanID;
                        o.Account.Status = "مفعل";
                        o.Account.IsDebit = true;

                        Account.InsertData(o.Account);
                    }
                }
                f.FamilyOrphans = Orphans;
                lvOrphans.ItemsSource = Orphans;
            }
        }

        private void btnUpdateOrphan_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrphans.SelectedItem != null)
            {
                var o = lvOrphans.SelectedItem as Orphan;
                var f = DataContext as Family;
                var w = new OrphanWindow(o);
                if (w.ShowDialog() == true)
                    lvOrphans.Items.Refresh();
            }
        }
    }
}
