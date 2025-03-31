using Microsoft.UI;
using Microsoft.UI.Xaml.Media;

namespace csc13001_plant_pos.Converter
{
    public class PercentageToForegroundConverter
    {
        public static SolidColorBrush Convert(int percentage)
        {
            return new SolidColorBrush(percentage < 0 ? ColorHelper.FromArgb(255, 255, 77, 77) //(#FF4D4D)
                                                      : ColorHelper.FromArgb(255, 76, 175, 80)); //(#4CAF50)
        }
    }
}
