using System;
using System.Windows;
using System.Globalization;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Конвертер для видимости знака ударения над буквой, принимает разные параметры взависимости от типа буквы
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

                bool isPressed = false;

                if (MainWindowViewModel.MainInstance.SelectedItem != null)
                    isPressed = MainWindowViewModel.MainInstance.SelectedItem.Letters.IndexOf(letter) == pressedIndex;

                if ((isMouseOver && isTestStarted && !isWordReveal) || isPressed)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            else if (values.Length == 3)
            {
                bool isMouseOver;
                bool isTestStarted;
                bool isWordReveal;

                try
                {
                    isMouseOver = (bool)values[0];
                    isTestStarted = (bool)values[1];
                    isWordReveal = (bool)values[2];
                }
                catch (InvalidCastException)
                {
                    return Visibility.Visible;
                }

                if (!isTestStarted || (isMouseOver && isTestStarted) || isWordReveal)
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
