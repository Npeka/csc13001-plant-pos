using System;
using csc13001_plant_pos.Model;
using Microsoft.UI.Xaml.Data;

namespace csc13001_plant_pos.Converter
{
    public class DiscountStatusToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DiscountProgram discount)
            {
                DateTime now = DateTime.Now;

                if (discount.StartDate > now && discount.EndDate > now)
                    return "Sắp diễn ra";
                else if (discount.EndDate < now)
                    return "Hết hạn";
                else
                    return "Đang diễn ra";
            }
            return "Không xác định";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
