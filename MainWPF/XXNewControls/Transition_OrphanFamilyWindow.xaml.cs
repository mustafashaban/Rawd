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
    public partial class Transition_OrphanFamilyWindow : Window
    {
        Family f;
        public Transition_OrphanFamilyWindow(Family f)
        {
            InitializeComponent();
            this.f = f;
            txtFamily.Text = f.FamilyCode;
            var i = new Invoice();
            foreach (var o in f.FamilyOrphans.Where(x => x.Type != "طالب علم" && x.CurrentSponsorship != null))
            {
                var t = i.AddTransition();
                t.RightAccount = o.Account;
                t.Value = o.CurrentSponsorship.AvailableSponsorship.SponsorshipValue;
                t.Value = o.CurrentSponsorship.IsDouble ? t.Value * 2 : t.Value;
                t.Details = "دفعة من صندوق الايتام";
                t.LeftAccount = new Account() { Id = (o.Type == "يتيم" ? 2 : 3) };//the id of ophan fund (2) account or ophan student fund (3) account 
                t.RelatedSponsorship = o.CurrentSponsorship;
                t.RelatedSponsorship.OrphanName = o.FirstName + " " + o.LastName;
                t.SponsorshipID = o.CurrentSponsorship.ID;
                t.AccountType = (o.Type == "يتيم" ? Account.AccountType.Orphan : Account.AccountType.OrphanStudent); //the ophan type (2) or ophan student type (3)
            }

            i.Serial = "A";
            i.Receiver = BaseDataBase._Scalar($"select FirstName + ' ' + Isnull(LastName,'') from Parent where FamilyID = {f.FamilyID} and Gender = 'أنثى'");
            i.ReceiverPID = BaseDataBase._Scalar($"select IsNull(PID,'') from Parent where FamilyID = {f.FamilyID} and Gender = 'أنثى'");
            this.DataContext = i;
        }

        void GetData()
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var i = this.DataContext as Invoice;
            if (i.IsValidate())
            {
                if (Invoice.InsertData(i))
                {
                    foreach (var t in i.Transitions)
                    {
                        if (Transition.InsertData(t))
                        {
                            Transition tt = new Transition();
                            tt.Details = "ترصيد حساب اليتيم في الكفيل";
                            tt.LeftAccount = t.RightAccount;
                            tt.RightAccount = Account.GetAccountByOwnerID(Account.AccountType.Sponsor, t.RelatedSponsorship.AvailableSponsorship.RelatedSponsor.SponsorID.Value, false);
                            tt.SponsorshipID = t.RelatedSponsorship.ID;
                            tt.Value = t.Value;
                            tt.AccountType = t.AccountType;
                            Transition.InsertData(tt);
                        }
                        else
                        {
                            MyMessageBox.Show("حدث خطأ اثناء حفظ البيانات يرجى مراجعة مسؤل البرنامج");
                            return;
                        }
                    }
                    PrintTicket.printInvoiceA6(i, 2);
                    MyMessage.InsertMessage();
                    DialogResult = true;
                }
                else
                {
                    MyMessageBox.Show("حدث خطأ اثناء حفظ البيانات يرجى مراجعة مسؤل البرنامج");
                    return;
                }
            }
        }

        private void cmboReciever_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                var i = this.DataContext as Invoice;
                if ((sender as ComboBox).SelectedIndex == 3)
                {
                    txtReceiver.IsReadOnly = false;
                    txtReceiverPID.IsReadOnly = false;
                    i.Receiver = "";
                    i.ReceiverPID = "";
                }
                else
                {
                    txtReceiver.IsReadOnly = true;
                    txtReceiverPID.IsReadOnly = true;
                    if ((sender as ComboBox).SelectedIndex == 0)
                    {
                        i.Receiver = BaseDataBase._Scalar($"select FirstName + ' ' + Isnull(LastName,'') from Parent where FamilyID = {f.FamilyID} and Gender = 'أنثى'");
                        i.ReceiverPID = BaseDataBase._Scalar($"select IsNull(PID,'') from Parent where FamilyID = {f.FamilyID} and Gender = 'أنثى'");
                    }
                    else if ((sender as ComboBox).SelectedIndex == 1)
                    {
                        i.Receiver = BaseDataBase._Scalar($"select FirstName + ' ' + Isnull(LastName,'') from Guardian where FamilyID = {f.FamilyID} and Gender = 'أنثى'");
                        i.ReceiverPID = BaseDataBase._Scalar($"select IsNull(PID,'') from Guardian where FamilyID = {f.FamilyID} and Gender = 'أنثى'");
                    }
                    else
                    {
                        i.Receiver = BaseDataBase._Scalar($"select FirstName + ' ' + Isnull(LastName,'') from Guardian where FamilyID = {f.FamilyID} and Gender = N'ذكر'");
                        i.ReceiverPID = BaseDataBase._Scalar($"select IsNull(PID,'') from Guardian where FamilyID = {f.FamilyID} and Gender = N'ذكر'");
                    }
                }
            }
        }
    }
}
