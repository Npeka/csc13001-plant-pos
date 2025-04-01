using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace csc13001_plant_pos.Views
{
    public sealed partial class CustomerProfile : Page
    {
        public CustomerProfile()
        {
            this.InitializeComponent();
        }
        private bool hasChanges = false;

        private void OnFieldChanged(object sender, RoutedEventArgs e)
        {
            hasChanges = true;
            DiscardButton.Visibility = Visibility.Visible; // Hiện nút Hủy thay đổi
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string email = EmailTextBox.Text;
            string address = AddressTextBox.Text;
            string phoneNumber = PhoneNumberTextBox.Text;
            string postalCode = PostalCodeTextBox.Text;
            string location = (LocationComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
           
            ContentDialog saveDialog = new ContentDialog
            {
                Title = "Save Changes",
                Content = $"Changes saved for {firstName} {lastName}.",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            _ = saveDialog.ShowAsync();
        }

        private void DiscardChanges_Click(object sender, RoutedEventArgs e)
        {
            FirstNameTextBox.Text = "Roland";
            LastNameTextBox.Text = "Donald";
            EmailTextBox.Text = "rolandDonald@gmail.com";
            AddressTextBox.Text = "3605 Parker Rd.";
            PhoneNumberTextBox.Text = "(405) 555-0128";
            PostalCodeTextBox.Text = "30301";
            LocationComboBox.SelectedIndex = 0;
            ProfileCreatedDateText.Text = "11/11/2025";
            hasChanges = false;
            DiscardButton.Visibility = Visibility.Collapsed;
            ContentDialog discardDialog = new ContentDialog
            {
                Title = "Hủy thay đổi",
                Content = "Đã quay lại trạng thái lưu cuối cùng.",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            _ = discardDialog.ShowAsync();
        }
    }
}
