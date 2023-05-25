using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class Initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MarkedEmails",
                columns: table => new
                {
                    MarkedEmailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserRole = table.Column<byte>(type: "tinyint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkedEmails", x => x.MarkedEmailId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Firstname = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Lastname = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HasProfilePicture = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderKey = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LoginProvider = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CommentHistories",
                columns: table => new
                {
                    CommentHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EditedById = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CommentStatus = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    EditedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReactionGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentHistories", x => x.CommentHistoryId);
                    table.ForeignKey(
                        name: "FK_CommentHistories_Users_EditedById",
                        column: x => x.EditedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CommentStatusTotals",
                columns: table => new
                {
                    CommentStatusTotalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectStatisticsId = table.Column<int>(type: "int", nullable: false),
                    CommentStatus = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentStatusTotals", x => x.CommentStatusTotalId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocReviewHistories",
                columns: table => new
                {
                    DocReviewHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EditorId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EditedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DocReviewId = table.Column<int>(type: "int", nullable: false),
                    DocReviewStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocReviewHistories", x => x.DocReviewHistoryId);
                    table.ForeignKey(
                        name: "FK_DocReviewHistories_Users_EditorId",
                        column: x => x.EditorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocReviews",
                columns: table => new
                {
                    DocReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocReviewText = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WrittenById = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocReviewSettings_IsCommentingAllowed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DocReviewSettings_IsSubCommentingAllowed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DocReviewSettings_AreEmojisAllowed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DocReviewSettings_IsClosedForComments = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DocReviewSettings_IsLogInRequired = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DocReviewSettings_IsPostModerated = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocReviews", x => x.DocReviewId);
                    table.ForeignKey(
                        name: "FK_DocReviews_Users_WrittenById",
                        column: x => x.WrittenById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Emojis",
                columns: table => new
                {
                    EmojiId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocReviewId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emojis", x => x.EmojiId);
                    table.ForeignKey(
                        name: "FK_Emojis_DocReviews_DocReviewId",
                        column: x => x.DocReviewId,
                        principalTable: "DocReviews",
                        principalColumn: "DocReviewId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    SurveyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AreMultipleOptionsAllowed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BeginChar = table.Column<int>(type: "int", nullable: false),
                    EndChar = table.Column<int>(type: "int", nullable: false),
                    DocReviewId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.SurveyId);
                    table.ForeignKey(
                        name: "FK_Surveys_DocReviews_DocReviewId",
                        column: x => x.DocReviewId,
                        principalTable: "DocReviews",
                        principalColumn: "DocReviewId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CommentComposites",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlacedOnReactionGroupId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocReviewId = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmojiId = table.Column<int>(type: "int", nullable: true),
                    CommentText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BeginChar = table.Column<int>(type: "int", nullable: true),
                    EndChar = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentComposites", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_CommentComposites_CommentComposites_PlacedOnReactionGroupId",
                        column: x => x.PlacedOnReactionGroupId,
                        principalTable: "CommentComposites",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentComposites_DocReviews_DocReviewId",
                        column: x => x.DocReviewId,
                        principalTable: "DocReviews",
                        principalColumn: "DocReviewId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentComposites_Emojis_EmojiId",
                        column: x => x.EmojiId,
                        principalTable: "Emojis",
                        principalColumn: "EmojiId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentComposites_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SurveyOptions",
                columns: table => new
                {
                    SurveyOptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Option = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SurveyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyOptions", x => x.SurveyOptionId);
                    table.ForeignKey(
                        name: "FK_SurveyOptions_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "SurveyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SurveyAnswers",
                columns: table => new
                {
                    UserSurveyAnswerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SurveyId = table.Column<int>(type: "int", nullable: false),
                    ChosenOptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyAnswers", x => x.UserSurveyAnswerId);
                    table.ForeignKey(
                        name: "FK_SurveyAnswers_SurveyOptions_ChosenOptionId",
                        column: x => x.ChosenOptionId,
                        principalTable: "SurveyOptions",
                        principalColumn: "SurveyOptionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SurveyAnswers_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "SurveyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SurveyAnswers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CommentTags",
                columns: table => new
                {
                    CommentTagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectTagId = table.Column<int>(type: "int", nullable: false),
                    ReactionGroupId = table.Column<int>(type: "int", nullable: false),
                    PlacedByUserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentTags", x => x.CommentTagId);
                    table.ForeignKey(
                        name: "FK_CommentTags_CommentComposites_ReactionGroupId",
                        column: x => x.ReactionGroupId,
                        principalTable: "CommentComposites",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentTags_Users_PlacedByUserId",
                        column: x => x.PlacedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocReviewStatusTotals",
                columns: table => new
                {
                    DocReviewStatusTotalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectStatisticsId = table.Column<int>(type: "int", nullable: false),
                    DocReviewStatus = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocReviewStatusTotals", x => x.DocReviewStatusTotalId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EmojiTypeTotals",
                columns: table => new
                {
                    EmojiTypeTotalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectStatisticsId = table.Column<int>(type: "int", nullable: false),
                    EmojiId = table.Column<int>(type: "int", nullable: true),
                    Total = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmojiTypeTotals", x => x.EmojiTypeTotalId);
                    table.ForeignKey(
                        name: "FK_EmojiTypeTotals_Emojis_EmojiId",
                        column: x => x.EmojiId,
                        principalTable: "Emojis",
                        principalColumn: "EmojiId",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MarkedEmailProject",
                columns: table => new
                {
                    MarkedEmailsMarkedEmailId = table.Column<int>(type: "int", nullable: false),
                    ProjectsProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkedEmailProject", x => new { x.MarkedEmailsMarkedEmailId, x.ProjectsProjectId });
                    table.ForeignKey(
                        name: "FK_MarkedEmailProject_MarkedEmails_MarkedEmailsMarkedEmailId",
                        column: x => x.MarkedEmailsMarkedEmailId,
                        principalTable: "MarkedEmails",
                        principalColumn: "MarkedEmailId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProjectFooterLogos",
                columns: table => new
                {
                    FooterLogoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ImageName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFooterLogos", x => x.FooterLogoId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProjectHistories",
                columns: table => new
                {
                    ProjectHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EditedById = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    EditedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ProjectStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectHistories", x => x.ProjectHistoryId);
                    table.ForeignKey(
                        name: "FK_ProjectHistories_Users_EditedById",
                        column: x => x.EditedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InternalName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExternalName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProjectTitle = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Introduction = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProjectStylingId = table.Column<int>(type: "int", nullable: true),
                    PrivacyStatement = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccessibilityStatement = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProjectStatistics",
                columns: table => new
                {
                    ProjectStatisticsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LastUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ReactionGroupAmount = table.Column<int>(type: "int", nullable: false),
                    EmojiAmount = table.Column<int>(type: "int", nullable: false),
                    UsersAmount = table.Column<int>(type: "int", nullable: false),
                    ManagersAmount = table.Column<int>(type: "int", nullable: false),
                    DocReviewsAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectStatistics", x => x.ProjectStatisticsId);
                    table.ForeignKey(
                        name: "FK_ProjectStatistics_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProjectTags",
                columns: table => new
                {
                    ProjectTagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsTextWhite = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsPublic = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTags", x => x.ProjectTagId);
                    table.ForeignKey(
                        name: "FK_ProjectTags_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProjectUser",
                columns: table => new
                {
                    RegisteredForProjectsProjectId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUser", x => new { x.RegisteredForProjectsProjectId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ProjectUser_Projects_RegisteredForProjectsProjectId",
                        column: x => x.RegisteredForProjectsProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ThemeStyles",
                columns: table => new
                {
                    ThemeStylesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    GenericName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ColorLight = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ColorMedium = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ColorDark = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ColorDarkest = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThemeStyles", x => x.ThemeStylesId);
                    table.ForeignKey(
                        name: "FK_ThemeStyles_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TimeLines",
                columns: table => new
                {
                    TimeLineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeLines", x => x.TimeLineId);
                    table.ForeignKey(
                        name: "FK_TimeLines_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserPropertiesNames",
                columns: table => new
                {
                    UserPropertyNameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserPropertyLabel = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserPropertyType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    IsRequired = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RegisteredForProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPropertiesNames", x => x.UserPropertyNameId);
                    table.ForeignKey(
                        name: "FK_UserPropertiesNames_Projects_RegisteredForProjectId",
                        column: x => x.RegisteredForProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProjectStylings",
                columns: table => new
                {
                    ProjectStylingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ThemeStylesId = table.Column<int>(type: "int", nullable: false),
                    FontSerif = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FontSansSerif = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectStylings", x => x.ProjectStylingId);
                    table.ForeignKey(
                        name: "FK_ProjectStylings_ThemeStyles_ThemeStylesId",
                        column: x => x.ThemeStylesId,
                        principalTable: "ThemeStyles",
                        principalColumn: "ThemeStylesId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TimeLinePhases",
                columns: table => new
                {
                    TimeLinePhaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TimeLineId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BeginDate = table.Column<DateTime>(type: "date", nullable: false),
                    DocReviewId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeLinePhases", x => x.TimeLinePhaseId);
                    table.ForeignKey(
                        name: "FK_TimeLinePhases_DocReviews_DocReviewId",
                        column: x => x.DocReviewId,
                        principalTable: "DocReviews",
                        principalColumn: "DocReviewId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TimeLinePhases_TimeLines_TimeLineId",
                        column: x => x.TimeLineId,
                        principalTable: "TimeLines",
                        principalColumn: "TimeLineId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserPropertyValues",
                columns: table => new
                {
                    UserPropertyValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserPropertyLabel = table.Column<int>(type: "int", nullable: false),
                    UserPropertyNameId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserPropertyDateValue_Value = table.Column<DateTime>(type: "date", nullable: true),
                    UserPropertyDecimalValue_Value = table.Column<double>(type: "double", nullable: true),
                    UserPropertyNumericValue_Value = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPropertyValues", x => x.UserPropertyValueId);
                    table.ForeignKey(
                        name: "FK_UserPropertyValues_UserPropertiesNames_UserPropertyLabel",
                        column: x => x.UserPropertyLabel,
                        principalTable: "UserPropertiesNames",
                        principalColumn: "UserPropertyNameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPropertyValues_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Emojis",
                columns: new[] { "EmojiId", "Code", "DocReviewId" },
                values: new object[,]
                {
                    { 1, "128545", null },
                    { 2, "128514", null },
                    { 3, "129315", null },
                    { 4, "129300", null },
                    { 5, "128078", null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "Admin", "fc91912b-90fc-4262-bb97-f06ba29675d4", "Admin", "ADMIN" },
                    { "ProjectManager", "d994a042-7de5-45d5-b707-ee40571f4b03", "ProjectManager", "PROJECTMANAGER" },
                    { "RegularUser", "0a46b56b-de43-4322-a941-7acb26d38a9e", "RegularUser", "REGULARUSER" }
                });

            migrationBuilder.InsertData(
                table: "ThemeStyles",
                columns: new[] { "ThemeStylesId", "ColorDark", "ColorDarkest", "ColorLight", "ColorMedium", "DisplayName", "GenericName", "ProjectId" },
                values: new object[,]
                {
                    { 1, "3F3F3F", "000000", "000000", "3F3F3F", "KdG", "Black & White", null },
                    { 2, "6A783B", "283618", "90A955", "7A8E49", "Forest Fresh", "Green", null },
                    { 3, "54a0ff", "2e86de", "A4DDED", "73C2FB", "Deep Sky Blue", "Light Blue", null },
                    { 4, "0077B6", "003459", "00a8e8", "0096C7", "Ocean Breeze", "Blue", null },
                    { 5, "FF6000", "FF4800", "F8C537", "FF8500", "Summer Hot", "Yellow", null },
                    { 6, "9D0208", "6A040F", "Ef3907", "D00000", "Fiery Red", "Red", null },
                    { 7, "6F1D1B", "582F0E", "A68A64", "936639", "Autumn Leaves", "Brown", null },
                    { 8, "B56576", "6D597A", "EAAC8B", "E56B6F", "Pink", "Pink", null },
                    { 9, "0AEFFF", "147DF5", "DEFF0A", "0AFF99", "Eye Burn", "Neon", null },
                    { 10, "6411AD", "47126B", "C05299", "973AA8", "Purple", "Purple", null },
                    { 11, "798478", "4D6A6D", "C9ADA1", "A0A083", "Boredom Grey", "Saturated Grey", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "Firstname", "HasProfilePicture", "Lastname", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "5938ddaa-f5e7-4259-8ebe-2b35c14326d8", 0, "632f66a0-1829-4d89-9e2b-31391270d084", "admin@groenpunt.be", true, "Groenpunt", false, "Root Admin", false, null, "ADMIN@GROENPUNT.BE", "ADMIN@GROENPUNT.BE_ADMIN", "AQAAAAEAACcQAAAAELIqx/HvWn22Fv58gvJC0JmAxwur/wRGLuw2fKoqz1pgxwkTnxjMKpnPURLwvAKTLQ==", null, false, "d09f62cd-dffe-47bf-830d-baced85b5b13", false, "admin@groenpunt.be_admin" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "Admin", "5938ddaa-f5e7-4259-8ebe-2b35c14326d8" });

            migrationBuilder.CreateIndex(
                name: "IX_CommentComposites_DocReviewId",
                table: "CommentComposites",
                column: "DocReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentComposites_EmojiId",
                table: "CommentComposites",
                column: "EmojiId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentComposites_PlacedOnReactionGroupId",
                table: "CommentComposites",
                column: "PlacedOnReactionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentComposites_UserId",
                table: "CommentComposites",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentHistories_EditedById",
                table: "CommentHistories",
                column: "EditedById");

            migrationBuilder.CreateIndex(
                name: "IX_CommentHistories_ReactionGroupId",
                table: "CommentHistories",
                column: "ReactionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentStatusTotals_ProjectStatisticsId",
                table: "CommentStatusTotals",
                column: "ProjectStatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentTags_PlacedByUserId",
                table: "CommentTags",
                column: "PlacedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentTags_ProjectTagId_ReactionGroupId",
                table: "CommentTags",
                columns: new[] { "ProjectTagId", "ReactionGroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentTags_ReactionGroupId",
                table: "CommentTags",
                column: "ReactionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DocReviewHistories_DocReviewId",
                table: "DocReviewHistories",
                column: "DocReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_DocReviewHistories_EditorId",
                table: "DocReviewHistories",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_DocReviews_ProjectId",
                table: "DocReviews",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DocReviews_WrittenById",
                table: "DocReviews",
                column: "WrittenById");

            migrationBuilder.CreateIndex(
                name: "IX_DocReviewStatusTotals_ProjectStatisticsId",
                table: "DocReviewStatusTotals",
                column: "ProjectStatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_Emojis_Code_DocReviewId",
                table: "Emojis",
                columns: new[] { "Code", "DocReviewId" });

            migrationBuilder.CreateIndex(
                name: "IX_Emojis_DocReviewId",
                table: "Emojis",
                column: "DocReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_EmojiTypeTotals_EmojiId",
                table: "EmojiTypeTotals",
                column: "EmojiId");

            migrationBuilder.CreateIndex(
                name: "IX_EmojiTypeTotals_ProjectStatisticsId",
                table: "EmojiTypeTotals",
                column: "ProjectStatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkedEmailProject_ProjectsProjectId",
                table: "MarkedEmailProject",
                column: "ProjectsProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkedEmails_Email",
                table: "MarkedEmails",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFooterLogos_ImageName_ProjectId",
                table: "ProjectFooterLogos",
                columns: new[] { "ImageName", "ProjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFooterLogos_ProjectId",
                table: "ProjectFooterLogos",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectHistories_EditedById",
                table: "ProjectHistories",
                column: "EditedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectHistories_ProjectId",
                table: "ProjectHistories",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ExternalName",
                table: "Projects",
                column: "ExternalName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_InternalName",
                table: "Projects",
                column: "InternalName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectStylingId",
                table: "Projects",
                column: "ProjectStylingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStatistics_ProjectId",
                table: "ProjectStatistics",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStylings_ThemeStylesId",
                table: "ProjectStylings",
                column: "ThemeStylesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTags_ProjectId_Name",
                table: "ProjectTags",
                columns: new[] { "ProjectId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUser_UsersId",
                table: "ProjectUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAnswers_ChosenOptionId",
                table: "SurveyAnswers",
                column: "ChosenOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAnswers_SurveyId",
                table: "SurveyAnswers",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAnswers_UserId",
                table: "SurveyAnswers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyOptions_SurveyId",
                table: "SurveyOptions",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_DocReviewId",
                table: "Surveys",
                column: "DocReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_ThemeStyles_ProjectId",
                table: "ThemeStyles",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLinePhases_DocReviewId",
                table: "TimeLinePhases",
                column: "DocReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLinePhases_TimeLineId",
                table: "TimeLinePhases",
                column: "TimeLineId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLines_ProjectId",
                table: "TimeLines",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPropertiesNames_RegisteredForProjectId",
                table: "UserPropertiesNames",
                column: "RegisteredForProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPropertiesNames_UserPropertyLabel_RegisteredForProjectId",
                table: "UserPropertiesNames",
                columns: new[] { "UserPropertyLabel", "RegisteredForProjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPropertyValues_UserId",
                table: "UserPropertyValues",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPropertyValues_UserPropertyLabel",
                table: "UserPropertyValues",
                column: "UserPropertyLabel");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentHistories_CommentComposites_ReactionGroupId",
                table: "CommentHistories",
                column: "ReactionGroupId",
                principalTable: "CommentComposites",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentStatusTotals_ProjectStatistics_ProjectStatisticsId",
                table: "CommentStatusTotals",
                column: "ProjectStatisticsId",
                principalTable: "ProjectStatistics",
                principalColumn: "ProjectStatisticsId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocReviewHistories_DocReviews_DocReviewId",
                table: "DocReviewHistories",
                column: "DocReviewId",
                principalTable: "DocReviews",
                principalColumn: "DocReviewId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocReviews_Projects_ProjectId",
                table: "DocReviews",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentTags_ProjectTags_ProjectTagId",
                table: "CommentTags",
                column: "ProjectTagId",
                principalTable: "ProjectTags",
                principalColumn: "ProjectTagId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocReviewStatusTotals_ProjectStatistics_ProjectStatisticsId",
                table: "DocReviewStatusTotals",
                column: "ProjectStatisticsId",
                principalTable: "ProjectStatistics",
                principalColumn: "ProjectStatisticsId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmojiTypeTotals_ProjectStatistics_ProjectStatisticsId",
                table: "EmojiTypeTotals",
                column: "ProjectStatisticsId",
                principalTable: "ProjectStatistics",
                principalColumn: "ProjectStatisticsId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MarkedEmailProject_Projects_ProjectsProjectId",
                table: "MarkedEmailProject",
                column: "ProjectsProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFooterLogos_Projects_ProjectId",
                table: "ProjectFooterLogos",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectHistories_Projects_ProjectId",
                table: "ProjectHistories",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectStylings_ProjectStylingId",
                table: "Projects",
                column: "ProjectStylingId",
                principalTable: "ProjectStylings",
                principalColumn: "ProjectStylingId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThemeStyles_Projects_ProjectId",
                table: "ThemeStyles");

            migrationBuilder.DropTable(
                name: "CommentHistories");

            migrationBuilder.DropTable(
                name: "CommentStatusTotals");

            migrationBuilder.DropTable(
                name: "CommentTags");

            migrationBuilder.DropTable(
                name: "DocReviewHistories");

            migrationBuilder.DropTable(
                name: "DocReviewStatusTotals");

            migrationBuilder.DropTable(
                name: "EmojiTypeTotals");

            migrationBuilder.DropTable(
                name: "MarkedEmailProject");

            migrationBuilder.DropTable(
                name: "ProjectFooterLogos");

            migrationBuilder.DropTable(
                name: "ProjectHistories");

            migrationBuilder.DropTable(
                name: "ProjectUser");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "SurveyAnswers");

            migrationBuilder.DropTable(
                name: "TimeLinePhases");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserPropertyValues");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "CommentComposites");

            migrationBuilder.DropTable(
                name: "ProjectTags");

            migrationBuilder.DropTable(
                name: "ProjectStatistics");

            migrationBuilder.DropTable(
                name: "MarkedEmails");

            migrationBuilder.DropTable(
                name: "SurveyOptions");

            migrationBuilder.DropTable(
                name: "TimeLines");

            migrationBuilder.DropTable(
                name: "UserPropertiesNames");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Emojis");

            migrationBuilder.DropTable(
                name: "Surveys");

            migrationBuilder.DropTable(
                name: "DocReviews");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "ProjectStylings");

            migrationBuilder.DropTable(
                name: "ThemeStyles");
        }
    }
}
