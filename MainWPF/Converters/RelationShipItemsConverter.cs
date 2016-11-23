using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MainWPF.Converters
{
    public class RelationShipItemsConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<string> lst;
            if (value == null)
            {
                return null;
            }
            if (value.ToString() == "أنثى")
            {
                string[] input = { "ابنة", "أخت", "زوجة", "جدة", "عمة", "خالة", "حفيدة", "زوجة", "والدة الزوج", "والدة الزوجة", "غير ذلك" };
                lst = new List<string>(input);
            }
            else
            {
                string[] input = { "ابن", "أخ", "جد", "عم", "خال", "حفيد", "زوج", "والد الزوج", "والد الزوجة", "غير ذلك" };
                lst = new List<string>(input);
            }
            return lst;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
