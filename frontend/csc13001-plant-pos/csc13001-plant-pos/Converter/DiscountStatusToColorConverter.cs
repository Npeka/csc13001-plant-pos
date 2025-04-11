using System;
using csc13001_plant_pos.Model;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace csc13001_plant_pos.Converter
{
    public class DiscountStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DiscountProgram discount)
            {
                DateTime now = DateTime.Now;

                if (discount.StartDate > now && discount.EndDate > now)
                {
                    return new SolidColorBrush(ColorHelper.FromArgb(255, 255, 160, 0));
                }
                else if (discount.EndDate < now)
                {
                    return new SolidColorBrush(Colors.Red); // Red
                }
                else
                {
                    return new SolidColorBrush(ColorHelper.FromArgb(255, 76, 175, 80));
                }
            }

            return new SolidColorBrush(ColorHelper.FromArgb(255, 76, 175, 80));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
