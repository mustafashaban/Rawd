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
                DateTime? DOB = (DateTime?)value;
                int? Age = null;
                int? Age1 = 0;
                if (DOB != null)
                {
                    Age = ((TimeSpan)(DateTime.Today - DOB)).Days;
                    if (Age > 30)
                    {
                        Age1 = Age % 365;
                        Age1 /= 30;
                        s = " شهر ";
                        Age /= 30;
                        if (Age > 12)
                        {
                            s = " عام ";
                            Age /= 12;
                        }
                    }
                }
                if (s == " عام " && Age1!=0)
                    return Age + s + "-" + Age1 + " شهر "; 
                 else   return Age + s;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
