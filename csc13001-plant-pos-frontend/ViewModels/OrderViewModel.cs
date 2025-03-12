using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using csc13001_plant_pos_frontend.Contracts.ViewModels;
using csc13001_plant_pos_frontend.Core.Contracts.Services;
using csc13001_plant_pos_frontend.Core.Models;
using csc13001_plant_pos_frontend.Services;
using csc13001_plant_pos_frontend.Core.DTOs.Orders;
using System.Diagnostics;

namespace csc13001_plant_pos_frontend.ViewModels;

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
