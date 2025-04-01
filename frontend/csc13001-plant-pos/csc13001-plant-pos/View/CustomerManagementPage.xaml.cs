using csc13001_plant_pos.ViewModel;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace csc13001_plant_pos.View
{
    
    public sealed partial class CustomerManagementPage : Page
    {
        public CustomerManagementViewModel ViewModel { get; }

        public CustomerManagementPage()
        {
            ViewModel = App.GetService<CustomerManagementViewModel>();
            this.InitializeComponent();
        }


        private SolidColorBrush GetRankColor(string rank) => rank switch
        {
            "Đồng" => new SolidColorBrush(Colors.Brown),
            "Bạc" => new SolidColorBrush(Colors.Silver),
            "Vàng" => new SolidColorBrush(Colors.Gold),
            "Bạch Kim" => new SolidColorBrush(Colors.LightGray),
            "Kim Cương" => new SolidColorBrush(Colors.DeepSkyBlue),
            _ => new SolidColorBrush(Colors.Gray),
        };
    }
}
