using System;
using System.Linq;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;
using csc13001_plant_pos.ViewModel.Authentication;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.View.Authentication;

public sealed partial class FormLogin : UserControl
{
    public LoginViewModel ViewModel { get; }

    private readonly UserSessionService _userSessionService;

    public FormLogin()
    {
        this.InitializeComponent();
        this.DataContext = ViewModel = App.GetService<LoginViewModel>();
        ViewModel.NavigateToDashboard += OnNavigateToDashboard;
    }

    private void Password_PasswordChanged(object sender, RoutedEventArgs e)
    {
        ViewModel.Password = PasswordBox.Password;
    }

    private void OnNavigateToDashboard(object sender, NavigateEventArgs e)
    {
        var user = e.User;
        var mainWindow = (App.Current as App)?.GetMainWindow();
        if (mainWindow?.Content is Frame frame)
        {
            var userSessionService = App.GetService<UserSessionService>();
            if (user.IsAdmin)
            {
                frame.Navigate(typeof(AdminDashBoard), userSessionService);
            }
            else
            {
                frame.Navigate(typeof(SaleDashBoard), userSessionService);
            }
        }
    }

    private void NavigateToForgotPassword_Click(object sender, RoutedEventArgs e)
    {
        var mainWindow = (App.Current as App)?.GetMainWindow();
        if (mainWindow?.Content is Frame frame && frame.Content is AuthenticationPage authenticationPage)
        {
            authenticationPage.NavigateToForgotPassword();
        }
    }

    private void UsernameBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var suggestions = ViewModel.SavedCredentials
                .Where(c => c.Username.Contains(sender.Text, StringComparison.OrdinalIgnoreCase))
                .ToList();

            sender.ItemsSource = suggestions.Count > 0 ? suggestions : null;
        }
    }

    private void UsernameBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        if (args.SelectedItem is RememberedCredential credential)
        {
            sender.Text = credential.Username;
            ViewModel.Username = credential.Username;
            ViewModel.Password = credential.Password;
            PasswordBox.Password = credential.Password;
            sender.ItemsSource = null;
        }
    }

    private void DeleteCredential_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string username)
        {
            var credential = ViewModel.SavedCredentials.FirstOrDefault(c => c.Username == username);
            if (credential != null)
            {
                ViewModel.DeleteSavedCredentialCommand.Execute(credential);

                if (UsernameBox.Text.Length > 0)
                {
                    var suggestions = ViewModel.SavedCredentials
                        .Where(c => c.Username.Contains(UsernameBox.Text, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    UsernameBox.ItemsSource = suggestions;
                }
            }
        }
    }
}