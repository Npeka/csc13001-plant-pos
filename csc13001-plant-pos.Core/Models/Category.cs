using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csc13001_plant_pos.Core.Models;

[Table("Categories")]
public class Category
{
    [Key]
    [Column("category_id")]
    public int CategoryId
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

    //// Relationship: Một Category có nhiều Products
    //public ICollection<Product> Products { get; set; } = new List<Product>();
}
