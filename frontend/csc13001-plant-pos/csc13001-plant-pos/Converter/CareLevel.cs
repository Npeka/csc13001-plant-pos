using System;
using Microsoft.UI.Xaml.Data;

namespace csc13001_plant_pos.Converter
{
    public class CareLevel : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int intValue)
            {
                // Mức độ chăm sóc từ 1 đến 5
                switch (intValue)
                {
                    case 1:
                        return "Dễ chăm sóc";
                    case 2:
                        return "Chăm sóc trung bình";
                    case 3:
                        return "Khó chăm sóc";
                    case 4:
                        return "Rất khó chăm sóc";
                    case 5:
                        return "Cần chăm sóc đặc biệt";
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