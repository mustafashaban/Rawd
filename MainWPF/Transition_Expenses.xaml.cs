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
    public partial class Transition_Expenses : Window
    {
        public Transition_Expenses(FundType ft)
        {
            InitializeComponent();
            var i = new Invoice();
            var t = i.AddTransition();
            t.LeftAccount = new Account { Id = 1 };     //1 is general fund account id
            t.RightAccount = new Account { Id = 5 };    //5 is expenses account id

            if (ft == FundType.GeneralHidden)
            {
                t.IsHidden = true;
                txtMsg1.Visibility = Visibility.Visible;
                txtMsg2.Visibility = Visibility.Collapsed;
                var r = BaseDataBase._Scalar($@"select dbo.fn_GetHiddenBalance()");
                txtBalance.Text = r;
            }
            else
            {
                var r = BaseDataBase._Scalar($@"(select
			        (select isnull(sum(Value),0) from Transition where IsHidden = 0 and RightAccount = 1 - 
			        (select isnull(sum(Value),0) from Transition where IsHidden = 0 and LeftAccount = 1))");
                txtBalance.Text = r;
            }
            this.DataContext = i;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var i = this.DataContext as Invoice;
            i.Description = i.Transitions[0].Details;
            if (i.IsValidate())
            {
                if (i.TotalValue < double.Parse(txtBalance.Text))
                    MyMessageBox.Show("لايوجد رصيد كافي في الصندوق");
                else if (Invoice.InsertData(i) && Transition.InsertData(i.Transitions[0]))
                {
                    PrintTicket.printInvoiceA6(i, 1);
                    MyMessage.InsertMessage();
                    DialogResult = true;
                }
            }
        }
    }
}
