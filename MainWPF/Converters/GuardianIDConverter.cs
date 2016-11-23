using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MainWPF.Converters
{
    public class GuardianIDConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int? GuardainID = (int?)value;
            if (GuardainID == null)
            {
                return "لم يتم الاختيار بعد";
            }
            Guardian g = Guardian.GetGuardianByID((int)GuardainID);
            return g.FirstName + " " + g.LastName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
