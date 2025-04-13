using System;
using System.Globalization;
using Microsoft.UI.Xaml.Data;

namespace csc13001_plant_pos.Converter
{
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal decimalValue)
            {
                CultureInfo culture = new CultureInfo("vi-VN")
                {
                    NumberFormat = { NumberGroupSeparator = "." }
                };
                return decimalValue.ToString("N0", culture) + " ₫";
            }
            if (value is double longValue)
            {
                var culture = new CultureInfo("vi-VN")
                {
                    NumberFormat = { NumberGroupSeparator = "." }
                };
                return longValue.ToString("N0", culture) + " ₫";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
