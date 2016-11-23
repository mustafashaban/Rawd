using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MainWPF.Converters
{
    public class TrainedIDConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int? ID = (int?)value;
            if (ID == null)
            {
                return "ERROR";
            }
            return BaseDataBase._Scalar("select [dbo].[GetTrainedName](" + ID + ")");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
