using Domain.Comment;
using Domain.ProjectStatistics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.ProjectStatistics;

/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="CommentStatusTotal"/> class.
/// </summary>
public class CommentStatusTotalEntityConfiguration : IEntityTypeConfiguration<CommentStatusTotal>
{
    public void Configure(EntityTypeBuilder<CommentStatusTotal> builder)
    {
         builder
            .HasOne(e => e.ProjectStatistics)
            .WithMany(e => e.CommentStatusTypeAmount)
            .HasForeignKey(e => e.ProjectStatisticsId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);
        
        // Primary key.
        builder.HasKey(e => e.CommentStatusTotalId);
    } // Configure.
}