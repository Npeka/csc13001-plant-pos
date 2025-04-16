using System;
using Microsoft.UI.Xaml.Data;

namespace csc13001_plant_pos.Converter
{
    public class FromBotToColumnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isFromBot = (bool)value;
            string[] columns = parameter.ToString().Split(':');
            return isFromBot ? int.Parse(columns[0]) : int.Parse(columns[1]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
