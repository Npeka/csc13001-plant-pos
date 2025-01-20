using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using csc13001_plant_pos.Data.Contexts;
using csc13001_plant_pos.Core.Models;

namespace csc13001_plant_pos.Services;
public class AuthenticationService
{
    private readonly ApplicationDbContext _context;

    public AuthenticationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RegisterAsync(string username, string password)
    {
        if (await _context.Users.AnyAsync(u => u.Username == username))
        {
            return false;
        }

        var user = new User
        {
            Username = username,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            Email = "admin@plantstore.com",
            IsAdmin = false,
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<User?> LoginAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return user;
        }

        return null;
    }
}
