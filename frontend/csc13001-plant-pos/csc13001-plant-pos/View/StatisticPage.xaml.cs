using System.ComponentModel;
using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.LoadDataAsync();
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