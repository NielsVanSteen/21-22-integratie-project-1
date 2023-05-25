using Domain.DocReview;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.FluentApi.DocReview;

/// <author>Niels Van Van Steen</author>
/// <summary>
/// This class will do all the Fluent API configuration for the <see cref="UserSurveyAnswer"/> class.
/// </summary>
public class UserSurveyAnswerEntityConfiguration : IEntityTypeConfiguration<UserSurveyAnswer>
{
    public void Configure(EntityTypeBuilder<UserSurveyAnswer> builder)
    {
        // One-To-Many relation between : UserSurveyAnswer - User
        builder
            .HasOne(u => u.User)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(u => u.UserId)
            .IsRequired(true);

        // One-To-Many relation between : UserSurveyAnswer - Survey
        builder
            .HasOne(u => u.Survey)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(u => u.SurveyId)
            .IsRequired(true);

        // One-To-Many relation between : UserSurveyAnswer - SurveyOption
        builder
            .HasOne(u => u.ChosenOption)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(u => u.ChosenOptionId)
            .IsRequired(true);

        // Composite Primary key -> A USER can have 1 OPTION (of the same type) per SURVEY.
        //builder.HasIndex(nameof(UserSurveyAnswer.UserId), nameof(UserSurveyAnswer.SurveyId), nameof(UserSurveyAnswer.ChosenOptionId)).IsUnique(true);

        builder.HasKey(u => u.UserSurveyAnswerId);

    } // Configure.
}