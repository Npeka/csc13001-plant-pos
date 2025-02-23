using csc13001_plant_pos.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.Views;

public sealed partial class SalePage : Page
{
    public SaleViewModel ViewModel
    {
        get;
    }

    public SalePage()
    {
        ViewModel = App.GetService<SaleViewModel>();
        DataContext = ViewModel;
        InitializeComponent();
    }
    private void ProductItem_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
    {
        if (sender is FrameworkElement element && element.Tag is Product product)
        {
            ViewModel.AddToOrderCommand.Execute(product);
        }
    }
    private async void NoteButton_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var orderItem = button.DataContext as OrderItem;

        // Create TextBox
        var textBox = new TextBox
        {
            PlaceholderText = "Enter note here...",
            Text = orderItem.Note,
            TextWrapping = TextWrapping.Wrap,
            AcceptsReturn = true,
            Height = 100,
            Margin = new Thickness(0, 10, 0, 0)
        };

        // Create ContentDialog
        var dialog = new ContentDialog
        {
            Title = "Add Note",
            Content = textBox,
            PrimaryButtonText = "Save",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            XamlRoot = this.XamlRoot
        };

        // Show dialog and handle result
        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            orderItem.Note = textBox.Text;
        }
    }
}
