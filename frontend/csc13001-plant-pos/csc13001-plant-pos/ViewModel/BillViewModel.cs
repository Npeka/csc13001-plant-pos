using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.Service;

namespace csc13001_plant_pos.ViewModel
{
    public partial class BillViewModel : ObservableObject
    {
        [ObservableProperty]
        private int orderId;

        [ObservableProperty]
        private string staffName;

        [ObservableProperty]
        private string customerName;

        [ObservableProperty]
        private DateTime orderDate;

        [ObservableProperty]
        private double discountRate;

        [ObservableProperty]
        private decimal totalPrice;

        [ObservableProperty]
        private decimal finalPrice;

        [ObservableProperty]
        private ObservableCollection<OrderItem> orderItems = new ObservableCollection<OrderItem>();

        public decimal DiscountAmount => TotalPrice * (decimal)(DiscountRate / 100.0);
        public int TotalItem => OrderItems.Sum(item => item.Quantity);

        private readonly IOrderService _orderService;

        public BillViewModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task LoadOrderAsync(string orderId)
        {
            var response = await _orderService.GetOrderByIdAsync(orderId);
            if (response?.Status == "success" && response.Data != null)
            {
                var order = response.Data;
                OrderId = order.OrderId;
                StaffName = order.Staff?.Fullname ?? "N/A";
                CustomerName = order.Customer?.Name ?? "N/A";
                OrderDate = order.OrderDate;
                DiscountRate = order.DiscountProgram?.DiscountRate ?? 0;
                TotalPrice = order.TotalPrice;
                FinalPrice = order.FinalPrice;

                OrderItems.Clear();
                if (order.OrderItems != null)
                {
                    foreach (var item in order.OrderItems)
                    {
                        OrderItems.Add(item);
                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load order {orderId}: {response?.Message}");
            }
            OnPropertyChanged(nameof(TotalItem));
        }
    }
}