using Domain.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.Project;


/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="ProjectHistory"/> class.
/// </summary>
public class ProjectHistoryEntityConfiguration : IEntityTypeConfiguration<ProjectHistory>
{
    public void Configure(EntityTypeBuilder<ProjectHistory> builder)
    {
        // One-to-Many relation 1-x. ProjectHistory - Project.
        builder
            .HasOne(h => h.Project)
            .WithMany(p => p.ProjectHistories)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(h => h.ProjectId)
            .IsRequired(true);

        // One-to-Many relation between: ProjectHistory - User.
        builder
            .HasOne(h => h.EditedBy)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull)
            .HasForeignKey(h => h.EditedById)
            .IsRequired(false);
        
        // Primary key.
        builder.HasKey(h => h.ProjectHistoryId);
    } // Configure.
}