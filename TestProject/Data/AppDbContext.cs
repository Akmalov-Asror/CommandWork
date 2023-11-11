using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
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
        this.SeedRoles(builder);
        this.SeedUsers(builder);
        this.SeedUserRoles(builder);

        //builder.ApplyConfiguration<IdentityRole>(new RoleConfiguration(Services));
        builder.Entity<Product>()
       .HasData(
           new Product { Id = 1, Title = "HDD 1TB", Quantity = 55, Price = 74.09M },
           new Product { Id = 2, Title = "HDD SDD 512 GB", Quantity = 102, Price = 190.99M},
           new Product { Id = 3, Title = "RAM DDR4 16GB",   Quantity =  47, Price = 80.32M}
       );
    }

    private void SeedUsers(ModelBuilder builder)
    {
        User user = new User()
        {
            Id = "b74ddd14-6340-4840-95c2-db12554843e5",
            UserName = "Admin",
            Email = "admin@gmail.com",
            NormalizedEmail = "ADMIN@GMAIL.COM",
            LockoutEnabled = false,
            PhoneNumber = "1234567890",
            PasswordHash = "Admin*123",
            
        };
    
        PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
        user.PasswordHash = passwordHasher.HashPassword(user, "Admin*123");  // Assign the hashed password

        builder.Entity<User>().HasData(user);
    }
    private void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData(
         new IdentityRole { Id = "33ff05ee-c91f-4590-bf6d-8a3a6678caa2", Name = "ADMIN", NormalizedName = "ADMIN" },
        new IdentityRole { Id = "878775ae-a821-41c3-bc97-4f7c91715150", Name = "USER", NormalizedName = "USER" }
    
        );
    }


    private void SeedUserRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>() { RoleId = "33ff05ee-c91f-4590-bf6d-8a3a6678caa2", UserId = "b74ddd14-6340-4840-95c2-db12554843e5" }
        );
    }

}