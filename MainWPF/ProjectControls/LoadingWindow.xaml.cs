using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        public LoadingWindow()
        {
            InitializeComponent();
        }
        public LoadingWindow(string Message)
        {
            InitializeComponent();
            txt.Text = Message;
        }

        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard sb1 = FindResource("sbx1") as Storyboard;
            sb1.SetValue(Storyboard.TargetNameProperty, "el1");
            sb1.Begin(this);

            Storyboard sb2 = FindResource("sbx2") as Storyboard;
            sb2.SetValue(Storyboard.TargetNameProperty, "el2");
            sb2.Begin(this);
        }
    }
}
