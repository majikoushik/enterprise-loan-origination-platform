using System.Reflection;
using Eligibility.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Eligibility.Api.Infrastructure.Data;

public class EligibilityDbContext : DbContext
{
    public EligibilityDbContext(DbContextOptions<EligibilityDbContext> options)
        : base(options)
    {
    }

    public DbSet<EligibilityResult> EligibilityResults => Set<EligibilityResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
