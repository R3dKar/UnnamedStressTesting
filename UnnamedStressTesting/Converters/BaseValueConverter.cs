using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Класс-шаблон для конвертера значений
    /// </summary>
    /// <typeparam name="T">Тип создаваемого конвертера</typeparam>
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter where T : class, new()
    {
        private static T instance = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return instance ?? (instance = new T());
        }

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}
