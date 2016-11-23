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
    /// Interaction logic for CancelTempFamilyWindow.xaml
    /// </summary>
    public partial class CancelTempFamilyWindow : Window
    {
        public CancelTempFamilyWindow(TempFamily tf)
        {
            InitializeComponent();
            this.DataContext = tf;
        }


         private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

         private void btnSave_Click(object sender, RoutedEventArgs e)
         {
             TempFamily tf = DataContext as TempFamily;
             if (tf != null)
             {
                 if (chk.IsChecked == false)
                 {
                     TempFamily.UpadteData(tf);
                     DialogResult = true;
                 }
                 else
                 {
                     TempFamily.UpadteData(tf);
                     DialogResult = true;
                 }
             }
         }
    }
}
