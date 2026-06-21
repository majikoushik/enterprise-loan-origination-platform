using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Notification.Worker.Infrastructure.Data;

public sealed class NotificationDbContextFactory : IDesignTimeDbContextFactory<NotificationDbContext>
{
    public NotificationDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<NotificationDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LoanOrigination_NotificationDb;Trusted_Connection=True;MultipleActiveResultSets=true")
            .Options;

        return new NotificationDbContext(options);
    }
}
