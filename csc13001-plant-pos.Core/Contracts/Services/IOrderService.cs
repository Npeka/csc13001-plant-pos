using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csc13001_plant_pos.Core.Models;
using csc13001_plant_pos.Core.DTOs;
using csc13001_plant_pos.Core.DTOs.Orders;

namespace csc13001_plant_pos.Core.Contracts.Services;
public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetOrders();
    Task<OrderDetailDto> GetOrderDetails(int orderId);
    Task GetOrder();
    Task AddOrder();
    Task UpdateOrder();
    Task DeleteOrder();
}
