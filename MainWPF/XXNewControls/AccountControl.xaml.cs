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
    /// Interaction logic for AccountControl.xaml
    /// </summary>
    public partial class AccountControl : UserControl
    {
        public AccountControl()
        {
            InitializeComponent();
        }
        public Account Account
        {
            set
            {
                this.DataContext = value;
                if (value.Transitions != null)
                {
                    txtRight.Text = value.Transitions.Sum(x => x.Rightvalue).ToString();
                    txtLeft.Text = value.Transitions.Sum(x => x.LeftValue).ToString();
                    txtTotal.Text = (double.Parse(txtRight.Text) - double.Parse(txtLeft.Text)).ToString();
                }
            }
        }
    }
}
