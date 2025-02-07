using csc13001_plant_pos.Core.Models;

namespace csc13001_plant_pos.Core.DTOs.Orders;
public class OrderDto
{
    public int OrderId
    {
        get; set;
    }
    public string CustomerName
    {
        get; set;
    }
    public string StaffName
    {
        get; set;
    }
    public DateTime OrderDate
    {
        get; set;
    }
    public decimal TotalPrice
    {
        get; set;
    }
    public string Status
    {
        get; set;
    }
}