//using ServiceCenterApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace diplomaNet.View
{
    internal class WorkListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<Work> works = value as List<Work>;
            string worksString = "";
            foreach (Work work in works)
            {
                worksString += work.Name + "\n";
            }
            return worksString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
