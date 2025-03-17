using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csc13001_plant_pos.Core.Models;

[Table("staff")]
public class Staff
{
    [Key]
    [Column("staff_id")]
    public int StaffId
    {
        get; set;
    }

    [Column("user_id")]
    [ForeignKey("user_id")]
    public int UserId
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
}
