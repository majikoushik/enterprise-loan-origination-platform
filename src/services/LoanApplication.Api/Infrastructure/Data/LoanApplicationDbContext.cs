using System;
using System.Reflection;
using LoanApplication.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanApplication.Api.Infrastructure.Data;

public class LoanApplicationDbContext : DbContext
{
    public LoanApplicationDbContext(DbContextOptions<LoanApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<LoanApplicationEntity> LoanApplications => Set<LoanApplicationEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
