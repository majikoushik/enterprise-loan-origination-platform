using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Audit.Api.Infrastructure.Data;

public sealed class AuditDbContextFactory : IDesignTimeDbContextFactory<AuditDbContext>
{
    public AuditDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AuditDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LoanOrigination_AuditDb;Trusted_Connection=True;MultipleActiveResultSets=true")
            .Options;

        return new AuditDbContext(options);
    }
}
