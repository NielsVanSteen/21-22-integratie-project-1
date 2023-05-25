using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.User;

///<author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="UserPropertyValue"/> class.
/// </summary>
public class UserPropertyValueEntityConfiguration : IEntityTypeConfiguration<UserPropertyValue>
{
    public void Configure(EntityTypeBuilder<UserPropertyValue> builder)
    {
        builder.HasKey(u => u.UserPropertyValueId);
        
        // One-To-Many relation. UserPropertyValue - User.
        builder
            .HasOne(u => u.User)
            .WithMany(u => u.UserPropertyValues)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(u => u.UserId)
            .IsRequired(true);

        // One-To-Many relation. UserPropertyValue - UserPropertyName.
        builder
            .HasOne(u => u.UserPropertyName)
            .WithMany()
            .HasForeignKey(nameof(UserPropertyName.UserPropertyLabel)) // Must reference the entire .HasKey of 'UserPropertyNameEntityConfiguration'. (because that class also has a composite key).
            .IsRequired(true);
        
    } // Configure.
}