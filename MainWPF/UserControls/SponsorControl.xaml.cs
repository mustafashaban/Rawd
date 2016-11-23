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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for SponsorControl.xaml
    /// </summary>
    public partial class SponsorControl : Window
    {
        public SponsorControl()
        {
            InitializeComponent();
            Sponsor x = new Sponsor();
            myWindow.DataContext = x;
        }

        public SponsorControl(Sponsor s)
        {
            InitializeComponent();
            myWindow.DataContext = s;
            btnUpdate.Visibility = System.Windows.Visibility.Visible;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var x = (Sponsor)this.DataContext;
            if (x.IsValidate())
            {
                if (x.InsertSponsorData())
                {
                    MyMessage.InsertMessage();
                    DialogResult = true;
                }
            }
        }
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var x = (Sponsor)this.DataContext;
            if (x.IsValidate())
            {
                if (x.UpdateSponsorData())
                {
                    MyMessage.UpdateMessage();
                    DialogResult = true;
                }
            }
        }
    }
}
