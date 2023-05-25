using Domain.DocReview;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.DocReview;

/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="Survey"/> class.
/// </summary>
public class SurveyEntityConfiguration : IEntityTypeConfiguration<Survey>
{
    public void Configure(EntityTypeBuilder<Survey> builder)
    {
        // One-To-Many Relation between: Survey - DocReview.
        builder
            .HasOne(s => s.DocReview)
            .WithMany(d => d.Surveys)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(s => s.DocReviewId)
            .IsRequired(true);

        // Primary key.
        builder.HasKey(s => s.SurveyId);

    } // Configure.
}