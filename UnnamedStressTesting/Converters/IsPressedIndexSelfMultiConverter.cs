using System;
using System.Globalization;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Конвертер, возвращающий true, если <see cref="MainWindowViewModel.PressedIndex"/> принадлежит <see cref="LetterViewModel"/>
    /// </summary>
    public class IsPressedIndexSelfMultiConverter : BaseMultiValueConverter<IsPressedIndexSelfMultiConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2)
            {
                int pressedIndex;
                LetterViewModel letter;

                try
                {
                    pressedIndex = (int)values[0];
                    letter = (LetterViewModel)values[1];
                }
                catch (InvalidCastException)
                {
                    return false;
                }

                bool isPressed = false;

                if (MainWindowViewModel.MainInstance.SelectedItem != null)
                    isPressed = MainWindowViewModel.MainInstance.SelectedItem.Letters.IndexOf(letter) == pressedIndex;

                return isPressed;
            }
            return false;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
