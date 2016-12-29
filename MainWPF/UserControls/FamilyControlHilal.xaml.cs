using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for FamilyControlHilal.xaml
    /// </summary>
    public partial class FamilyControlHilal : UserControl
    {
        public FamilyControlHilal()
        {
            InitializeComponent();
            GetData();
            if (BaseDataBase.CurrentUser.IsAdmin)
                txtNotes.IsReadOnly = false;

        }

        private async void GetData()
        {
            List<string> lst1 = null, lst2 = null, lst3 = null, lst4 = null, lst5 = null, lst6 = null;
            await Task.Run(() =>
            {
                lst1 = House.GetOldAddresses;
                lst2 = House.GetHouseSections;
                lst3 = House.GetHouseStreets;
                lst4 = SystemData.GetHouseStatuses;
                lst5 = SystemData.GetHouseTypes;
                lst6 = SystemData.GetFamilyTypes;
            });
            cmboOldAddress.ItemsSource = lst1;
            cmboHouseSection.ItemsSource = lst2;
            cmboHouseStreet.ItemsSource = lst3;
            cmboHouseStatus.ItemsSource = lst4;
            cmboHouseType.ItemsSource = lst5;
            cmboFamilyType.ItemsSource = lst6;
        }

        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //var f = DataContext as Family;
            //try
            //{
            //    if (f.FamilyID.HasValue)
            //    {
            //        string s = "";
            //        for (int i = 0; i < f.FamilyCode.Length; i++)
            //        {
            //            if (!char.IsNumber(f.FamilyCode[i]))
            //                s += f.FamilyCode[i];
            //            else break;
            //        }
            //        cmboPlace.SelectedValue = s;
            //    }
            //}
            //catch { }
        }

        public void cmboPlace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                try
                {
                    var f = DataContext as Family;
                    //if (!f.FamilyID.HasValue || BaseDataBase.CurrentUser.IsAdmin)
                    {
                        if (cmboSector.SelectedIndex == -1)
                        {
                            f.FamilyCode = "";
                            return;
                        }
                        string a = (cmboSector.SelectedItem as Sector).Code;
                        string s = a + BaseDataBase._Scalar_StoredProcedure("sp_GetMaxFamilyCodeByChar", new SqlParameter("@char", a));
                        f.FamilyCode = s;
                    }
                }
                catch { }
            }
        }

        private void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;
            if (txt != null && txt.IsFocused)
            {
                txt.SelectAll();
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

        private void DeleteFamilyPerson_CommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(BaseDataBase.CurrentUser.CanUpdateFamily || BaseDataBase.CurrentUser.PointAdmin))
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

        private void btnDeleteFamilyPerson_Click(object sender, RoutedEventArgs e)
        {
            DeleteFamilyPerson_CommandExecuted(null, null);
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

        private void dg_LostFocus(object sender, RoutedEventArgs e)
        {
            //var dg = sender as DataGrid;
            //if(dg != null)
            //{
            //    if (dg.SelectionUnit == DataGridSelectionUnit.Cell)
            //        dg.SelectedCells.Clear();
            //    else dg.SelectedItems.Clear();

            //    if (dg.Items.Count > 0)
            //        dg.ScrollIntoView(dg.Items[0], dg.Columns[0]);
            //}
        }

        private void btnUploadImage_Click(object sender, RoutedEventArgs e)
        {
            var f = this.DataContext as Family;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images Files|*.jpg;*.jpeg;*.png;*.bmp";
            ofd.Title = "اختر صورة العائلة";
            if (ofd.ShowDialog() == true)
                f.FamilyReportImage = ofd.FileName;
        }

        private void btnRemoveImage_Click(object sender, RoutedEventArgs e)
        {
            var f = this.DataContext as Family;
            f.FamilyReportImage = "";
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var f = this.DataContext as Family;
            if (File.Exists(f.FamilyReportImage))
            {
                try
                {
                    ImagePreviewWindow w = new ImagePreviewWindow();
                    FileInfo fi = new FileInfo(f.FamilyReportImage);
                    w.img.Source = new BitmapImage(new Uri(fi.FullName, UriKind.Absolute));
                    w.ShowDialog();
                }
                catch { }
            }
        }

        private void btnAddNotes_Click(object sender, RoutedEventArgs e)
        {
            NotesWindow w = new MainWPF.NotesWindow();
            if (w.ShowDialog() == true)
            {
                var f = this.DataContext as Family;
                if (!string.IsNullOrWhiteSpace(f.Notes))
                    f.Notes += "\n-------------------\n";
                f.Notes += w.txt.Text + $" ({BaseDataBase.CurrentUser.Name} - {BaseDataBase.DateNow.ToString("dd/MM/yyy")})";
                f.NotifyPropertyChanged("Notes");
            }
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
    }
}