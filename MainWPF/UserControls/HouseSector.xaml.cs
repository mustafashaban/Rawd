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
    /// Interaction logic for HouseSector.xaml
    /// </summary>
    public partial class HouseSector : Window
    {
        public HouseSector()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender as TextBlock).Text.Contains("السريان"))
            {
                (this.DataContext as House).SectorPartNum = int.Parse((sender as TextBlock).Text.Remove(1));
                DialogResult = true;
            }
            else
            {
                (this.DataContext as House).SectorPartNum = int.Parse((sender as TextBlock).Text.Remove(2));
                DialogResult = true;
            }
        }
    }
}
