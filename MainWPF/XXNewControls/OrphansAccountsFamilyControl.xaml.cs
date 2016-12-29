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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainWPF
{
    public partial class OrphansAccountsFamilyControl : UserControl
    {
        public OrphansAccountsFamilyControl()
        {
            InitializeComponent();
        }
        public int FamilyID { get; set; }
        private void btnWithdrawn_Click(object sender, RoutedEventArgs e)
        {
            var x = DataContext as Orphan;
            if (x.OrphanFamily.FamilyOrphans == null || x.OrphanFamily.FamilyOrphans.Where(c => c.Type != "طالب علم").Count() == 0)
            {
                MyMessageBox.Show("لايوجد ايتام في هذه العائلة");
                return;
            }
            string OrphanNotMached = "";
            bool CanContinue = false;
            int diff = 30;
            foreach (var o in x.OrphanFamily.FamilyOrphans.Where(c => c.Type != "طالب علم"))
            {
                if (o.Account != null)
                {
                    if (o.CurrentSponsorship == null)
                        OrphanNotMached += o.FirstName + "\n";
                    else
                    {
                        CanContinue = true;
                        var t = Transition.GetLastTransitionByAccount(o.Account);
                        if (t != null && t.Id.HasValue)
                        {
                            var diff2 = (BaseDataBase.DateNow - t.CreateDate.Value).Days;
                            if (diff2 < diff)
                                diff = diff2;
                        }
                    }
                }
            }
            if (CanContinue)
            {
                if (OrphanNotMached != "" && MyMessageBox.Show($"الايتام التالية غير مكفولة بعد\n{OrphanNotMached}هل تريد الاستمرار؟", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                    return;
            }
            else
            {
                MyMessageBox.Show("لايوجد اي يتيم مكفول في هذه العائلة");
                return;
            }


            if (diff < 25)
            {
                if (!BaseDataBase.CurrentUser.IsAdmin)
                {
                    MyMessageBox.Show($"اخر دفعة تم تسليمها لليتيم منذ {diff} يوم\nلا يمكن التسليم حاليا");
                    return;
                }
                else if (MyMessageBox.Show($"اخر دفعة تم تسليمها لليتيم منذ {diff} يوم\nهل تريد المتابعة؟", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                    return;
            }


            Transition_OrphanFamilyWindow w = new Transition_OrphanFamilyWindow(x.OrphanFamily);
            if (w.ShowDialog() == true)
            {
                lvInvoices.ItemsSource = null;
                lvInvoices.ItemsSource = Invoice.GetAllInvoiceByFamilyID(FamilyID);
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var lst = lvInvoices.ItemsSource as List<Invoice>;
            var i = lvInvoices.SelectedItem as Invoice;
            if (i != null)
            {
                if (!BaseDataBase.CurrentUser.IsAdmin)
                {
                    MyMessageBox.Show("ليس لديك صلاحية حذف دفعة");
                    return;
                }
                else if ((BaseDataBase.DateNow - i.CreateDate.Value).Hours > 24)
                {
                    MyMessageBox.Show("لايمكن حذف الدفعة الا في يوم تاريخ الادخال حصراً");
                    return;
                }
                else if (MyMessageBox.Show("هل تريد تأكيد حذف الدفعة", MessageBoxButton.YesNo) == MessageBoxResult.Yes && Invoice.DeleteData(i))
                {
                    MyMessage.DeleteMessage();
                    lst.Remove(i);
                    lvInvoices.Items.Refresh();
                }
            }
        }
    }
}
