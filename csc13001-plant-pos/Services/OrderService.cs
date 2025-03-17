using Microsoft.EntityFrameworkCore;
using csc13001_plant_pos.Core.Models;
using csc13001_plant_pos.Data.Contexts;
using csc13001_plant_pos.Core.DTOs.Orders;
using csc13001_plant_pos.Core.Contracts.Services;
using System.Diagnostics;

namespace csc13001_plant_pos.Services;
public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrderDto>> GetOrders()
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Staff)
            .Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                CustomerName = o.Customer.Name,
                StaffName = o.Staff.Name,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                Status = o.Status.ToString(), // Convert OrderStatus to string
            })
            .ToListAsync();
    }

    public async Task<OrderDetailDto> GetOrderDetails(int orderId)
    {
        var orderDetailItemDtos = await _context.OrderDetails
            .Include(od => od.Order)
            .Include(od => od.Product)
            .Where(od => od.OrderId == 2)
            .Select(od => new OrderDetailItemDto
            {
                //OrderId = od.OrderId,
                //OrderDetailId = od.OrderDetailId,
                //ProductName = od.Product.Name,
                //UnitPrice = od.UnitPrice,
                //Quantity = od.Quantity,
                //TotalPrice = od.Quantity * od.UnitPrice,
            })
            .ToListAsync();

        Debug.Print("OrderService.GetOrderDetails...........................");

        return new OrderDetailDto
        {
            OrderId = orderId,
            TotalPrice = orderDetailItemDtos.Sum(od => od.TotalPrice),
            PaymentMethod = "Cash",
            //OrderDetailItemDtos = orderDetailItemDtos
        };
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

