using Domain.DocReview;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.DocReview;

/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="DocReviewHistory"/> class.
/// </summary>
public class DocReviewHistoryEntityConfiguration : IEntityTypeConfiguration<DocReviewHistory>
{
    public void Configure(EntityTypeBuilder<DocReviewHistory> builder)
    {
        // One-To-Many relation between: DocReviewHistory - User.
        builder
            .HasOne(h => h.Editor)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull)
            .HasForeignKey(h => h.EditorId)
            .IsRequired(false);
        
        // One-To-Many relation between: DocReviewHistory - DocReview.
        builder
            .HasOne(h => h.DocReview)
            .WithMany(d => d.DocReviewHistories)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(h => h.DocReviewId)
            .IsRequired(true);

        // Primary key.
        builder.HasKey(h => h.DocReviewHistoryId);

    } // Configure.
}