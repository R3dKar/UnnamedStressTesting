using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Controls;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Конвертер для <see cref="ObservableCollection{T}"/>, которая возвращает false, если элементов в нём много, и наоборот 
    /// </summary>
    class ItemsToIsExpandedConverter : BaseValueConverter<ItemsToIsExpandedConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return false;

            if ((value as ObservableCollection<WordViewModel>).Count < 150)
                return true;
            else
                return false;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
