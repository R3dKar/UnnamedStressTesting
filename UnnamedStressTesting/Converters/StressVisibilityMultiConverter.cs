using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace UnnamedStressTesting
{
    public class StressVisibilityMultiConverter : BaseMultiValueConverter<StressVisibilityMultiConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Count() == 4)
            {
                bool isStressed = (bool)values[0];
                bool isVowel = (bool)values[1];
                bool isMouseOver = (bool)values[2];
                bool isTestStarted = (bool)values[3];

                if ((isStressed && !isTestStarted) || (isTestStarted && isMouseOver && isVowel))
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            else
                return Visibility.Collapsed;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
