using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.ViewModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SkiaSharp;

namespace csc13001_plant_pos.View
{
    public sealed partial class StatisticPage : Page, INotifyPropertyChanged
    {

        public StatisticViewModel ViewModel { get; }


        public StatisticPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel = App.GetService<StatisticViewModel>();
        }

        public void ClickNavigate(object sender, RoutedEventArgs e)
        {
                Frame.Navigate(typeof(TopSellingProductsPage));
        }
        private async void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            var currentWindow = ((App)Application.Current).GetMainWindow();
            await ViewModel.ExportToExcelAsync(currentWindow);
        }
    }

}