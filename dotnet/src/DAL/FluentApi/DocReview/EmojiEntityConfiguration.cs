using Domain.DocReview;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.DocReview;

/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="Emoji"/> class.
/// </summary>
public class EmojiEntityConfiguration : IEntityTypeConfiguration<Emoji>
{
    public void Configure(EntityTypeBuilder<Emoji> builder)
    {
        // One-To-Many relation between: Emoji - DocReview.
        builder
            .HasOne(e => e.DocReview)
            .WithMany(d => d.AvailableEmoji)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(e => e.DocReviewId)
            .IsRequired(false);

        // Composite primary key. -> emoji is unique per docReview.
        builder.HasIndex(nameof(Emoji.Code), nameof(Emoji.DocReviewId));

        builder.HasKey(e => e.EmojiId);
    } // Configure.
}