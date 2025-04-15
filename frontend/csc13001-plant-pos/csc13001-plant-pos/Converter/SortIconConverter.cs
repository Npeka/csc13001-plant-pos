using System;
using Microsoft.UI.Xaml.Data;

namespace csc13001_plant_pos.Converter;

public class SortIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        bool isAscending = (bool)value;
        return isAscending ? "\uE74A" : "\uE74B";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
