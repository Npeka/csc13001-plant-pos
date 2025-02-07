using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csc13001_plant_pos.Core.Models;

[Table("order_details")]
public class OrderDetail
{
    [Key]
    [Column("order_detail_id")]
    public int OrderDetailId
    {
        get; set;
    }

    [Column("order_id")]
    [ForeignKey("Order")]
    public int OrderId
    {
        get; set;
    }
    public Order Order
    {
        get; set;
    }


    [Column("product_id")]
    [ForeignKey("Product")]
    public int ProductId
    {
        get; set;
    }
    public Product Product
    {
        get; set;
    }

    [Column("quantity")]
    public int Quantity
    {
        get; set;
    }

    [Column("unit_price", TypeName = "decimal(10,2)")]
    public decimal UnitPrice
    {
        get; set;
    }

    [Column("discount_id")]
    [ForeignKey("Discount")]
    public int? DiscountId
    {
        get; set;
    }
    public DiscountProgram? Discount
    {
        get; set;
    }
}
