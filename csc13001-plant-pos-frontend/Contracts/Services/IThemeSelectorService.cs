using System.Threading.Tasks;
using Microsoft.UI.Xaml;

namespace csc13001_plant_pos_frontend.Contracts.Services;

public interface IThemeSelectorService
{
    ElementTheme Theme
    {
        get;
    }

    Task InitializeAsync();

    Task SetThemeAsync(ElementTheme theme);

    Task SetRequestedThemeAsync();
}
