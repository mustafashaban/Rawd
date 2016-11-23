using System;
using System.Collections.Generic;
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
    /// Interaction logic for FamilyControl.xaml
    /// </summary>
    public partial class TempFamilyControl : UserControl
    {
        public TempFamilyControl(TempFamily tf)
        {
            InitializeComponent();
            if (tf != null)
                this.DataContext = tf;
            else
            {
                tf = new TempFamily();
                tf.ApplyDate = BaseDataBase.DateNow;
                tf.FamilyType = "نازحين";
                this.DataContext = tf;
            }

            grdChild.DataContext = new TempChild();

            cmboHouseSection.ItemsSource = House.GetHouseSections;
            cmboHouseStreet.ItemsSource = House.GetHouseStreets;
            cmboOldHouse.ItemsSource = House.GetOldAddresses;
            if (BaseDataBase.CurrentUser.IsAdmin)
                txtNotes.IsReadOnly = false;
        }

        private void btnChild_Click(object sender, RoutedEventArgs e)
        {
            var tf = this.DataContext as TempFamily;
            var tc = grdChild.DataContext as TempChild;
            if (tc == null) return;
            if (radDel.IsChecked == true)
            {
                if (MyMessageBox.Show("هل تريد تأكيد الحذف", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    if (!tc.ChildID.HasValue || TempChild.DeleteData(tc))
                    {
                        tf.TempChilds.Remove(tc);
                        dgChild.Items.Refresh();
                        MyMessage.DeleteMessage();
                        radAdd.IsChecked = true;
                    }
                }
            }
            else
            {
                if (tc.IsValidate())
                {
                    if (radAdd.IsChecked == true)
                    {
                        tc.TempFamilyID = tf.ID;
                        if (!tf.ID.HasValue || TempChild.InsertData(tc))
                        {
                            tf.TempChilds.Add(tc);
                            dgChild.Items.Refresh();
                            grdChild.DataContext = new TempChild();
                            Keyboard.Focus(txtChildName);
                            if (tf.ID.HasValue)
                                MyMessage.InsertMessage();
                        }
                    }
                    else
                    {
                        tc.TempFamilyID = tf.ID;
                        if (!tf.ID.HasValue || TempChild.UpdateData(tc))
                        {
                            dgChild.Items.Refresh();
                            if (tf.ID.HasValue)
                                MyMessage.UpdateMessage();
                        }
                    }
                }
            }
        }

        private void dgChild_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (radAdd.IsChecked != true)
            {
                grdChild.DataContext = dgChild.SelectedItem;
            }
        }
        private void rad_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                if (radAdd.IsChecked == true)
                {
                    grdChild.DataContext = new TempChild();
                    btnChild.Content = "إضافة";
                }
                else
                {
                    grdChild.DataContext = dgChild.SelectedItem;
                    btnChild.Content = radUpd.IsChecked == true ? "تعديل" : "حذف";
                }
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

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

            var tf = this.DataContext as TempFamily;
            if (tf != null)
            {
                if (tf.IsValidate())
                {
                    if (!tf.ID.HasValue)
                    {
                        if (TempFamily.InsertData(tf))
                        {
                            MyMessage.InsertMessage();
                            ColortxtFamilyCode();
                            btnExecute.Content = "تعديل";
                        }
                    }
                    else
                    {
                        if (TempFamily.UpadteData(tf))
                            MyMessage.UpdateMessage();
                    }
                }
            }
        }
        async void ColortxtFamilyCode()
        {
            txtFamilyCode.Background = Brushes.Yellow;
            await Task.Delay(3000);
            txtFamilyCode.Background = Brushes.White;
        }
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var newtf = new TempFamily();
            var oldtf = this.DataContext as TempFamily;
            if (oldtf != null)
            {
                newtf.FamilyType = oldtf.FamilyType;
                newtf.ApplyDate = oldtf.ApplyDate;
                newtf.HouseSection = oldtf.HouseSection;
                newtf.HouseStreet = oldtf.HouseStreet;
                newtf.HouseOldAddress = oldtf.HouseOldAddress;
                newtf.HouseBuildingNumber = oldtf.HouseBuildingNumber;
                newtf.HouseFloor = oldtf.HouseFloor;
                newtf.HouseAddress = oldtf.HouseAddress;
                newtf.Mobile1 = oldtf.Mobile1;
                newtf.Mobile2 = oldtf.Mobile2;
                newtf.Phone = oldtf.Phone;
            }
            this.DataContext = null;
            this.DataContext = newtf;
            grdChild.DataContext = new TempChild();
            //cmboPlace_SelectionChanged(null, null);
            Keyboard.Focus(cmboFamilyType);
        }
        private void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            btnExecute_Click(null, null);
        }
        private void NewCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            btnNew_Click(null, null);
        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var tf = dgChild.DataContext  as TempFamily;
            if (tf != null && tf.ID.HasValue)
                btnExecute.Content = "تعديل";

            //try
            //{
            //    var f = grdFamily.DataContext as TempFamily;
            //    if (f.ID.HasValue)
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

        private void cmboPlace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (IsLoaded)
            //{
            //    try
            //    {
            //        var f = grdFamily.DataContext as TempFamily;
            //        if (!f.ID.HasValue)
            //        {
            //            if (cmboPlace.SelectedIndex == 0)
            //            {
            //                txtFamilyCode.Text = "";
            //                return;
            //            }
            //            string a = cmboPlace.SelectedValue.ToString();
            //            string s = a + BaseDataBase._Scalar_StoredProcedure("sp_GetMaxTempFamilyCodeByChar", new SqlParameter("@char", a));
            //            f.FamilyCode = s;
            //        }
            //    }
            //    catch { }
            //}
        }

        private void btnAddNotes_Click(object sender, RoutedEventArgs e)
        {
            NotesWindow w = new MainWPF.NotesWindow();
            if (w.ShowDialog() == true)
            {
                var f = this.DataContext as TempFamily;
                if (!string.IsNullOrWhiteSpace(f.Notes))
                    f.Notes += "\n";
                f.Notes += w.txt.Text + $" ({BaseDataBase.CurrentUser.Name} - {BaseDataBase.DateNow.ToString("dd/MM/yyy")})";
                f.NotifyPropertyChanged("Notes");
            }
        }
    }
}
