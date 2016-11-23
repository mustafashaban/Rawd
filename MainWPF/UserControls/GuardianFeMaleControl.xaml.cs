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
    /// Interaction logic for GuardianMaleControl.xaml
    /// </summary>
    public partial class GuardianFeMaleControl : Window
    {
        public GuardianFeMaleControl()
        {
            InitializeComponent();
        }

        private void btnUpdateGuardian_Click(object sender, RoutedEventArgs e)
        {
            if (dgGuardian.SelectedIndex != -1)
            {
                GuardianControl w = new GuardianControl(dgGuardian.SelectedItem as Guardian);
                if (w.ShowDialog() == true)
                {
                    dgGuardian.ItemsSource = Guardian.GetAllGuardianFeMale;
                }
            }
        }

        private void Control_Changed(object sender, TextChangedEventArgs e)
        {
            ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(dgGuardian.ItemsSource);
            view.Filter = delegate(object item)
            {
                Guardian G = (Guardian)item;
                if ((txtFirstName.Text != "" && !G.FirstName.Contains(txtFirstName.Text)))
                { return false; }
                if ((txtLastName.Text != "" && !G.LastName.Contains(txtLastName.Text)))
                { return false; }
                return true;
            };
        }


        private void btnAddGuardian_Click(object sender, RoutedEventArgs e)
        {
            GuardianControl g = new GuardianControl("أنثى");
            if (g.ShowDialog() == true)
            {
                dgGuardian.ItemsSource = Guardian.GetAllGuardianFeMale;
            }
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (dgGuardian.SelectedIndex != -1)
            {
                DialogResult = true;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
