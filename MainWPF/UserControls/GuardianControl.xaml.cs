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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for GuardianControl.xaml
    /// </summary>
    public partial class GuardianControl : Window
    {
        public GuardianControl(string Gender)
        {
            InitializeComponent();
            Guardian G = new Guardian();
            G.Gender = Gender;
            this.DataContext = G;
        }
        public GuardianControl(Guardian G)
        {
            InitializeComponent();
            this.DataContext = G;
            btnUpdate.Visibility = System.Windows.Visibility.Visible;
            btnExecute.Visibility = System.Windows.Visibility.Hidden;
        }


        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            Guardian x = (Guardian)this.DataContext;
            if (x.IsValidate())
            {
                x.InsertGuardianData();
                MyMessage.InsertMessage();
                DialogResult = true;
            }
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Guardian x = (Guardian)this.DataContext;
            if (x.IsValidate())
            {
                x.UpdateGuardianData();
                MyMessage.UpdateMessage();
                DialogResult = true;
            }
        }
    }
}
