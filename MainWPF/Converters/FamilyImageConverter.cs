using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MainWPF.Converters
{
    public class FamilyImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string imagePath = "";
            if (value != null)
                imagePath = value.ToString();
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            if(string.IsNullOrEmpty(imagePath))
                bi.UriSource = new Uri("/MainWPF;component/Images/USER.PNG", UriKind.RelativeOrAbsolute);
            else
            {
                FileInfo fi = new FileInfo(imagePath);
                if (File.Exists(fi.FullName))
                    bi.UriSource = new Uri(fi.FullName);
                else 
                    bi.UriSource = new Uri("/MainWPF;component/Images/USER.PNG", UriKind.RelativeOrAbsolute);
            }
            bi.EndInit();
            return bi;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
