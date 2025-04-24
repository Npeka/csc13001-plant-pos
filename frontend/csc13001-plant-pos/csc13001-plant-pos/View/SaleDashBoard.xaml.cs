using System;
using csc13001_plant_pos.Service;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.View
{
    public sealed partial class SaleDashBoard : Page
    {
        private readonly UserSessionService _userSessionService;
        

        public SaleDashBoard()
        {
            this.InitializeComponent();
            this.Loaded += SaleDashBoard_Loaded;
            this.DataContext = _userSessionService = App.GetService<UserSessionService>();
        }

        private void SaleDashBoard_Loaded(object sender, RoutedEventArgs e)
        {
            saleFrame.Navigate(typeof(SalePage));
        }
        private async void navigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                return;
            }

            // Lấy item được chọn
            var item = sender.SelectedItem as NavigationViewItem;
            if (item != null && item.Tag != null)
            {
                string tag = item.Tag.ToString();

                if (tag == "Logout")
                {
                    ContentDialog dialog = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                        Title = "Xác nhận đăng xuất",
                        Content = "Bạn có chắc chắn muốn đăng xuất?",
                        CloseButtonText = "Không",
                        PrimaryButtonText = "Có",
                        DefaultButton = ContentDialogButton.Primary,
                    };

                    var result = await dialog.ShowAsync();
                    if (result == ContentDialogResult.Primary)
                    {
                        var mainWindow = (App.Current as App)?.GetMainWindow();
                        if (mainWindow?.Content is Frame frame)
                        {
                            await _userSessionService.ClearUser();
                            frame.Navigate(typeof(AuthenticationPage));
                        }
                    }
                }

                // Điều hướng trang con
                Type pageType = Type.GetType($"{GetType().Namespace}.{tag}");
                if (pageType != null)
                {
                    saleFrame.Navigate(pageType);
                }
            }
        }


        private void navigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {

        }
    }
}
