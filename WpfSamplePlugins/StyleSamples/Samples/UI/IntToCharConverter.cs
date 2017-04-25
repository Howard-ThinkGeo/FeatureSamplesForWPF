using System;
using System.Globalization;
using System.Windows.Data;

namespace SlimGis.Samples
{
    public class IntToCharConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int) return System.Convert.ToChar((int)value);
            else if (value is char) return System.Convert.ToInt32((char)value);
            else return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
