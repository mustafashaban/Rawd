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
    public partial class NewSponsorshipsWindow : Window
    {
        public NewSponsorshipsWindow(AvailableSponsorship a)
        {
            InitializeComponent();
            this.DataContext = a;
            if (!a.ID.HasValue)
                a.SponsorType = "كفالة خاصة";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var a = this.DataContext as AvailableSponsorship;
            if (a != null)
            {
                if (a.IsValidate())
                {
                    if (AvailableSponsorship.InsertData(a))
                    {
                        MyMessage.InsertMessage();
                        DialogResult = true;
                    }
                }
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                var a = this.DataContext as AvailableSponsorship;
                a.SponsorType = radClearSponsor.IsChecked == true ? radClearSponsor.Content.ToString() : radNotClearSponsor.Content.ToString();
            }
        }
    }
}
