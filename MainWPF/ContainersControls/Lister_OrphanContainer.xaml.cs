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
    /// Interaction logic for Lister_OrphanShow.xaml
    /// </summary>
    public partial class Lister_OrphanContainer : Window
    {
        public Lister_OrphanContainer()
        {
            InitializeComponent();
        }

        private void cmboGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Control_Changed(null, null);
        }
        private void Control_Changed(object sender, TextChangedEventArgs e)
        {
            if (dgListers.ItemsSource != null)
            {
                ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(dgListers.ItemsSource);
                view.Filter = delegate(object item)
                {
                    Lister l = (Lister)item;
                    if (txtFirstName.Text != "" && !l.FirstName.Contains(txtFirstName.Text))
                    {
                        return false;
                    }
                    if (txtLastName.Text != "" && !l.LastName.Contains(txtLastName.Text))
                    {
                        return false;
                    }
                    if (cmboGender.SelectedIndex > 0)
                    {
                        return (cmboGender.SelectedIndex == 1) ? (l.Gender == "ذكر") : (l.Gender == "أنثى");
                    }
                    return true;
                };
                view.Refresh();
            }
        }

        private void btnAddLister_Click(object sender, RoutedEventArgs e)
        {
            if (dgListers.SelectedIndex != -1)
            {
                List<Lister> ListersLB;
                if (lbListerGroup.ItemsSource == null)
                {
                    ListersLB = new List<Lister>();
                    lbListerGroup.ItemsSource = ListersLB;
                }
                else
                    ListersLB = lbListerGroup.ItemsSource as List<Lister>;
                Lister l = dgListers.SelectedItem as Lister;
                if (!ListersLB.Contains(l))
                    ListersLB.Add(l);
                lbListerGroup.Items.Refresh();
            }
        }
        private void btnDelLister_Click(object sender, RoutedEventArgs e)
        {
            if (lbListerGroup.SelectedIndex != -1)
            {
                List<Lister> ListersLB = (List<Lister>)lbListerGroup.ItemsSource;
                Lister l = lbListerGroup.SelectedItem as Lister;
                ListersLB.Remove(l);
                lbListerGroup.Items.Refresh();
            }
        }
        private void dgListers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnAddLister_Click(null, null);
        }
        private void lbListerGroup_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnDelLister_Click(null, null);
        }

        private void btnAddNewLister_Click(object sender, RoutedEventArgs e)
        {
            ListerControl w = new ListerControl();
            if (w.ShowDialog() == true)
            {
                var ls = dgListers.ItemsSource as List<Lister>;
                ls.Add(w.DataContext as Lister);
                dgListers.Items.Refresh();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
