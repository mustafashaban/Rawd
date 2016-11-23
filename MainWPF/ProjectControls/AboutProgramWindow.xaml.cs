using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for AboutProgramWindow.xaml
    /// </summary>
    public partial class AboutProgramWindow : Window
    {
        public AboutProgramWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Environment.CurrentDirectory + "\\User Guide.pdf"))
                Process.Start(Environment.CurrentDirectory + "\\User Guide.pdf");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Process.Start("www.DawatKhir.com");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Process.Start("www.m3n-sy.com");
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            txtCharity.Text = BaseDataBase._Scalar("select top 1 Name from MergedStudySide where IsLocal = 1");
            txtVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
