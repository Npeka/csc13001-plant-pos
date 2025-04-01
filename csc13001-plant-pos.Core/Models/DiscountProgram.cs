using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csc13001_plant_pos.Core.Models;

[Table("discount_programs")]
public class DiscountProgram
{
    [Key]
    [Column("discount_id")]
    public int DiscoundId
    {
        get; set;
    }

    [Column("name")]
    public string Name
    {
        get; set;
    }

    [Column("start_date")]
    public DateTime StartDate
    {
        get; set;
    }

    [Column("end_date")]
    public DateTime EndDate
    {
        get; set;
    }

    [Column("applicable_customer_type")]
    public string ApplicableCustomerType
    {
        get; set;
    }
}
