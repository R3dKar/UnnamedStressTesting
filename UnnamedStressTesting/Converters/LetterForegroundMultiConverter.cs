using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Конвертер для цвета буквы, принимает в аргументы <see cref="LetterViewModel"/> и <see cref="MainWindowViewModel.IsTestStarted"/>
    /// </summary>
    public class LetterForegroundMultiConverter : BaseMultiValueConverter<LetterForegroundMultiConverter>
    {
        private static Color stressPreviewLetterColor;
        /// <summary>
        /// Ресурс StressPreviewLetterColor
        /// </summary>
        public static Color StressPreviewLetterColor
        {
            get
            {
                if (stressPreviewLetterColor == Color.FromArgb(0,0,0,0))
                    stressPreviewLetterColor = (Color)Application.Current.FindResource("StressPreviewLetterColor");
                
                return stressPreviewLetterColor;
            }
        }

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
                    return StressPreviewLetterColor;
            }
            return Colors.Black;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
