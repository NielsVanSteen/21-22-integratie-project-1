using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeLine = Domain.Project.TimeLine;

namespace DAL.FluentApi.Project;

/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="Project"/> class.
/// </summary>
public class ProjectEntityConfiguration : IEntityTypeConfiguration<Domain.Project.Project>
{
    public void Configure(EntityTypeBuilder<Domain.Project.Project> builder)
    {
        // On-To-One relation between: Project - ProjectStyling.
        builder
            .HasOne(p => p.ProjectStyling)
            .WithOne()
            .HasForeignKey<Domain.Project.Project>(p => p.ProjectStylingId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        // Make External project name the primary key.
        builder.HasKey(p => p.ProjectId);

        // Both the internal and external project name should be unique.
        builder.HasIndex(p => p.InternalName).IsUnique();
        builder.HasIndex(p => p.ExternalName).IsUnique();
    } // Configure.
}