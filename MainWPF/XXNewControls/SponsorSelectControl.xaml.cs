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
    /// Interaction logic for SupervisorSelectControl.xaml
    /// </summary>
    public partial class SponsorSelectControl : Window
    {
        public SponsorSelectControl()
        {
            InitializeComponent();
        }

        private void btnUpdateGuardian_Click(object sender, RoutedEventArgs e)
        {
            //if (dgSupervisor.SelectedIndex != -1)
            //{
            //    SponsorControl w = new SponsorControl(dgSupervisor.SelectedItem as Sponsor);
            //    if (w.ShowDialog() == true)
            //    {
            //        dgSupervisor.ItemsSource = Sponsor.GetAllSponsors;
            //    }
            //}
        }

        private void Control_Changed(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void Filter()
        {
            ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(dgSponsor.ItemsSource);
            view.Filter = delegate (object item)
            {
                Sponsor S = (Sponsor)item;
                if ((txtName.Text != "" && !S.Name.Contains(txtName.Text)))
                { return false; }
                return true;
            };
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (dgSponsor.SelectedIndex != -1)
            {
                DialogResult = true;
            }
        }

        private void btnAddGuardian_Click(object sender, RoutedEventArgs e)
        {
            //SponsorControl s = new SponsorControl(new Sponsor());
            //if (s.ShowDialog() == true)
            //{
            //    dgSupervisor.ItemsSource = Sponsor.GetAllSponsors;
            //}
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void dgSponsor_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgSponsor.SelectedIndex != -1)
            {
                DialogResult = true;
            }
        }

        private void cmboSelection_Changed(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }
    }
}
