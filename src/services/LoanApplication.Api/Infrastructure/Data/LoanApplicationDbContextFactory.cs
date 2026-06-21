using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LoanApplication.Api.Infrastructure.Data;

public sealed class LoanApplicationDbContextFactory : IDesignTimeDbContextFactory<LoanApplicationDbContext>
{
    public LoanApplicationDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<LoanApplicationDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LoanOrigination_LoanApplicationDb;Trusted_Connection=True;MultipleActiveResultSets=true")
            .Options;

        return new LoanApplicationDbContext(options);
    }
}
