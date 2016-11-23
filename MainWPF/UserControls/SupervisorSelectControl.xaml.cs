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
    public partial class SupervisorSelectControl : Window
    {
        public SupervisorSelectControl()
        {
            InitializeComponent();
        }

        private void btnUpdateGuardian_Click(object sender, RoutedEventArgs e)
        {
            if (dgSupervisor.SelectedIndex != -1)
            {
                SupervisorControl w = new SupervisorControl(dgSupervisor.SelectedItem as Supervisor);
                if (w.ShowDialog() == true)
                {
                    dgSupervisor.ItemsSource = Supervisor.GetAllSupervisors;
                }
            }
        }

        private void Control_Changed(object sender, TextChangedEventArgs e)
        {
            ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(dgSupervisor.ItemsSource);
            view.Filter = delegate(object item)
            {
                Supervisor S = (Supervisor)item;
                if ((txtFirstName.Text != "" && !S.FirstName.Contains(txtFirstName.Text)))
                { return false; }
                if ((txtLastName.Text != "" && !S.LastName.Contains(txtLastName.Text)))
                { return false; }
                return true;
            };
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (dgSupervisor.SelectedIndex != -1)
            {
                DialogResult = true;
            }
        }

        private void btnAddGuardian_Click(object sender, RoutedEventArgs e)
        {
            SupervisorControl s = new SupervisorControl();
            if (s.ShowDialog() == true)
            {
                dgSupervisor.ItemsSource = Supervisor.GetAllSupervisors;
            }
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void dgGuardian_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgSupervisor.SelectedIndex != -1)
            {
                DialogResult = true;
            }
        }
    }
}
