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
    /// Interaction logic for Transition_StudentWindow.xaml
    /// </summary>
    public partial class Transition_StudentWindow : Window
    {
        Orphan o;
        public Transition_StudentWindow(Orphan o)
        {
            InitializeComponent();
            this.o = o;
            txtStudent.Text = o.FirstName + " " + o.LastName;
            var i = new Invoice();
            var t = i.AddTransition();
            t.RightAccount = o.Account;
            t.LeftAccount = new Account() { Id = 4 }; //4 is the id of student fund account
            t.Value = o.CurrentSponsorship.AvailableSponsorship.SponsorshipValue;
            t.SponsorshipID = o.CurrentSponsorship.ID;
            t.AccountType = Account.AccountType.Student; // 4 is the student type id
            txtSponsor.Text = o.CurrentSponsorship.AvailableSponsorship.RelatedSponsor.Name;
            this.DataContext = i;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var i = this.DataContext as Invoice;
            i.Description = i.Transitions[0].Details;
            if (i.IsValidate())
            {
                if (Invoice.InsertData(i) && Transition.InsertData(i.Transitions[0]))
                {
                    Transition tt = new Transition();
                    tt.Details = "ترصيد حساب طالب العلم في الكفيل";
                    tt.LeftAccount = i.Transitions[0].RightAccount;
                    tt.RightAccount = Account.GetAccountByOwnerID(Account.AccountType.Sponsor, o.CurrentSponsorship.AvailableSponsorship.RelatedSponsor.SponsorID.Value, false);
                    tt.SponsorshipID = o.CurrentSponsorship.ID;
                    tt.Value = i.Transitions[0].Value;
                    tt.AccountType = Account.AccountType.Student;
                    Transition.InsertData(tt);
                    MyMessage.InsertMessage();
                    DialogResult = true;
                }
            }
        }
    }
}
