using Customer.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customer.Api.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<CustomerProfile>
{
    public void Configure(EntityTypeBuilder<CustomerProfile> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(250);

        builder.HasIndex(c => c.Email)
            .IsUnique();

        builder.Property(c => c.MobileNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.EmploymentType)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.MonthlyIncome)
            .HasColumnType("decimal(18,2)");

        builder.Property(c => c.ExistingMonthlyObligations)
            .HasColumnType("decimal(18,2)");
    }
}
