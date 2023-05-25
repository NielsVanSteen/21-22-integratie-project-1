using Domain.Comment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.Comment;

/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="CommentHistory"/> class.
/// </summary>
public class CommentHistoryEntityConfiguration : IEntityTypeConfiguration<CommentHistory>
{
    public void Configure(EntityTypeBuilder<CommentHistory> builder)
    {

        // One-To-Many relation between: CommentHistory - User.
        builder
            .HasOne(h => h.EditedBy)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull)
            .HasForeignKey(h => h.EditedById)
            .IsRequired(false);

        // One-To-Many relation between: CommentHistory - Comment.
        builder
            .HasOne(h => h.ReactionGroup)
            .WithMany(c => c.CommentHistories)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(h => h.ReactionGroupId);

        // Primary key.
        builder.HasKey(h => h.CommentHistoryId);

    } // Configure.
}