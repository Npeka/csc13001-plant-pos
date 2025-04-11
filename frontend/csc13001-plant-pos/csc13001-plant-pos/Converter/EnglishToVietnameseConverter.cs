using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Data;

namespace csc13001_plant_pos.Converter
{
    public class EnglishToVietnameseConverter : IValueConverter
    {
        private static readonly Dictionary<string, string> StatusMap = new()
        {
            { "Working", "Đang làm việc" },
            { "OnLeave", "Đang nghỉ phép" },
            { "Resigned", "Đã nghỉ việc" },
            { "All", "Tất cả" }
        };

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is string status && StatusMap.TryGetValue(status, out var vietnameseStatus)
                ? vietnameseStatus
                : "Không xác định"; // Default for unknown values
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is string vietnameseStatus && StatusMap.ContainsValue(vietnameseStatus)
                ? StatusMap.FirstOrDefault(x => x.Value == vietnameseStatus).Key
                : "Unknown"; // Default for unknown values
        }
    }
}
