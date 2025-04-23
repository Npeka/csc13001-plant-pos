using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;

namespace csc13001_plant_pos.View
{
    public sealed partial class ChatbotPage : Page
    {
        public ChatbotViewModel ViewModel { get; }

        public ChatbotPage()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<ChatbotViewModel>();
            ViewModel.Messages.CollectionChanged += Messages_CollectionChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.LoadMessagesAsync();
        }

        private async void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            await Task.Delay(100);
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            if (ChatScrollViewer != null)
            {
                ChatScrollViewer.ChangeView(null, ChatScrollViewer.ScrollableHeight, null, true);
            }
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && !e.Handled)
            {
                ViewModel.SendMessageCommand.Execute(null);
                e.Handled = true;
            }
        }

        private void HistoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectDateCommand.Execute(e);
        }
    }
}