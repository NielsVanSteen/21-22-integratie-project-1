using Domain.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.Project;

///<author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="ProjectTag"/> class.
/// </summary>
public class ProjectTagEntityConfiguration : IEntityTypeConfiguration<ProjectTag>
{
    public void Configure(EntityTypeBuilder<ProjectTag> builder)
    {
        // One-To-Many relation between: ProjectTag - Project.
        builder
            .HasOne(p => p.Project)
            .WithMany(p => p.ProjectTags)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(p => p.ProjectId)
            .IsRequired(true);

        // Primary key.
        builder.HasKey(t => t.ProjectTagId);

        // Unique index.
        builder.HasIndex(nameof(ProjectTag.ProjectId), nameof(ProjectTag.Name)).IsUnique(true);
    } // Configure.
}