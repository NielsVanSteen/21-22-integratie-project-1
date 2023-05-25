using Domain.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.Project;

/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="TimeLinePhase"/> class.
/// </summary>
public class TimeLinePhaseEntityConfiguration : IEntityTypeConfiguration<TimeLinePhase>
{
    public void Configure(EntityTypeBuilder<TimeLinePhase> builder)
    {
        // One-To-One relation between: DocReview - TimeLinePhase.
        builder
            .HasOne(p => p.DocReview)
            .WithMany(d => d.TimeLinePhases)
            .OnDelete(DeleteBehavior.SetNull)
            .HasForeignKey(p => p.DocReviewId)
            .IsRequired(false);

        builder
            .HasOne(p => p.TimeLine)
            .WithMany(t => t.TimeLinePhases)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(p => p.TimeLineId)
            .IsRequired(false);

        // Set primary key.
        builder.HasKey(p => p.TimeLinePhaseId);
    } // Configure.
}