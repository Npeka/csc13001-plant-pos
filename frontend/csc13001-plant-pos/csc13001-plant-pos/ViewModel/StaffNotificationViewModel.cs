using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;

namespace csc13001_plant_pos.ViewModel
{
    public partial class StaffNotificationViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Notification> notifications = new ObservableCollection<Notification>();

        [ObservableProperty]
        private ObservableCollection<Notification> filteredNotifications = new ObservableCollection<Notification>();

        [ObservableProperty]
        private string selectedTag = "All";

        [ObservableProperty]
        private DateTimeOffset? selectedDate;

        [ObservableProperty]
        private string selectedReadFilter = "All";

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private int pageSize = 10;

        [ObservableProperty]
        private int totalPages;

        private readonly int[] pageSizeOptions = { 5, 10, 20, 50 };
        private readonly INotificationService _notificationService;
        private readonly UserSessionService _userSession;

        public int[] PageSizeOptions => pageSizeOptions;

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

        [RelayCommand]
        private void ApplyFilters()
        {
            FilteredNotifications.Clear();
            var filtered = Notifications.AsEnumerable();

            if (SelectedTag != "All")
            {
                filtered = filtered.Where(n => n.Type == SelectedTag);
            }

            if (SelectedDate.HasValue)
            {
                var selectedDateOnly = SelectedDate.Value.Date;
                filtered = filtered.Where(n => n.CreatedAt.Date == selectedDateOnly);
            }

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

            filtered = filtered.OrderByDescending(n => n.CreatedAt);

            var totalItems = filtered.Count();
            TotalPages = (int)Math.Ceiling((double)totalItems / PageSize);

            if (CurrentPage > TotalPages) CurrentPage = TotalPages > 0 ? TotalPages : 1;
            if (CurrentPage < 1) CurrentPage = 1;

            filtered = filtered
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize);

            foreach (var notification in filtered)
            {
                FilteredNotifications.Add(notification);
            }

            OnPropertyChanged(nameof(FilteredNotifications));
        }

        [RelayCommand]
        private void NextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                ApplyFilters();
            }
        }

        [RelayCommand]
        private void PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                ApplyFilters();
            }
        }

        partial void OnSelectedTagChanged(string value)
        {
            CurrentPage = 1;
            ApplyFilters();
        }

        partial void OnSelectedDateChanged(DateTimeOffset? value)
        {
            CurrentPage = 1;
            ApplyFilters();
        }

        partial void OnSelectedReadFilterChanged(string value)
        {
            CurrentPage = 1;
            ApplyFilters();
        }

        partial void OnPageSizeChanged(int oldValue, int newValue)
        {
            if (oldValue != newValue)
            {
                CurrentPage = 1;
                ApplyFilters();
            }
        }

        [RelayCommand]
        private async Task MarkAsRead(Notification notification)
        {
            if (notification == null || notification.IsRead) return;
            var success = await _notificationService.MarkNotificationAsReadAsync(notification.NotificationUserId);
            if (success)
            {
                notification.IsRead = true;
                if (SelectedReadFilter == "Unread" && FilteredNotifications.Contains(notification))
                {
                    FilteredNotifications.Remove(notification);
                    ApplyFilters();
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Failed to mark notification {notification.NotificationUserId} as read.");
            }
        }
    }
}