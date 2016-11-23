using System;
using System.Collections.Generic;
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
    /// Interaction logic for CancelFamilyWindow.xaml
    /// </summary>
    public partial class CancelFamilyWindow : Window
    {
        string OldReason;
        bool OldIsCanceled;
        public CancelFamilyWindow(int? FamilyID)
        {
            InitializeComponent();
            var f = Family.GetFamilyCancelDataByID(FamilyID);
            if (!f.CancelDate.HasValue)
                f.CancelDate = BaseDataBase.DateNow;
            if (!f.AcquittanceDate.HasValue)
                f.AcquittanceDate = BaseDataBase.DateNow;

            OldReason = f.CancelReason;
            OldIsCanceled = f.IsCanceled;
            this.DataContext = f;

            if (BaseDataBase.CurrentUser.IsAdmin)
                txtNotes.IsReadOnly = false;
        }

        

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Family f = this.DataContext as Family;
            if (f != null)
            {
                if (chk.IsChecked == false)
                {
                    //f.CancelDate = null;
                    //f.CancelReason = "";
                    DBMain.UpdateCancelData(f);
                    DialogResult = true;
                }
                else if (f.CancelDate == null)
                {
                    MessageBox.Show("يجب إدخال تاريخ الإلغاء");
                }
                /*else if(chkAcquittance.IsChecked == false && f.AcquittanceDate == null)
                {
                    MessageBox.Show("يجب إدخال تاريخ منح براءة الذمة");
                }*/
                else if(OldIsCanceled != f.IsCanceled && f.CancelReason == OldReason)
                {
                    MyMessageBox.Show("يجب إدخال سبب الالغاء او التفعيل");
                }
                else
                {
                    DBMain.UpdateCancelData(f);
                    DialogResult = true;
                }
            }
        }

        private void btnAddNotes_Click(object sender, RoutedEventArgs e)
        {
            NotesWindow w = new MainWPF.NotesWindow();
            w.txtHeader.Text = "سبب الالغاء";
            if (w.ShowDialog() == true)
            {
                var f = this.DataContext as Family;
                if (!string.IsNullOrWhiteSpace(f.CancelReason))
                    f.CancelReason += "\n";
                f.CancelReason += w.txt.Text + $" ({BaseDataBase.CurrentUser.Name} - {BaseDataBase.DateNow.ToString("dd/MM/yyy")})";
                f.NotifyPropertyChanged("CancelReason");
            }
        }
    }
}
