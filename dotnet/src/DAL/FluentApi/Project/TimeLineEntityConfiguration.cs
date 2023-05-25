using Domain.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.Project;

/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="TimeLine"/> class.
/// </summary>
public class TimeLineEntityConfiguration : IEntityTypeConfiguration<TimeLine>
{
    public void Configure(EntityTypeBuilder<TimeLine> builder)
    {
        // One-to-mane relation between: Project - TimeLine.
        builder
            .HasOne(t => t.Project)
            .WithOne(p => p.TimeLine)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey<TimeLine>(t => t.ProjectId)
            .IsRequired(true);

        builder.HasKey(t => t.TimeLineId);

    } // Configure.
}