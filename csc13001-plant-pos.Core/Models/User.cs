using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace csc13001_plant_pos.Core.Models;

[Table("users")]
public class User
{
    [Key]
    [Column("user_id")]
    public int UserId
    {
        get; set;
    }

    [Column("username")]
    [StringLength(256)]
    [NotNull]
    public string Username
    {
        get; set;
    }

    [Column("email")]
    [StringLength(256)]
    public string Email
    {
        get; set;
    }

    [Column("password")]
    [StringLength(256)]
    [NotNull]
    public string Password
    {
        get; set;
    }

    [Column("is_admin")]
    [NotNull]
    public bool IsAdmin
    {
        get; set;
    }

    public User()
    {

    }

    public User(string username, string password)
    {
        Username = username;
        Password = password;
        IsAdmin = false;
    }

    public User(int userId, string username, string email, string password, bool isAdmin)
    {
        UserId = userId;
        Username = username;
        Email = email;
        Password = password;
        IsAdmin = isAdmin;
    }
}
