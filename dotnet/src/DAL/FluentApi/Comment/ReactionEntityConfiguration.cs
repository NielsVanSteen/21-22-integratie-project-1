using Domain.Comment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.Comment;

/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="Reaction"/> class.
/// </summary>
public class ReactionEntityConfiguration : IEntityTypeConfiguration<Reaction>
{
    public void Configure(EntityTypeBuilder<Reaction> builder)
    {
        // One-To-Many relation between: Comment - User.
        builder
            .HasOne(c => c.User)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull)
            .HasForeignKey(c => c.UserId)
            .IsRequired(false);

        // One-To-Many relation between: EmojiReaction - Comment.

        // One-To-Many relation between: Comment - DocReview.
        builder
            .HasOne(c => c.DocReview)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(c => c.DocReviewId)
            .IsRequired(true);

        // One-To-Many relation between: CommentComposite - Comment.
        builder
            .HasOne(c => c.PlacedOnReactionGroup)
            .WithMany(r => r.Reactions)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(c => c.PlacedOnReactionGroupId)
            .IsRequired(false);

        // Primary key.
        builder.HasKey(c => c.CommentId);
    } // Configure.
}