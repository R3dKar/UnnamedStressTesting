using System;
using System.Globalization;
using System.Windows.Input;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Конвертер для курсора буквы, принимает разные параметры взависимости от типа буквы
    /// </summary>
    public class LetterCursorMultiConverter : BaseMultiValueConverter<LetterCursorMultiConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2)
            {
                bool isTestStarted;
                bool isWordReveal;

                try
                {
                    isTestStarted = (bool)values[0];
                    isWordReveal = (bool)values[1];
                }
                catch (InvalidCastException)
                {
                    return Cursors.Arrow;
                }

                if (isTestStarted && !isWordReveal)
                    return Cursors.Hand;
            }
            else if (values.Length == 1)
            {
                bool isTestStarted;

                try
                {
                    isTestStarted = (bool)values[0];
                }
                catch (InvalidCastException)
                {
                    return Cursors.Arrow;
                }

                if (isTestStarted)
                    return Cursors.Hand;
            }
            return Cursors.Arrow;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
