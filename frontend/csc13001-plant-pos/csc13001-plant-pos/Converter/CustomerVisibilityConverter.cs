using System;
using csc13001_plant_pos.Model;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace csc13001_plant_pos.Converter
{
    public class CustomerVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Customer customer && customer != null)
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
