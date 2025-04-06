using System;
using Microsoft.UI.Xaml.Data;

namespace csc13001_plant_pos.Converter
{
    public class LightRequirement : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int intValue)
            {
                // Mức độ ánh sáng từ 1 đến 5
                switch (intValue)
                {
                    case 1:
                        return "Ít ánh sáng";
                    case 2:
                        return "Ánh sáng gián tiếp";
                    case 3:
                        return "Ánh sáng tán xa";
                    case 4:
                        return "Ánh sáng trực tiếp một phần";
                    case 5:
                        return "Cần nhiều ánh sáng trực tiếp";
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
