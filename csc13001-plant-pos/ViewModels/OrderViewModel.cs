using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using csc13001_plant_pos.Contracts.ViewModels;
using csc13001_plant_pos.Core.Contracts.Services;
using csc13001_plant_pos.Core.Models;
using csc13001_plant_pos.Services;
using csc13001_plant_pos.Core.DTOs.Orders;
using System.Diagnostics;

namespace csc13001_plant_pos.ViewModels;

public partial class OrderViewModel : ObservableRecipient, INavigationAware
{
    private readonly IOrderService _orderService;

    public ObservableCollection<OrderDto> Source { get; } = new ObservableCollection<OrderDto>();
    public OrderDetailDto OrderDetailItem { get; set; } = new();

    public OrderViewModel(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Debug.Print("OrderViewModel.OnNavigatedTo");
        Source.Clear();
        var data = await _orderService.GetOrders();
        if (data != null)
        {

            Debug.Print("_orderService.GetOrders...");
            foreach (var item in data)
            {
                Source.Add(item);
            }

            Debug.Print("_orderService.GetOrders...complete");
            Debug.Print("_orderService.GetOrderDetails...");

            OrderDetailItem = await _orderService.GetOrderDetails(Source[0].OrderId);
            Debug.Print("_orderService.GetOrderDetails...complete");

        }
    }

    public void OnNavigatedFrom()
    {
    }
}
