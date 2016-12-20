using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MainWPF.Converters
{
    public class AgeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value != DBNull.Value)
            {
                string s = " يوم ";
                var DOB = (DateTime)value;
                int AgeDays = (DateTime.Today - DOB).Days;
                int years = AgeDays / 365;
                int months = (AgeDays - 365 * years) / 30;
                int days = AgeDays - 365 * years - 30 * months;
                if (years > 0)
                    return months > 0 ? years + " عام" + " - " + months + " شهر" : years + " عام";
                else if (months > 0)
                    return  days > 0 ? months + " شهر" + " - " + days + " يوم" : months + " شهر";
                else return days + " يوم";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
