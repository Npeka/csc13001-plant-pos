using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.View
{
    public sealed partial class AdminNotification : Page
    {
        public AdminNotificationViewModel ViewModel { get; }

        public AdminNotification()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<AdminNotificationViewModel>();
            this.DataContext = ViewModel;
        }

        private void PageSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.DataContext is AdminNotificationViewModel viewModel)
            {
                viewModel.ApplyFiltersCommand.Execute(null);
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SendNotificationCommand.Execute(null);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NewNotificationTitle = string.Empty;
            ViewModel.NewNotificationContent = string.Empty;
            ViewModel.IsAllStaffSelected = false;
            foreach (var staff in ViewModel.StaffList)
            {
                staff.IsSelected = false;
            }
        }
    }
}