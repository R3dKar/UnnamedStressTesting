using System;
using System.Globalization;
using System.Windows;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Преобразовывает <see cref="bool"/> в <see cref="Visibility"/>. Если параметр поставлен в true, то ввод дополнительно инвертируется
    /// </summary>
    class BooleanToVisibilityConverter : BaseValueConverter<BooleanToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null && (string)parameter == bool.TrueString)
                value = !(bool)value;

            if ((bool)value)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
