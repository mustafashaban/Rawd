using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    /// Interaction logic for SupervisorOrphanControl.xaml
    /// </summary>
    public partial class SponsorOrphanControl : UserControl, INotifyPropertyChanged
    {
        public SponsorOrphanControl()
        {
            InitializeComponent();
        }

        int? orphanID;
        public int? OrphanID
        {
            get { return orphanID; }
            set
            {
                orphanID = value;
                OrphanSponsors = Sponsorship.GetSponsorshipAllByOrphanID(value.Value);
            }
        }


        List<Sponsorship> orphanSponsors;
        public List<Sponsorship> OrphanSponsors
        {
            get { return orphanSponsors; }
            set
            {
                orphanSponsors = value;
                dgAvailableSponsorship.ItemsSource = null;
                dgAvailableSponsorship.ItemsSource = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private void NewSponsorship_Click(object sender, RoutedEventArgs e)
        {
            SelectAvailableSponsorshipControl w = new SelectAvailableSponsorshipControl();
            if (w.ShowDialog() == true)
            {
                var a = w.dgAvailableSponsorships.SelectedItem as AvailableSponsorship;
                Sponsorship ss = new MainWPF.Sponsorship();
                ss.AvailableSponsorship = a;
                ss.StartDate = w.dtpStartDate.SelectedDate;
                if (a.SponsorType == "كفالة عامة")
                    ss.EndDate = null;
                else ss.EndDate = w.dtpStartDate.SelectedDate.Value.AddMonths((int)a.Duration.Value);
                ss.OrphanID = OrphanID;
                if (Sponsorship.InsertData(ss))
                {
                    MyMessage.InsertMessage();
                    OrphanSponsors.Add(ss);
                    dgAvailableSponsorship.Items.Refresh();
                }
            }
        }
    }
}
