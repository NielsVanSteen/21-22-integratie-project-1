using Domain.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.Project;

/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="ThemeStyles"/> class.
/// </summary>
public class ThemeStylesEntityConfiguration : IEntityTypeConfiguration<ThemeStyles>
{
    public void Configure(EntityTypeBuilder<ThemeStyles> builder)
    {
        // Primary key.
        builder.HasKey(t => t.ThemeStylesId);
        
        // One-To-Many relation between: ThemeStyle - Project.
        builder
            .HasOne(t => t.Project)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(t => t.ProjectId)
            .IsRequired(false);
    }
}