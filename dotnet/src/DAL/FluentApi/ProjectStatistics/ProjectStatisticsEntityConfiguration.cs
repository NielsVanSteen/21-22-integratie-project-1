using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.ProjectStatistics;

/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="ProjectStatistics"/> class.
/// </summary>
public class ProjectStatisticsEntityConfiguration : IEntityTypeConfiguration<Domain.ProjectStatistics.ProjectStatistics>
{
    public void Configure(EntityTypeBuilder<Domain.ProjectStatistics.ProjectStatistics> builder)
    {
        // One-to-Many relation between: ProjectStatistics - Project
        builder
            .HasOne(e => e.Project)
            .WithMany()
            .HasForeignKey(e => e.ProjectId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);

        // Primary key.
        builder.HasKey(e => e.ProjectStatisticsId);
    } // Configure.
}