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
            NewSponsorshipsWindow w = new MainWPF.NewSponsorshipsWindow(s);
           if( w.ShowDialog() == true)
            {
                dgSponsorship.Items.Refresh();
                s.NotifyPropertyChanged("AllSponsorships");
                s.NotifyPropertyChanged("EndedSponsorships");
                s.NotifyPropertyChanged("WaitSponsorships");
                s.NotifyPropertyChanged("CurrentSponsorships");
            }
        }

        private void btnDeleteSponsorship_Click(object sender, RoutedEventArgs e)
        {
            if (dgSponsorship.SelectedIndex >= 0)
            {
                var s = this.DataContext as Sponsor;
                var ss = dgSponsorship.SelectedItem as Sponsorship;
                if (ss.Status == "بالانتظار")
                {
                    if (MyMessageBox.Show("هل تريد تأكيد حذف الكفالة", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        if (Sponsorship.DeleteData(ss))
                        {
                            MyMessage.DeleteMessage();
                            s.Sponsorships.Remove(ss);
                            dgSponsorship.Items.Refresh();
                            s.NotifyPropertyChanged("AllSponsorships");
                            s.NotifyPropertyChanged("EndedSponsorships");
                            s.NotifyPropertyChanged("WaitSponsorships");
                            s.NotifyPropertyChanged("CurrentSponsorships");
                        }
                }
                else MyMessageBox.Show("لايمكن حذف الكفالات المنتهية وغير المنتهية");
            }
        }
    }
}
