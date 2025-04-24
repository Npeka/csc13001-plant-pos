using System;
using csc13001_plant_pos.Model;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;

namespace csc13001_plant_pos.Converter
{
    public class MessageToProfilePictureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Message message)
            {
                if (message.FromBot)
                {
                    return new BitmapImage(new Uri("ms-appx:///Assets/Icon/chatbot.jpg"));
                }
                else if (!string.IsNullOrEmpty(message.User?.ImageUrl))
                {
                    try
                    {
                        return new BitmapImage(new Uri(message.User.ImageUrl));
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}