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
    /// Interaction logic for FormedBasketSelectControl.xaml
    /// </summary>
    public partial class FormedBasketSelectControl : Window
    {
        public FormedBasketSelectControl(List<FormedBasket> items)
        {
            InitializeComponent();
            lstFormedBakset.ItemsSource = items;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (lstFormedBakset.SelectedItem != null)
                DialogResult = true;
            else MyMessageBox.Show("يجب اختيار تشكيلة مواد");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (lstFormedBakset.Items.Count == 1)
            {
                lstFormedBakset.SelectedItem = lstFormedBakset.Items[0];
                DialogResult = true;
            }
        }
    }
}
