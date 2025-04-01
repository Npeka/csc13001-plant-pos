using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos_frontend.Helpers;

public static class FrameExtensions
{
    public static object? GetPageViewModel(this Frame frame) => frame?.Content?.GetType().GetProperty("ViewModel")?.GetValue(frame.Content, null);
}
