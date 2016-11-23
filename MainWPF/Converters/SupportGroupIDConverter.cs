using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MainWPF.Converters
{
    public class SupportGroupIDConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int? ID = (int?)value;
            if (ID == null)
            {
                return "Naaalllll";
            }
            string s = BaseDataBase._Scalar("select dbo.[GetSupportGrouptAll](" + ID + ")");
            return s.Replace("????", "\n").Replace("???", "\t\t\t");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
