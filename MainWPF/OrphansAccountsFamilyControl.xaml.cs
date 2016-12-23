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
    /// <summary>
    /// Interaction logic for OrphansAccountsFamilyControl.xaml
    /// </summary>
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
            foreach (var o in x.OrphanFamily.FamilyOrphans.Where(c => c.Type != "طالب علم"))
            {
                if (o.Account != null)
                {
                    if (o.CurrentSponsorship == null)
                    {
                        MyMessageBox.Show($"اليتيم ليس مكفولا بعد\nيجب اختيار كفالة لليتيم '{o.FirstName}' أولا");
                        return;
                    }
                }
            }

            Transition_OrphanFamilyWindow w = new Transition_OrphanFamilyWindow(x.OrphanFamily);
            if (w.ShowDialog() == true)
            {
                lvInvoices.ItemsSource = null;
                lvInvoices.ItemsSource = Invoice.GetAllInvoiceByFamilyID(FamilyID);
            }
        }
    }
}
