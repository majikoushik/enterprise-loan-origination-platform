using Audit.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Audit.Api.Infrastructure.Data;

public class AuditDbContext : DbContext
{
    public AuditDbContext(DbContextOptions<AuditDbContext> options)
        : base(options)
    {
    }

    public DbSet<AuditEvent> AuditEvents => Set<AuditEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AuditEvent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CorrelationId).HasMaxLength(100);
            entity.Property(e => e.EventType).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Category).HasMaxLength(50).IsRequired();
            entity.Property(e => e.EntityType).HasMaxLength(100);
            entity.Property(e => e.EntityId).HasMaxLength(100);
            entity.Property(e => e.ActorType).HasMaxLength(50);
            entity.Property(e => e.ActorId).HasMaxLength(100);
            entity.Property(e => e.Action).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Summary).HasMaxLength(500).IsRequired();
            entity.Property(e => e.SourceService).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Severity).HasMaxLength(50).IsRequired();
            
            // Index for faster queries
            entity.HasIndex(e => e.EntityId);
            entity.HasIndex(e => e.CorrelationId);
            entity.HasIndex(e => e.OccurredAtUtc);
            entity.HasIndex(e => e.CustomerId);
        });
    }
}
