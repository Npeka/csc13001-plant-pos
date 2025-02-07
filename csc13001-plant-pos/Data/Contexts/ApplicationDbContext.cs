using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;

using csc13001_plant_pos.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace csc13001_plant_pos.Data.Contexts;
public class ApplicationDbContext : DbContext
{
    private readonly ILogger<ApplicationDbContext> _logger;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ILogger<ApplicationDbContext> logger)
        : base(options)
    {
        _logger = logger;

        try
        {
            if (!this.Database.EnsureCreated())
            {
            }
            this.Database.Migrate();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating the database.");
        }
    }

    public DbSet<User> Users
    {
        get; set;
    }

    public DbSet<Order> Orders
    {
        get; set;
    }

    public DbSet<OrderDetail> OrderDetails
    {
        get; set;
    }
    public DbSet<Product> Products
    {
        get; set;
    }

    public DbSet<Category> Categories
    {
        get; set;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.LogTo(log => _logger.LogInformation(log), LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
