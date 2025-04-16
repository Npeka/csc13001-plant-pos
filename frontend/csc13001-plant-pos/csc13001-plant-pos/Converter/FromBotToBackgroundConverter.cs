using System;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace csc13001_plant_pos.Converter
{
    public class FromBotToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool fromBot)
            {
                Color backgroundColor = fromBot
                    ? ColorHelper.FromArgb(255, 255, 255, 255)  
                    : ColorHelper.FromArgb(255, 242, 243, 245);
                return new SolidColorBrush(backgroundColor);
            }
            return new SolidColorBrush(ColorHelper.FromArgb(255, 255, 255, 255));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}