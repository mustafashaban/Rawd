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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainWPF
{
    public partial class SponsorControl : UserControl
    {
        public SponsorControl(Sponsor s)
        {
            InitializeComponent();
            myWindow.DataContext = s;
            cAccount.Account = s.Account;
        }

        private void BtnExecute_Click(object sender, RoutedEventArgs e)
        {
            var x = (Sponsor)this.DataContext;
            if (x.IsValidate())
            {
                if (!x.SponsorID.HasValue)
                {
                    if (Sponsor.InsertData(x))
                    {
                        MyMessage.InsertMessage();

                        x.Account = new MainWPF.Account();
                        x.Account.Name = x.Name;
                        x.Account.Type = Account.AccountType.Sponsor;
                        x.Account.CurrentBalance = 0;
                        x.Account.CreateDate = BaseDataBase.DateNow;
                        x.Account.OwnerID = x.SponsorID;
                        x.Account.Status = "مفعل";
                        x.Account.IsDebit = true;
                        Account.InsertData(x.Account);
                    }
                }
                else
                {
                    if (Sponsor.UpdateData(x))
                    {
                        MyMessage.UpdateMessage();
                    }
                }
            }
        }

        private void btnAddNewSponsorship_Click(object sender, RoutedEventArgs e)
        {
            var s = this.DataContext as Sponsor;
            NewSponsorshipsWindow w = new MainWPF.NewSponsorshipsWindow(new AvailableSponsorship() { RelatedSponsor = s });
            if (w.ShowDialog() == true)
            {
                var a = w.DataContext as AvailableSponsorship;
                if (s.AvailableSponsorships == null)
                    s.AvailableSponsorships = new List<MainWPF.AvailableSponsorship>();
                s.AvailableSponsorships.Add(a);
                dgAvailableSponsorship.Items.Refresh();
                s.NotifyPropertyChanged("AllSponsorships");
                s.NotifyPropertyChanged("EndedSponsorships");
                s.NotifyPropertyChanged("WaitSponsorships");
            }
        }

        private void btnDeleteSponsorship_Click(object sender, RoutedEventArgs e)
        {
            if (dgAvailableSponsorship.SelectedIndex >= 0)
            {
                var s = this.DataContext as Sponsor;
                var a = dgSponsorship.SelectedItem as AvailableSponsorship;
                ////if (canremove)
                //{
                //    if (MyMessageBox.Show("هل تريد تأكيد حذف الكفالة", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                //        if (AvailableSponsorship.DeleteData(a))
                //        {
                //            MyMessage.DeleteMessage();
                //            s.AvailableSponsorships.Remove(a);
                //            dgSponsorship.Items.Refresh();
                //            s.NotifyPropertyChanged("AllSponsorships");
                //            s.NotifyPropertyChanged("EndedSponsorships");
                //            s.NotifyPropertyChanged("WaitSponsorships");
                //        }
                //}
                //else MyMessageBox.Show("لايمكن حذف الكفالات المنتهية وغير المنتهية");
            }
        }


        private void btnDeposit1_Click(object sender, RoutedEventArgs e)
        {
            var s = this.DataContext as Sponsor;
            if (s != null && s.Account != null)
            {
                if (s.AvailableSponsorships == null || s.AvailableSponsorships.Count == 0)
                {
                    MyMessageBox.Show("الكفيل الحالي ليس لديه اي كفالات مدخلة\nيجب ادخال كفالات أولاً");
                    return;
                }
                Transition_SponsorWindow w = new Transition_SponsorWindow(s, FundType.Private);
                if (w.ShowDialog() == true)
                {
                    s.Account.Transitions = Transition.GetAllTransitionByAccount(s.Account);
                    s.Account.NotifyPropertyChanged("Transitions");
                    cAccount.dgTransitions.Items.Refresh();
                    cAccount.RefreshValues();
                }
            }
        }

        private void btnDeposit2_Click(object sender, RoutedEventArgs e)
        {
            var s = this.DataContext as Sponsor;
            if (s != null && s.Account != null)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift) && Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftAlt))
                {
                    Transition_SponsorWindow w = new Transition_SponsorWindow(s, FundType.GeneralHidden);
                    if (w.ShowDialog() == true) ;//No Refresh
                }
                else
                {
                    Transition_SponsorWindow w = new Transition_SponsorWindow(s, FundType.General);
                    if (w.ShowDialog() == true)
                    {
                        s.Account = Account.GetAccountByOwnerID(Account.AccountType.Sponsor, s.SponsorID.Value);
                        cAccount.Account = s.Account;
                    }
                }
            }
        }

        private void btnWithdrawn_Click(object sender, RoutedEventArgs e)
        {
            //var a = this.DataContext as Sponsor;
            //if (a != null && a.Account != null)
            //{
            //    MyMessageBox.Show("الكفيل الحالي ليس لديه اي كفالات مدخلة\nيجب ادخال كفالات أولاً");
            //    return;
            //}
        }
    }
}
