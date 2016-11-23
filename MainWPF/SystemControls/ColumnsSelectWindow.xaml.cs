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
    /// Interaction logic for ColumnsSelectWindow.xaml
    /// </summary>
    public partial class ColumnsSelectWindow : Window
    {
        public ColumnsSelectWindow()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in lst.ItemsSource as System.Collections.ObjectModel.ObservableCollection<DataGridColumn>)
            {
                item.IsReadOnly = true;
            }
            lst.Items.Refresh();
        }
    }
}
