using System;
using System.Globalization;
using System.Windows.Media;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Конвертер для цвета буквы, принимает в аргументы <see cref="LetterViewModel"/> и <see cref="MainWindowViewModel.IsTestStarted"/>
    /// </summary>
    public class LetterForegroundMultiConverter : BaseMultiValueConverter<LetterForegroundMultiConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2)
            {
                LetterViewModel letter;
                bool isTestStarted;

                try
                {
                    letter = (LetterViewModel)values[0];
                    isTestStarted = (bool)values[1];
                }
                catch (InvalidCastException)
                {
                    return Colors.Black;
                }

                if (letter.IsStressed && !isTestStarted)
                    return Colors.Green;
            }
            return Colors.Black;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
