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
    /// Interaction logic for FingerPrintActivatorWindow.xaml
    /// </summary>
    public partial class FingerPrintActivatorWindow : Window
    {
        public FingerPrintActivatorWindow()
        {
            InitializeComponent();
            chk.IsChecked = Properties.Settings.Default.ActiveFingerPrint;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ActiveFingerPrint = chk.IsChecked.Value;
            Properties.Settings.Default.Save();
            MyMessage.CustomMessage(chk.IsChecked.Value ? "تم التفعيل بنجاح" : "تم الالغاء بنجاح");
            this.Close();
        }
    }
}
