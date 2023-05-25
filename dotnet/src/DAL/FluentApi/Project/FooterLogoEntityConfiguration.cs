using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FooterLogo = Domain.Project.FooterLogo;

namespace DAL.FluentApi.Project;

///<author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="Project"/> class.
/// </summary>
public class FooterLogoEntityConfiguration : IEntityTypeConfiguration<FooterLogo>
{
    public void Configure(EntityTypeBuilder<FooterLogo> builder)
    {
        // Primary key
        builder
            .HasKey(l => l.FooterLogoId);

        // One-to-Many relation 1-x. ProjectFooterLogo - Project.
        builder
            .HasOne(l => l.Project)
            .WithMany(p => p.FooterLogos)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(l => l.ProjectId)
            .IsRequired(true);

        // Composite index on 
        builder
            .HasIndex(nameof(FooterLogo.ImageName), nameof(FooterLogo.ProjectId))
            .IsUnique();
    } // Configure.
}