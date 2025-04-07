using System;
using Microsoft.UI.Xaml.Data;

namespace csc13001_plant_pos.Converter
{
    public class TypeNotiToGlyphConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string type)
            {
                return type switch
                {
                    "Summary" => "\uE71D",
                    "OwnerAnnouncement" => "\uE715",
                    "NewPromotion" => "\uE8EC",
                    "ExpirationNotice" => "\uECC5",
                    "NewProduct" => "\uEC0A",
                    "PlantCareTip" => "\uE95E",
                    _ => "\uE909"
                };
            }
            return "\uE909";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}