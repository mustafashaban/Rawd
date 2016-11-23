using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MainWPF.Converters
{
    public class InternalSupportIDConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int? InternalSupportID = (int?)value;
            if (InternalSupportID == null)
            {
                return "Error";
            }
            return BaseDataBase._Scalar("select dbo.[GetInternalSupportByID](" + InternalSupportID + ")");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
