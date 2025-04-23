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

        [ObservableProperty]
        private bool isInputAreaVisible = true;

        [ObservableProperty]
        private bool isBotTyping = false;

        private readonly IMessageService _messageService;
        private readonly UserSessionService _userSession;
        private long _currentSequence = 0;

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
                    message.Sequence = _currentSequence++;
                    Messages.Add(message);
                }
                UpdateMessageGroups();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load messages for user {userId}: {response?.Message}");
            }
        }

        private void UpdateMessageGroups()
        {
            MessageGroups.Clear();

            var allDates = Messages
                .GroupBy(m => m.SentAt.Date)
                .Select(g => g.Key)
                .OrderBy(d => d)
                .ToList();

            MessageDates.Clear();
            foreach (var date in allDates)
            {
                MessageDates.Add(FormatDate(date));
            }

            var filteredMessages = SelectedDate.HasValue
                ? Messages.Where(m => m.SentAt.Date == SelectedDate.Value.Date)
                : Messages;

            var grouped = filteredMessages
                .GroupBy(m => m.SentAt.Date)
                .OrderBy(g => g.Key)
                .ToList();

            foreach (var group in grouped)
            {
                var newGroup = new MessageDateGroup(group.Key, group.OrderBy(m => m.Sequence));
                MessageGroups.Add(newGroup);
            }

            if (!MessageGroups.Any() && !SelectedDate.HasValue)
            {
                var today = DateTime.Today;
                MessageGroups.Add(new MessageDateGroup(today, Enumerable.Empty<Message>()));
                if (!MessageDates.Contains("Hôm nay"))
                {
                    MessageDates.Add("Hôm nay");
                }
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
                Sequence = _currentSequence++
            };

            Messages.Add(userMessage);
            UpdateMessageGroups();

            NewMessage = string.Empty;
            IsBotTyping = true;

            var messageDto = new CreateMessageDto
            {
                UserId = userId,
                Message = messageText
            };

            var response = await _messageService.SendMessageAsync(messageDto);
            System.Diagnostics.Debug.WriteLine($"Send message status: {response?.Status}, Message: {response?.Message}");
            IsBotTyping = false;

            if (response?.Status == "success" && response.Data != null)
            {
                response.Data.Sequence = _currentSequence++;
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
                {
                    SelectedDate = today;
                    IsInputAreaVisible = true;
                }
                else if (dateStr == "Hôm qua")
                {
                    SelectedDate = today.AddDays(-1);
                    IsInputAreaVisible = false;
                }
                else if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var parsedDate))
                {
                    SelectedDate = parsedDate;
                    IsInputAreaVisible = parsedDate.Date == today;
                }
                else
                {
                    SelectedDate = null;
                    IsInputAreaVisible = true;
                }

                UpdateMessageGroups();
            }
        }
    }
}