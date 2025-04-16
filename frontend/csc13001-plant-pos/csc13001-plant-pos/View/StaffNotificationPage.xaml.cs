using csc13001_plant_pos.Model;
using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.View
{
    public sealed partial class StaffNotificationPage : Page
    {
        public StaffNotificationViewModel ViewModel { get; }

        public StaffNotificationPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel = App.GetService<StaffNotificationViewModel>();
        }

        private void PageSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.DataContext is StaffNotificationViewModel viewModel)
            {
                viewModel.ApplyFiltersCommand.Execute(null);
            }
        }

        private void OnNotificationTapped(object sender, RoutedEventArgs e)
        {
            if (sender is Grid grid && grid.DataContext is Notification notification)
            {
                ViewModel.MarkAsReadCommand.Execute(notification);
            }
        }
    }
}
