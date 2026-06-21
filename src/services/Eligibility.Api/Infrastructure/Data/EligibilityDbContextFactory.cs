using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Eligibility.Api.Infrastructure.Data;

public sealed class EligibilityDbContextFactory : IDesignTimeDbContextFactory<EligibilityDbContext>
{
    public EligibilityDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<EligibilityDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LoanOrigination_EligibilityDb;Trusted_Connection=True;MultipleActiveResultSets=true")
            .Options;

        return new EligibilityDbContext(options);
    }
}
