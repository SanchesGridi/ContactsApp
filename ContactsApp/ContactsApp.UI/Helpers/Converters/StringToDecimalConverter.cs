using System;
using System.Windows.Data;
using System.Globalization;

namespace ContactsApp.UI.Helpers.Converters
{
    public class StringToDecimalConverter : IValueConverter
    {
        public object Convert(object valueFromModel, Type targetType, object parameter, CultureInfo culture)
        {
            var toControlValue = ((decimal)valueFromModel).ToString();

            return toControlValue;
        }
        public object ConvertBack(object valueFromControl, Type targetType, object parameter, CultureInfo culture)
        {
            var temporaryValueConversion = valueFromControl as string;

            var formatProvider = new NumberFormatInfo { NumberDecimalSeparator = "." };
            if (decimal.TryParse(temporaryValueConversion, NumberStyles.AllowDecimalPoint, formatProvider, out var toModelValue))
            {
                return toModelValue;
            }
            else
            {
                return 0m;
            }
        }
    }
}