using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace HomeAssistant.Core.Converters
{
    public class GeneralRefreshStateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count() > 0 && values.ToList().TrueForAll(x => x is bool))
                return
                    values.Any(x => (bool)x == true) ?
                    Visibility.Visible : Visibility.Collapsed;

            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
