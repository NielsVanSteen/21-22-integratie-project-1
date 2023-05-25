using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.Comment;

/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="Comment"/> class.
/// </summary>
public class ReactionGroupEntityConfiguration : IEntityTypeConfiguration<Domain.Comment.ReactionGroup>
{
    public void Configure(EntityTypeBuilder<Domain.Comment.ReactionGroup> builder)
    { } // Configure.
}