using System;
using Microsoft.UI.Xaml.Data;

namespace csc13001_plant_pos.Converter
{
    public class WateringNeed : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int intValue)
            {
                // Mức độ tưới nước từ 1 đến 5
                switch (intValue)
                {
                    case 1:
                        return "Tưới nước rất ít (1 lần/tháng)";
                    case 2:
                        return "Tưới nước ít (1 lần/tuần)";
                    case 3:
                        return "Tưới nước vừa phải (2 lần/tuần)";
                    case 4:
                        return "Tưới nước thường xuyên (3 lần/tuần)";
                    case 5:
                        return "Tưới nước nhiều (4-5 lần/tuần)";
                    default:
                        return "Không xác định";
                }
            }
            return "Không xác định";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}