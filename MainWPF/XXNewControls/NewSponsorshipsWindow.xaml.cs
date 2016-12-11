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
        Sponsor s;
        public NewSponsorshipsWindow(Sponsor s)
        {
            InitializeComponent();
            this.s = s;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (s != null && s.SponsorID.HasValue)
            {
                if (checkValues())
                {
                    for (int i = 0; i < nudNumberofBenef.Value; i++)
                    {
                        Sponsorship ss = new MainWPF.Sponsorship();
                        ss.SponsorID = s.SponsorID;
                        ss.Duration = nudDuration.Value;
                        ss.SponsorValue = nudSponsorvalue.Value;
                        ss.SponsorType = (cmboSponsorshipType.SelectedItem as ComboBoxItem).Content.ToString();

                        Sponsorship.InsertData(ss);
                        s.Sponsorships.Add(ss);
                    }
                    MyMessage.InsertMessage();
                    DialogResult = true;
                }
            }
        }

        private bool checkValues()
        {
            if (nudNumberofBenef.Value.HasValue && nudNumberofBenef.Value > 0 &&
                nudDuration.Value.HasValue && nudDuration.Value > 0 &&
                nudSponsorvalue.Value.HasValue && nudSponsorvalue.Value > 0 && cmboSponsorshipType.SelectedIndex != -1)
                return true;
            if (cmboSponsorshipType.SelectedIndex == -1)
                MyMessageBox.Show("يجب اختيار نوع الكفالة");
            else MyMessageBox.Show("القيم يجب ان يكون اكبر من الصفر");
            return false;
        }

        private void nud_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (nudNumberofBenef.Value.HasValue && nudDuration.Value.HasValue && nudSponsorvalue.Value.HasValue)
            {
                txtTotal.Text = (nudNumberofBenef.Value * nudDuration.Value * nudSponsorvalue.Value).ToString();
                nudPaidAmount.Maximum = (int)(nudNumberofBenef.Value * nudDuration.Value * nudSponsorvalue.Value);
                nudPaidAmount.Value = 0;
            }
            else txtTotal.Text = "0";
        }

        private void cmboPayType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmboPayType.SelectedIndex == 1)
                nudPaidAmount.IsEnabled = true;
            else
            {
                nudPaidAmount.IsEnabled = false;
                nudPaidAmount.Value = cmboPayType.SelectedIndex == 0 ? int.Parse(txtTotal.Text) : 0;
            }
        }
    }
}
