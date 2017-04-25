using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SlimGis.Samples
{
    public class StringToMediaFontFamilyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string) return new FontFamily((string)value);
            else if (value is FontFamily) return ((FontFamily)value).Source;
            else return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
