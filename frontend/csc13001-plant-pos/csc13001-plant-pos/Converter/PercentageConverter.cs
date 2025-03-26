using System;
using Microsoft.UI.Xaml.Data;

namespace csc13001_plant_pos.Converter
{
    public class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double discountRate)
            {
                bool isNegative = parameter != null; // Nếu có tham số, đảo dấu

                return isNegative ? $"-{discountRate:0.##}%" : $"{discountRate:0.##}%";
            }
            return "0%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string strValue && strValue.EndsWith("%"))
            {
                if (double.TryParse(strValue.TrimEnd('%'), out double result))
                {
                    bool isNegative = parameter != null; // Nếu có tham số, đảo dấu
                    return isNegative ? -result : result;
                }
            }
            return 0.0;
        }
    }
}
