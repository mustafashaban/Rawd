using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MainWPF.Converters
{
    public class MessageDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value != DBNull.Value)
            {
                var date = (DateTime)value;
                if (date.ToShortDateString() == DateTime.Now.ToShortDateString())
                    return date.ToString("hh:mm");
                else if(date.ToShortDateString() == DateTime.Now.AddDays(-1).ToShortDateString())
                    return "الامس";
                else 
                    return date.ToString("dd/MM/yyy");
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
