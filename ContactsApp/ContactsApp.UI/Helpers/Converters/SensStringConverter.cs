using System;
using System.Security;
using System.Windows.Data;
using System.Globalization;

using ContactsApp.Infrastructure.Disposable;
using ContactsApp.Infrastructure.Extensions;

namespace ContactsApp.UI.Helpers.Converters
{
    public class SensStringConverter : IValueConverter
    {
        public object Convert(object valueFromModel, Type targetType, object parameter, CultureInfo culture)
        {
            var temporaryValueConversion = valueFromModel as SecureString;
            var toControlValue = temporaryValueConversion.Copy().SecureStringToString();

            using (new DisposableSource(() => toControlValue = null))
            {
                return toControlValue.Clone().ToString();
            }
        }
        public object ConvertBack(object valueFromControl, Type targetType, object parameter, CultureInfo culture)
        {
            var temporaryValueConversion = valueFromControl.ToString();

            using (new DisposableSource(() => temporaryValueConversion = null))
            {
                using (var toModelValue = temporaryValueConversion.StringToSecureString())
                {
                    return toModelValue.Copy();
                }
            }
        }
    }
}