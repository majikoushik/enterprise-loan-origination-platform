using LoanApplication.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanApplication.Api.Infrastructure.Data.Configurations;

public class LoanApplicationConfiguration : IEntityTypeConfiguration<LoanApplicationEntity>
{
    public void Configure(EntityTypeBuilder<LoanApplicationEntity> builder)
    {
        builder.ToTable("LoanApplications");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.CustomerId)
            .IsRequired();

        builder.HasIndex(e => e.CustomerId);

        builder.Property(e => e.LoanType)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.RequestedAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.DeclaredMonthlyIncome)
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.ExistingEmiObligations)
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.Purpose)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(50);
    }
}
