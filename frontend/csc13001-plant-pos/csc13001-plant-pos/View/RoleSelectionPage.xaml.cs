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
    public sealed partial class RoleSelectionPage : Page
    {
        public RoleSelectionPage()
        {
            this.InitializeComponent();
        }

        private void SaleButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SaleDashBoard));
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AdminDashBoard));
        }
    }
}
