using System;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace csc13001_plant_pos.Converter
{
    public class TypeNotiToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string type)
            {
                return type switch
                {
                    "Summary" => new SolidColorBrush(ColorHelper.FromArgb(255, 0, 120, 215)), // #0078D7
                    "OwnerAnnouncement" => new SolidColorBrush(ColorHelper.FromArgb(255, 232, 17, 35)), // #E81123
                    "NewPromotion" => new SolidColorBrush(ColorHelper.FromArgb(255, 16, 124, 16)), // #107C10
                    "ExpirationNotice" => new SolidColorBrush(ColorHelper.FromArgb(255, 136, 23, 152)), // #881798
                    "NewProduct" => new SolidColorBrush(ColorHelper.FromArgb(255, 0, 133, 117)), // #008575
                    "PlantCareTip" => new SolidColorBrush(ColorHelper.FromArgb(255, 0, 183, 195)), // #00B7C3
                    _ => new SolidColorBrush(ColorHelper.FromArgb(255, 85, 85, 85)) // #555555
                };
            }
            return new SolidColorBrush(ColorHelper.FromArgb(255, 85, 85, 85)); // #555555
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}