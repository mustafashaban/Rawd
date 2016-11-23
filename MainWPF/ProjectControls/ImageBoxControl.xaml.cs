using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    /// Interaction logic for ImageBoxControl.xaml
    /// </summary>
    public partial class ImageBoxControl : UserControl, INotifyPropertyChanged
    {
        public ImageBoxControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ValueProperty;
        static ImageBoxControl()
        {
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, XXXX);
            ImageBoxControl.ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(ImageBoxControl), metadata);
        }

        private static void XXXX(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageBoxControl x = (ImageBoxControl)d;
            x.Value = (string)e.NewValue;
        }

        public string Value
        {
            set
            {
                SetValue(ValueProperty, value);
                this.NotifyPropertyChanged("Value");
                if (string.IsNullOrEmpty(value))
                {
                    btnDel.IsEnabled = btnShow.IsEnabled = false;
                }
                else
                {
                    btnDel.IsEnabled = btnShow.IsEnabled = true;
                }
            }
            get
            {
                return (string)GetValue(ImageBoxControl.ValueProperty);
            }
        }

        private void btnImageSet_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JPG Files|*.jpg|PNG Files|*.png";
            if (ofd.ShowDialog() == true)
            {
                BitmapImage b = new BitmapImage(new Uri(ofd.FileName));
                ImagePreviewWindow w = new ImagePreviewWindow();
                w.img.Source = b;
                w.ShowDialog();
                if (MyMessageBox.Show("هل تريد إختيار الصورة", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Value = ofd.FileName;
                }
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            Value = string.Empty;
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Value))
            {
                try
                {
                    ImagePreviewWindow w = new ImagePreviewWindow();
                    FileInfo fi = new FileInfo(Value);
                    w.img.Source = new BitmapImage(new Uri(fi.FullName, UriKind.Absolute));
                    w.ShowDialog();
                }
                catch { }
            }
        }





        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }
}
