﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MainWPF.Converters
{
    public class SupervisorIDConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int? SupervisorID = (int?)value;
            if (SupervisorID == null)
            {
                return "لم يتم الاختيار بعد";
            }
            var s = Supervisor.GetSupervisorByID((int)SupervisorID);
            return s.FirstName + " " + s.LastName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
