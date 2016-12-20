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
    public partial class SelectAvailableSponsorshipControl : Window
    {
        public SelectAvailableSponsorshipControl()
        {
            InitializeComponent();
            dgAvailableSponsorships.ItemsSource = AvailableSponsorship.GetAllAvailableSponsorship();
        }

        private void Control_Changed(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void Filter()
        {
            ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(dgAvailableSponsorships.ItemsSource);
            view.Filter = delegate (object item)
            {
                AvailableSponsorship a = (AvailableSponsorship)item;
                if ((txtName.Text != "" && !a.RelatedSponsor.Name.Contains(txtName.Text)))
                { return false; }
                if (cmboIsCompensated.SelectedIndex > 0 && (cmboIsCompensated.SelectedIndex == 1 && !a.IsCompensated) || (cmboIsCompensated.SelectedIndex == 2 && a.IsCompensated))
                { return false; }
                if (cmboIsGeneral.SelectedIndex > 0 && a.SponsorType != (cmboIsGeneral.SelectedIndex == 1 ? "كفالة خاصة" : "كفالة عامة"))
                { return false; }
                return true;
            };
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (dgAvailableSponsorships.SelectedIndex != -1)
            {
                if (!dtpStartDate.SelectedDate.HasValue)
                {
                    MyMessageBox.Show("يجب اختيار تاريخ بداية الكفالة");
                    return;
                }
                DialogResult = true;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void dgAvailableSponsorships_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            btnAdd_Click(null, null);
        }

        private void cmboSelection_Changed(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }
    }
}
