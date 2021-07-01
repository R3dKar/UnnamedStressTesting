using System;
using System.Globalization;
using System.Windows.Input;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Конвертер для курсора буквы, принимает в аргументы <see cref="LetterViewModel"/>, 
    /// <see cref="MainWindowViewModel.IsTestStarted"/> и <see cref="MainWindowViewModel.IsWordReveal"/>
    /// </summary>
    public class LetterCursorMultiConverter : BaseMultiValueConverter<LetterCursorMultiConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 3)
            {
                LetterViewModel letter;
                bool isTestStarted;
                bool isWordReveal;

                try
                {
                    letter = (LetterViewModel)values[0];
                    isTestStarted = (bool)values[1];
                    isWordReveal = (bool)values[2];
                }
                catch (InvalidCastException)
                {
                    return Cursors.Arrow;
                }

                if ((letter.IsVowel && isTestStarted && !isWordReveal) || (letter.IsStressed && isWordReveal))
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
