using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.User;

///<author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="MarkedEmail"/> class.
/// </summary>
public class MarkedEmailEntityConfiguration : IEntityTypeConfiguration<MarkedEmail>
{
    public void Configure(EntityTypeBuilder<MarkedEmail> builder)
    {
        // One-To-Many relation. MarkedForEmail - Project.
        builder
            .HasMany(e => e.Projects)
            .WithMany(p => p.MarkedEmails)
            .UsingEntity(j => j.ToTable("MarkedEmailProject"));

        // Unique index on email.
        builder.HasIndex(m => m.Email).IsUnique(true);
        
        // Primary key.
        builder.HasKey(m => m.MarkedEmailId);

    } // Configure.
}