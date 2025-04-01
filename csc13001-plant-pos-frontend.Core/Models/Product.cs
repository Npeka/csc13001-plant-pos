using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csc13001_plant_pos_frontend.Core.Models;

[Table("products")]
public class Product
{
    [Key]
    [Column("product_id")]
    public int ProductId
    {
        get; set;
    }

    [Column("category_id")]
    [ForeignKey("Category")]
    public int CategoryId
    {
        get; set;
    }
    public Category Category
    {
        get; set;
    }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string? Description
    {
        get; set;
    }

    [Column("price", TypeName = "decimal(18,2)")]
    public decimal Price
    {
        get; set;
    }

    [Column("stock")]
    public int Stock
    {
        get; set;
    }

    [Column("care_level")]
    [Range(1, 5)]
    public int CareLevel
    {
        get; set;
    }

    [Column("environment_type")]
    public string EnvironmentType { get; set; } = string.Empty;

    [Column("size")]
    public string Size { get; set; } = string.Empty;

    [Column("light_requirement")]
    public int LightRequirement
    {
        get; set;
    }

    [Column("watering_schedule")]
    public int WateringSchedule
    {
        get; set;
    }
}
