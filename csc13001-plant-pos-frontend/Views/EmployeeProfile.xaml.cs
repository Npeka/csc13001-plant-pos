using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace csc13001_plant_pos_frontend.Views
{
    public sealed partial class EmployeeProfile : Page
    {
        private Employee CurrentEmployee;
        private bool hasChanges = false;

        public EmployeeProfile()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Employee employee)
            {
                CurrentEmployee = employee;
                LoadEmployeeData();
            }
        }

        private void LoadEmployeeData()
        {
            if (CurrentEmployee != null)
            {
                EmployeeNameText.Text = $"{CurrentEmployee.Name}";
                EmployeePositionText.Text = CurrentEmployee.Position;

                string[] nameParts = CurrentEmployee.Name.Split(' ');
                FirstNameTextBox.Text = nameParts.Length > 1 ? nameParts[0] : "";
                LastNameTextBox.Text = nameParts.Length > 1 ? nameParts[1] : CurrentEmployee.Name;
                EmployeeIDTextBox.Text = CurrentEmployee.ID;
                PhoneNumberTextBox.Text = CurrentEmployee.PhoneNumber;
                StartDateTextBox.Text = CurrentEmployee.StartDate.ToString("dd/MM/yyyy");

                foreach (ComboBoxItem item in PositionComboBox.Items)
                {
                    if (item.Content.ToString() == CurrentEmployee.Position)
                    {
                        PositionComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void OnFieldChanged(object sender, RoutedEventArgs e)
        {
            hasChanges = true;
            DiscardButton.Visibility = Visibility.Visible;
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentEmployee != null)
            {
                CurrentEmployee.Name = $"{FirstNameTextBox.Text} {LastNameTextBox.Text}";
                CurrentEmployee.PhoneNumber = PhoneNumberTextBox.Text;
                CurrentEmployee.Position = (PositionComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                ContentDialog saveDialog = new ContentDialog
                {
                    Title = "Lưu thay đổi",
                    Content = $"Thông tin của {CurrentEmployee.Name} đã được lưu.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                _ = saveDialog.ShowAsync();

                hasChanges = false;
                DiscardButton.Visibility = Visibility.Collapsed;
            }
        }

        private void DiscardChanges_Click(object sender, RoutedEventArgs e)
        {
            LoadEmployeeData();
            hasChanges = false;
            DiscardButton.Visibility = Visibility.Collapsed;

            ContentDialog discardDialog = new ContentDialog
            {
                Title = "Hủy thay đổi",
                Content = "Đã quay lại thông tin nhân viên ban đầu.",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            _ = discardDialog.ShowAsync();
        }
    }
}
