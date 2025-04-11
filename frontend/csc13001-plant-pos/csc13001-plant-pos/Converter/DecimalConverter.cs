using System;
using Microsoft.UI.Xaml.Data;

namespace csc13001_plant_pos.Converter
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal decimalValue && decimalValue > 0)
            {
                return decimalValue.ToString("#,##0").Replace(",", ".");
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string stringValue)
            {
                stringValue = stringValue.Replace(".", "");
                if (decimal.TryParse(stringValue, out decimal result) && result >= 0)
                {
                    return result;
                }
            }
            return 0m;
        }
    }
}