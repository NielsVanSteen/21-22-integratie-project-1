using DAL.Converters;
using DAL.FluentApi.Comment;
using DAL.FluentApi.DocReview;
using DAL.FluentApi.Project;
using DAL.FluentApi.ProjectStatistics;
using DAL.FluentApi.User;
using Domain.Comment;
using Domain.DocReview;
using Domain.Project;
using Domain.ProjectStatistics;
using Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL;

///<author>Niels Van Steen</author>
/// <summary>
/// The root DbContext for the application.
/// References all classes in DbSets. and configures the settings.
/// </summary>
public class DocReviewDbContext : IdentityDbContext<User>
{
    public static readonly int MaxSqlLength = 82;

    // Properties.

    #region UserPackageDbSets

    public DbSet<MarkedEmail> MarkedEmails { get; set; }
    public DbSet<UserPropertyValue> UserPropertiesValues { get; set; }
    public DbSet<UserPropertyDateValue> UserPropertiesDateValues { get; set; }
    public DbSet<UserPropertyStringValue> UserPropertiesStringValues { get; set; }
    public DbSet<UserPropertyNumericValue> UserPropertiesNumericValues { get; set; }
    public DbSet<UserPropertyDecimalValue> UserPropertiesDecimalValues { get; set; }
    public DbSet<UserPropertyName> UserPropertiesNames { get; set; }

    #endregion

    #region ProjectPackageDbSets

    public DbSet<Project> Projects { get; set; }
    public DbSet<FooterLogo> FooterLogos { get; set; }
    public DbSet<ProjectTag> ProjectTags { get; set; }
    public DbSet<TimeLine> TimeLines { get; set; }
    public DbSet<ProjectHistory> ProjectHistories { get; set; }
    public DbSet<TimeLinePhase> TimeLinePhases { get; set; }
    public DbSet<ProjectStyling> ProjectStylings { get; set; }
    public DbSet<ThemeStyles> ThemeStyles { get; set; }

    #endregion

    #region ProjectSatisticsDbSets

    public DbSet<ProjectStatistics> ProjectStatistics { get; set; }
    public DbSet<CommentStatusTotal> CommentStatusTotals { get; set; }
    public DbSet<EmojiTypeTotal> EmojiTypeTotals { get; set; }
    public DbSet<DocReviewStatusTotal> DocReviewStatusTotals { get; set; }

    #endregion

    #region DocReviewPackageDbSets

    public DbSet<DocReview> DocReviews { get; set; }
    public DbSet<DocReviewHistory> DocReviewHistories { get; set; }
    public DbSet<Emoji> Emojis { get; set; }
    public DbSet<Survey> Surveys { get; set; }
    public DbSet<SurveyOption> SurveyOptions { get; set; }
    public DbSet<UserSurveyAnswer> SurveyAnswers { get; set; }

    #endregion

    #region CommentPackageDbSets

    public DbSet<ReactionGroup> Comments { get; set; }
    public DbSet<EmojiReaction> EmojiReactions { get; set; }

    public DbSet<Reaction> CommentComposites { get; set; }
    public DbSet<CommentHistory> CommentHistories { get; set; }
    public DbSet<CommentTag> CommentTags { get; set; }

    #endregion

    // Constructor.
    public DocReviewDbContext(DbContextOptions<DocReviewDbContext> options) : base(options)
    {
    }

    // Methods.
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Initializing is done on program startup. in program.cs.
    } // OnConfiguring.

    ///<author>Niels Van Steen</author>
    /// <summary>
    /// Configures all the DB settings for the entities.
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        ChangeIdentityTableNames(builder);

        #region UserPackageConfiguration

        // Add the configuration for the User class.
        builder.ApplyConfiguration(new UserEntityConfiguration());

        // Add the configuration for the MarkedEmail class.
        builder.ApplyConfiguration(new MarkedEmailEntityConfiguration());

        // Add the configuration for the UserPropertyName class.
        builder.ApplyConfiguration(new UserPropertyNameEntityConfiguration());

        // Add the configuration for the UserPropertyValue class.
        builder.ApplyConfiguration(new UserPropertyValueEntityConfiguration());

        #endregion

        #region ProjectPackageConfiguration

        // Add the configuration for the Project class.
        builder.ApplyConfiguration(new FooterLogoEntityConfiguration());

        // Add the configuration for the Project class.
        builder.ApplyConfiguration(new ProjectEntityConfiguration());

        // Add the configuration for the ProjectTag class.
        builder.ApplyConfiguration(new ProjectTagEntityConfiguration());

        // Add the configuration for the ProjectTimeLine class.
        builder.ApplyConfiguration(new TimeLineEntityConfiguration());

        // Add the configuration for the ProjectTimeLinePhase class.
        builder.ApplyConfiguration(new TimeLinePhaseEntityConfiguration());

        // Add the configuration for the ProjectHistory class.
        builder.ApplyConfiguration(new ProjectHistoryEntityConfiguration());

        // Add the configuration for the ProjectStyling class.
        builder.ApplyConfiguration(new ProjectStylingEntityConfiguration());

        // Add the configuration for the  ThemeStyles class.
        builder.ApplyConfiguration(new ThemeStylesEntityConfiguration());

        #endregion

        #region ProjectStatisticsPackageConfiguration

        // Add the configuration for the ProjectStatistics class.
        builder.ApplyConfiguration(new ProjectStatisticsEntityConfiguration());

        // Add the configuration for the EmojiTypeTotal class.
        builder.ApplyConfiguration(new EmojiTypeTotalEntityConfiguration());

        // Add the configuration for the CommentStatusTotal class.
        builder.ApplyConfiguration(new CommentStatusTotalEntityConfiguration());

        // Add the configuration for the DocReviewStatusTotal class.
        builder.ApplyConfiguration(new DocReviewStatusTotalEntityConfiguration());

        #endregion

        #region DocReviewPackageConfiguration

        // Add the configuration for the DocReview class.
        builder.ApplyConfiguration(new DocReviewEntityConfiguration());

        // Add the configuration for the SurveyOption class.
        builder.ApplyConfiguration(new SurveyOptionEntityConfiguration());

        // Add the configuration for the UserSurveyAnswer class.
        builder.ApplyConfiguration(new UserSurveyAnswerEntityConfiguration());

        // Add the configuration for the DocReviewHistory class.
        builder.ApplyConfiguration(new DocReviewHistoryEntityConfiguration());

        // Add the configuration for the Emoji class.
        builder.ApplyConfiguration(new EmojiEntityConfiguration());

        // Add the configuration for the Survey class.
        builder.ApplyConfiguration(new SurveyEntityConfiguration());

        #endregion

        #region CommentPackageConfiguration

        // Add the configuration for the CommentComposite class.
        builder.ApplyConfiguration(new ReactionEntityConfiguration());

        // Add the configuration for the Comment class.
        builder.ApplyConfiguration(new ReactionGroupEntityConfiguration());

        // Add the configuration for the CommentHistory class.
        builder.ApplyConfiguration(new CommentHistoryEntityConfiguration());

        // Add the configuration for the CommentTag class.
        builder.ApplyConfiguration(new CommentTagEntityConfiguration());

        // Add the configuration for the EmojiReaction class.
        builder.ApplyConfiguration(new EmojiReactionEntityConfiguration());

        #endregion

        #region Seeding

        // Seed roles.
        foreach (var index in Enum.GetValues(typeof(UserRole)))
        {
            var role = Enum.GetName(typeof(UserRole), index);
            builder
                .Entity<IdentityRole>()
                .HasData(new IdentityRole
                {
                    Id = index.ToString(),
                    Name = index.ToString(),
                    NormalizedName = index?.ToString()?.ToUpper()
                });
        } // Foreach.

        // Seeding the project styles.
        int count = 0;
        foreach (var theme in Domain.Project.ThemeStyles.ProjectColorStyles)
        {
            builder
                .Entity<ThemeStyles>()
                .HasData(
                    new ThemeStyles()
                    {
                        ColorLight = theme.colorLight,
                        ColorMedium = theme.colorMedium,
                        ColorDark = theme.colorDark,
                        ColorDarkest = theme.colorDarkest,
                        ThemeStylesId = ++count,
                        DisplayName = theme.displayName,
                        GenericName = theme.genericName
                    }
                );
        }

        var hasher = new PasswordHasher<IdentityUser>();
        builder
            .Entity<User>()
            .HasData(
                new User
                {
                    Id = User.RootUserId,
                    UserName = "admin@groenpunt.be_admin",
                    Firstname = "Groenpunt",
                    Lastname = "Root Admin",
                    NormalizedUserName = "ADMIN@GROENPUNT.BE_ADMIN",
                    Email = "admin@groenpunt.be",
                    NormalizedEmail = "ADMIN@GROENPUNT.BE",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PasswordHash = hasher.HashPassword(null, "Admin123/")
                }
            );

        builder
            .Entity<IdentityUserRole<string>>()
            .HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "Admin",
                    UserId = User.RootUserId,
                }
            );

        builder
            .Entity<Emoji>()
            .HasData(
                new Emoji()
                {
                    Code = "128545",
                    EmojiId = 1
                },
                new Emoji()
                {
                    Code = "128514",
                    EmojiId = 2
                },
                new Emoji()
                {
                    Code = "129315",
                    EmojiId = 3
                },
                new Emoji()
                {
                    Code = "129300",
                    EmojiId = 4
                },
                new Emoji()
                {
                    Code = "128078",
                    EmojiId = 5
                }
            );

        #endregion
    } // OnModelCreating.

    ///<author>Niels Van Steen</author>
    /// <summary>
    /// Changes the default table names for the tables generated by identity framework.
    /// </summary>
    private void ChangeIdentityTableNames(ModelBuilder builder)
    {
        // Remove the 'AspNet' prefix from the tables.
        builder.Entity<User>(entity => entity.ToTable(name: "Users"));
        builder.Entity<IdentityRole>(entity => entity.ToTable(name: "Roles"));
        builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("UserRoles"));
        builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("UserClaims"));
        builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("UserLogins"));
        builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("RoleClaims"));
        builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("UserTokens"));

        // Change other tables to plural (the one's that aren't done automatically.
        builder.Entity<FooterLogo>(entity => entity.ToTable("ProjectFooterLogos"));
        builder.Entity<UserPropertyValue>(entity => entity.ToTable("UserPropertyValues"));
        builder.Entity<Reaction>(entity => entity.ToTable("CommentComposites"));
        builder.Entity<ProjectTag>(entity => entity.ToTable("ProjectTags"));
    } // ChangeIdentityTableNames.

    /// <author>Brian Nys</author>
    /// <summary>
    /// Configures global settings for all DB models.
    /// </summary>
    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");
    }
}