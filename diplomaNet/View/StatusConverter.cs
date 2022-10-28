using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace diplomaNet.View
{
    internal class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value.ToString();
            if (status == "Added")
                return "Добавлен";
            else if (status == "Planned")
                return "Запланирован";
            else if (status == "AtWork")
                return "В работе";
            else
                return "Завершен";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
