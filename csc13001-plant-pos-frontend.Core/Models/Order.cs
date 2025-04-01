using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csc13001_plant_pos_frontend.Core.Models;

[Table("orders")]
public class Order
{
    [Key]
    [Column("order_id")]
    public int OrderId
    {
        get; set;
    }

    [Column("customer_id")]
    [ForeignKey("Customer")]
    public int CustomerId
    {
        get; set;
    }
    public Customer Customer
    {
        get; set;
    }

    [Column("staff_id")]
    [ForeignKey("Staff")]
    public int StaffId
    {
        get; set;
    }
    public Staff Staff
    {
        get; set;
    }

    [Column("order_date")]
    [DataType(DataType.DateTime)]
    public DateTime OrderDate
    {
        get; set;
    }

    [Column("total_price")]
    public decimal TotalPrice
    {
        get; set;
    }

    [Column("status")]
    public string Status
    {
        get => _status.ToString();
        set => _status = Enum.Parse<OrderStatus>(value);
    }
    private OrderStatus _status;
}

public enum OrderStatus
{
    Pending = 0,
    Completed = 1,
    Canceled = 2
}

