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
    /// Interaction logic for ReportHeaderWindow.xaml
    /// </summary>
    public partial class ReportHeaderWindow : Window
    {
        public ReportHeaderWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtDescription.Text = Properties.Settings.Default.ReportHeaderDescription;
            txtVoucherHeader.Text = Properties.Settings.Default.VoucherHeaderText;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.AssociationLogoPath) 
                    && System.IO.File.Exists(Properties.Settings.Default.AssociationLogoPath))
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(Properties.Settings.Default.AssociationLogoPath, UriKind.RelativeOrAbsolute);
                bi.EndInit();
                img.Source = bi;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog od = new Microsoft.Win32.OpenFileDialog();
            od.Filter = "Image Files|*.jpg;*.png";
            if (od.ShowDialog() == true)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(od.FileName);
                if (fi.Length / 1024 / 1024 > 1)
                {
                    MyMessageBox.Show("حجم الصورة يجب أن يكون أقل من 1 ميغا");
                    return;
                }
                Properties.Settings.Default.AssociationLogoPath = fi.FullName;
                Properties.Settings.Default.Save();

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(fi.FullName, UriKind.RelativeOrAbsolute);
                bi.EndInit();

                img.Source = bi;
                MyMessage.CustomMessage("تم الحفظ بنجاح");
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ReportHeaderDescription = txtDescription.Text;
            Properties.Settings.Default.Save();
            MyMessage.CustomMessage("تم الحفظ بنجاح");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.VoucherHeaderText = txtVoucherHeader.Text;
            Properties.Settings.Default.Save();
            MyMessage.CustomMessage("تم الحفظ بنجاح");
        }
    }
}
