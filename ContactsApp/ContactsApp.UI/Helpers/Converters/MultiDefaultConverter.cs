using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace ContactsApp.UI.Helpers.Converters
{
    public class MultiDefaultConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new[] { DependencyProperty.UnsetValue };
        }
    }
}