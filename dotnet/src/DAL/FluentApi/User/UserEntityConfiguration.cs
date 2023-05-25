using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.User;

/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="User"/> class.
/// </summary>
public class UserEntityConfiguration : IEntityTypeConfiguration<Domain.User.User>
{
    public void Configure(EntityTypeBuilder<Domain.User.User> builder)
    {
        // Many-To-One relation between: User - UserPropertyValues
        builder
            .HasMany(u => u.UserPropertyValues)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);
    
        // Many-To-Many relation between: User - Project
        builder
            .HasMany(u => u.RegisteredForProjects)
            .WithMany(p => p.Users);

    } // Configure.
}