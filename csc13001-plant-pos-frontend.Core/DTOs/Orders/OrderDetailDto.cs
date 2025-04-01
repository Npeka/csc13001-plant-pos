namespace csc13001_plant_pos_frontend.Core.DTOs.Orders;
public class OrderDetailDto
{
    public int OrderId
    {
        get; set;
    }

    public decimal TotalPrice
    {
        get; set;
    }

    public string PaymentMethod
    {
        get; set;
    }

    public IEnumerable<OrderDetailItemDto> OrderDetailItemDtos
    {
        get; set;
    }
}

public class OrderDetailItemDto
{
    public int OrderId
    {
        get; set;
    }

    public int OrderDetailId
    {
        get; set;
    }

    public string ProductName
    {
        get; set;
    }

    public decimal UnitPrice
    {
        get; set;
    }

    public int Quantity
    {
        get; set;
    }

    public decimal TotalPrice
    {
        get; set;
    }
}
