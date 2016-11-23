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
    /// Interaction logic for VoucherTypeControl.xaml
    /// </summary>
    public partial class VoucherTypeControl : Window
    {
        public VoucherTypeControl()
        {
            InitializeComponent();
            cmbo.SelectedIndex = Properties.Settings.Default.VoucherType;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.VoucherType = cmbo.SelectedIndex;
            Properties.Settings.Default.Save();
            MyMessage.CustomMessage("تم الحفظ بنجاح");
            this.Close();
        }
    }
}
