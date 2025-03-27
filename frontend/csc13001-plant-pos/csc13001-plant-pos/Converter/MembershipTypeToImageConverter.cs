using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace csc13001_plant_pos.Converter
{
    public class MembershipTypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string membershipType)
            {
                string iconPath = membershipType switch
                {
                    "Bronze" => "ms-appx:///Assets/Icon/Bronze.svg",
                    "Gold" => "ms-appx:///Assets/Icon/Gold.svg",
                    "Silver" => "ms-appx:///Assets/Icon/Silver.svg",
                    "Platinum" => "ms-appx:///Assets/Icon/Platinum.svg",
                    "All" => "ms-appx:///Assets/Icon/All.svg",
                    _ => "ms-appx:///Assets/Icon/All.svg"
                };

                return new SvgImageSource(new Uri(iconPath));
            }
            return new SvgImageSource(new Uri("ms-appx:///Assets/Icon/All.svg"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
