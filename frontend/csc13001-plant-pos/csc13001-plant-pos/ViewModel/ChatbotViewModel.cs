using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using csc13001_plant_pos.DTO.Message;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;
using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.View
{
    public partial class ChatbotViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Message> messages = new ObservableCollection<Message>();

        [ObservableProperty]
        private ObservableCollection<MessageDateGroup> messageGroups = new ObservableCollection<MessageDateGroup>();

        [ObservableProperty]
        private ObservableCollection<string> messageDates = new ObservableCollection<string>();

        [ObservableProperty]
        private string newMessage;

        [ObservableProperty]
        private bool isHistoryPanelVisible = false;

        [ObservableProperty]
        private DateTime? selectedDate;

        private readonly IMessageService _messageService;
        private readonly UserSessionService _userSession;
        private long _currentSequence = 0; // Biến để theo dõi thứ tự tin nhắn

        public ChatbotViewModel(IMessageService messageService, UserSessionService userSession)
        {
            _messageService = messageService;
            _userSession = userSession;
            LoadMessagesAsync();
        }

        public async void LoadMessagesAsync()
        {
            if (_userSession.CurrentUser == null)
            {
                System.Diagnostics.Debug.WriteLine("User session not found.");
                return;
            }

            var userId = _userSession.CurrentUser.UserId;
            var response = await _messageService.GetMessagesByUserIdAsync(userId);
            System.Diagnostics.Debug.WriteLine($"Status: {response?.Status}, Message: {response?.Message}");
            if (response?.Status == "success" && response.Data != null)
            {
                Messages.Clear();
                foreach (var message in response.Data.OrderBy(m => m.SentAt))
                {
                    message.Sequence = _currentSequence++; // Gán Sequence cho tin nhắn lịch sử
                    Messages.Add(message);
                }
                UpdateMessageGroups(false);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load messages for user {userId}: {response?.Message}");
            }
        }

        private void UpdateMessageGroups(bool preserveScrollPosition = true)
        {
            var previousGroups = new List<MessageDateGroup>(MessageGroups);

            if (!preserveScrollPosition)
            {
                MessageGroups.Clear();
                MessageDates.Clear();
            }

            var filteredMessages = SelectedDate.HasValue
                ? Messages.Where(m => m.SentAt.Date == SelectedDate.Value.Date)
                : Messages;

            var grouped = filteredMessages
                .GroupBy(m => m.SentAt.Date)
                .OrderByDescending(g => g.Key)
                .ToList();

            foreach (var group in grouped)
            {
                var existingGroup = preserveScrollPosition
                    ? previousGroups.FirstOrDefault(g => g.Date.Date == group.Key.Date)
                    : null;

                if (existingGroup != null && MessageGroups.Contains(existingGroup))
                {
                    existingGroup.Messages.Clear();
                    foreach (var msg in group.OrderBy(m => m.Sequence)) // Sắp xếp theo Sequence
                    {
                        existingGroup.Messages.Add(msg);
                    }
                }
                else
                {
                    var newGroup = new MessageDateGroup(group.Key, group.OrderBy(m => m.Sequence));
                    if (!MessageGroups.Any(g => g.Date.Date == group.Key.Date))
                    {
                        MessageGroups.Add(newGroup);
                        MessageDates.Add(FormatDate(group.Key));
                    }
                }
            }

            var datesToRemove = MessageGroups
                .Where(g => !grouped.Any(gr => gr.Key.Date == g.Date.Date))
                .ToList();
            foreach (var group in datesToRemove)
            {
                MessageGroups.Remove(group);
                MessageDates.Remove(FormatDate(group.Date));
            }

            if (!MessageGroups.Any() && !SelectedDate.HasValue)
            {
                var today = DateTime.Today;
                MessageGroups.Add(new MessageDateGroup(today, Enumerable.Empty<Message>()));
                MessageDates.Add("Hôm nay");
            }
        }

        private string FormatDate(DateTime date)
        {
            var today = DateTime.Today;
            if (date.Date == today)
                return "Hôm nay";
            if (date.Date == today.AddDays(-1))
                return "Hôm qua";
            return date.ToString("dd/MM/yyyy");
        }

        [RelayCommand]
        private async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(NewMessage) || _userSession.CurrentUser == null)
                return;

            var userId = _userSession.CurrentUser.UserId;
            string messageText = NewMessage.Trim();

            var userMessage = new Message
            {
                MessageId = -1,
                Content = messageText,
                FromBot = false,
                SentAt = DateTime.Now,
                User = _userSession.CurrentUser,
                Sequence = _currentSequence++ // Gán Sequence cho tin nhắn người dùng
            };

            Messages.Add(userMessage);
            UpdateMessageGroups();

            NewMessage = string.Empty;

            var messageDto = new CreateMessageDto
            {
                UserId = userId,
                Message = messageText
            };

            var response = await _messageService.SendMessageAsync(messageDto);
            System.Diagnostics.Debug.WriteLine($"Send message status: {response?.Status}, Message: {response?.Message}");
            if (response?.Status == "success" && response.Data != null)
            {
                response.Data.Sequence = _currentSequence++; // Gán Sequence cho tin nhắn bot
                Messages.Add(response.Data);
                UpdateMessageGroups();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Failed to send message: {response?.Message}");
                Messages.Remove(userMessage);
                UpdateMessageGroups();
            }
        }

        [RelayCommand]
        private void ToggleHistoryPanel()
        {
            IsHistoryPanelVisible = !IsHistoryPanelVisible;
        }

        [RelayCommand]
        private void SelectDate(SelectionChangedEventArgs args)
        {
            if (args.AddedItems.FirstOrDefault() is string dateStr)
            {
                var today = DateTime.Today;
                if (dateStr == "Hôm nay")
                    SelectedDate = today;
                else if (dateStr == "Hôm qua")
                    SelectedDate = today.AddDays(-1);
                else if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var parsedDate))
                    SelectedDate = parsedDate;
                else
                    SelectedDate = null;

                UpdateMessageGroups(false);
            }
        }
    }
}