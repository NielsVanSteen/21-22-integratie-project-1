using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectStyling = Domain.Project.ProjectStyling;

namespace DAL.FluentApi.Project;

/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="ProjectStyling"/> class.
/// </summary>
public class ProjectStylingEntityConfiguration : IEntityTypeConfiguration<ProjectStyling>
{
    public void Configure(EntityTypeBuilder<ProjectStyling> builder)
    {
        
        // One-to-Many relation between ProjectStyling and ThemeStyling.
        builder
            .HasOne(p => p.ThemeStyle)
            .WithMany()
            .HasForeignKey(p => p.ThemeStylesId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);
        
        // Primary key.
        builder.HasKey(p => p.ProjectStylingId);
    }
}