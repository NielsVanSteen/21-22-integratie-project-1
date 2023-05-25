using Domain.ProjectStatistics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.ProjectStatistics;

/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="EmojiTypeTotal"/> class.
/// </summary>
public class EmojiTypeTotalEntityConfiguration : IEntityTypeConfiguration<EmojiTypeTotal>
{
    public void Configure(EntityTypeBuilder<EmojiTypeTotal> builder)
    {
        // One-to-Many relation between: EmojiTypeTotal - Emoji
        builder
            .HasOne(e => e.Emoji)
            .WithMany()
            .HasForeignKey(e => e.EmojiId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder
            .HasOne(e => e.ProjectStatistics)
            .WithMany(e => e.EmojiTypeAmount)
            .HasForeignKey(e => e.ProjectStatisticsId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);

        // Primary key.
        builder.HasKey(e => e.EmojiTypeTotalId);
    } // Configure.
}