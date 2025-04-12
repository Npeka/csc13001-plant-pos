using System;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace csc13001_plant_pos.Converter
{
    public class PercentageToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double percentage)
            {
                return new SolidColorBrush(
                    percentage < 0
                        ? ColorHelper.FromArgb(255, 255, 77, 77) // #FF4D4D - Red
                        : ColorHelper.FromArgb(255, 76, 175, 80)  // #4CAF50 - Green
                );
            }

            return new SolidColorBrush(Colors.Blue); // fallback color
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
