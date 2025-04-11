using System;
using Microsoft.UI.Xaml.Data;

namespace csc13001_plant_pos.Converter
{
    public class DateTimeWithHourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime date)
            {
                return date.ToString("dd/MM/yyyy HH:mm");
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (DateTime.TryParse(value as string, out DateTime date))
            {
                return date;
            }
            return DateTime.MinValue;
        }
    }
}