using Eligibility.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eligibility.Api.Infrastructure.Data.Configurations;

public class EligibilityResultConfiguration : IEntityTypeConfiguration<EligibilityResult>
{
    public void Configure(EntityTypeBuilder<EligibilityResult> builder)
    {
        builder.ToTable("EligibilityResults");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.ApplicationId).IsRequired();
        builder.Property(e => e.CustomerId).IsRequired();
        
        builder.HasIndex(e => e.ApplicationId);

        builder.Property(e => e.Decision)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.RuleVersion)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.RequestedAmount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.DeclaredMonthlyIncome).HasColumnType("decimal(18,2)");
        builder.Property(e => e.ExistingEmiObligations).HasColumnType("decimal(18,2)");
        builder.Property(e => e.DebtToIncomeRatio).HasColumnType("decimal(18,2)");

        builder.HasMany(e => e.RuleResults)
            .WithOne()
            .HasForeignKey("EligibilityResultId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class RuleResultConfiguration : IEntityTypeConfiguration<RuleResult>
{
    public void Configure(EntityTypeBuilder<RuleResult> builder)
    {
        builder.ToTable("RuleResults");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.RuleCode).IsRequired().HasMaxLength(50);
        builder.Property(e => e.RuleName).IsRequired().HasMaxLength(200);
        builder.Property(e => e.ActualValue).IsRequired().HasMaxLength(200);
        builder.Property(e => e.ExpectedValue).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Explanation).IsRequired().HasMaxLength(500);
    }
}
