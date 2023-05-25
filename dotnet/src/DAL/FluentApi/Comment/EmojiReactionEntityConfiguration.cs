using Domain.Comment;
using Domain.DocReview;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.Comment;

/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="ReactionGroup"/> class.
/// </summary>
public class EmojiReactionEntityConfiguration : IEntityTypeConfiguration<EmojiReaction>
{
    public void Configure(EntityTypeBuilder<EmojiReaction> builder)
    {
        // One-To-Many relation between: EmojiReaction - Emoji.
        builder
            .HasOne(r => r.Emoji)
            .WithMany()
            .HasForeignKey(e => e.EmojiId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
    } // Configure.
}