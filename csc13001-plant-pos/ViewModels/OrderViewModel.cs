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
        Source.Clear();
        var data = await _orderService.GetOrders();
        if (data != null)
        {

            foreach (var item in data)
            {
                Source.Add(item);
            }


            OrderDetailItem = await _orderService.GetOrderDetails(Source[0].OrderId);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
