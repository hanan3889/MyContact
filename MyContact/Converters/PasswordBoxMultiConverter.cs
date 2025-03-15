using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace MyContact.Converters
{
    public class PasswordsMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 3 &&
                values[0] is string password &&
                values[1] is string confirmPassword &&
                values[2] is string secretCode)
            {
                return new string[] { password, confirmPassword, secretCode };
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
