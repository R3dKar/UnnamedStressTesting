using System;
using System.Globalization;
using System.Windows;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Конвертер для перечёркивающего удрание знака, принимает <see cref="MainWindowViewModel.IsWordReveal"/>, 
    /// <see cref="MainWindowViewModel.PressedIndex"/> и <see cref="LetterViewModel"/>
    /// </summary>
    public class MistakeStressVisibilityMultiConverter : BaseMultiValueConverter<MistakeStressVisibilityMultiConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 3)
            {
                bool isWordReveal;
                int pressedIndex;
                LetterViewModel letter;

                try
                {
                    isWordReveal = (bool)values[0];
                    pressedIndex = (int)values[1];
                    letter = (LetterViewModel)values[2];
                }
                catch (InvalidCastException)
                {
                    return Visibility.Collapsed;
                }

                bool isPressed = false;

                if (MainWindowViewModel.MainInstance.SelectedItem != null)
                    isPressed = MainWindowViewModel.MainInstance.SelectedItem.Letters.IndexOf(letter) == pressedIndex;

                if (isWordReveal && isPressed)
                    return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
