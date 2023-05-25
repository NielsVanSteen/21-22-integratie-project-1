using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.User;

///<author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="UserPropertyName"/> class.
/// </summary>
public class UserPropertyNameEntityConfiguration : IEntityTypeConfiguration<UserPropertyName>
{
    public void Configure(EntityTypeBuilder<UserPropertyName> builder)
    {
        // One-To-Many relation: UserPropertyName - Project.
        builder
            .HasOne(u => u.RegisteredForProject)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(u => u.RegisteredForProjectId)
            .IsRequired(true);
        
        // Unique index for UserPropertyName -> a property can only exist once PER project.
        builder.HasIndex(nameof(UserPropertyName.UserPropertyLabel), nameof(UserPropertyName.RegisteredForProjectId)).IsUnique(true);
        
        // Primary key.
        builder.HasKey(p => p.UserPropertyNameId);

    } // Configure.
}