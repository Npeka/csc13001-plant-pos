using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace csc13001_plant_pos.Core.Models;

[Table("Users")]
public class User
{
    [Key]
    public int UserId
    {
        get; set;
    }

    [StringLength(256)]
    public string Username
    {
        get; set;
    }

    [StringLength(256)]
    public string Email
    {
        get; set;
    }

    [StringLength(256)]
    [NotNull]
    public string Password
    {
        get; set;
    }

    [NotNull]
    public bool IsAdmin
    {
        get; set;
    }

    public User()
    {

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
