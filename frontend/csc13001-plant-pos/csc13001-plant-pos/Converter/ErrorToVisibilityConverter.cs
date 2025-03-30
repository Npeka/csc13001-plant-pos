using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace csc13001_plant_pos.Converter
{
    public class ErrorToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string error && !string.IsNullOrWhiteSpace(error))
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
