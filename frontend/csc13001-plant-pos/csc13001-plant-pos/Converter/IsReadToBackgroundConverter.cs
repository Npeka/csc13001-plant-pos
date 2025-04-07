using System;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace csc13001_plant_pos.Converter
{
    public class IsReadToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool isRead && parameter is string colors)
            {
                var colorPair = colors.Split(':');
                var selectedColor = isRead ? colorPair[0] : colorPair[1];

                return selectedColor switch
                {
                    "#FAFAFA" => new SolidColorBrush(ColorHelper.FromArgb(255, 250, 250, 250)),
                    "#F0F7FF" => new SolidColorBrush(ColorHelper.FromArgb(255, 240, 247, 255)),
                    "#E5E5E5" => new SolidColorBrush(ColorHelper.FromArgb(255, 229, 229, 229)),
                    "#CCE5FF" => new SolidColorBrush(ColorHelper.FromArgb(255, 204, 229, 255)),
                    _ => new SolidColorBrush(ColorHelper.FromArgb(255, 240, 247, 255))
                };
            }
            return new SolidColorBrush(ColorHelper.FromArgb(255, 240, 247, 255));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}