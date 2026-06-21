using System;
using System.Reflection;
using Customer.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Customer.Api.Infrastructure.Data;

public class CustomerDbContext : DbContext
{
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
        : base(options)
    {
    }

    public DbSet<CustomerProfile> Customers => Set<CustomerProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Seed synthetic data for demo purposes
        var seedId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var seedId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");

        // We use reflection or an anonymous type/bypass to seed since we have private setters
        // For EF core seeding, we can use the anonymous object approach.
        modelBuilder.Entity<CustomerProfile>().HasData(
            new 
            {
                Id = seedId1,
                FullName = "Alice Smith",
                Email = "alice.smith@example.com",
                MobileNumber = "+1234567890",
                DateOfBirth = new DateTime(1985, 5, 15, 0, 0, 0, DateTimeKind.Utc),
                EmploymentType = EmploymentType.Salaried,
                MonthlyIncome = 5000m,
                ExistingMonthlyObligations = 1000m,
                CreatedAt = DateTime.UtcNow
            },
            new 
            {
                Id = seedId2,
                FullName = "Bob Johnson",
                Email = "bob.johnson@example.com",
                MobileNumber = "+1987654321",
                DateOfBirth = new DateTime(1990, 8, 20, 0, 0, 0, DateTimeKind.Utc),
                EmploymentType = EmploymentType.SelfEmployed,
                MonthlyIncome = 8000m,
                ExistingMonthlyObligations = 2000m,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}
