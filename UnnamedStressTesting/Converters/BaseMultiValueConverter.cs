using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Класс-шаблон для конвертера с несколькими значениями
    /// </summary>
    /// <typeparam name="T">Тип создаваемого конвертера</typeparam>
    public abstract class BaseMultiValueConverter<T> : MarkupExtension, IMultiValueConverter where T : class, new()
    {
        private static T instance = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return instance ?? (instance = new T());
        }

        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
    }
}
