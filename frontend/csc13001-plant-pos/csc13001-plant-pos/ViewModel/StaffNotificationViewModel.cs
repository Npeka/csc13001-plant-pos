using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.DTO.NotificationDTO;
using csc13001_plant_pos.Service;

namespace csc13001_plant_pos.ViewModel
{
    public partial class StaffNotificationViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<NotificationDto> notifications = new ObservableCollection<NotificationDto>();

        [ObservableProperty]
        private ObservableCollection<NotificationDto> filteredNotifications = new ObservableCollection<NotificationDto>();

        [ObservableProperty]
        private string selectedTag = "All";

        [ObservableProperty]
        private DateTimeOffset? selectedDate;

        [ObservableProperty]
        private string selectedReadFilter = "All";

        private readonly INotificationService _notificationService;
        private readonly UserSessionService _userSession;

        public StaffNotificationViewModel(INotificationService notificationService, UserSessionService userSession)
        {
            _notificationService = notificationService;
            _userSession = userSession;

            LoadNotificationsAsync();
        }

        private async void LoadNotificationsAsync()
        {
            if (_userSession.CurrentUser == null)
            {
                System.Diagnostics.Debug.WriteLine("User session not found.");
                return;
            }

            var staffId = _userSession.CurrentUser.UserId.ToString();
            var response = await _notificationService.GetNotificationsAsync(staffId);

            if (response?.Status == "success" && response.Data != null)
            {
                Notifications.Clear();
                foreach (var notification in response.Data)
                {
                    Notifications.Add(notification);
                }
                ApplyFilters();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load notifications: {response?.Message}");
            }
        }

        private void ApplyFilters()
        {
            FilteredNotifications.Clear();
            var filtered = Notifications.AsEnumerable();

            // Lọc theo tag
            if (SelectedTag != "All")
            {
                filtered = filtered.Where(n => n.Type == SelectedTag);
            }

            // Lọc theo ngày
            if (SelectedDate.HasValue)
            {
                var selectedDateOnly = SelectedDate.Value.Date;
                filtered = filtered.Where(n => n.CreatedAt.Date == selectedDateOnly);
            }

            // Lọc theo trạng thái đọc
            switch (SelectedReadFilter)
            {
                case "Unread":
                    filtered = filtered.Where(n => !n.IsRead);
                    break;
                case "Read":
                    filtered = filtered.Where(n => n.IsRead);
                    break;
                case "All":
                default:
                    break;
            }

            foreach (var notification in filtered)
            {
                FilteredNotifications.Add(notification);
            }
        }

        partial void OnSelectedTagChanged(string value)
        {
            ApplyFilters();
        }

        partial void OnSelectedDateChanged(DateTimeOffset? value)
        {
            ApplyFilters();
        }

        partial void OnSelectedReadFilterChanged(string value)
        {
            ApplyFilters();
        }

        [RelayCommand]
        private async Task MarkAsRead(NotificationDto notification)
        {
            if (notification == null || notification.IsRead) return;
            var success = await _notificationService.MarkNotificationAsReadAsync(notification.NotificationUserId);
            if (success)
            {
                notification.IsRead = true;
                if (SelectedReadFilter == "Unread" && FilteredNotifications.Contains(notification))
                {
                    FilteredNotifications.Remove(notification);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Failed to mark notification {notification.NotificationUserId} as read.");
            }
        }
    }
}