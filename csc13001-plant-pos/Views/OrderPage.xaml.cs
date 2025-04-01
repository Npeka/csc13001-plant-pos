using System.Diagnostics;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using csc13001_plant_pos.ViewModels;
using csc13001_plant_pos.Data.Contexts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using csc13001_plant_pos.Contracts.Services;

namespace csc13001_plant_pos.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class OrderPage : Page
{
    public OrderViewModel ViewModel
    {
        get;
    }

    public OrderPage()
    {
        try
        {
            this.InitializeComponent();
            ViewModel = App.GetService<OrderViewModel>();
        }
        catch (Exception exception)  // Use 'exception' instead of '$exception'
        {
            // Handle the error
            var errorMessage = exception.Message;
            // Log or display the error as needed
        }
    }

    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Update SelectedOrder in ViewModel
    }

    private void ViewDetails_Click(object sender, RoutedEventArgs e)
    {
        OrderDetailsPanel.Visibility = Visibility.Visible;
        try
        {
            // Logic xử lý sự kiện
        }
        catch (Exception ex)  // Đảm bảo dùng tên 'ex' hoặc tên khác hợp lệ
        {
            // Xử lý lỗi tại đây
            Debug.WriteLine($"Error: {ex.Message}");
        }
    }

    private void DeleteOrder_Click(object sender, RoutedEventArgs e)
    {
        // Handle order deletion
        try
        {
            // Delete order logic
        }
        catch (Exception exception)
        {
            // Handle error
            var errorMessage = exception.Message;
        }
    }

    private void CloseDetails_Click(object sender, RoutedEventArgs e)
    {
        OrderDetailsPanel.Visibility = Visibility.Collapsed;
    }
}
