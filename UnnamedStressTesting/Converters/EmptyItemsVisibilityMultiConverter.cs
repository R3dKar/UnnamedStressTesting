using System;
using System.Globalization;
using System.Windows;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Конвертер для видимости текста при отсутствии слов, принимает в аргументы <see cref="MainWindowViewModel.IsItemsEmpty"/> и <see cref="MainWindowViewModel.IsWordRefresh"/>
    /// </summary>
    public class EmptyItemsVisibilityMultiConverter : BaseMultiValueConverter<EmptyItemsVisibilityMultiConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2)
            {
                bool isItemsEmpty;
                bool isWordRefresh;

                try
                {
                    isItemsEmpty = (bool)values[0];
                    isWordRefresh = (bool)values[1];
                }
                catch (InvalidCastException)
                {
                    return Visibility.Visible;
                }

                if (isWordRefresh || !isItemsEmpty)
                    return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
