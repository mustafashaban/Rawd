using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MainWPF.Converters
{
    public class GroupSupportsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                float sum = 0;
                var v = (value as CollectionViewGroup);
                if (v != null)
                {
                    if (v.ItemCount > 0)
                        if (v.Items[0] is DataRowView)
                        {
                            sum = (from x in v.Items let m = (x as DataRowView) where m != null select (float)m["Quantity"]).Sum();
                        }
                        else if (v.Items[0] is CollectionViewGroup)
                        {
                            foreach (CollectionViewGroup item in v.Items)
                            {
                                sum += (from x in item.Items let m = (x as DataRowView) where m != null select (float)m["Quantity"]).Sum();
                            }
                        }
                }
                return sum;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
