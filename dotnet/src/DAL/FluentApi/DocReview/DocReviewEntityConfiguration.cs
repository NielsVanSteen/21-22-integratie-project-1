using Domain.DocReview;
using Domain.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.DocReview;

/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="DocReview"/> class.
/// </summary>
public class DocReviewEntityConfiguration : IEntityTypeConfiguration<Domain.DocReview.DocReview>
{
    public void Configure(EntityTypeBuilder<Domain.DocReview.DocReview> builder)
    {
        // One-To-Many relation between: DocReview - Project.
        builder
            .HasOne(d => d.Project)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(d => d.ProjectId)
            .IsRequired(true);

        // One-To-Many relation between: DocReview - User.
        builder
            .HasOne(d => d.WrittenBy)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull)
            .HasForeignKey(d => d.WrittenById)
            .IsRequired(false);

        // Owned relation between: DocReview - DocReviewSettings.
        builder.OwnsOne(d => d.DocReviewSettings);

        // Primary key.
        builder.HasKey(d => d.DocReviewId);
    } // Configure.
}