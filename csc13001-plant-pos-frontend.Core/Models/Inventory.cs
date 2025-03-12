using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csc13001_plant_pos_frontend.Core.Models;

[Table("inventory")]
public class Inventory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BatchId
    {
        get; set;
    }

    [Required]
    [MaxLength(255)]
    public string Supplier { get; set; } = string.Empty;

    [ForeignKey("product")]
    public int ProductId
    {
        get; set;
    }

    public int Quantity
    {
        get; set;
    }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PurchasePrice
    {
        get; set;
    }

    [Column(TypeName = "datetime")]
    public DateTime PurchaseDate { get; set; } = DateTime.Now;

    public Product Product
    {
        get; set;
    }
}
