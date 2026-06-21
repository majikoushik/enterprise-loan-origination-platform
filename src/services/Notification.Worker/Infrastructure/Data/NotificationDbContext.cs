using Microsoft.EntityFrameworkCore;
using Notification.Worker.Domain.Models;

namespace Notification.Worker.Infrastructure.Data;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
        : base(options)
    {
    }

    public DbSet<NotificationRequest> NotificationRequests => Set<NotificationRequest>();
    public DbSet<NotificationDeliveryAttempt> NotificationDeliveryAttempts => Set<NotificationDeliveryAttempt>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<NotificationRequest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CorrelationId).HasMaxLength(100);
            entity.Property(e => e.EventType).HasMaxLength(100).IsRequired();
            entity.Property(e => e.EntityType).HasMaxLength(100);
            entity.Property(e => e.Recipient).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Subject).HasMaxLength(255).IsRequired();
            entity.Property(e => e.MessageBody).IsRequired();
            entity.Property(e => e.RetryCount).IsRequired();

            entity.HasMany(e => e.DeliveryAttempts)
                  .WithOne()
                  .HasForeignKey(e => e.NotificationRequestId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.Navigation(e => e.DeliveryAttempts).UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<NotificationDeliveryAttempt>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ProviderResponse).HasMaxLength(1000);
            entity.Property(e => e.FailureReason).HasMaxLength(1000);
        });
    }
}
