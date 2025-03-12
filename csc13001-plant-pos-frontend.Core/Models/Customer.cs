using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csc13001_plant_pos_frontend.Core.Models;

[Table("customers")]
public class Customer
{
    [Key]
    [Column("customer_id")]
    public int CustomerId
    {
        get; set;
    }

    [Column("name")]
    [StringLength(256)]
    public string Name
    {
        get; set;
    }

    [Column("phone")]
    [StringLength(20)]
    public string Phone
    {
        get; set;
    }

    [Column("email")]
    [StringLength(256)]
    public string Email
    {
        get; set;
    }

    [Column("address")]
    public string Address
    {
        get; set;
    }

    [Column("loyalty_points")]
    public int LoyaltyPoints
    {
        get; set;
    }

    [Column("loyalty_card_type")]
    public string LoyaltyCardType
    {
        get; set;
    }
}
