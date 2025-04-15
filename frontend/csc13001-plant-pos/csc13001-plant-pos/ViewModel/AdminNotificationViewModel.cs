using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.DTO.NotificationDTO;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;

namespace csc13001_plant_pos.ViewModel
{
    public partial class AdminNotificationViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<NotificationDto> notifications = new ObservableCollection<NotificationDto>();

        [ObservableProperty]
        private ObservableCollection<NotificationDto> filteredNotifications = new ObservableCollection<NotificationDto>();

        [ObservableProperty]
        private ObservableCollection<User> staffList = new ObservableCollection<User>();

        [ObservableProperty]
        private string newNotificationTitle;

        [ObservableProperty]
        private string newNotificationContent;

        [ObservableProperty]
        private string selectedNotificationType = "OwnerAnnouncement";

        [ObservableProperty]
        private DateTimeOffset? selectedDate;

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private int pageSize = 10;

        [ObservableProperty]
        private int totalPages;

        private readonly int[] pageSizeOptions = { 5, 10, 20, 50 };
        private bool _isAllStaffSelected;

        public bool IsAllStaffSelected
        {
            get => _isAllStaffSelected;
            set
            {
                if (SetProperty(ref _isAllStaffSelected, value))
                {
                    if (!_updatingFromIndividualCheckboxes)
                    {
                        _updatingFromToggle = true;
                        foreach (var staff in StaffList)
                        {
                            staff.IsSelected = value;
                        }
                        _updatingFromToggle = false;
                        UpdateSelectedCount();
                    }
                }
            }
        }

        [ObservableProperty]
        private int selectedStaffCount;

        public int[] PageSizeOptions => pageSizeOptions;

        partial void OnSelectedStaffCountChanged(int value)
        {
            if (_updatingFromToggle) return;

            _updatingFromIndividualCheckboxes = true;
            IsAllStaffSelected = value == StaffList.Count && StaffList.Count > 0;
            _updatingFromIndividualCheckboxes = false;
        }

        private bool _updatingFromToggle = false;
        private bool _updatingFromIndividualCheckboxes = false;

        private readonly INotificationService _notificationService;
        private readonly IStaffService _staffService;

        public AdminNotificationViewModel(INotificationService notificationService, IStaffService staffService)
        {
            _notificationService = notificationService;
            _staffService = staffService;
            IsAllStaffSelected = false;
            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            var notificationResponse = await _notificationService.GetAllNotificationsAsync();
            if (notificationResponse?.Status == "success" && notificationResponse.Data != null)
            {
                Notifications.Clear();
                foreach (var notification in notificationResponse.Data)
                {
                    Notifications.Add(notification);
                }
                ApplyFilters();
            }

            var staffResponse = await _staffService.GetListStaffAsync();
            if (staffResponse?.Status == "success" && staffResponse.Data != null)
            {
                StaffList.Clear();
                foreach (var staff in staffResponse.Data)
                {
                    staff.PropertyChanged += (sender, e) =>
                    {
                        if (e.PropertyName == nameof(User.IsSelected))
                        {
                            UpdateSelectedCount();
                        }
                    };
                    StaffList.Add(staff);
                }
            }
        }

        private void UpdateSelectedCount()
        {
            SelectedStaffCount = StaffList.Count(s => s.IsSelected);
        }

        [RelayCommand]
        private void ApplyFilters()
        {
            FilteredNotifications.Clear();
            var filtered = Notifications.AsEnumerable();

            if (SelectedDate.HasValue)
            {
                var selectedDateOnly = SelectedDate.Value.Date;
                filtered = filtered.Where(n => n.CreatedAt.Date == selectedDateOnly);
            }

            // Sắp xếp theo CreatedAt (mới nhất trước)
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

            // Force collection change notification
            OnPropertyChanged(nameof(FilteredNotifications));
        }

        [RelayCommand]
        private async Task SendNotification()
        {
            var notification = new CreateNotificationDto
            {
                Title = NewNotificationTitle,
                Content = NewNotificationContent,
                Type = SelectedNotificationType,
                To = IsAllStaffSelected ? null : StaffList.Where(s => s.IsSelected).Select(s => s.UserId).ToList()
            };

            var success = await _notificationService.CreateNotificationAsync(notification);
            if (success)
            {
                NewNotificationTitle = string.Empty;
                NewNotificationContent = string.Empty;
                IsAllStaffSelected = false;
                foreach (var staff in StaffList)
                {
                    staff.IsSelected = false;
                }
                LoadDataAsync();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Failed to send notification.");
            }
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

        partial void OnSelectedDateChanged(DateTimeOffset? value)
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
    }
}