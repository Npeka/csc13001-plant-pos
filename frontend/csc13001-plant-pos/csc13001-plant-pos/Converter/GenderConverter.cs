using System;
using Microsoft.UI.Xaml.Data;

namespace csc13001_plant_pos.Converter
{
    public class GenderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string gender)
            {
                return gender switch
                {
                    "Male" => "Nam",
                    "Female" => "Nữ",
                    _ => gender
                };
            }
            return "Không xác định";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
