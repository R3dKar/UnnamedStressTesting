using System;
using System.Windows;
using System.Globalization;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Конвертер для видимости знака ударения над буквой, принимает IsMouseOver, <see cref="MainWindowViewModel.IsTestStarted"/>, 
    /// <see cref="MainWindowViewModel.IsWordReveal"/>, <see cref="MainWindowViewModel.PressedIndex"/> и <see cref="LetterViewModel"/>
    /// </summary>
    public class StressVisibilityMultiConverter : BaseMultiValueConverter<StressVisibilityMultiConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 5)
            {
                bool isMouseOver;
                bool isTestStarted;
                bool isWordReveal;
                int pressedIndex;
                LetterViewModel letter;

                try
                {
                    isMouseOver = (bool)values[0];
                    isTestStarted = (bool)values[1];
                    isWordReveal = (bool)values[2];
                    pressedIndex = (int)values[3];
                    letter = (LetterViewModel)values[4];
                }
                catch (InvalidCastException)
                {
                    return Visibility.Visible;
                }

                bool isPressed;

                if (MainWindowViewModel.MainInstance.SelectedItem is null)
                    isPressed = false;
                else
                    isPressed = MainWindowViewModel.MainInstance.SelectedItem.Letters.IndexOf(letter) == pressedIndex;

                if ((letter.IsStressed && !isTestStarted) || (isTestStarted && !isWordReveal && isMouseOver && letter.IsVowel) || (isTestStarted && isWordReveal && (letter.IsStressed || isPressed)))
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            else
                return Visibility.Visible;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
