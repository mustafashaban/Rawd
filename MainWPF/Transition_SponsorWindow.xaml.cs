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
    public partial class Transition_SponsorWindow : Window
    {
        public enum FundType { General, GeneralHidden, Private };
        public Transition_SponsorWindow(Sponsor s, FundType ft)
        {
            InitializeComponent();
            txtSponsorName.Text = s.Name;
            var i = new Invoice();
            i.AddTransition();
            i.Transitions[0].LeftAccount = s.Account;

            if (ft == FundType.GeneralHidden)
            {
                i.Transitions[0].IsHidden = true;
                txtHeader.Text += " (ايداع مخفي)";
            }

            if (ft == FundType.Private)
                cmboAccounts.ItemsSource = BaseDataBase._Tabling("select Id, Name from Account where Id between 2 and 4").DefaultView;
            else
            {
                cmboAccounts.ItemsSource = BaseDataBase._Tabling("select Id, Name from Account where Id = 1").DefaultView;
                cmboAccounts.SelectedIndex = 0;
                GetCurrentBalance(s.Account.Id.Value, 1);
            }

            this.DataContext = i;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var i = this.DataContext as Invoice;
            if (cmboAccounts.SelectedItem == null)
            {
                MyMessageBox.Show("يجب اختيار الحساب");
                return;
            }
            i.Transitions[0].RightAccount = new Account() { Id = (int)cmboAccounts.SelectedValue };
            i.Description = i.Transitions[0].Details;
            if (i.IsValidate())
            {
                if (Invoice.InsertData(i) && Transition.InsertData(i.Transitions[0]))
                {
                    MyMessage.InsertMessage();
                    DialogResult = true;
                }
            }
        }

        private void cmboAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded && cmboAccounts.SelectedIndex != -1)
            {
                var i = this.DataContext as Invoice;
                GetCurrentBalance(i.Transitions[0].LeftAccount.Id.Value, (int)cmboAccounts.SelectedValue);
            }
        }

        void GetCurrentBalance(int MainId, int CrossID)
        {
            var r = BaseDataBase._Scalar($@"select dbo.fn_GetCrossAccountBalance({MainId},{CrossID})");
            txtCurrentBalance.Text = r;
        }
    }
}
