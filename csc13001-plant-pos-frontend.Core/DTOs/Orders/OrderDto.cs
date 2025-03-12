using csc13001_plant_pos_frontend.Core.Models;

namespace csc13001_plant_pos_frontend.Core.DTOs.Orders;
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