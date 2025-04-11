using csc13001_plant_pos.DTO.NotificationDTO;
using csc13001_plant_pos.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.View
{
    public sealed partial class StaffNotification : Page
    {
        public StaffNotificationViewModel ViewModel { get; }

        public StaffNotification()
        {
            this.InitializeComponent();
            this.DataContext =  ViewModel = App.GetService<StaffNotificationViewModel>();
        }
        private void OnNotificationTapped(object sender, RoutedEventArgs e)
        {
            if (sender is Grid grid && grid.DataContext is NotificationDto notification)
            {
                ViewModel.MarkAsReadCommand.Execute(notification);
            }
        }
    }
}