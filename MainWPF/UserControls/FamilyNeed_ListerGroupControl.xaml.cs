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
    /// Interaction logic for FamilyNeed_ListerGroupControl.xaml
    /// </summary>
    public partial class FamilyNeed_ListerGroupControl : Window
    {
        public FamilyNeed_ListerGroupControl(List<FamilyNeed_ListerGroup> lst)
        {
            InitializeComponent();


            var xs = FamilyNeedByLister.GetFamilyNeedByListerAll;
            foreach (var item in lst)
            {
                foreach (var x in xs)
                {
                    if (x.ID == item.FamilyNeedByLister.ID)
                    {
                        xs.Remove(x);
                        break;
                    }
                }
            }
            lbFamilyNeed.ItemsSource = xs;
            dgFamilyNeed.ItemsSource = new List<FamilyNeedByLister>();
        }

        void RefreshPanel()
        {
            dgFamilyNeed.Items.Refresh();
            lbFamilyNeed.Items.Refresh();
        }

        private void btnAddFamilyNeed_Click(object sender, RoutedEventArgs e)
        {
            if (lbFamilyNeed.SelectedItem != null)
            {
                (dgFamilyNeed.ItemsSource as List<FamilyNeedByLister>).Add(lbFamilyNeed.SelectedItem as FamilyNeedByLister);
                (lbFamilyNeed.ItemsSource as List<FamilyNeedByLister>).Remove(lbFamilyNeed.SelectedItem as FamilyNeedByLister);

                RefreshPanel();
            }
        }

        private void btnDelFamilyNeed_Click(object sender, RoutedEventArgs e)
        {
            if (dgFamilyNeed.SelectedItem != null)
            {
                (lbFamilyNeed.ItemsSource as List<FamilyNeedByLister>).Add(dgFamilyNeed.SelectedItem as FamilyNeedByLister);
                (dgFamilyNeed.ItemsSource as List<FamilyNeedByLister>).Remove(dgFamilyNeed.SelectedItem as FamilyNeedByLister);

                RefreshPanel();
            }
        }

        private void lbFamilyNeed_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnAddFamilyNeed_Click(null, null);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
