using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace MyContact.Converters
{
    public class PasswordsMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is PasswordBox passwordBox && values[1] is PasswordBox confirmPasswordBox)
            {
                return new PasswordBox[] { passwordBox, confirmPasswordBox };
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
