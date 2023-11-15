using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestProject.Domains;

namespace TestProject.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options, IServiceProvider services) : base(options) => this.Services = services;
    public IServiceProvider Services { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<AuditLog> AuditLog { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration<IdentityRole>(new RoleConfiguration(Services));
        builder.Entity<Product>()
       .HasData(
           new Product { Id = 1, Title = "HDD 1TB", Quantity = 55, Price = 74.09123 },
           new Product { Id = 2, Title = "HDD SDD 512 GB", Quantity = 102, Price = 190.99},
           new Product { Id = 3, Title = "RAM DDR4 16GB",   Quantity =  47, Price = 80.32}
       );
    }
}