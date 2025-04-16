using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace csc13001_plant_pos.Converter
{
    public class FromBotToAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isFromBot = (bool)value;
            string[] alignments = parameter.ToString().Split(':');
            return isFromBot ? HorizontalAlignment.Left : HorizontalAlignment.Right;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
