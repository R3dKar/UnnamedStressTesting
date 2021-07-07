using System;
using System.Globalization;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Скрывает букву Ё во время теста, так как она всегда ударная, принимает в аргументы <see cref="LetterViewModel.UppercaseString"/>,
    /// <see cref="MainWindowViewModel.IsTestStarted"/> и <see cref="MainWindowViewModel.IsWordReveal"/>
    /// </summary>
    public class HideYoMultiConverter : BaseMultiValueConverter<HideYoMultiConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 3)
            {
                string uppercase;
                bool isTestStarted;
                bool isWordReveal;

                try
                {
                    uppercase = (string)values[0];
                    isTestStarted = (bool)values[1];
                    isWordReveal = (bool)values[2];
                }
                catch (InvalidCastException)
                {
                    return "Error";
                }

                if (isTestStarted && !isWordReveal && uppercase == "Ё")
                    return "Е";
                else
                    return uppercase;
            }

            return "Error";
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
