using Domain.DocReview;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.DocReview;

/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="SurveyOption"/> class.
/// </summary>
public class SurveyOptionEntityConfiguration : IEntityTypeConfiguration<SurveyOption>
{
    public void Configure(EntityTypeBuilder<SurveyOption> builder)
    {
        // One-To-Many relation between: SurveyOptions - Survey.
        builder
            .HasOne(o => o.Survey)
            .WithMany(s => s.SurveyOptions)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(o => o.SurveyId)
            .IsRequired(true);

        // Primary key.
        builder.HasKey(o => o.SurveyOptionId);

    } // Configure.
}