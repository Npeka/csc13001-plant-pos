using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;


namespace csc13001_plant_pos.View
{
    public sealed partial class AdminDashBoard : Page
    {
        public AdminDashBoard()
        {
            this.InitializeComponent();
        }

        private void AdminDashBoard_Loaded(object sender, RoutedEventArgs e)
        {
            adminFrame.Navigate(typeof(StatisticPage));
        }

        private void navigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
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
                    Frame.Navigate(typeof(RoleSelectionPage));
                }

                // Điều hướng trang con
                Type pageType = Type.GetType($"{GetType().Namespace}.{tag}");
                if (pageType != null)
                {
                    adminFrame.Navigate(pageType);
                }
            }
        }


        private void navigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {

        }
    }
}
