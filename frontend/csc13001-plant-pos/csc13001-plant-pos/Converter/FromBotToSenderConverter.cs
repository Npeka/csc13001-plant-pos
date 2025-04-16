using System;
using Microsoft.UI.Xaml.Data;

namespace csc13001_plant_pos.Converter
{
    public class FromBotToSenderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool fromBot && parameter is string names)
            {
                var namePair = names.Split(':');
                return fromBot ? namePair[0] : namePair[1];
            }
            return "Bạn";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
