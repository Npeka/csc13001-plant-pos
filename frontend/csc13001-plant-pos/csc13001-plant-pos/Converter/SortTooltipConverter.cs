using System;
using Microsoft.UI.Xaml.Data;

namespace csc13001_plant_pos.Converter;

public class SortTooltipConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        bool isAscending = (bool)value;
        return isAscending ? "Sắp xếp giảm dần" : "Sắp xếp tăng dần";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
