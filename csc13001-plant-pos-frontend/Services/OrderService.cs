using csc13001_plant_pos_frontend.Core.Models;
using csc13001_plant_pos_frontend.Core.DTOs.Orders;
using csc13001_plant_pos_frontend.Core.Contracts.Services;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace csc13001_plant_pos_frontend.Services;
public class OrderService : IOrderService
{

    public OrderService()
    {
    }

    public async Task<IEnumerable<OrderDto>> GetOrders()
    {
        return null;
    }

    public async Task<OrderDetailDto> GetOrderDetails(int orderId)
    {
        return null;

    }

    public async Task GetOrder()
    {
    }

    public async Task AddOrder()
    {

    }

    public async Task UpdateOrder()
    {
    }

    public async Task DeleteOrder()
    {
    }
}

