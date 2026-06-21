using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Customer.Api.Infrastructure.Data;

public sealed class CustomerDbContextFactory : IDesignTimeDbContextFactory<CustomerDbContext>
{
    public CustomerDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<CustomerDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LoanOrigination_CustomerDb;Trusted_Connection=True;MultipleActiveResultSets=true")
            .Options;

        return new CustomerDbContext(options);
    }
}
