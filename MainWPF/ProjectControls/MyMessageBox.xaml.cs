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
    /// Interaction logic for MyMessageBox.xaml
    /// </summary>
    public partial class MyMessageBox : Window
    {
        private MessageBoxResult Result = MessageBoxResult.Cancel;
        public MyMessageBox()
        {
            InitializeComponent();
        }


        public static MessageBoxResult Show(string Text)
        {
            MyMessageBox m = new MyMessageBox();
            m.gOK.Visibility = Visibility.Visible;
            m.txt.Text = Text;
            m.ShowDialog();
            return m.Result;
        }
        public static MessageBoxResult Show(string Text, MessageBoxButton Buttons)
        {
            MyMessageBox m = new MyMessageBox();
            if (Buttons == MessageBoxButton.YesNo)
                m.gYesNo.Visibility = Visibility.Visible;
            m.txt.Text = Text;
            m.ShowDialog();
            return m.Result;
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            this.Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            this.Close();
        }
        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Yes;
            this.Close();
        }
        private void No_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.No;
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (gYesNo.Visibility == System.Windows.Visibility.Visible)
                {
                    Yes_Click(null, null);
                }
                else if (gOK.Visibility == System.Windows.Visibility.Visible)
                    OK_Click(null, null);
            }
        }
    }
}
