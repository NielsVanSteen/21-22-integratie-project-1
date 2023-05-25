using Domain.Comment;
using Domain.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.Comment;

/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="CommentTag"/> class.
/// </summary>
public class CommentTagEntityConfiguration : IEntityTypeConfiguration<CommentTag>
{
    public void Configure(EntityTypeBuilder<CommentTag> builder)
    {
        // One-To-Many relation between: CommentTag - ProjectTag.
        builder
            .HasOne(c => c.ProjectTag)
            .WithMany()
            .HasForeignKey(c => c.ProjectTagId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);

        // One-To-Many relation between: CommentTag - Comment.
        builder
            .HasOne(c => c.ReactionGroup)
            .WithMany(g => g.CommentTags)
            .HasForeignKey(c => c.ReactionGroupId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);

        // One-To-Many relation between: CommentTag - User.
        builder
            .HasOne(c => c.PlacedByUser)
            .WithMany()
            .HasForeignKey(c => c.PlacedByUserId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        // Primary key.
        builder.HasKey(t => t.CommentTagId);

        // Unique index. -> a TAG can only be placed once per COMMENT.
        builder.HasIndex(nameof(CommentTag.ProjectTagId), nameof(CommentTag.ReactionGroupId)).IsUnique(true);

    } // Configure.
}