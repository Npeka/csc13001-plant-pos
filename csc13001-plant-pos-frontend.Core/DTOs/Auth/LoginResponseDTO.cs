using csc13001_plant_pos_frontend.Core.Models;

namespace csc13001_plant_pos_frontend.Core.DTOs.Auth;

public class LoginResponseDTO
{
    public UserDTO User
    {
        get; set;
    }
    public string AccessToken
    {
        get; set;
    }
}

public class UserDTO
{
    public int UserId
    {
        get; set;
    }
    public string Username
    {
        get; set;
    }
    public string Role
    {
        get; set;
    } // API trả về "role"
    public bool Admin
    {
        get; set;
    } // API trả về "admin"
}

