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
    internal class CountOrdersConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mainWindow = new MainWindow();
            var orders = mainWindow.Orders;
            mainWindow.Close();
            return orders.Where(o => o.Client.Id == (int)value).Count();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
