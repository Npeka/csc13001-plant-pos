using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using System;
using Microsoft.UI;

namespace csc13001_plant_pos.Converter
{
    public class MembershipTypeToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string membershipType)
            {
                Color backgroundColor = membershipType switch
                {
                    "Silver" => ColorHelper.FromArgb(255, 240, 240, 242), 
                    "Gold" => ColorHelper.FromArgb(255, 255, 251, 229),  
                    "All" => ColorHelper.FromArgb(255, 245, 227, 250),  
                    "Bronze" => ColorHelper.FromArgb(255, 245, 236, 229),
                    "Platinum" => ColorHelper.FromArgb(255, 229, 255, 246), 
                    _ => ColorHelper.FromArgb(255, 245, 227, 250)
                };
                System.Diagnostics.Debug.WriteLine($"MembershipTypeToBackgroundConverter: {membershipType} -> {backgroundColor}");
                return new SolidColorBrush(backgroundColor);
            }
            return new SolidColorBrush(ColorHelper.FromArgb(255, 245, 227, 250));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
