using CommunityToolkit.Mvvm.ComponentModel;

using csc13001_plant_pos.Contracts.Services;
using csc13001_plant_pos.Contracts.ViewModels;
using csc13001_plant_pos.Views;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace csc13001_plant_pos.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool isBackEnabled;

    [ObservableProperty]
    private object? selected;

    public INavigationService NavigationService
    {
        get;
    }

    public INavigationViewService NavigationViewService
    {
        get;
    }

    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }

        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }

    public void UpdateNavigationItemsBasedOnRole(bool isAdmin)
    {
        var notRole = !isAdmin ? "Admin" : "Staff";

        if (NavigationViewService.MenuItems != null)
        {
            var MenuItems = NavigationViewService.MenuItems
                   .OfType<NavigationViewItem>()
                   .Where(item => item.Name.StartsWith(notRole))
                   .ToList();

            foreach (var item in MenuItems)
            {
                NavigationViewService.MenuItems?.Remove(item);
            }
        }

        Selected = NavigationViewService.MenuItems?[0];
    }
}
