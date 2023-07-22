using AZURE_EXAMPLE.Models;
using Microsoft.EntityFrameworkCore;

namespace AZURE_EXAMPLE;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Employee> Employees { get; set; }

}