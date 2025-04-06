using System;
using Microsoft.UI.Xaml.Data;

namespace csc13001_plant_pos.Converter
{
    public class SizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int intValue)
            {
                // Kích thước từ 1 đến 5
                switch (intValue)
                {
                    case 1:
                        return "Rất nhỏ";
                    case 2:
                        return "Nhỏ";
                    case 3:
                        return "Vừa";
                    case 4:
                        return "Lớn";
                    case 5:
                        return "Rất lớn";
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